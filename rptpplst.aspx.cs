using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rptpplst : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Filldesg();
            Fillorg();
            Fillbranch();
            Fillcateg();
            ControlControls();
        }
    }
    private void Filldesg()
    {
        string sql = "SELECT desgabb ,desgcode FROM pshr.mast_desg where gazcode = 10 order by desgabb";
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
    private void Fillorg()
    {
        string sql = "SELECT locabb ,loccode FROM pshr.mast_loc where loccode like '___000000' order by locabb";
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

    private void Fillbranch()
    {
        string sql = "SELECT branchtext ,branchcode FROM pshr.mast_branch order by branchtext";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        drpbranch.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            drpbranch.Items.Add(new ListItem("--Select--", "0"));
            foreach (System.Data.DataRow row in ds.Tables[0].Rows)
            {
                drpbranch.Items.Add(new ListItem(row["branchtext"].ToString(), row["branchcode"].ToString()));
            }
        }
    }

    private void Fillcateg()
    {
        string sql = "SELECT categtext ,categcode FROM pshr.mast_categ order by categtext";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        drpcateg.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            drpcateg.Items.Add(new ListItem("--Select--", "0"));
            foreach (System.Data.DataRow row in ds.Tables[0].Rows)
            {
                drpcateg.Items.Add(new ListItem(row["categtext"].ToString(), row["categcode"].ToString()));
            }
        }
    }

    protected void btnclear_Click(object sender, EventArgs e)
    {
        cleardata();
    }

    private void cleardata()
    {
        drpdesg.SelectedValue = "0";
        drporg.SelectedValue = "0";
        drpbranch.SelectedValue = "0";
        drpcateg.SelectedValue = "0";
    }

    private void ControlControls()
    {
        if (optpost.SelectedValue == "0")
        {
            drpbranch.Enabled = false;
            drpcateg.Enabled = false;
        }

        else if (optpost.SelectedValue == "1")
        {
            drpbranch.Enabled = true;
            drpcateg.Enabled = true;
        }
    }

    protected void optpost_SelectedIndexChanged(object sender, EventArgs e)
    {
        ControlControls();
    }

    protected void btnshow_Click(object sender, EventArgs e)
    {
        Session.Contents.Add("empid", txtempid.Text);
        Session.Contents.Add("desg", drpdesg.SelectedValue);
        Session.Contents.Add("org", drporg.SelectedValue);
        Session.Contents.Add("branch", drpbranch.SelectedValue);
        Session.Contents.Add("categ", drpcateg.SelectedValue);
        if (optpost.SelectedValue == "0")
        {
            Response.Redirect("frmrptpdetails.aspx");
            //Response.Write("<script type='text/javascript'>detailedresults=window.open('frmrptpdetails.aspx','_new');</script>");
        }
        else if (optpost.SelectedValue == "1")
        {
            Response.Redirect("frmrptpplist.aspx");
            //Response.Write("<script type='text/javascript'>detailedresults=window.open('frmrptpplist.aspx','_new');</script>");
        }
    }
}