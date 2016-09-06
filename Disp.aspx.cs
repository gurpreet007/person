using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;
using Microsoft.VisualBasic.FileIO;
public partial class Disp : System.Web.UI.Page
{
    private struct DispFields
    {
        public int empcd;                   //#3
        private string fileno;               //#2

        public string Fileno
        {
            get { return fileno; }
            set { fileno = value.Length>10?value.Substring(0,9):value; }
        }
        private string cs_scn_no;            //#16 'Charge Sheet/ Show Cause No'

        public string Cs_scn_no
        {
            get { return cs_scn_no; }
            set { cs_scn_no = value.Length>32?value.Substring(0,31):value; }
        }
        public DateTime cs_scn_date;        //#17 'Charge Sheet/ Show Cause Date',
        private string cs_scn_exp;           //#44 'Possibly Explanation if it is CS or SCN'

        public string Cs_scn_exp
        {
            get { return cs_scn_exp; }
            set { cs_scn_exp = value.Length > 32 ? value.Substring(0, 31) : value; }
        }
        private string charge;               //#18

        public string Charge
        {
            get { return charge; }
            set { charge = value.Length > 4000 ? value.Substring(0, 3999) : value; }
        }
        private string fir_no;               //#39 'FIR number'

        public string Fir_no
        {
            get { return fir_no; }
            set { fir_no = value.Length > 32 ? value.Substring(0, 31) : value; }
        }
        public DateTime fir_date;           //#40
        public DateTime dor;                //#20 'Date of Reply'
        private string decided;              //#45 'Case Status D - Decided,  P - Pending etc' Default 0

        public string Decided
        {
            get { return decided; }
            set { decided = value.Length > 32 ? value.Substring(0, 31) : value; }
        }
        public DateTime dod;                //#27 'Date of Decision'
        private string decision_oo;          //#26 'Decision O/O No.'

        public string Decision_oo
        {
            get { return decision_oo; }
            set { decision_oo = value.Length>32?value.Substring(0,31):value; }
        }
        private string decision;             //#25

        public string Decision
        {
            get { return decision; }
            set { decision = value.Length > 4000 ? value.Substring(0, 3999) : value; }
        }
        private string appealdecision;       //#36

        public string Appealdecision
        {
            get { return appealdecision; }
            set { appealdecision = value.Length > 4000 ? value.Substring(0, 3999) : value; }
        }
        private string appealdecision_oo;    //#37

        public string Appealdecision_oo
        {
            get { return appealdecision_oo; }
            set { appealdecision_oo = value.Length > 32 ? value.Substring(0, 31) : value; }
        }
        public DateTime appeal_oo_date;     //#34
        public DateTime modified;           //'When updated'
    }
    private Dictionary<string,int> dictDispFields = new Dictionary<string,int>(16);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["loginy"] == null || Session["loginy"].ToString() != "1")
        {
            Response.Redirect("./Login.aspx");
        }
        dictDispFields.Add("empcd", 3);
        dictDispFields.Add("fileno", 2);
        dictDispFields.Add("cs_scn_no", 16);
        dictDispFields.Add("cs_scn_date", 17);
        dictDispFields.Add("cs_scn_exp", 44);
        dictDispFields.Add("charge", 18);
        dictDispFields.Add("fir_no", 39);
        dictDispFields.Add("fir_date", 40);
        dictDispFields.Add("dor", 20);
        dictDispFields.Add("decided", 45);
        dictDispFields.Add("dod", 27);
        dictDispFields.Add("decision_oo", 26);
        dictDispFields.Add("decision", 25);
        dictDispFields.Add("appealdecision", 36);
        dictDispFields.Add("appealdecision_oo", 37);
        dictDispFields.Add("appeal_oo_date", 34);

        LastUp();
    }
    private void LastUp()
    {
        lblLastAt.Text = "Last update at " + OraDBConnection.GetScalar("select to_char(max(MODIFIED),'dd-Mon-yyyy hh24:mi') from cadre.chargesheetdata") + " containing ";
        lnkLastEntries.Text = OraDBConnection.GetScalar("select count(*) from cadre.chargesheetdata") + " Entries";
    }
    private void SaveContentLines(OracleConnection con, long line, string[] fields)
    {
        const string DATE_FORMAT_NET = "dd/MM/yyyy";
        const string DATE_FORMAT_NET2 = "dd/MMM/yyyy";
        const string DATE_FORMAT_NET_FULL = "dd/MMM/yyyy HH:mm:ss";
        const string DATE_FORMAT_ORA_FULL = "dd-Mon-yyyy hh24:mi:ss";
        string sql;
        DateTime modTime = DateTime.Now;
        DateTime tryValidDate;
        int tryValidInt;
        
        DispFields dispFields = new DispFields();

        if (fields.Length != 56)
        {
            OraDBConnection.ExecQryOnConnection(con, 
                string.Format("INSERT INTO CADRE.CHARGESHEETDATA_ERRORS VALUES({0},'{1}')", 
                line, "Invalid Columns: " + fields.Length));
            return;
        }
        //trim fields and escape quotes
        fields = fields.Select(s => s.Replace("'", "''").Trim(new char[] { ' ', '/', '"' })).ToArray();

        //copy values in struct variable
        if (int.TryParse(fields[dictDispFields["empcd"]], out tryValidInt))
        {
            if (tryValidInt.ToString().Length == 6)
                dispFields.empcd = tryValidInt;
            else if (tryValidInt.ToString().Length > 6)
            {
                OraDBConnection.ExecQryOnConnection(con, 
                    string.Format("INSERT INTO CADRE.CHARGESHEETDATA_ERRORS VALUES({0},'{1}')", 
                    line, "Invalid Empid: " + fields[dictDispFields["empcd"]]));
                return;
            }
            else
                dispFields.empcd = 100000 + tryValidInt;
        }
        else
        {
            OraDBConnection.ExecQryOnConnection(con, 
                string.Format("INSERT INTO CADRE.CHARGESHEETDATA_ERRORS VALUES({0},'{1}')", 
                line, "Invalid Empid: " + fields[dictDispFields["empcd"]]));
            return;
        }
        dispFields.Fileno = fields[dictDispFields["fileno"]];
        dispFields.Cs_scn_no = fields[dictDispFields["cs_scn_no"]];
        dispFields.cs_scn_date = DateTime.TryParseExact(fields[dictDispFields["cs_scn_date"]],
                DATE_FORMAT_NET,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out tryValidDate) ? tryValidDate : DateTime.MinValue;
        dispFields.Cs_scn_exp = fields[dictDispFields["cs_scn_exp"]];
        dispFields.Charge = fields[dictDispFields["charge"]];
        dispFields.Fir_no = fields[dictDispFields["fir_no"]];
        dispFields.fir_date = DateTime.TryParseExact(fields[dictDispFields["fir_date"]],
                DATE_FORMAT_NET,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out tryValidDate) ? tryValidDate : DateTime.MinValue;
        dispFields.dor = DateTime.TryParseExact(fields[dictDispFields["dor"]],
                DATE_FORMAT_NET,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out tryValidDate) ? tryValidDate : DateTime.MinValue;
        dispFields.Decided = fields[dictDispFields["decided"]];
        dispFields.dod = DateTime.TryParseExact(fields[dictDispFields["dod"]],
                DATE_FORMAT_NET,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out tryValidDate) ? tryValidDate : DateTime.MinValue;
        dispFields.Decision_oo = fields[dictDispFields["decision_oo"]];
        dispFields.Decision = fields[dictDispFields["decision"]];
        dispFields.Appealdecision = fields[dictDispFields["appealdecision"]];
        dispFields.Appealdecision_oo = fields[dictDispFields["appealdecision_oo"]];
        dispFields.appeal_oo_date = DateTime.TryParseExact(fields[dictDispFields["appeal_oo_date"]],
                DATE_FORMAT_NET,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out tryValidDate) ? tryValidDate : DateTime.MinValue;
        dispFields.modified = modTime;

        //create sql
        sql = string.Format("INSERT INTO CADRE.CHARGESHEETDATA " +
            "(ID," +
            "EMPCD,FILENO,CS_SCN_NO,CS_SCN_DATE,CS_SCN_EXP," +
            "CHARGE,FIR_NO,FIR_DATE,DOR,DECIDED," +
            "DOD,DECISION_OO,DECISION,APPEALDECISION,APPEALDECISION_OO," +
            "APPEAL_OO_DATE,MODIFIED) " +
            "VALUES (" +
            "{17}," +
            "{0},'{1}','{2}',"+
            "'{3}','{4}'," +
            "'{5}','{6}',"+
            "'{7}',"+
            "'{8}','{9}'," +
            "'{10}','{11}',"+
            "'{12}','{13}','{14}'," +
            "'{15}',"+
            "to_date('{16}','{18}'))",
            dispFields.empcd, dispFields.Fileno, dispFields.Cs_scn_no, 
            dispFields.cs_scn_date == DateTime.MinValue ? "" : dispFields.cs_scn_date.ToString(DATE_FORMAT_NET2), dispFields.Cs_scn_exp,
            dispFields.Charge, dispFields.Fir_no, 
            dispFields.fir_date == DateTime.MinValue ? "" : dispFields.fir_date.ToString(DATE_FORMAT_NET2), 
            dispFields.dor == DateTime.MinValue ? "" : dispFields.dor.ToString(DATE_FORMAT_NET2), dispFields.Decided,
            dispFields.dod == DateTime.MinValue ? "" : dispFields.dod.ToString(DATE_FORMAT_NET2), dispFields.Decision_oo, 
            dispFields.Decision, dispFields.Appealdecision, dispFields.Appealdecision_oo,
            dispFields.appeal_oo_date == DateTime.MinValue ? "" : dispFields.appeal_oo_date.ToString(DATE_FORMAT_NET2), 
            dispFields.modified.ToString(DATE_FORMAT_NET_FULL), line, DATE_FORMAT_ORA_FULL);

        //execute sql
        try
        {
            OraDBConnection.ExecQryOnConnection(con, sql);
        }
        catch (Exception ex)
        {
            OraDBConnection.ExecQryOnConnection(con, 
                string.Format("INSERT INTO CADRE.CHARGESHEETDATA_ERRORS VALUES({0},'{1}')", 
                line, ex.Message));
            return;
        }
    }
    private void SaveContent(string file)
    {
        OracleConnection con = OraDBConnection.ConnectionOpen();
        //truncate tables
        OraDBConnection.ExecQryOnConnection(con,"TRUNCATE TABLE CADRE.CHARGESHEETDATA");
        OraDBConnection.ExecQryOnConnection(con, "TRUNCATE TABLE CADRE.CHARGESHEETDATA_ERRORS");
        using (TextFieldParser MyParser = new TextFieldParser(file))
        {
            MyParser.TextFieldType = FieldType.Delimited;
            MyParser.SetDelimiters(",");
            MyParser.TrimWhiteSpace = true;
            MyParser.HasFieldsEnclosedInQuotes = true;
            //skip header
            MyParser.ReadLine();
            while (!MyParser.EndOfData)
            {
                try
                {
                    SaveContentLines(con, MyParser.LineNumber, MyParser.ReadFields());
                }
                catch (Exception ex)
                {
                    OraDBConnection.ExecQryOnConnection(con, 
                        string.Format("INSERT INTO CADRE.CHARGESHEETDATA_ERRORS VALUES({0},'{1}')", 
                        MyParser.ErrorLineNumber, ex.Message));
                }
            }
        }
        OraDBConnection.ConnectionClose(con);
        
        //delete uploaded file
        System.IO.File.Delete(file);

        lnkSuccess.Text = OraDBConnection.GetScalar("select count(*) as cnt from CADRE.CHARGESHEETDATA");
        lnkError.Text = OraDBConnection.GetScalar("select count(*) as cnt from CADRE.CHARGESHEETDATA_ERRORS");
        lblStatus.Text = "Completed";
    }
    private void GetData(string filename, LinkButton lnkUsed)
    {
        const string DATE_FORMAT_ORA = "dd-Mon-yyyy";
        const string DATE_FORMAT_ORA_FULL = "dd-Mon-yyyy hh24:mi:ss";
        string sql;

        if (lnkUsed.Text == "N/A")
        {
            return;
        }
        sql = string.Format("SELECT ID,EMPCD,FILENO,CS_SCN_NO,to_char(CS_SCN_DATE,'{0}') CS_SCN_DATE, " +
            "CS_SCN_EXP,CHARGE,FIR_NO,to_char(FIR_DATE,'{0}') FIR_DATE, " +
            "to_char(DOR,'{0}') DOR, DECIDED, to_char(DOD,'{0}') DOD, " +
            "DECISION_OO,DECISION,APPEALDECISION,APPEALDECISION_OO," +
            "to_char(APPEAL_OO_DATE,'{0}') APPEAL_OO_DATE, to_char(MODIFIED,'{1}') MODIFIED" +
            " FROM CADRE.CHARGESHEETDATA", DATE_FORMAT_ORA, DATE_FORMAT_ORA_FULL);
        Utils.DownloadXLS(sql, filename, this);
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (fupCSV.HasFile &&
            fupCSV.PostedFile != null && 
            fupCSV.PostedFile.ContentLength > 0 && 
            (fupCSV.PostedFile.ContentType == "text/csv" || fupCSV.PostedFile.ContentType == "application/octet-stream")
           )
        {
            string fn = System.IO.Path.GetFileName(fupCSV.PostedFile.FileName);
            string SaveLocation = Server.MapPath("office_orders") + "\\" + fn;
            try
            {
                fupCSV.PostedFile.SaveAs(SaveLocation);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
            SaveContent(SaveLocation);
        }
        else
        {
            lblStatus.Text = "Please select CSV to upload";
        }
    }
    protected void lnkSuccess_Click(object sender, EventArgs e)
    {
        GetData("success_disp.xls", lnkSuccess);
    }
    protected void lnkError_Click(object sender, EventArgs e)
    {
        string sql;

        if (lnkError.Text == "N/A")
        {
            return;
        }
        sql = string.Format("SELECT * FROM CADRE.CHARGESHEETDATA_ERRORS");
        Utils.DownloadXLS(sql, "errors_disp.xls", this);
    }
    protected void lnkLastEntries_Click(object sender, EventArgs e)
    {
        GetData("lastentries.xls", lnkLastEntries);
    }
}