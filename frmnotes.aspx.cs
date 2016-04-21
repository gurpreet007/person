using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmnotes : System.Web.UI.Page
{
    string prono;
    protected void Page_Load(object sender, EventArgs e)
    {
        prono = Session["proposalno"].ToString();
        if (!IsPostBack)
        {
            FillListAll();
            FillListprt();
        }
    }

    private void FillListAll(string filter = "")
    {
        string sql;
        sql = "select notes,sno from cadre.notes_person " + "where upper(notes) like upper('%" + filter + "%') order by sno";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        lstAll.Items.Clear();
        lstAll.DataSource = ds.Tables[0];
        lstAll.DataTextField = "notes";
        lstAll.DataValueField = "sno";
        lstAll.DataBind();
    }

    private void FillListprt(string filter = "")
    {
        string sql;
        sql = "select notes,sno from cadre.notes_proposal_person where proposalno = " + prono + " order by sno";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lstCC.Items.Clear();
            lstCC.DataSource = ds.Tables[0];
            lstCC.DataTextField = "notes";
            lstCC.DataValueField = "sno";
            lstCC.DataBind();
        }
    }

    private string GetSNo()
    {
        return OraDBConnection.GetScalar("select nvl(max(sno),0) + 1 from cadre.notes_person");
    }
    private void SetDefault(bool toset)
    {
        string sql;
        int sno;


        sno = int.Parse(lstAll.SelectedValue);

        sql = "update cadre.notes_person set defaultitem = '" + ((toset) ? "1" : "0") + "' where sno = " + sno;

    }
    protected void btnAddLoc_Click(object sender, EventArgs e)
    {
        string sql;

        if (btnAddnote.Text == "Add Note")
        {
            if (txtNewnote.Text.Length > 0)
            {
                sql = "insert into cadre.notes_person(sno,notes,ccnum) values(" + GetSNo() + ",'" + txtNewnote.Text + "',-1)";
                OraDBConnection.ExecQry(sql);
            }
            txtNewnote.Text = "";
        }



        if (btnAddnote.Text == "Update Note")
        {

            if (txtNewnote.Text.Length > 0)
            {
                sql = "update cadre.notes_person set notes = '" + txtNewnote.Text + "' where sno = " + lbllstsno.Text;
                OraDBConnection.ExecQry(sql);
            }
            txtNewnote.Text = "";
            lbllstsno.Text = "";
            btnAddnote.Text = "Add Note";
        }
        FillListAll();
    }
    protected void btnRemLoc_Click(object sender, EventArgs e)
    {
        string sql;

        if (lstAll.SelectedValue != "")
        {
            sql = "delete from cadre.notes_person where sno=" + lstAll.SelectedValue;
            OraDBConnection.ExecQry(sql);
            FillListAll();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (lstAll.SelectedItem != null)
        {
            if (!lstCC.Items.Contains(lstAll.SelectedItem))
            {
                lstCC.Items.Add(lstAll.SelectedItem);
                lstAll.SelectedIndex = 0;
            }
        }
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        lstCC.Items.Remove(lstCC.SelectedItem);
    }
    protected void btnUp_Click(object sender, EventArgs e)
    {
        ListItem litem;
        int index;

        litem = lstCC.SelectedItem;
        index = lstCC.SelectedIndex;
        if (index != 0)
        {
            lstCC.Items.Remove(lstCC.SelectedItem);
            lstCC.Items.Insert(index - 1, litem);
        }
    }
    protected void txtNewnote_TextChanged(object sender, EventArgs e)
    {
        FillListAll(txtNewnote.Text);
    }
    protected void btnDown_Click(object sender, EventArgs e)
    {
        ListItem litem;
        int index;

        litem = lstCC.SelectedItem;
        index = lstCC.SelectedIndex;
        if (index != lstCC.Items.Count - 1)
        {
            lstCC.Items.Remove(lstCC.SelectedItem);
            lstCC.Items.Insert(index + 1, litem);
        }
    }
    protected void btnDone_Click(object sender, EventArgs e)
    {
        string sql;
        int i;
        int ccnum = 0;
        OraDBConnection.ExecQry("delete from cadre.notes_proposal_person where proposalno = "+ prono);
        for (i = 0; i < lstCC.Items.Count; i++)
        {
            ccnum = i + 1;
            sql = string.Format("Insert into cadre.notes_proposal_person(sno,ccnum, notes, proposalno) values ({0},{1},'{2}',{3})", ccnum, lstCC.Items[i].Value, lstCC.Items[i].Text, prono);

            //sql = "update cadre.cnotes_person set ccnum=" + ccnum + " where sno=" + lstCC.Items[i].Value;
            OraDBConnection.ExecQry(sql);
        }
        Response.Write("<script>window.close()</script>");
    }
    protected void btnSetDef_Click(object sender, EventArgs e)
    {
        SetDefault(true);
        Response.Redirect(Request.RawUrl);
    }
    protected void btnUnsetDef_Click(object sender, EventArgs e)
    {
        SetDefault(false);
        Response.Redirect(Request.RawUrl);
    }
    protected void btneditloc_Click(object sender, EventArgs e)
    {
        if (lstAll.SelectedValue != "")
        {
            txtNewnote.Text = lstAll.SelectedItem.Text;
            lbllstsno.Text = lstAll.SelectedValue;
            btnAddnote.Text = "Update Note";
        }

    }


}