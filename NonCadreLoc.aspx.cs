using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NonCadreLoc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if ((Session["loginy"] == null) || (Session["loginy"].ToString() != "1"))
            {
                Response.Redirect("Login.aspx");
                return;
            }
        }
    }
    private void FillLocations(string loctype, bool alllocs = false, string filter = "")
    {
        string sql;

        sql = "Select Loccode ,decode(aloc,1,locabb,2,'<'||locabb||'>') as locabb "+
            "from pshr.mast_loc where " +
            "aloc  in (1,2) and " +
            "loccode not like '6%' and " +
            "loccode not like '7%' and " +
            (alllocs?"":("loctype  <  " + loctype + " and ")) +
            "post like '%" + filter.ToUpper() + "%' " +
            "order by loctype desc ";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        drpLocs.Items.Clear();
        drpLocs.DataSource = ds.Tables[0];
        drpLocs.DataTextField = "locabb";
        drpLocs.DataValueField = "loccode";
        drpLocs.DataBind();
    }
    protected void txtFilter_TextChanged(object sender, EventArgs e)
    {
        FillLocations(drploctype.SelectedValue, chkShowAll.Checked, txtFilter.Text);
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        string aloc;

        if (drpLocs.SelectedValue == "")
        {
            Utils.ShowMessageBox(this, "No Location To Delete");
            return;
        }

        aloc = OraDBConnection.GetScalar("select aloc from pshr.mast_loc where loccode = " + drpLocs.SelectedValue);
        if (aloc != "2")
        {
            Utils.ShowMessageBox(this, "Not Permitted");
        }
        else if (aloc == "2")
        {
            if (OraDBConnection.ExecQry("delete from pshr.mast_loc where loccode = " + drpLocs.SelectedValue))
            {
                Utils.ShowMessageBox(this, "Location Deleted");
            }
            else
            {
                Utils.ShowMessageBox(this, "Error Deleting Location");
            }
        }
        FillLocations(drploctype.SelectedValue, chkShowAll.Checked, txtFilter.Text);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string loctype,locrep,locname;
        string loccode;
        string sql;
        if (drpLocs.SelectedValue == "")
        {
            Utils.ShowMessageBox(this, "No Reporting Location Selected");
            return;
        }
        if (txtNew.Text == "")
        {
            Utils.ShowMessageBox(this, "Enter Name of Location to Create");
            return;
        }
        if (drploctype.SelectedValue == "0")
        {
            Utils.ShowMessageBox(this, "No Location Type Selected");
            return;
        }
        locrep = drpLocs.SelectedValue;
        loctype = drploctype.SelectedValue.ToString();
        locname = txtNew.Text;
        sql = string.Format("select cadre.get_next_loccode({0},{1}) from dual",locrep,loctype);
        loccode = OraDBConnection.GetScalar(sql);

        if (loccode == "")
        {
            Utils.ShowMessageBox(this,"Error getting next loccode");
            return;
        }
        sql = string.Format("insert into pshr.mast_loc(loccode,locname,locabb,post,loctype,locrep,aloc) " +
            "values({0},'{1}','{1}','{1}',{2},{3},'2')", loccode, locname, loctype, locrep);
        if (OraDBConnection.ExecQry(sql))
        {
            Utils.ShowMessageBox(this, "Location Added");
        }
        else
        {
            Utils.ShowMessageBox(this, "Error Adding Location");
        }
        FillLocations(drploctype.SelectedValue, chkShowAll.Checked, txtFilter.Text);
        return;
    }
    protected void drploctype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLocations(drploctype.SelectedValue, chkShowAll.Checked, txtFilter.Text);
    }
    protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
    {
        FillLocations(drploctype.SelectedValue, chkShowAll.Checked, txtFilter.Text);
    }
}