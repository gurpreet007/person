using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.Shared;
public partial class pplist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillDesg();
        }
    }
    private void FillDesg()
    {
        string sql = "select desgcode, desgabb from pshr.mast_desg where adesg='A' order by desgabb";
        DataSet ds = OraDBConnection.GetData(sql);
        drpDesg.DataSource = ds.Tables[0];
        drpDesg.DataTextField = "desgabb";
        drpDesg.DataValueField = "desgcode";
        drpDesg.DataBind();
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        string desg = drpDesg.SelectedValue;
        if (desg == "9048" || desg == "9047")
        {
            desg = "9047,9048";
        }
        else if (desg == "9050" || desg == "9365")
        {
            desg = "9050,9365";
        }
        else if (desg == "9052" || desg == "9366" || desg=="9477")
        {
            //ADDL SE, SRXEN AND SSM
            desg = "9052,9366,9477";
        }

        string sql = "SELECT h.empid," +
            "p.firstname || ' ' || p.middlename || ' ' || p.lastname as FullName, " +
            "mc.categtext as cat_text, " +
            "p.dob, " +
            "h.todate," +
            "md.desgabb, " +
            "p.cloccode," +
            "p.cdesgcode, " +
            "pshr.get_org(pshr.get_chief(ml.loccode)) as pzone, " +
            "ml.post AS posting, " +
            "h.pcloccode, " +
            "pshr.get_post(h.pcloccode) AS paychrg, " +
            "pshr.get_desg(h.sancdesg) AS sancdesg," +
            "h.sancindx, " +
            "h.oonum, " +
            "h.odate, " +
            "md.hecode AS hecode, " +
            "p.branchcode, " +
            "p.seniorityno as sen, " +
            "mb.branchtext " +
            "FROM pshr.empperso p, emphistory h, mast_categ mc, mast_desg md, mast_loc ml, mast_branch mb " +
            "where h.empid = p.empid " +
            "AND mc.categcode = p.soccategory " +
            "and md.desgcode = p.cdesgcode " +
            "and ml.loccode = p.cloccode " +
            "and mb.branchcode = p.branchcode " +
            "AND h.rowno = (SELECT MAX(rowno) FROM pshr.emphistory WHERE empid = p.empid  AND oonum IS NOT NULL) " +
            "AND p.recstatus = 10 " +
            "AND p.cdesgcode in (" + desg + ")" +
            "ORDER BY hecode, " +  (chkBySen.Checked ? "seniorityno" : "empid");

        OraDBConnection oraCn = new OraDBConnection();
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        //get sysdate
        string sysdate = OraDBConnection.GetScalar("select to_char(sysdate,'dd-mm-yyyy') from dual");
        string pdfPath = Server.MapPath("office_orders\\" + "pplist-" + sysdate + ".pdf");

        //save office order at server
        CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rptpplist.rpt");
        CrystalReportSource1.ReportDocument.SetDatabaseLogon("pshr", "pshr");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);
        Utils.DownloadFile(pdfPath);
    }
}