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
        //string empid = 102972;
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

        string sql = "select p.photo2, a.* from ( " +
            "select e.empid, pshr.get_fullname(e.empid), e.fathername, e.dob,e.doj, d.desgtext, pshr.get_org(e.cloccode), e.gpfno,e.panno " +
            "from pshr.empperso e,pshr.mast_desg d where e.cdesgcode = d.desgcode " +
            ((empid != "") ? " and e.empid in( " + empid + ")" : "") +
            ((desg != "0") ? " and e.cdesgcode  in(  " + desg + ")" : "") +
            ((org != "0") ? " and e.cloccode  =  " + org : "") +
            ((branch != "0") ? " and e.branchcode  =  " + branch : "") +
            ((categ != "0") ? " and e.soccategory  =  " + categ  : "") +
            " order by d.hecode,e.empid) a " +
            " left outer join IMG_PSHR.img p on p.empid = a.empid ";
        
        OraDBConnection oraCn = new OraDBConnection();
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        //get sysdate
        string sysdate = OraDBConnection.GetScalar("select to_char(sysdate,'dd-mm-yyyy') from dual");
        string pdfPath = Server.MapPath("office_orders\\" + "pdetail-" + empid + ".pdf");

        //save office order at server
        CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\panelreport.rpt");
        CrystalReportSource1.ReportDocument.SetDatabaseLogon("pshr", "123");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        //CrystalReportSource1.DataBind();

        //CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);
        //DownloadFile(pdfPath);


        string sql2 = "select h.empid,e.event,h.todate, h.fromdate,pshr.get_desg(h.desgcode), pshr.get_org(h.loccode)" +
               " from pshr.empperso p , pshr.emphistory h, pshr.mast_event e" +
                " where h.eventcode = e.eventcode and p.empid = h.empid ";

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
        //CrystalReportSource1.DataBind();
        
        string sql4 = "select d.empid, d.fileno ||' / '|| dcp.get_section(d0.section) sec, " +
                "dcp.get_dfttype(d0.DFTTYPECODE) dft, dcp.get_chgmemo(d.dcpno) chg, " +
                "d0.charges, decode (s.STATCODE, 1, dcp.get_casestatus_c(d.dcpno), " +
                "    2, dcp.get_decsub(d.dcpno,7)  ||' , Case Decided', " +
                "    3, dcp.get_casestatus_c(d.dcpno), " +
                "    4, dcp.get_decsub(d.dcpno,8)||', Case Decided'," +
                "    5, dcp.get_casestatus_c(d.dcpno)," +
                "    6, dcp.get_decsub(d.dcpno,9) ||', Case Decided'," +
                "    7, dcp.get_casestatus_c(d.dcpno)," +
                "    8, dcp.get_casestatus_c(d.dcpno)," +
                "    9, dcp.get_casestatus_c(d.dcpno)," +
                "    10,dcp.get_decsub(d.dcpno,10)||' , Case Decided') dec " +
                " from dcp.dcpno d,dcp.dcp0 d0, " +
                " dcp.dcpstatus s "+
                //", dcp.prom_id p " +
                " where d.dcpno = d0.dcpno and d.dcpno = s.dcpno "+
                //" and d.empid = p.empid "+
                " and d.empid in( " + empid + ")" +
                "order by s.statcode, d.fileno";
        System.Data.DataSet ds4 = OraDBConnection.GetData(sql4);
        CrystalReportSource1.ReportDocument.Subreports["dcases"].SetDataSource(ds4.Tables[0]);
        //CrystalReportSource1.DataBind();

        string sql5 = "select acr.empid, acr.fromdate, acr.todate, acr.apprscore, int.inttext, stat.statustext " +
                        "from ACR.acr_history_12 acr, PSHR.mast_integrity int, PSHR.mast_acrstatus stat " +
                        "where acr.apprintcode = int.intcode and acr.apprstatcode = stat.statuscode " +
                        "and acr.empid = " + empid +
                        //+ " and acr.fromdate >= add_months(sysdate,-(5*12))"
                        " order by acr.todate";
        System.Data.DataSet ds5 = OraDBConnection.GetData(sql5);
        CrystalReportSource1.ReportDocument.Subreports["acr"].SetDataSource(ds5.Tables[0]);
        CrystalReportSource1.DataBind();

        string sql6 = "select e.empid, e.passdate, p.testname, e.oonum from empdtest e, " +
                        "mast_depttest p where e.testcode = p.testcode and e.empid = " + empid + 
                        " order by e.rowno";
        System.Data.DataSet ds6 = OraDBConnection.GetData(sql6);
        CrystalReportSource1.ReportDocument.Subreports["dae"].SetDataSource(ds6.Tables[0]);
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