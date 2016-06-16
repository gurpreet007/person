using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class LocatePost : System.Web.UI.Page
{
    string GetRLOC()
    {
        string rloc = "0";

        rloc = (ddDirect.SelectedValue != "" && ddDirect.SelectedValue != "0") ? ddDirect.SelectedValue : rloc;
        rloc = (ddOrg.SelectedValue != "" && ddOrg.SelectedValue != "0") ? ddOrg.SelectedValue : rloc;
        rloc = (ddCircle.SelectedValue != "" && ddCircle.SelectedValue != "0") ? ddCircle.SelectedValue : rloc;
        rloc = (ddDiv.SelectedValue != "" && ddDiv.SelectedValue != "0") ? ddDiv.SelectedValue : rloc;
        rloc = (ddSubDiv.SelectedValue != "" && ddSubDiv.SelectedValue != "0") ? ddSubDiv.SelectedValue : rloc;

        return rloc;
    }
    string GetLoccode()
    {
        if (ddSubDiv.SelectedValue != "" && ddSubDiv.SelectedValue != "0")
            return ddSubDiv.SelectedValue;
        if (ddDiv.SelectedValue != "" && ddDiv.SelectedValue != "0")
            return ddDiv.SelectedValue;
        if (ddCircle.SelectedValue != "" && ddCircle.SelectedValue != "0")
            return ddCircle.SelectedValue;
        if (ddOrg.SelectedValue != "" && ddOrg.SelectedValue != "0")
            return ddOrg.SelectedValue;
        if (ddDirect.SelectedValue != "" && ddDirect.SelectedValue != "0")
            return ddDirect.SelectedValue;
        return "";
    }
    void FillDirectors()
    {
        ddDirect.Items.Clear(); 
        ddDirect.Items.Add(new ListItem("--Select Director--", "0"));
        ddDirect.Items.Add(new ListItem("CMD/PSPCL", "800000000"));
        ddDirect.Items.Add(new ListItem("Director/Distribution", "803000000"));
        ddDirect.Items.Add(new ListItem("Director/Generation", "802000000"));
        ddDirect.Items.Add(new ListItem("Director/Finance", "804000000"));
        ddDirect.Items.Add(new ListItem("Director/Commercial", "805000000"));
        ddDirect.Items.Add(new ListItem("Director/HR", "806000000"));
        ddDirect.Items.Add(new ListItem("Director/Admin", "801000000"));
        ddDirect.Items.Add(new ListItem("Deputation", "600000000"));
        ddDirect.Items.Add(new ListItem("Ombudsman", "810000000"));
    }
    void FillOrg()
    {
        string sql;
        string rloc = GetRLOC();
        DataSet ds;

        sql = "select loccode, locabb from pshr.mast_loc where locname is not null and loccode like '%000000' and {0} and loctype <> 11 and aloc = 1 order by loccode ";
        sql = String.Format(sql, (rloc != "600000000")?string.Format("locrep = '{0}'", rloc):"(loccode like '6%' or loccode like '7%')");
        ds = OraDBConnection.GetData(sql);
        ddOrg.DataSource = ds;
        ddOrg.DataTextField = "locabb";
        ddOrg.DataValueField = "loccode";
        ddOrg.DataBind();
        ddOrg.Items.Insert(0, new ListItem("--Select Org--","0"));
        ds.Clear();
        ds.Dispose();
    }
    void FillCircle()
    {
        string sql;
        string rloc = GetRLOC();
        DataSet ds;

        sql = string.Format("select loccode, locabb from pshr.mast_loc where locrep = '{0}' and loctype = 30 and loccode not like '%000000' and aloc=1 order by loccode ", rloc);
        ds = OraDBConnection.GetData(sql);
        ddCircle.DataSource = ds;
        ddCircle.DataTextField = "locabb";
        ddCircle.DataValueField = "loccode";
        ddCircle.DataBind();
        ddCircle.Items.Insert(0, new ListItem("--Select Circle--", "0"));
        ds.Clear();
        ds.Dispose();
    }
    void FillDiv()
    {
        string sql;
        string rloc = GetRLOC();
        DataSet ds;

        sql = string.Format("select loccode, locabb from pshr.mast_loc where locrep = '{0}' and loctype = 40 and aloc=1 order by loccode ", rloc);
        ds = OraDBConnection.GetData(sql);
        ddDiv.DataSource = ds;
        ddDiv.DataTextField = "locabb";
        ddDiv.DataValueField = "loccode";
        ddDiv.DataBind();
        ddDiv.Items.Insert(0, new ListItem("--Select Division--", "0"));
        ds.Clear();
        ds.Dispose();
    }
    void FillSubDiv()
    {
        string sql;
        string rloc = GetRLOC();
        DataSet ds;

        sql = string.Format("select loccode, locabb from pshr.mast_loc where locrep = '{0}' and loctype in (50,60,70) and aloc=1 order by loccode ", rloc);
        ds = OraDBConnection.GetData(sql);
        ddSubDiv.DataSource = ds;
        ddSubDiv.DataTextField = "locabb";
        ddSubDiv.DataValueField = "loccode";
        ddSubDiv.DataBind();
        ddSubDiv.Items.Insert(0, new ListItem("--Select S/Division--", "0"));
        ds.Clear();
        ds.Dispose();
    }
    void FillGrid()
    {
        string sql;
        DataSet ds;
        string loccode = GetLoccode();

        sql = String.Format("select "+
            "pshr.get_desg(c.desgcode) || '-' || c.indx || ' at ' || "+
            "pshr.get_org(loccode) || nvl2(cm.empid,' (' || cm.empid || ') ','') as desg, c.rowno " +
            "from cadre.cadr c inner join pshr.mast_desg m on c.desgcode = m.desgcode " +
            "left outer join cadre.cadrmap cm on c.rowno = cm.rowno " +
            "where c.loccode = '{0}' and c.hia=0 and " +
            "m.servcode = 30 and m.gazcode = 10 and m.cadrecode=10 and m.adesg='A' " +
            "order by c.loccode, m.hecode, c.indx", loccode);
        ds = OraDBConnection.GetData(sql);
        ddPost.DataSource = ds;
        ddPost.DataTextField = "desg";
        ddPost.DataValueField = "rowno";
        ddPost.DataBind();
        ds.Clear();
        ds.Dispose();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillDirectors();
        }
    }
    protected void ddDirect_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddOrg.Items.Clear();
        ddCircle.Items.Clear();
        ddDiv.Items.Clear();
        ddSubDiv.Items.Clear();

        if(ddDirect.SelectedValue != "0")
        {
            FillOrg();
            FillCircle();
            FillDiv();
            FillSubDiv();
            FillGrid();
        }
    }
    protected void ddOrg_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCircle.Items.Clear();
        ddDiv.Items.Clear();
        ddSubDiv.Items.Clear();
        FillCircle();
        FillDiv();
        FillSubDiv();
        FillGrid();
    }
    protected void ddCircle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddDiv.Items.Clear();
        ddSubDiv.Items.Clear();
        FillDiv();
        FillSubDiv();
        FillGrid();
    }
    protected void ddDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSubDiv.Items.Clear();
        FillSubDiv();
        FillGrid();
    }
    protected void ddSubDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void btnCopyPost_Click(object sender, EventArgs e)
    {
        if (ddPost.SelectedItem == null)
        {
            return;
        }
        Session["LOCATEPOSTVAL"] = ddPost.SelectedValue;
        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
    }
}