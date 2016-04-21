using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class frmrptpplist : System.Web.UI.Page
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
            //ADDL SE, SRXEN AND SSM
            desg = "9052,9366,9477";
        }
        
        string org = Session.Contents["org"].ToString();
        string branch = Session.Contents["branch"].ToString();
        string categ = Session.Contents["categ"].ToString();

        string sql = "SELECT h.empid,"+
            "p.firstname || ' ' || p.middlename || ' ' || p.lastname as FullName, "+
            "mc.categtext as cat_text, "+
            "p.dob, "+
            "h.todate,"+
            "md.desgabb, "+
            "p.cloccode,"+
            "p.cdesgcode, "+
            //"(SELECT locabb FROM MAST_LOC WHERE loccode = concat(substr(ml.loccode,1,3),'000000')) as pzone, "+
            "pshr.get_org(pshr.get_chief(ml.loccode)) as pzone, "+
            "ml.post AS posting, "+
            "h.pcloccode, "+
            "pshr.get_post(h.pcloccode) AS paychrg, "+
            "pshr.get_desg(h.sancdesg) AS sancdesg,"+
            "h.sancindx, "+
            "h.oonum, "+
            "h.odate, "+
            "md.hecode AS hecode, "+
            "p.branchcode, "+
            "mb.branchtext " +
            "FROM pshr.empperso p, emphistory h, mast_categ mc, mast_desg md, mast_loc ml, mast_branch mb "+
            "where h.empid = p.empid "+
            "AND mc.categcode = p.soccategory "+
            "and md.desgcode = p.cdesgcode "+
            "and ml.loccode = p.cloccode "+
            "and mb.branchcode = p.branchcode "+
            "AND h.rowno = (SELECT MAX(rowno) FROM pshr.emphistory WHERE empid = p.empid  AND oonum IS NOT NULL) "+
            "AND p.recstatus = 10 "+
            ((empid != "") ? " and p.empid in (" + empid + ")" : "") +
            ((desg != "0") ? " and p.cdesgcode in (" + desg + ")" : "") +
            ((org != "0") ? " and p.cloccode  in (select loccode from pshr.mast_loc start with  loccode =  " + org + " connect by prior loccode  = locrep) " : "") +
            ((branch != "0") ? " and p.branchcode  =  " + branch : "") +
            ((categ != "0") ? " and p.soccategory  =  " + categ : "") +
            "ORDER BY hecode, p.empid ";
        OraDBConnection oraCn = new OraDBConnection();
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        //foreach (System.Data.DataRow drow in ds.Tables[0].Rows)
        //{
        //    sql = "select nvl(pshr.get_dojp("+drow["e1"]+"),'') from dual";
        //    System.Data.DataSet ds1 = OraDBConnection.GetData(sql);
        //}

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