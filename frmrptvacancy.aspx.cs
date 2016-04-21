using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class frmrptvacancy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "";
        string file_name = "";
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
        string loctype = Session.Contents["loctype"].ToString();
        string sloc = Session.Contents["sloc"].ToString();
        string fname = "";

       
        //get sysdate
        string sysdate = OraDBConnection.GetScalar("select to_char(sysdate,'dd-mm-yyyy') from dual");
        string g;
        if (loctype == "Zone")
        {
            g = "pshr.get_zone(loccode)";
        }
        else if (loctype == "Circle")
        {
            g = "pshr.get_circle(loccode)";
        }
        else
        {
            g = "pshr.get_org(loccode)";
        }

        if ((desg != "0") && (org != "0"))
        {
            if (sloc == "0")
            {

             sql = " select " + g + " as office,pshr.get_desg(desgcode) as Designation ,sum(sanc) as sanctioned,sum(post) as posted  from (select a.loccode as loccode ,a.desgcode as desgcode, nvl(a.sanc,0) as sanc,nvl(b.post,0) as post from" +
             " (select desgcode,loccode,rowno, count(*) as sanc from cadre.cadr where loccode = " + org + "and desgcode in (" + desg + ")" +
             ((branch != "0") ? " and branch  =  " + branch : "") +
            "   group by loccode,desgcode,rowno) a, " +
             "(select desgcode,loccode, rowno, count(*) as post from cadre.cadr where loccode = " + org + "and desgcode in (" + desg + ") and rowno in (select rowno from cadre.cadrmap)" +
             ((branch != "0") ? " and branch  =  " + branch : "") +
             " group by loccode,desgcode,rowno) b where a.rowno = b.rowno(+) ) d" +
             " group by " + g  + " ,pshr.get_desg(desgcode)";
            }
            else            
            {
                sql = " select " + g + " as office ,pshr.get_desg(desgcode) as Designation ,sum(sanc) as sanctioned,sum(post) as posted from (select a.loccode as loccode ,a.desgcode as desgcode, nvl(a.sanc,0) as sanc,nvl(b.post,0) as post from" +
             " (select desgcode,loccode,rowno, count(*) as sanc from cadre.cadr where loccode  in (select loccode from pshr.mast_loc start with loccode  = " + org + " connect by prior loccode = locrep) and desgcode in (" + desg + ") " +
             ((branch != "0") ? " and branch  =  " + branch : "") +
             " group by desgcode,loccode,rowno)a, " +
             "(select desgcode,loccode, rowno, count(*) as post from cadre.cadr where loccode  in (select loccode from pshr.mast_loc start with loccode  = " + org + " connect by prior loccode = locrep) and desgcode in (" + desg + ") and rowno in (select rowno from cadre.cadrmap)" +
             ((branch != "0") ? " and branch  =  " + branch : "") +
             " group by desgcode,loccode,rowno) b where a.rowno = b.rowno(+)) d" +
             " group by " + g + " ,pshr.get_desg(desgcode)";
            }

            file_name = "Reports\\rptdesgvacancy.rpt";
            fname = "vdesgdet";
        }
        else if ((desg != "0") && (org == "0"))
        {

            sql = "select " + g + " as office ,sum(sanc) as sanctioned,sum(post) as posted from (select a.loccode as loccode, nvl(a.sanc,0) as sanc,nvl(b.post,0) as post from" +
            " (select loccode,rowno, count(*) as sanc from cadre.cadr where desgcode in (" + desg + ")" +
            ((branch != "0") ? " and branch  =  " + branch : "") +
            " group by loccode,rowno)a, " +
            "(select loccode, rowno, count(*) as post from cadre.cadr where desgcode in (" + desg + ") and rowno in (select rowno from cadre.cadrmap)" +
            ((branch != "0") ? " and branch  =  " + branch : "") +
            "group by loccode,rowno) b where a.rowno = b.rowno(+) and a.loccode not like '608%') d" +
            " group by " + g ;
            
            file_name = "Reports\\rptdesgvacancy.rpt";
            fname = "vdesgdet";
        }

        else if ((desg == "0") && (org != "0"))
        {

            if (sloc == "0")
            {
                sql = " select pshr.get_org(" + org + ") as office,desgcode,pshr.get_desg(desgcode) as designation ,sum(sanc) as sanctioned,sum(post) as posted from (select a.loccode as loccode, a.desgcode as desgcode, nvl(a.sanc,0) as sanc,nvl(b.post,0) as post from" +
                      " (select loccode, desgcode,rowno, count(*) as sanc from cadre.cadr where loccode   = " + org +
                      ((branch != "0") ? " and branch  =  " + branch : "") +
                      "   group by loccode, desgcode,rowno)a, " +
                      "(select loccode, desgcode,rowno, count(*) as post from cadre.cadr where loccode    = " + org + " and rowno in (select rowno from cadre.cadrmap)" +
                      ((branch != "0") ? " and branch  =  " + branch : "") +
                      "group by loccode, desgcode,rowno) b where a.rowno = b.rowno(+) and a.loccode not like '608%') d" +
                      " group by desgcode order by cadre.get_hecode(desgcode)";
            }
            else
            {
                sql = " select pshr.get_org(" + org + ") as office,desgcode,pshr.get_desg(desgcode) as designation ,sum(sanc) as sanctioned,sum(post) as posted from (select a.loccode as loccode, a.desgcode as desgcode, nvl(a.sanc,0) as sanc,nvl(b.post,0) as post from" +
                  " (select loccode, desgcode,rowno, count(*) as sanc from cadre.cadr where loccode  in (select loccode from pshr.mast_loc start with loccode  = " + org + " connect by prior loccode = locrep)" +
                  ((branch != "0") ? " and branch  =  " + branch : "") +
                  "group by loccode, desgcode,rowno)a, " +
                  "(select loccode, desgcode,rowno, count(*) as post from cadre.cadr where loccode   in (select loccode from pshr.mast_loc start with loccode  = " + org + "connect by prior loccode = locrep) and rowno in (select rowno from cadre.cadrmap)" +
                  ((branch != "0") ? " and branch  =  " + branch : "") +
                  "group by loccode, desgcode,rowno) b where a.rowno = b.rowno(+) and a.loccode not like '608%') d" +
                  " group by desgcode order by cadre.get_hecode(desgcode)";
            }
            file_name = "Reports\\rptlocvacancy.rpt";
            fname = "vlocdet";

        }
        OraDBConnection oraCn = new OraDBConnection();
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        string pdfPath = Server.MapPath("office_orders\\" + fname + sysdate + "-" + org + ".pdf");

        //save Report at server
        CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
        CrystalReportSource1.Report.FileName = Server.MapPath(file_name);
        CrystalReportSource1.ReportDocument.SetDatabaseLogon("pshr", "123");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);
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