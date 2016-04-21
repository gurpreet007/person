using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class frmrptmisc : System.Web.UI.Page
{
    private enum Choices
    {
        NonCompliance = 1,
        OnLeave,
        OnDeputation,
        RelievingDateEmpty
    };
    protected void btnShowData_Click(object sender, EventArgs e)
    {
        string sql = "";

        if (drpotp.SelectedValue == "1")
        {
            sql = "SELECT e.empid as Empid, pshr.get_fullname(e.empid) as Name,pshr.get_desg(e.cdesgcode) as Desg, "+
                  "pshr.get_org(e.cloccode) as Loc FROM pshr.empperso e, pshr.emphistory h where e.recstatus = 10 and e.empid = h.empid " +
                  "and h.todate is null and h.oonum is not null and e.cloccode != 999999999 and h.eventcode not in (select eventcode from pshr.mast_event where egrp = 'LEAVE') ";
        }
        else if (drpotp.SelectedValue == "2")
        {
            //sql = "SELECT e.empid as Empid, pshr.get_fullname(e.empid) as Name, pshr.get_desg(e.cdesgcode) as Desg, "+
            //      "pshr.get_org(e.cloccode) as Loc FROM pshr.empperso e where e.cloccode = 999999999 order by e.cloccode";
            sql = "SELECT e.empid as Empid, pshr.get_fullname(e.empid) as Name, pshr.get_desg(e.cdesgcode) as Desg, " +
                  "pshr.get_org(e.cloccode) as Loc , h.fromdate, h.todate FROM pshr.empperso e,pshr.emphistory h where e.empid = h.empid and e.recstatus = 10 and h.rowno = select rowno from emphistory where empid  = e.empid)" +
                  "and h.eventcode in (select eventcode from mast_event where EGRP = 'LEAVE')    ";

        }
        else if (drpotp.SelectedValue == "3")
        {
            sql = "SELECT e.empid as Empid, pshr.get_fullname(e.empid) as Name, pshr.get_desg(e.cdesgcode) as Desg, "+
                  "pshr.get_org(e.cloccode) as Loc FROM pshr.empperso e,pshr.mast_loc l where e.recstatus  =10 and e.cloccode = l.loccode and l.orgcode in (45,46) order by l.loccode";
        }
        else if (drpotp.SelectedValue == "4")
        {
            sql = "SELECT e.empid as Empid, pshr.get_fullname(e.empid) as Name, pshr.get_desg(e.cdesgcode) as Desg, "+
                  "pshr.get_org(e.cloccode) as Loc FROM pshr.empperso e, pshr.emphistory h where e.recstatus =  10 and e.empid = h.empid " +
                  "and h.fromdate is null and h.oonum is not null";
        }
        else if (drpotp.SelectedValue == "5")
        {
            sql = "SELECT e.empid as Empid, pshr.get_fullname(e.empid) as Name, pshr.get_desg(e.cdesgcode) as Desg, " +
                  "pshr.get_org(e.cloccode) as Loc FROM pshr.empperso e, pshr.emphistory h where e.recstatus = 10 and e.empid = h.empid " +
                  "and h.eventcode = 17 and h.rowno = (select max(rowno) from pshr.emphistory where empid = h.empid) ";
        }

        OraDBConnection.FillGrid(ref gvData, sql);
    }
    protected void btnPrintData_Click(object sender, EventArgs e)
    {
        StringWriter stringwrite;
        HtmlTextWriter htmlwrite;
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=FileName.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        //Form.EnableViewState = false;
        stringwrite = new System.IO.StringWriter();
        htmlwrite = new HtmlTextWriter(stringwrite);
        gvData.RenderControl(htmlwrite);

        Response.Write(stringwrite.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control Control)
    {
        //Confirms that an HtmlForm control is rendered for the 
        //specified ASP.NET server control at run time. 
        //No code required here. 
    }
}