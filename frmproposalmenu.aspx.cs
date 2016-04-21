using System;
using System.Data; 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmproposalmenu : System.Web.UI.Page
{
    string myaddress = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty((String)Session["rd_or_tp"]))
            {
                PropType.Value = (string)Session["rd_or_tp"];
            }
            FillGrid();
            panNewProp.Visible = false;
            txtpropname.Text = "";
        }
    }
    private void FillGrid()
    {
        string sql;
        DataSet ds;
        sql = "select pno as \"Proposal_No\",pname as \"Proposal_Name\"," +
                "to_char(pdate,'DD-Mon-YYYY') as \"Creation_Date\",TYPE from cadre.proposals" +
                " WHERE status='" + rbStatus.SelectedValue + "' and type='" + PropType.Value + "'";
        ds = OraDBConnection.GetData(sql);
        gvProposals.DataSource = ds;
        gvProposals.DataBind();
    }
    protected void btncproposal_Click(object sender, EventArgs e)
    {
        panNewProp.Visible = true;
    }
    protected void gvProposals_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Session["proposalno"] = gvProposals.Rows[e.NewEditIndex].Cells[1].Text.ToString();
        Session["proposalname"] = gvProposals.Rows[e.NewEditIndex].Cells[2].Text.ToString();
        Session["proposaldate"] = gvProposals.Rows[e.NewEditIndex].Cells[3].Text.ToString();

        string redirect = (PropType.Value == "TP") ? Utils.frmaddresses["transpro_link"] : Utils.frmaddresses["redesig_link"];
        
        Response.Redirect(redirect);
    }
    protected void gvProposals_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string prono = gvProposals.Rows[e.RowIndex].Cells[1].Text.ToString();
        
        string sql = "delete from cadre.propcadrmap where propno =" + prono;
        OraDBConnection.ExecQry(sql);

        sql = "delete from cadre.cclist_proposal_person  where proposalno =" + prono;
        OraDBConnection.ExecQry(sql);

        sql = "delete from cadre.notes_proposal_person  where proposalno =" + prono;
        OraDBConnection.ExecQry(sql);

        sql = "delete from cadre.proposals  where pno =" + prono;
        OraDBConnection.ExecQry(sql);

        FillGrid();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        panNewProp.Visible = false;
        txtpropname.Text = "";
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (txtpropname.Text == "")
        {
            Utils.ShowMessageBox(this, "Enter Proposal Name");
            return;
        }

        //status: U-Unsaved, S-Saved
        //type: TP-Transfer/Promotion, RD-Redesignation
        string sql = "insert into cadre.proposals(pno,pname,pdate,status,type) values" +
                     "((select nvl(max(pno),0)+1 from cadre.proposals),"+
                     "'" + txtpropname.Text + "',sysdate,'U','"+PropType.Value+"')";
        
        bool ret = false;
        try
        {
            ret = OraDBConnection.ExecQry(sql);
            if (ret == false)
            {
                throw new Exception("Error in Creating Proposal");
            }
        }
        catch (Exception ex)
        {
            Utils.ShowMessageBox(this, ex.Message);
            return;
        }
        panNewProp.Visible = false;
        txtpropname.Text = "";
        FillGrid();
    }
    protected void rbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
}