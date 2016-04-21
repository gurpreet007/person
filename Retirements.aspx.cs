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
    void Controls_Vis(bool visible)
    {
        btnPreview.Visible = btnSave.Visible = txtoodate.Visible = txtoonum.Visible = visible;
    }
    void Search_Visible(bool visible)
    {
        //dt_start.Visible = dt_end.Visible = visible;
        //btnSearch.Visible = visible;
        //Label2.Visible = Label4.Visible = visible;
        //drpDesg.Visible = visible;
    }
    int FillGrid()
    {
        //string sql = "select empid as EmpID, name as Name, to_char(dob,'dd-Mon-YYYY') as DOB, locabb as Office,"+
        //    "to_char(fdate,'dd-Mon-YYYY') as \"Promotion Date\" from cadre.ret_list where propno = " + propno + " order by fdate,empid";
        //string sql = "SELECT EMPID, PSHR.GET_FULLNAME(empid) as NAME, pshr.get_desg(pshr.get_desg_by_id(empid)) as DESG," +
        //                "pshr.get_org(pshr.get_loc_by_id(empid)) as LOC, pshr.get_dob_by_id(empid) as DOB " +
        //                "FROM cadre.ret_list WHERE propno = " + propno + " order by empid";
        string sql = "select rownum as \"S.No.\", a.* from (SELECT e.empid, firstname||' '|| decode(middlename, NULL, '', middlename||' ') || lastname AS name, "+
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
    private void ModifyControlsByType(string type)
    {
        if (type == "S")
        {
            panSup.Visible = true;
            panVol.Visible = false;
            //txtEmpID.Visible = false;
            //btnShow.Visible = false;
            //btnAdd.Visible = false;
            //lblDesgMsg.Visible = false;
            //panEmpHist.Visible = false;
            //txtDor.Visible = false;
        }
        else if (type == "V")
        {
            panSup.Visible = false;
            panVol.Visible = true;
            //lblDOR.Visible = false;
            //dt_start.Visible = false;
            //dt_end.Visible = false;
            //rev_start.Visible = false;
            //rev_end.Visible = false;
            //btnSearch.Visible = false;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql;
        System.Data.DataSet ds;
        System.Data.DataRow drow;
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
            //check if entries for this proposal no. exist in DB
            sql = "select to_char(fromdate,'dd-Mon-YYYY') as From_Date,to_char(todate,'dd-Mon-YYYY') as To_Date,type from cadre.ret_proposals where pno = " + propno;
            ds = OraDBConnection.GetData(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                drow = ds.Tables[0].Rows[0];
                //drpDesg.SelectedIndex = drpDesg.Items.IndexOf(drpDesg.Items.FindByValue(drow["olddesg"].ToString()));
                dt_start.Text = drow["From_Date"].ToString();
                dt_end.Text = drow["To_Date"].ToString();
                Search_Visible(false);
                Controls_Vis(true);
                ModifyControlsByType(drow["type"].ToString());
                FillGrid();
            }
            else
            {
                Search_Visible(true);
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string desg = drpDesg.SelectedValue;
        string startdt = dt_start.Text;
        string enddt = dt_end.Text;
        string sql = String.Empty;

        //reset fromdate,todat
        sql = "update cadre.ret_proposals set fromdate = null, todate = null where pno = " + propno;
        OraDBConnection.ExecQry(sql);

        //delete earlier entered records (if any) for this proposal
        sql = "delete from cadre.ret_list where propno = " + propno;
        OraDBConnection.ExecQry(sql);

        //insert matched records in prop_redesig
        //sql = String.Format("insert into CADRE.prop_redesig SELECT e.empid, e.firstname  || ' '  || e.middlename  || ' '  || e.lastname AS name, "+
        //    "to_char(e.dob,'dd-Mon-YYYY') as DOB, m.locabb, to_char(h.fromdate,'dd-Mon-YYYY') AS Promoted_On, {3}, {0} FROM " +
        //    "(SELECT empid, MIN(fromdate) AS fromdate FROM emphistory WHERE desgcode = {0} GROUP BY empid) h,  empperso e,  mast_loc m "+
        //    "WHERE e.cdesgcode = {0} AND e.recstatus = 10 AND h.empid = e.empid AND m.loccode = e.cloccode AND h.fromdate BETWEEN '{1}' AND '{2}'", desg, startdt, enddt,propno);
        //sql = "SELECT e.empid, firstname||' '|| decode(middlename, NULL, '', middlename||' ') || lastname AS name,"+
        //        "e.dob AS DOB,  md.desgtext,  ml.locname,"+
        //        "last_day (add_months(e.dob - 1, md.retdage * 12)) as DOR"+
        //        "FROM pshr.empperso e, pshr.mast_loc ml, pshr.mast_desg md"+
        //        "WHERE last_day (add_months(e.dob - 1, md.retdage * 12)) between '01-Jul-2013' AND '01-Sep-2013'"+
        //        "AND e.empid like '1%'"+
        //        "AND e.cloccode = ml.loccode"+
        //        "AND e.cdesgcode = md.desgcode"+
        //        "AND e.recstatus   = 10" +
        //        "order by md.hecode,e.empid"
        sql = "INSERT INTO cadre.ret_list "+
                "SELECT e.empid,last_day (add_months(e.dob - 1, md.retdage * 12)) as DOR, "+ propno +", cdesgcode,null " +
                "FROM pshr.empperso e, pshr.mast_desg md " +
                "WHERE last_day (add_months(e.dob - 1, md.retdage * 12)) between '"+ startdt +"' AND '"+ enddt +"' " +
                "AND e.empid like '1%' " +
                "AND e.cdesgcode = md.desgcode " +
                "AND e.recstatus   = 10";
        OraDBConnection.ExecQry(sql);

        //enter fromdate, todate in ret_proposals
        sql = string.Format("update cadre.ret_proposals set fromdate = '{0}', todate = '{1}' where pno={2}",startdt,enddt,propno);
        OraDBConnection.ExecQry(sql);

        FillGrid();
        Search_Visible(false);
        Controls_Vis(gv_redesig.Rows.Count > 0);
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
        string sql = string.Format("update cadre.ret_proposals set status='S',oonum='{0}',oodate='{1}' where pno={2}",
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
        string type;
        string reportname;

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

        //select report according to type (voluntary or supperannuation)
        type = ds.Tables[0].Rows[0]["reptype"].ToString();
        if (type == "S")
        {
            reportname = "rptret_SuperAnn.rpt";
        }
        else
        {
            reportname = "rptret_Voluntary.rpt";
        }

        string pdfPath;
        if (save)
        {
            pdfPath = Server.MapPath("office_orders\\" + oonum + "-BEG-3" + oodate + ".pdf");
        }
        else
        {
            pdfPath = Server.MapPath("office_orders\\preview_ret-" + propno + "-" + DateTime.Now.ToString("yyyyMMdd-HHmmssfff") + ".pdf");
        }

        CrystalReportSource CrystalReportSource1 = new CrystalReportSource();
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\" + reportname);
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);
        CrystalReportSource1.Dispose();
        DownloadFile(pdfPath);
    }
    private void DownloadFile(String pdfPath)
    {
        System.IO.FileInfo objFi = new System.IO.FileInfo(pdfPath);
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + objFi.Name);
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AddHeader("Content-Length", objFi.Length.ToString());
        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.WriteFile(objFi.FullName);
        HttpContext.Current.Response.End();
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        string empid = txtEmpID.Text;
        string sql = String.Empty;

        sql = "select count(*) from pshr.empperso where empid= " + empid;
        if (string.IsNullOrEmpty(empid) || OraDBConnection.GetScalar(sql) == "0")
        {
            return;
        }

        //panEmpHist.Visible = true;
        sql = "select pshr.get_fullname(empid) || ' (' || pshr.get_desg(cdesgcode) || '), ' || pshr.get_org(cloccode) as Info"+
            " from pshr.empperso where empid = "+empid;
        lblEmpInfo.Text = OraDBConnection.GetScalar(sql);

        //show last 5 rows of emphistory
        sql = "select * from (select eref as Event,to_char(fromdate,'dd-Mon-YYYY') as FromDate,to_char(todate,'dd-Mon-YYYY') as ToDate,"+
            "pshr.get_desg(desgcode) as Desg,pshr.get_org(loccode) as Location from " +
            "pshr.emphistory eh,pshr.mast_event me where eh.eventcode = me.eventcode and empid=" + empid + " order by rowno desc) a " +
            "where rownum < 6";
        OraDBConnection.FillGrid(ref gvEmpHist, sql);

        lblDesgMsg.Text = "";
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string empid = txtEmpID.Text;
        string sql = String.Empty;
        string dor = txtDor.Text;
        DateTime tempDate;
        //panEmpHist.Visible = false;
        //if empid doesn't exist
        //or desg doesn't match
        //or recstatus != 10
        //then show error


        if(System.DateTime.TryParse(dor,out tempDate) == false)
        {
            lblDesgMsg.Text = "Invalid DOR";
            return;
        }
        sql = string.Format("select count(*) from pshr.empperso where empid = {0} and recstatus=10",empid);
        if (OraDBConnection.GetScalar(sql) != "1")
        {
            lblDesgMsg.Text = "Invalid EmpID";
            return;
        }

        sql = string.Format("select count(*) from cadre.ret_list where empid = {0} and propno = {1}", empid, propno);
        if (OraDBConnection.GetScalar(sql) != "0")
        {
            lblDesgMsg.Text = "EmpID Already Added";
            return;
        }
        //sql = String.Format("insert into CADRE.prop_redesig SELECT e.empid, e.firstname  || ' '  || e.middlename  || ' '  || e.lastname AS name, " +
        //    "to_char(e.dob,'dd-Mon-YYYY') as DOB, m.locabb, to_char(h.fromdate,'dd-Mon-YYYY') AS Promoted_On, {1}, {0} FROM " +
        //    "(SELECT empid, MIN(fromdate) AS fromdate FROM emphistory WHERE desgcode = {0} GROUP BY empid) h,  empperso e,  mast_loc m " +
        //    "WHERE e.cdesgcode = {0} AND h.empid = e.empid AND m.loccode = e.cloccode and e.empid={2}", desg, propno, empid);
        sql = "INSERT INTO cadre.ret_list " +
                "SELECT e.empid,'"+ dor +"' as DOR, " + propno + ", cdesgcode,null " +
                "FROM pshr.empperso e, pshr.mast_desg md " +
                "WHERE e.cdesgcode = md.desgcode AND e.empid = " + empid;
        OraDBConnection.ExecQry(sql);
        lblDesgMsg.Text = "EmpID Added";
        FillGrid();
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        string empid = gv_redesig.Rows[((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex].Cells[2].Text.ToString();
        string sql = string.Empty;

        sql = "delete from cadre.ret_list where empid=" + empid + " and propno=" + propno;
        OraDBConnection.ExecQry(sql);

        FillGrid();
    }
}