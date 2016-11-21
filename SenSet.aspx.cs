using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class SenSet : System.Web.UI.Page
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
    protected void btnShow_Click(object sender, EventArgs e)
    {
        DataSet ds;
        string sql = "select pshr.get_fullname(empid) as name, pshr.get_desg(cdesgcode) as desg, pshr.get_org(cloccode) as loc, seniorityno from pshr.empperso where empid = " + txtEmpID.Text;
        ds = OraDBConnection.GetData(sql);
        lblNameDesg.Text = string.Format("{0}, {1}", ds.Tables[0].Rows[0]["name"].ToString(), ds.Tables[0].Rows[0]["desg"].ToString());
        lblLoc.Text = ds.Tables[0].Rows[0]["loc"].ToString();
        lblSen.Text = "Sen. No. " + ds.Tables[0].Rows[0]["seniorityno"].ToString();
        update.Visible = true;
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        double senno = 0;

        double.TryParse(txtNewSen.Text,out senno);
        string empid = txtEmpID.Text;
        if (senno == 0)
        {
            lblmsg.Text = "Invalid Seniority Number";
            return;
        }
        string sql = string.Format("update pshr.empperso set seniorityno={0} where empid = {1}", senno, empid);
        OraDBConnection.ExecQry(sql);
        lblmsg.Text = "Seniority Changed";
    }
}