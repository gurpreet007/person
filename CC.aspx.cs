using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CC : System.Web.UI.Page
{
    string prono;
    protected void Page_Load(object sender, EventArgs e)
    {
        prono = Session["proposalno"].ToString();
        //prono = "55";
        if (!IsPostBack)
        {
            FillListAll();
            FillListprt();
        }
    }
    private void FillListAll()
    {
        string sql;
        sql = "select loc,sno from cadre.cclist_person where sno not in "+
            "(select ccnum from cadre.cclist_proposal_person where proposalno = " + prono + 
            ") order by sno";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        lstAll.Items.Clear();
        lstAll.DataSource = ds.Tables[0];
        lstAll.DataTextField = "loc";
        lstAll.DataValueField = "sno";
        lstAll.DataBind();
    }
    private void FillListprt()
    {
        string sql;
        sql = "select loc,sno from cadre.cclist_proposal_person where proposalno = " + prono + " order by sno";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lstCC.Items.Clear();
            lstCC.DataSource = ds.Tables[0];
            lstCC.DataTextField = "loc";
            lstCC.DataValueField = "sno";
            lstCC.DataBind();
        }
    }
    private string GetSNo()
    {
        return OraDBConnection.GetScalar("select nvl(max(sno),0) + 1 from cadre.cclist_person");
    }
    private void SetDefault(bool toset)
    {
        string sql;
        int sno;


        sno = int.Parse(lstAll.SelectedValue);

        sql = "update cadre.cclist_person set defaultitem = '" + ((toset) ? "1" : "0") + "' where sno = " + sno;

    }
    protected void btnAddLoc_Click(object sender, EventArgs e)
    {
        string sql;

        if (btnAddLoc.Text == "Add Location")
        {
            if (txtNewLoc.Text.Length > 0)
            {
                sql = "insert into cadre.cclist_person(sno,loc,ccnum) values(" + GetSNo() + ",'" + txtNewLoc.Text + "',-1)";
                OraDBConnection.ExecQry(sql);
            }
            txtNewLoc.Text = "";
        }
        
        
        
        if (btnAddLoc.Text == "Update Location")
        {

            if (txtNewLoc.Text.Length > 0)
            {
                sql = "update cadre.cclist_person set loc = '" + txtNewLoc.Text + "' where sno = " + lbllstsno.Text ;
                OraDBConnection.ExecQry(sql);
            }
            txtNewLoc.Text = "";
            lbllstsno.Text = "";
            btnAddLoc.Text = "Add Location";      
        }
        FillListAll();
    }
    protected void btnRemLoc_Click(object sender, EventArgs e)
    {
        string sql;

        if (lstAll.SelectedValue != "")
        {
            sql = "delete from cadre.cclist_person where sno=" + lstAll.SelectedValue;
            OraDBConnection.ExecQry(sql);
            FillListAll();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (lstAll.SelectedItem != null && lstCC.Items.Contains(lstAll.SelectedItem) == false)
        {
            lstCC.Items.Add(lstAll.SelectedItem);
            lstAll.Items.Remove(lstAll.SelectedItem);
            lstAll.ClearSelection();
            lstCC.ClearSelection();
            if (lstAll.Items.Count > 0)
            {
                lstAll.SelectedIndex = 0;
            }
        }
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (lstCC.SelectedItem != null)
        {
            lstAll.Items.Add(lstCC.SelectedItem);
            lstCC.Items.Remove(lstCC.SelectedItem);
            lstCC.ClearSelection();
            lstAll.ClearSelection();
            if (lstCC.Items.Count > 0)
            {
                lstCC.SelectedIndex = 0;
            }
        }
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
    protected void txtNewLoc_TextChanged(object sender, EventArgs e)
    {
        //FillListAll(txtNewLoc.Text);
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
       
        OraDBConnection.ExecQry("delete from  cadre.cclist_proposal_person where proposalno = " + prono);
        for (i = 0; i < lstCC.Items.Count; i++)
        {
            ccnum = i + 1;
            sql = string.Format("Insert into cadre.cclist_proposal_person(sno,ccnum, loc, proposalno) values ({0},{1},'{2}',{3})", 
                ccnum, lstCC.Items[i].Value, lstCC.Items[i].Text,prono);
            
            //sql = "update cadre.cclist_person set ccnum=" + ccnum + " where sno=" + lstCC.Items[i].Value;
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
           txtNewLoc.Text = lstAll.SelectedItem.Text;
           lbllstsno.Text = lstAll.SelectedValue;
           btnAddLoc.Text = "Update Location";      
        }
    
    }
}