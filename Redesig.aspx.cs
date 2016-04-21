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

public partial class Redesig_Batch : System.Web.UI.Page
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
        string sql = "select empid as EmpID, name as Name, to_char(dob,'dd-Mon-YYYY') as DOB, locabb as Office,"+
            "to_char(fdate,'dd-Mon-YYYY') as \"Promotion Date\" from cadre.prop_redesig where propno = " + propno + " order by fdate,empid";
        OraDBConnection.FillGrid(ref gv_redesig, sql);
        return gv_redesig.Rows.Count;
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
            Response.Redirect("~/redesig_proposals.aspx.cs");
        }
        if (!IsPostBack)
        {
            //check if entries for this proposal no. exist in DB
            sql = "select to_char(fromdate,'dd-Mon-YYYY') as From_Date,to_char(todate,'dd-Mon-YYYY') as To_Date,olddesg from cadre.rd_proposals where pno = " + propno;
            ds = OraDBConnection.GetData(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                drow = ds.Tables[0].Rows[0];
                drpDesg.SelectedIndex = drpDesg.Items.IndexOf(drpDesg.Items.FindByValue(drow["olddesg"].ToString()));
                dt_start.Text = drow["From_Date"].ToString();
                dt_end.Text = drow["To_Date"].ToString();
                Search_Visible(false);
                Controls_Vis(true);
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

        //reset fromdate,todate and olddesg
        sql = "update cadre.rd_proposals set fromdate = null, todate = null, olddesg=null";
        OraDBConnection.ExecQry(sql);

        //delete earlier entered records (if any) for this proposal
        sql = "delete from cadre.prop_redesig where propno = " + propno;
        OraDBConnection.ExecQry(sql);

        //insert matched records in prop_redesig
        sql = String.Format("insert into CADRE.prop_redesig SELECT e.empid, e.firstname  || ' '  || e.middlename  || ' '  || e.lastname AS name, "+
            "to_char(e.dob,'dd-Mon-YYYY') as DOB, m.locabb, to_char(h.fromdate,'dd-Mon-YYYY') AS Promoted_On, {3}, {0} FROM " +
            "(SELECT empid, MIN(fromdate) AS fromdate FROM emphistory WHERE desgcode = {0} GROUP BY empid) h,  empperso e,  mast_loc m "+
            "WHERE e.cdesgcode = {0} AND e.recstatus = 10 AND h.empid = e.empid AND m.loccode = e.cloccode "+
            "AND h.fromdate BETWEEN  to_date('{1}','dd/mm/yyyy') AND to_date('{2}','dd/mm/yyyy')", desg, startdt, enddt, propno);
        OraDBConnection.ExecQry(sql);

        //enter fromdate, todate and old desg in rd_proposals
        sql = string.Format("update cadre.rd_proposals set fromdate =  to_date('{0}','dd/mm/yyyy'),"+
                                "todate = to_date('{1}','dd/mm/yyyy'), olddesg={2} where pno={3}",
                                startdt, enddt, desg, propno);
        OraDBConnection.ExecQry(sql);

        FillGrid();
        Search_Visible(false);
        Controls_Vis(gv_redesig.Rows.Count > 0);
    }
    private bool Save()
    {
        string sql = string.Empty;
        string empid = string.Empty;
        string newdesg = string.Empty;  
        string oonum = txtoonum.Text;
        string oodate = txtoodate.Text;
        string olddesg = string.Empty;
        
        System.Data.DataRow prow;
        sql = "select fromdate,todate,olddesg from cadre.rd_proposals where pno = " + propno;
        prow = OraDBConnection.GetData(sql).Tables[0].Rows[0];
        if (prow["fromdate"] != DBNull.Value)
        {

        }
        if (prow["fromdate"] == DBNull.Value ||
            prow["todate"] == DBNull.Value   ||
            prow["olddesg"] == DBNull.Value)
        {
            Utils.ShowMessageBox(this,"Error in proposal");
            return false;
        }
        
        olddesg = prow["olddesg"].ToString();
        //Sr.XEN(9052) --> AddSE(9366)
        //SE(9050)     --> Dy.CE(9365)
        newdesg = (olddesg == "9052") ? "9366" : "9365";

        sql = "select * from cadre.prop_redesig where propno = " + propno;
        foreach (DataRow drow in OraDBConnection.GetData(sql).Tables[0].Rows)
        {
            empid = drow["empid"].ToString();
            
            sql = string.Format("insert into pshr.emphistory(empid, eventcode,fromdate,todate,desgcode,loccode,rowno,holdingpost,eventhistoryid,status,pcloccode,sancdesg,sancindx,oonum,odate) values "+
                                                            "({0},  {1},        '{2}',  '{3}',    {4},    {5},    {6},    '{7}',        {8},      {9},    {10},   {11},   {12},   '{13}','{14}')",
                  empid,    //0
                  38,       //1, 38 = Redesig
                  oodate,   //2
                  oodate,   //3
                  newdesg,  //4
                  99999,    //5
                  "(select max(rowno)+1 from emphistory where empid = " + empid+")",    //6
                  "Regular Event",  //7
                  "pshr.evt_id.nextval",    //8
                  1,        //9
                  99999,    //10
                  8888,     //11
                  0,        //12
                  oonum,    //13
                  oodate    //14
                  );
            if (OraDBConnection.ExecQry(sql) == false)
            {
                Utils.ShowMessageBox(this, "Error Adding New Row in Emphistory for Empid " + empid);
                return false;
            }

            sql = "update pshr.empperso set cdesgcode = " + newdesg + " where empid = " + empid;
            if (OraDBConnection.ExecQry(sql) == false)
            {
                Utils.ShowMessageBox(this, "Error Updating Empperso for Empid " + empid);
                return false;
            }
        }
        return true;
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
        if (Save() == false)
        {
            Utils.ShowMessageBox(this, "Error Saving Proposal");
            return;
        }
        //mark proposal as saved
        string sql = string.Format("update cadre.rd_proposals set status='S',oonum='{0}',oodate=to_date('{1}','dd/mm/yyyy') where pno={2}",
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

        if (save)
        {
            oonum = txtoonum.Text;
            oodate = txtoodate.Text;
        }
        string sql = "select * from cadre.prop_redesig where propno = " + propno + " order by fdate,empid";

        OraDBConnection oraCn = new OraDBConnection();
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        string pdfPath;
        if (save)
        {
            pdfPath = Server.MapPath("office_orders\\" + oonum + "-BEG-I" + oodate + ".pdf");
        }
        else
        {
            pdfPath = Server.MapPath("office_orders\\preview_rdg-" + propno + "-" + DateTime.Now.ToString("yyyyMMdd-HHmmssfff") + ".pdf");
        }

        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rptredesig.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);
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
    protected void btnShow_Click(object sender, EventArgs e)
    {
        const string STRSHOW = "Show";
        const string STRHIDE = "Hide";
        string empid = txtEmpID.Text;
        string sql = String.Empty;
        
        panEmpHist.Visible = (btnShow.Text == STRSHOW);
        btnShow.Text = (btnShow.Text == STRSHOW) ? STRHIDE : STRSHOW;

        sql = "select pshr.get_fullname(empid) || ' (' || pshr.get_desg(cdesgcode) || '), ' || pshr.get_org(cloccode) as Info" +
            " from pshr.empperso where empid = '" + empid + "'";
        lblEmpInfo.Text = OraDBConnection.GetScalar(sql);

        if (lblEmpInfo.Text == "")
        {
            lblDesgMsg.Text = "Invalid Empid";
            panEmpHist.Visible = false;
            btnShow.Text = STRSHOW;
            return;
        }   
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
        string desg = drpDesg.SelectedValue;
        string sql = String.Empty;
        string res;

        panEmpHist.Visible = false;
        //show error if empid doesn't exist or desg doesn't match or recstatus != 10
        sql = string.Format("select count(*) from pshr.empperso where empid = '{0}' and cdesgcode = {1} and recstatus=10",empid,desg);
        res = OraDBConnection.GetScalar(sql);
        if (empid=="" || res != "1")
        {
            lblDesgMsg.Text = "Invalid EmpID";
            return;
        }

        sql = string.Format("select count(*) from cadre.prop_redesig where empid = {0} and propno = {1}", empid, propno);
        if (OraDBConnection.GetScalar(sql) != "0")
        {
            lblDesgMsg.Text = "EmpID Already Added";
            return;
        }
        sql = String.Format("insert into CADRE.prop_redesig SELECT e.empid, e.firstname  || ' '  || e.middlename  || ' '  || e.lastname AS name, " +
            "to_char(e.dob,'dd-Mon-YYYY') as DOB, m.locabb, to_char(h.fromdate,'dd-Mon-YYYY') AS Promoted_On, {1}, {0} FROM " +
            "(SELECT empid, MIN(fromdate) AS fromdate FROM emphistory WHERE desgcode = {0} GROUP BY empid) h,  empperso e,  mast_loc m " +
            "WHERE e.cdesgcode = {0} AND h.empid = e.empid AND m.loccode = e.cloccode and e.empid={2}", desg, propno, empid);
        OraDBConnection.ExecQry(sql);
        lblDesgMsg.Text = "EmpID Added";
        FillGrid();
    }
    protected void gv_redesig_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string empid = gv_redesig.Rows[e.RowIndex].Cells[1].Text.ToString();
        string sql = string.Empty;

        sql = "delete from cadre.prop_redesig where empid=" + empid+" and propno="+propno;
        OraDBConnection.ExecQry(sql);

        FillGrid();
    }
}