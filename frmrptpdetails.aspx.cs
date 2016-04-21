using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class frmrptpdetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string empid = Session.Contents["empid"].ToString();

        string desg = Session.Contents["desg"].ToString();

        if ((Session.Contents["desg"].ToString() == "9048") || (Session.Contents["desg"].ToString() == "9047"))
        {
             desg = "9047,9048";
        }
        else if ((Session.Contents["desg"].ToString() == "9050") || (Session.Contents["desg"].ToString() == "9365"))
        {
             desg = "9050,9365";
        }
        else if ((Session.Contents["desg"].ToString() == "9052") || (Session.Contents["desg"].ToString() == "9366"))
        {
             desg = "9052,9366";
        }
            
        


        string org = Session.Contents["org"].ToString();
        string branch = Session.Contents["branch"].ToString();
        string categ = Session.Contents["categ"].ToString();

        string sql = "select e.empid, pshr.get_fullname(e.empid), e.fathername, e.dob,e.doj, d.desgtext, pshr.get_org(e.cloccode), e.gpfno,e.panno " +
            "from pshr.empperso e,pshr.mast_desg d where e.recstatus = 10 and e.cdesgcode = d.desgcode " +
            ((empid != "") ? " and e.empid in( " + empid + ")" : "") +
            ((desg != "0") ? " and e.cdesgcode  in(  " + desg + ")" : "") +
            ((org != "0") ? " and e.cloccode  =  " + org : "") +
            ((branch != "0") ? " and e.branchcode  =  " + branch : "") +
            ((categ != "0") ? " and e.soccategory  =  " + categ  : "") +
            " order by d.hecode,e.empid ";
        
        OraDBConnection oraCn = new OraDBConnection();
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        //get sysdate
        string sysdate = OraDBConnection.GetScalar("select to_char(sysdate,'dd-mm-yyyy') from dual");
        string pdfPath = Server.MapPath("office_orders\\" + "pdetail-" + empid + ".pdf");

        //save office order at server
        CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rptpdetails.rpt");
        CrystalReportSource1.ReportDocument.SetDatabaseLogon("pshr", "123");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();

        string sql2 = "select h.empid,e.event,h.todate, h.fromdate,pshr.get_desg(h.desgcode), pshr.get_org(h.loccode)" +
               " from pshr.empperso p , pshr.emphistory h, pshr.mast_event e" +
                " where p.recstatus = 10 and h.eventcode = e.eventcode and p.empid = h.empid ";

        if (empid != "")
        {
            sql2 = sql2 + " and p.empid in( " + empid + ")";
        }

        if (desg != "0")
        {
            sql2 = sql2 + " and p.cdesgcode  in ( " + desg + ")";
        }

        if (org != "0")
        {
            sql2 = sql2 + " and p.cloccode  =  " + org;
        }

        if (branch != "0")
        {
            sql2 = sql2 + " and p.branchcode  =  " + branch;
        }
        if (categ != "0")
        {
            sql2 = sql2 + " and p.soccategory  =  " + categ;
        }

        sql2 = sql2 + " order by p.empid,h.rowno ";
        System.Data.DataSet ds3 = OraDBConnection.GetData(sql2);
        CrystalReportSource1.ReportDocument.Subreports["emphist"].SetDataSource(ds3.Tables[0]);
        CrystalReportSource1.DataBind();
        
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat,pdfPath);
        DownloadFile(pdfPath);
    }
    void DownloadFile(String pdfPath)
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
}