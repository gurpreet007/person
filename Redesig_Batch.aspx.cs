using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Redesig_Batch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string sql = "select empid,firstname,fathername,dob from pshr.empperso where cdesgcode = "+drpDesg.SelectedValue;
        OraDBConnection.FillGrid(ref gv_redesig ,sql);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {

    }
}