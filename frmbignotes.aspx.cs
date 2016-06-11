using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class frmNotes2 : System.Web.UI.Page
{
    private void FillNotesCreate()
    {
        string sql = "select name, name || '(' || tags || ')' as txtval from cadre.bignotes where type='T' order by name";
        ddCreate.DataSource = OraDBConnection.GetData(sql);
        ddCreate.DataTextField = "txtval";
        ddCreate.DataValueField = "name";
        ddCreate.DataBind();
        ddCreate.Items.Insert(0, new ListItem("New", "New"));
    }
    private void FillNotesOpen()
    {
        string sql = "select name, name || '(' || tags || ')' || '-' || type as txtval from cadre.bignotes order by type, name";
        ddOpen.DataSource = OraDBConnection.GetData(sql);
        ddOpen.DataTextField = "txtval";
        ddOpen.DataValueField = "name";
        ddOpen.DataBind();
    }
    private void DisableDataFields(bool disable = true)
    {
        note_name.Enabled = !disable;
        note_tags.Enabled = !disable;
        note_data.Enabled = !disable;
    }
    private void EmptyDataFields()
    {
        note_name.Text = string.Empty;
        note_tags.Text = string.Empty;
        note_data.Text = string.Empty;
    }
    private void SaveNote(bool template, bool saveas = false)
    {
        string name = saveas?txtSaveAsName.Text:note_name.Text;
        string tags = note_tags.Text.Replace("'","''");
        string data = note_data.Text.Replace("'","''");
        string sql;
        string type = template ? "T" : "N";

        if (!saveas && hidSource.Text == "T")
        {
            lblmsg.Text = "Cannot change template.";
            return;
        }

        sql = string.Format("merge into cadre.bignotes B using " +
                        "(select '{0}' as n, '{1}' as t, '{2}' as d , 'N' as ty from dual) D " +
                        "on (B.name = D.n and B.type='N') " +
                        "when matched then update set B.tags = D.t, B.data = D.d " +
                        "when not matched then insert (name, tags, data, type, addedon) values (D.n, D.t, D.d, D.ty, sysdate)"
                        ,name, tags, data);

        try
        {
            OraDBConnection.ExecQry(sql);
            lblmsg.Text = template?"Template Saved":"Note Saved";
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        DisableDataFields(true);
    }
    private void OpenNote(string name, string source)
    {
        /*We may be here because:
         * 1. Create New Note -- source == "New"
         * 2. Create from Template -- source == "T"
         * 3. Open a Template --Source == "T"
         * 4. Open a Note  -- Source = "N"
         * 5. Search and then open a Template -- Source = "T"
         * 6. Search and then open a Note -- Source == "N"
         */
        string sql;
        DataRow drow;

        EmptyDataFields();
        DisableDataFields(false);
        lblmsg.Text = string.Empty;
        div_data.Visible = true;
        hidSource.Text = source;

        if (source == "New")
        {
            btnSave.Enabled = true;
            btnMakeTemplate.Enabled = true;
            heading.InnerText = "Note";
        }
        else if (source == "N")
        {
            sql = string.Format("select name, tags, data, type from cadre.bignotes where name = '{0}'", name);
            drow = OraDBConnection.GetData(sql).Tables[0].Rows[0];

            note_name.Text = drow["name"].ToString();
            note_tags.Text = drow["tags"].ToString();
            note_data.Text = drow["data"].ToString();

            btnSave.Enabled = true;
            btnMakeTemplate.Enabled = true;
            heading.InnerText = "Note";
        }
        else if (source == "T")
        {
            sql = string.Format("select name, tags, data, type from cadre.bignotes where name = '{0}'", name);
            drow = OraDBConnection.GetData(sql).Tables[0].Rows[0];

            note_name.Text = drow["name"].ToString();
            note_tags.Text = drow["tags"].ToString();
            note_data.Text = drow["data"].ToString();

            btnSave.Enabled = false;
            btnMakeTemplate.Enabled = false;
            heading.InnerText = "Template";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["loginy"] == null || Session["loginy"].ToString() != "1")
        {
            Response.Redirect("./Login.aspx");
        }
        if (!IsPostBack)
        {
            div_create.Visible = false;
            div_open.Visible = false;
            div_search.Visible = false;
            div_data.Visible = false;
            div_data2_saveas.Visible = false;
        }
    }
    protected void btnOptions_Click(object sender, EventArgs e)
    {
        div_create.Visible = ddOptions.SelectedValue == "Create";
        div_open.Visible = ddOptions.SelectedValue == "Open";
        div_search.Visible = ddOptions.SelectedValue == "Search";
        div_data.Visible = false;
        div_data2_saveas.Visible = false;
        FillNotesCreate();
        FillNotesOpen();
    }
    protected void btnCreateStart_Click(object sender, EventArgs e)
    {
        string source = ddCreate.SelectedValue=="New"?"New":"T";
        OpenNote(ddCreate.SelectedValue, source);
    }
    protected void btnSaveAsStart_Click(object sender, EventArgs e)
    {
        div_data2_saveas.Visible = true;
        DisableDataFields(false);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveNote(false);
    }
    protected void btnMakeTemplate_Click(object sender, EventArgs e)
    {
        SaveNote(true);
    }
    protected void btnSaveAsGo_Click(object sender, EventArgs e)
    {
        SaveNote(false,true);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string sql = string.Empty;
        string pattern = txtSearch.Text;
        DataSet ds;

        if (string.IsNullOrWhiteSpace(pattern))
        {
            return;
        }

        sql = string.Format("select name || '(' || tags || ')-' || type as disptext,"+
            " name from cadre.bignotes where "+
            "name like '%{0}%' or tags like '%{0}%' or data like '%{0}%' order by type, name",pattern);

        ds = OraDBConnection.GetData(sql);
        ddResults.DataSource = ds;
        ddResults.DataTextField = "disptext";
        ddResults.DataValueField = "name";
        ddResults.DataBind();
    }
    protected void btnResultOpen_Click(object sender, EventArgs e)
    {
        if (ddResults.SelectedIndex < 0)
        {
            return;
        }
        string source = ddResults.SelectedItem.Text[ddResults.SelectedItem.Text.LastIndexOf("-") + 1].ToString();
        OpenNote(ddResults.SelectedValue, source);
    }
    protected void btnOpen_Click(object sender, EventArgs e)
    {
        string source = ddOpen.SelectedItem.Text[ddOpen.SelectedItem.Text.LastIndexOf("-") + 1].ToString();
        OpenNote(ddOpen.SelectedValue, source);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string name = note_name.Text;
        string sql = string.Format("delete from cadre.bignotes where name = '{0}'", name);
        OraDBConnection.ExecQry(sql);
        lblmsg.Text = "Note Deleted";
    }
}