using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    private void SetUpNavMenu()
    {
        string role = Session["role"] as string;
        MenuItem item;
        if (role == "S1")
        {
            item = NavMenu.FindItem("Actions/Redesignate");
            item.Parent.ChildItems.Remove(item);
            item = NavMenu.FindItem("Actions/Retirements");
            item.Parent.ChildItems.Remove(item);
        }
        else if (role == "S2")
        {
            item = NavMenu.FindItem("Actions/Redesignate");
            item.Parent.ChildItems.Remove(item);
            item = NavMenu.FindItem("Actions/Retirements");
            item.Parent.ChildItems.Remove(item);
        }
        else if (role == "S3")
        {
            item = NavMenu.FindItem("Actions/Data Entry");
            item.Parent.ChildItems.Remove(item);
            item = NavMenu.FindItem("Actions/TransferPromotion");
            item.Parent.ChildItems.Remove(item);
        }
        NavMenu.Visible = HttpContext.Current.User.Identity.IsAuthenticated;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        NavMenu.Visible = HttpContext.Current.User.Identity.IsAuthenticated;
        SetUpNavMenu();
        //if (!IsPostBack)
        //{
        //    if (!string.IsNullOrEmpty((string)Session["username"]))
        //    {
                
        //        Label1.Text = "Welcome  " + Session["username"].ToString();
        //        lbllogin.Visible = false;
        //        lbllogout.Visible = true;
        //    }
        //    else
        //    {
        //        Session.Clear();
        //        Label1.Text = "";
        //        lbllogin.Visible = true;
        //        lbllogout.Visible = false;
        //    }
        //}
    }
}
