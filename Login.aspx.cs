using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
public partial class _Default : System.Web.UI.Page
{
    private bool Authenticate(string username, string password)
    {
        if (Regex.IsMatch(username, @"^\w+$") == false
            || Regex.IsMatch(password, @"^\w+$") == false)
        {
            Utils.ShowMessageBox(this, "Invalid Username/Password");
            return false;
        }
        string sql = String.Format("SELECT role FROM cadre.person_users " +
            "WHERE uname='{0}' AND upass='{1}'", username, password);

        string role = OraDBConnection.GetScalar(sql);
        if (! String.IsNullOrEmpty(role))
        {
            Session["loginy"] = "1";
            Session["role"] = role;
            Session["username"] = username;
            return true;
        }
        return false;
    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        e.Authenticated = Authenticate(Login1.UserName, Login1.Password);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["loginy"] = "";

    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        this.MasterPageFile = "~/Site1.master";
    }
}