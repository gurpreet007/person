using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public partial class owner : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    string sql;
    //protected void Page_Load(object sender, EventArgs e)
    //{

    //string avi=""+Session.Contents["z"];
    //    if (avi == "")
    //    {
    //        string p="SESSION EXPIRED......LOGIN AGAIN";
    //        Session.Add("ferr", p);
    //        string li = "log.aspx";
    //        Session.Add("flk", li);
    //        Response.Redirect("err.aspx");
    //    }       
    //GridView1.Attributes.Add("bordercolor", "Black");
    //    //   GridView1.Attributes.Add("borderwidth", "100px");
    //}
    private void fn()
    {
        sql = TextBox3.Text;
        ds = Class2.getResut(sql);
        GridView1.DataSource = ds.Tables[0];
        GridView1.DataBind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        fn();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int i;

        for (i = 0; i < GridView1.Columns.Count; i++)
        {


            TextBox5.Text = e.OldValues[i].ToString();

            if (e.NewValues[i - 4].ToString() == null)

                TextBox4.Text = "0";

            else
                TextBox4.Text = e.NewValues[i - 4].ToString();



            //if (Convert.ToInt32(e.NewValues[0].ToString()) > Convert.ToInt32(e.OldValues[0].ToString()))
            //{
            //    GridView1.EditIndex = -1;
            //    RegisterClientScriptBlock("ddd", "<script type='text/javascript'> confirm('AMOUNT APPROVED CAN'T BE GREATER THAN REAISED DL REQUEST');</script>");

            //  //  lblmsg.Text = "AMOUNT APPROVED CAN'T BE GREATER THAN REAISED DL REQUEST";


            //    //  Response.Redirect("admin.aspx");

            //}


            //string sql;

            //sql = "update data_new_summary set " + GridView1.Columns[i].AccessibleHeaderText + "= '" + TextBox4.Text + "' where " + GridView1.Columns[i].AccessibleHeaderText + "= '" + TextBox3.Text + "' and head_code= '" + GridView1.Rows[e.RowIndex].Cells[3].Text + "' ";
            //sql = sql + " and token_id=" + TextBox6.Text;
            //Class2.execute(sql);
            //Class2.execute("commit");


            ////Label1.Text = Class2.getScaler("select loc_code||' - '||get_ddo_loc(loc_code)||' , MEMO NO- '||memo||', MEMO DATE- '||to_char(memo_date,'dd-Mon-yy')||', APPROVAL DATE- '||to_char(App_date,'dd-Mon-yy')||', TRANACTION DATE- '|| to_char(get_transfer_date(token_id),'dd-Mon-yy')||' , STATUS- '||get_status(read_status) from data_new_summary where token_id=" + TextBox6.Text);
            ////Label24.Text = Class2.numset(Class2.getScaler("select sum(add_req) from data_new_summary where token_id=" + TextBox6.Text));
            ////Label26.Text = Class2.NumberToText(Convert.ToInt32(Class2.getScaler("select sum(add_req) from data_new_summary where token_id=" + TextBox6.Text)));
            ////Label25.Text = Class2.numset(Class2.getScaler("select sum(amt_approved) from data_new_summary where token_id=" + TextBox6.Text));
            //// Int32 p = Convert.ToInt32(Class2.getScaler("select sum(amt_approved) from data_802_back where token_id=" + TextBox6.Text));

            //if (Label25.Text != "0" && Label25.Text != "")
            //{
            //    Int32 p = Convert.ToInt32(Class2.getScaler("select sum(amt_approved) from data_new_summary where token_id=" + TextBox6.Text));
            //    Label27.Text = Class2.NumberToText(p);
            //}




        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Class2.execute(TextBox6.Text);

    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;

        GridView1.DataBind();
        fn();
    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        fn();

    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        GridView1.Visible = true;
        //string sql = null;

         
       // System.Data.DataSet ds1 = Class2.getResut(sql);

       // GridView1.DataSource = ds;
      //  GridView1.DataBind();


        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=FileName.xls");
        Response.Charset = "";

        Response.ContentType = "application/vnd.xls";
        this.EnableViewState = false;
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        htmlWrite.WriteLine("<b><u><fontsize='5'>Arrear Detail For Location</font></u></b>");
        GridView1.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the 

        // specified ASP.NET server control at run time. 

        // No code required here. 
    }
}