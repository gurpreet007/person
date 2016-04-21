using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class frmvacancy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Fillorg();
            Filldesg();
        }
    }
    protected void drporg_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Filldesg();
        drpdesg.SelectedIndex = 0;
        
    }
    private void Fillorg()
    {
        string sql = "SELECT locabb ,loccode FROM pshr.mast_loc where loccode like '___000000' and aloc  = 1 and loccode not like '608%' order by locabb";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        drporg.Items.Clear();

        if (ds.Tables[0].Rows.Count > 0)
        {
            drporg.Items.Add(new ListItem("--Select--", "0"));
            foreach (System.Data.DataRow row in ds.Tables[0].Rows)
            {
                drporg.Items.Add(new ListItem(row["locabb"].ToString(), row["loccode"].ToString()));
            }
        }
    }

    private void Filldesg()
    {
        if (drporg.SelectedValue != "0")
        {
            string loc;
            loc = drporg.SelectedValue;
            loc = loc.Substring(0,3);

            string sql = "SELECT distinct(desgcode),pshr.get_desg(desgcode) as desgabb  FROM cadre.cadr where loccode like '" + loc + "%' and desgcode in (select desgcode from pshr.mast_desg where servcode = 30 and gazcode = 10 )";
            System.Data.DataSet ds = OraDBConnection.GetData(sql);
            drpdesg.Items.Clear();

            if (ds.Tables[0].Rows.Count > 0)
            {
                drpdesg.Items.Add(new ListItem("--Select--", "0"));
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    drpdesg.Items.Add(new ListItem(row["desgabb"].ToString(), row["desgcode"].ToString()));
                }
            }
      }
        else 
        {
            string sql = "SELECT desgcode,pshr.get_desg(desgcode) as desgabb  FROM pshr.mast_desg where servcode  =30 and gazcode  = 10 order by desgcode";
            System.Data.DataSet ds = OraDBConnection.GetData(sql);
            drpdesg.Items.Clear();

            if (ds.Tables[0].Rows.Count > 0)
            {
                drpdesg.Items.Add(new ListItem("--Select--", "0"));
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    drpdesg.Items.Add(new ListItem(row["desgabb"].ToString(), row["desgcode"].ToString()));
                }
            }
        }
    }



    protected void btnshow_Click(object sender, EventArgs e)
    {
        string sql = "";
        string file_name = "";
        string desg = drpdesg.SelectedValue.ToString();
        string org = drporg.SelectedValue.ToString();
        string fname = "";

        if (desg == "9050" || desg == "9365")
        {

            desg = "9050,9365";
        }
        
        //get sysdate
        string sysdate = OraDBConnection.GetScalar("select to_char(sysdate,'dd-mm-yyyy') from dual");
        
        if (org != "0")
        {
            
            //sql = " select loccode,pshr.get_org(loccode) as Office,desgcode,pshr.get_desg(desgcode) as designation ,sum(sanc) as sanctioned,sum(post) as posted from (select a.loccode as loccode, a.desgcode as desgcode, nvl(a.sanc,0) as sanc,nvl(b.post,0) as post from" +
            //      " (select loccode, desgcode,rowno, count(*) as sanc from cadre.cadr where loccode  in (select loccode from pshr.mast_loc start with loccode  = " + org + " connect by prior loccode = locrep)   group by loccode, desgcode,rowno)a, " +
            //      "(select loccode, desgcode,rowno, count(*) as post from cadre.cadr where loccode   in (select loccode from pshr.mast_loc start with loccode  = "+ org + "connect by prior loccode = locrep) and rowno in (select rowno from cadre.cadrmap)group by loccode, desgcode,rowno) b where a.rowno = b.rowno(+) and a.loccode not like '608%') d" +
            //      " group by loccode,desgcode order by loccode";

            sql = " select pshr.get_org("+ org + ") as office,desgcode,pshr.get_desg(desgcode) as designation ,"+
                    "sum(sanc) as sanctioned,sum(post) as posted from (select a.loccode as loccode, a.desgcode as desgcode, "+
                    "nvl(a.sanc,0) as sanc,nvl(b.post,0) as post from" +
                  " (select loccode, desgcode,rowno, count(*) as sanc from cadre.cadr where loccode  in"+
                  " (select loccode from pshr.mast_loc start with loccode  = " + org + " connect by prior loccode = locrep) "+
                  "  group by loccode, desgcode,rowno)a, " +
                  "(select loccode, desgcode,rowno, count(*) as post from cadre.cadr where loccode   in "+
                  "(select loccode from pshr.mast_loc start with loccode  = " + org + "connect by prior loccode = locrep) and "+
                  "rowno in (select rowno from cadre.cadrmap)group by loccode, desgcode,rowno) b where a.rowno = b.rowno(+) and a.loccode not like '608%') d" +
                  " group by desgcode order by cadre.get_hecode(desgcode)";

            file_name = "Reports\\rptlocvacancy.rpt";
            fname = "vlocdet";
        }

        else if (desg != "0")
        {
            sql = " select loccode,pshr.get_org(loccode) as Office,desgcode,pshr.get_desg(desgcode) as designation ,sum(sanc) as sanctioned,sum(post) as posted from (select a.loccode as loccode, a.desgcode as desgcode, nvl(a.sanc,0) as sanc,nvl(b.post,0) as post from" +
                  " (select loccode, desgcode,rowno, count(*) as sanc from cadre.cadr where desgcode in (" + desg + ") group by loccode, desgcode,rowno)a, " +
                  "(select loccode, desgcode,rowno, count(*) as post from cadre.cadr where desgcode in (" + desg + ") and rowno in (select rowno from cadre.cadrmap)group by loccode, desgcode,rowno) b where a.rowno = b.rowno(+) and a.loccode not like '608%') d" +
                  " group by loccode,desgcode order by loccode";

            file_name = "Reports\\rptdesgvacancy.rpt";
            fname = "vdesgdet";
        }

        else
        {
            return;
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

    public override void VerifyRenderingInServerForm(Control Control)
    {
        //Confirms that an HtmlForm control is rendered for the 
        //specified ASP.NET server control at run time. 
        //No code required here. 
    }
    protected void drpdesg_SelectedIndexChanged(object sender, EventArgs e)
    {
        drporg.SelectedIndex = 0;
        
    }
}