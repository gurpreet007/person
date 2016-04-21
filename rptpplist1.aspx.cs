using System;
using System.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;

public partial class rptpplist1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "select h.empid, pshr.get_fullname(h.empid),pshr.get_categtxt(soccategory),p.dob,pshr.get_dojp(h.empid), pshr.get_desg(p.cdesgcode), p.cloccode, " +
                     "pshr.get_org(p.cloccode) as posting,h.pcloccode,pshr.get_org(h.pcloccode) as paychrg,pshr.get_desg(h.sancdesg) as sancdesg,h.sancindx, h.oonum, " +
                     " h.odate,cadre.get_hecode(p.cdesgcode) as hecode,p.branchcode,pshr.get_branch(p.empid) " +
                     "from pshr.empperso p, emphistory h where p.empid = h.empid and " +
                     "h.empid = 102430 and " +
                     "h.rowno = (select max(rowno) from pshr.emphistory where empid = p.empid and oonum is not null) order by hecode,p.empid ";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        CrystalReportSource CrystalReportSource1 = new CrystalReportSource();
        CrystalReportSource1.Report.FileName = Server.MapPath("rptpplist.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);

        string pdfPath = Server.MapPath("a.pdf");
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);

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