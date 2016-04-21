using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
public partial class Retire_Batch : System.Web.UI.Page
{
    string propno;
    int FillGrid()
    {
        //string sql = "select empid as EmpID, name as Name, to_char(dob,'dd-Mon-YYYY') as DOB, locabb as Office,"+
        //    "to_char(fdate,'dd-Mon-YYYY') as \"Promotion Date\" from cadre.ret_list where propno = " + propno + 
        //      " order by fdate,empid";
        //string sql = "SELECT EMPID, PSHR.GET_FULLNAME(empid) as NAME, pshr.get_desg(pshr.get_desg_by_id(empid)) as DESG," +
        //                "pshr.get_org(pshr.get_loc_by_id(empid)) as LOC, pshr.get_dob_by_id(empid) as DOB " +
        //                "FROM cadre.ret_list WHERE propno = " + propno + " order by empid";
        string sql = "select rownum as \"S.No.\", a.* from "+
                        "(SELECT e.empid, firstname||' '|| decode(middlename, NULL, '', middlename||' ') || "+
                        "lastname AS name, "+
                        "to_char(e.dob,'dd-Mon-YYYY')  AS DOB,  md.desgtext as DESG,  ml.locname as LOC, " +
                        "to_char(rl.dor,'dd-Mon-YYYY') as DOR " +
                        "FROM cadre.ret_list rl, pshr.mast_loc ml, pshr.mast_desg md, pshr.empperso e "+
                        "WHERE rl.empid = e.empid "+
                        "AND e.cloccode = ml.loccode "+
                        "AND e.cdesgcode = md.desgcode "+
                        "AND rl.propno = " + propno +
                        " order by md.hecode,e.empid) a";
        OraDBConnection.FillGrid(ref gv_redesig, sql);
        return gv_redesig.Rows.Count;
    }
    void Controls_Vis(bool visible)
    {
        btnPreview.Visible = btnSave.Visible = txtoodate.Visible = txtoonum.Visible = visible;
    }
    void FillMonths()
    {
        DateTime curdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        for (int i = 0; i <= 11; i++)
        {
            string text = curdate.ToString("MMM, yyyy");
            string val = new DateTime(curdate.Year, curdate.Month, 
                DateTime.DaysInMonth(curdate.Year, curdate.Month)).ToString("dd-MMM-yyyy");
            curdate = curdate.AddMonths(1);
            //drpMonth.Items.Add(new ListItem(text,val));
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Convert.ToString(Session["proposalno"])))
        {
            propno = Session["proposalno"].ToString();
        }
        else
        {
            Response.Redirect("~/retirement_proposals.aspx");
        }
        if (!IsPostBack)
        {
            string dt;
            string sql;
            DataSet ds;

            sql = "select nvl(to_char(retdate,'dd-Mon-yyyy'),'0') as Ret_Date,type "+
                "from cadre.ret_proposals where pno = " + propno;
            ds = OraDBConnection.GetData(sql);
            dt = ds.Tables[0].Rows[0]["Ret_Date"].ToString();
            if (dt != "0")
            {
                //drpMonth.Items.Add(new ListItem(dt));
            }
            else
            {
                FillMonths();
            }

            Controls_Vis(true);
            FillGrid();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string sql;
        string role = Session["role"].ToString();
        //string date = drpMonth.SelectedValue;
        string date = "";

        sql = "BEGIN ";

        //delete earlier entered records (if any) for this proposal
        sql += " delete from cadre.ret_list where propno = " + propno + "; ";

        sql += "INSERT INTO cadre.ret_list " +
                "SELECT e.empid,last_day (add_months(e.dob - 1, md.retdage * 12)) as DOR, " + propno + ", cdesgcode,null " +
                "FROM pshr.empperso e, pshr.mast_desg md " +
                "WHERE last_day (add_months(e.dob - 1, md.retdage * 12)) = '" + date + "'" +
                "AND e.cdesgcode = md.desgcode " +
                "AND e.recstatus   = 10" +
                "AND e.cdesgcode in (SELECT desgcode from cadre.person_auth_desgs where role = '" + role + "'); ";

        sql += string.Format(" update cadre.ret_proposals set retdate = '{0}' where pno={1}; ", date, propno);
        sql += " END;";
        OraDBConnection.ExecQry(sql);
        Controls_Vis(true);
        FillGrid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtoonum.Text.Length < 1)
        {
            Utils.ShowMessageBox(this, "Enter O/o Number");
            return;
        }
        DateTime checkdate;
        if (DateTime.TryParse(txtoodate.Text, out checkdate) == false)
        {
            Utils.ShowMessageBox(this, "Enter a valid date");
            return;
        }
        
        //mark proposal as saved
        string sql = string.Format("update cadre.ret_proposals set status='S',oonum='{0}',oodate=to_date('{1}','dd/mm/yyyy') where pno={2}",
            txtoonum.Text, txtoodate.Text, propno);
        if (OraDBConnection.ExecQry(sql) == false)
        {
            Utils.ShowMessageBox(this, "Error marking proposal as saved");
            return;
        }
        MakeReport(true);
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        MakeReport(false);
    }
    private void MakeReport(bool save = false)
    {
        string oonum = "-";
        string oodate = "-";
        string propno = this.propno.ToString();
        string pdfPath;

        if (save)
        {
            oonum = txtoonum.Text;
            oodate = txtoodate.Text;
        }
        //string sql = "select * from cadre.ret_list where propno = " + propno + " order by empid";
        string sql = "select rownum as \"#\", '" + oonum + "' as onum, '" + oodate + "' as odate, a.* from " +
                        "(SELECT e.empid, "+
                        "firstname||' '|| decode(middlename, NULL, '', middlename||' ') || lastname AS name, " +
                        "to_char(e.dob,'dd/MM/YYYY') AS DOB,  md.desgtext as DESG,  ml.locname as LOC, " +
                        "to_char(rl.dor,'dd/MM/YYYY') as DOR, " +
                        "rp.type as reptype " +
                        "FROM cadre.ret_list rl, pshr.mast_loc ml, pshr.mast_desg md, pshr.empperso e, cadre.ret_proposals rp " +
                        "WHERE rl.empid = e.empid " +
                        "AND e.cloccode = ml.loccode " +
                        "AND e.cdesgcode = md.desgcode " +
                        "AND rl.propno = " + propno +
                        " AND rp.pno = rl.propno" +
                        " order by md.hecode,e.empid) a";

        OraDBConnection oraCn = new OraDBConnection();
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        if (save)
        {
            pdfPath = Server.MapPath("office_orders\\" + oonum + "-BEG-3" + oodate + ".pdf");
        }
        else
        {
            pdfPath = Server.MapPath("office_orders\\preview_ret-" + propno + "-" + DateTime.Now.ToString("yyyyMMdd-HHmmssfff") + ".pdf");
        }

        CrystalReportSource CrystalReportSource1 = new CrystalReportSource();
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rpt_ret_sup.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);
        CrystalReportSource1.Dispose();
        Utils.DownloadFile(pdfPath);
    }
    //private void DownloadFile(String pdfPath)
    //{
    //    System.IO.FileInfo objFi = new System.IO.FileInfo(pdfPath);
    //    HttpContext.Current.Response.Clear();
    //    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + objFi.Name);
    //    HttpContext.Current.Response.Charset = "";
    //    HttpContext.Current.Response.AddHeader("Content-Length", objFi.Length.ToString());
    //    HttpContext.Current.Response.ContentType = "application/pdf";
    //    HttpContext.Current.Response.WriteFile(objFi.FullName);
    //    HttpContext.Current.Response.End();
    //}
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        string empid = gv_redesig.Rows[((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex].Cells[2].Text.ToString();
        string sql = string.Empty;

        sql = "delete from cadre.ret_list where empid=" + empid + " and propno=" + propno;
        OraDBConnection.ExecQry(sql);

        FillGrid();
    }
}