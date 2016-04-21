using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class frmreports : System.Web.UI.Page
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
        if (chkorgopt.SelectedValue == "Zone")
        {

            string sql = "SELECT locabb ,loccode FROM pshr.mast_loc where loctype = 20 and aloc = 1 order by locabb";
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
        else if (chkorgopt.SelectedValue == "Circle")
        {

            string sql = "SELECT locabb ,loccode FROM pshr.mast_loc where loctype = 30 and aloc = 1 order by locabb";
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
        else if (chkorgopt.SelectedValue == "Division")
        {

            string sql = "SELECT locabb ,loccode FROM pshr.mast_loc where loctype = 40 and aloc = 1 order by locabb";
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
        else if (chkorgopt.SelectedValue == "SubDivision")
        {

            string sql = "SELECT locabb ,loccode FROM pshr.mast_loc where loctype = 50 and aloc = 1 order by locabb";
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
        else if (chkorgopt.SelectedValue == "All")
        {

            string sql = "SELECT locabb ,loccode FROM pshr.mast_loc where aloc = 1 order by locabb";
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



    }

    private void Fillbranch()
    {
        drpbranch.Items.Clear();
        if (optpost.SelectedValue == "2")
        {
            drpbranch.Items.Add(new ListItem("--Select--", "0"));
            drpbranch.Items.Add(new ListItem("Electrical", "1"));
            drpbranch.Items.Add(new ListItem("Civil", "4"));

        }
        else
        {
            string sql = "SELECT branchtext ,branchcode FROM pshr.mast_branch order by branchtext";
            System.Data.DataSet ds = OraDBConnection.GetData(sql);
            //drpbranch.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                drpbranch.Items.Add(new ListItem("--Select--", "0"));
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    drpbranch.Items.Add(new ListItem(row["branchtext"].ToString(), row["branchcode"].ToString()));
                }
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
    protected void optpost_SelectedIndexChanged(object sender, EventArgs e)
    {
        ControlControls();
        Fillbranch();
    }
    private void ControlControls()
    {

        if (optpost.SelectedValue == "0")
        {
            txtempid.Enabled = true;
            //rbWise.Enabled = false;
            //  drpbranch.Enabled = false;
           // drpcateg.Enabled = false;
        }

        else if (optpost.SelectedValue == "1")
        {
            txtempid.Enabled = false;
            drpbranch.Enabled = true;
            drpcateg.Enabled = true;
            //rbWise.Enabled = false;
            
        }


        else if (optpost.SelectedValue == "2")
        {
            txtempid.Enabled = false;
            //rbWise.Enabled = true;
            drpcateg.Enabled = false;
            
        }
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {
        Session.Contents.Add("empid", txtempid.Text);
        Session.Contents.Add("desg", drpdesg.SelectedValue);
        Session.Contents.Add("org", drporg.SelectedValue);
        Session.Contents.Add("branch", drpbranch.SelectedValue);
        Session.Contents.Add("categ", drpcateg.SelectedValue);
        if (chksloc.Checked == true)
        {
            Session.Contents.Add("sloc", 1);     
        }
        else if (chksloc.Checked == false)
        {
            Session.Contents.Add("sloc", 0);
        }


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
        else if (optpost.SelectedValue == "2")
        {
            Session.Contents.Add("loctype", chkorgopt.SelectedValue);
            //DW = Designation Wise
            //LW = Location Wise
            Session.Contents.Add("which_wise", rbWise.SelectedValue);
            Response.Redirect("frmrptvacancy.aspx");
            //Response.Write("<script type='text/javascript'>detailedresults=window.open('frmrptpplist.aspx','_new');</script>");
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

  
    protected void chkorgopt_SelectedIndexChanged1(object sender, EventArgs e)
    {
        Fillorg();
    }
    protected void rbWise_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (optpost.SelectedValue == "2")
        {
            if (rbWise.SelectedValue == "DW")
            {
                drpdesg.Enabled = true;
                drporg.Enabled = false;
            }
            else
            {
                drpdesg.Enabled = false;
                drporg.Enabled = true;
            }
        }
    }
    protected void btnshowpanel_Click(object sender, EventArgs e)
    {
        Session.Contents.Add("empid", txtempid.Text);
        Session.Contents.Add("desg", drpdesg.SelectedValue);
        Session.Contents.Add("org", drporg.SelectedValue);
        Session.Contents.Add("branch", drpbranch.SelectedValue);
        Session.Contents.Add("categ", drpcateg.SelectedValue);
        if (chksloc.Checked == true)
        {
            Session.Contents.Add("sloc", 1);
        }
        else if (chksloc.Checked == false)
        {
            Session.Contents.Add("sloc", 0);
        }


        if (optpost.SelectedValue == "0")
        {
            Response.Redirect("frmrptpanel.aspx");
            //Response.Write("<script type='text/javascript'>detailedresults=window.open('frmrptpdetails.aspx','_new');</script>");
        }
        //else if (optpost.SelectedValue == "1")
        //{
        //    Response.Redirect("frmrptpplist.aspx");
        //    //Response.Write("<script type='text/javascript'>detailedresults=window.open('frmrptpplist.aspx','_new');</script>");
        //}
        //else if (optpost.SelectedValue == "2")
        //{
        //    Session.Contents.Add("loctype", chkorgopt.SelectedValue);
        //    //DW = Designation Wise
        //    //LW = Location Wise
        //    Session.Contents.Add("which_wise", rbWise.SelectedValue);
        //    Response.Redirect("frmrptvacancy.aspx");
        //    //Response.Write("<script type='text/javascript'>detailedresults=window.open('frmrptpplist.aspx','_new');</script>");
        //}
    }
    protected void btnOPList_Click(object sender, EventArgs e)
    {
        string desglist = drpDesgs.SelectedValue;
        string sql = "select rownum as srno, a.* from (select pshr.get_fullname(e.empid) as name, " +
                        "md.desgabb as degn, e.empid as code, pshr.get_qual(e.empid) as qual, " +
                        "to_char(e.dob,'dd-mm-yyyy') as DOB, pshr.get_org(cloccode) as loc, "+
                        "to_char(pshr.get_since_lastloc(e.empid),'dd-mm-yyyy') as since, " +
                        "eaddr.phonecell as mobile "+
                        "from pshr.empperso e, PSHR.empaddr eaddr, PSHR.mast_desg md where "+
                        "e.cdesgcode in ("+desglist+") " +
                        "and eaddr.empid = e.empid "+
                        "and md.desgcode = e.cdesgcode and " +
                        "e.recstatus = 10 order by md.hecode, e.empid) a";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        
        DataGrid dg = new DataGrid();
        dg.DataSource = ds;
        dg.DataBind();
        Response.AddHeader("content-disposition", "attachment;filename=mappinglist.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter stringwrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlwrite = new System.Web.UI.HtmlTextWriter(stringwrite);
        //htmlwrite.WriteLine("TITLE");
        dg.RenderControl(htmlwrite);
        Response.Write(stringwrite.ToString());
        Response.End();
        dg.Dispose();
    }

    private void FillDesgsEngGaz()
    {
        drpDesgs.Items.Clear();
        drpDesgs.Items.Add(new ListItem("All Eng. Gaz.", "9047, 9048, 9365, 9050, 9366, 9052, 9056, 9057, 9060"));
        drpDesgs.Items.Add(new ListItem("EIC, CE", "9047, 9048"));
        drpDesgs.Items.Add(new ListItem("EIC", "9047"));
        drpDesgs.Items.Add(new ListItem("CE", "9048"));
        drpDesgs.Items.Add(new ListItem("Dy CE, SE", "9365, 9050"));
        drpDesgs.Items.Add(new ListItem("Dy CE", "9365"));
        drpDesgs.Items.Add(new ListItem("SE", "9050"));
        drpDesgs.Items.Add(new ListItem("Addl SE, Sr. XEN", "9366, 9052"));
        drpDesgs.Items.Add(new ListItem("Addl SE", "9366"));
        drpDesgs.Items.Add(new ListItem("Sr. XEN", "9052"));
        drpDesgs.Items.Add(new ListItem("AEE, AE, AE (OT)", "9056, 9057, 9060"));
        drpDesgs.Items.Add(new ListItem("AEE", "9056"));
        drpDesgs.Items.Add(new ListItem("AE", "9057"));
        drpDesgs.Items.Add(new ListItem("AE (OT)", "9060"));
    }
    protected void btnGetDesg_Click(object sender, EventArgs e)
    {
        string sql = string.Empty;
        string filtertext = Regex.Replace(txtDesgFilter.Text, @"[^\w\.%-_]", "").ToUpper();

        if (filtertext == "FILTER")
        {
            filtertext = "";
        }

        System.Data.DataSet ds;

        if (drpDesgCat.SelectedValue == "0")
        {
            FillDesgsEngGaz();
        }
        else if (drpDesgCat.SelectedValue == "100")
        {
            sql = "select desgtext, desgcode from pshr.mast_desg where adesg = 'A' " +
                " and desgtext like '%" + filtertext + "%' "+
                " order by hecode,desgtext";
            ds = OraDBConnection.GetData(sql);
            drpDesgs.DataSource = ds.Tables[0];
            drpDesgs.DataTextField = "desgtext";
            drpDesgs.DataValueField = "desgcode";
            drpDesgs.DataBind();
        }
        else
        {
            sql = "select desgtext, desgcode from pshr.mast_desg where adesg = 'A' "+
                    " and servcode = '" +drpDesgCat.SelectedValue + "' " +
                    " and desgtext like '%" + filtertext + "%' " +
                    " order by hecode,desgtext";
            ds = OraDBConnection.GetData(sql);
            drpDesgs.DataSource = ds.Tables[0];
            drpDesgs.DataTextField = "desgtext";
            drpDesgs.DataValueField = "desgcode";
            drpDesgs.DataBind();
        }
    }
    protected void btnOPList0_Click(object sender, EventArgs e)
    {
        string loccode = drporg.SelectedValue;
        string sql = string.Empty;

        sql = "SELECT lpad('.',4*(lvl-1),'.') || locname as loc, desgtext||'-'||indx as desg, emp.empid, " +
                "NVL(pshr.get_fullname(emp.empid),'') as name, emp.dob, pshr.get_qual(emp.empid) qual, " +
                 "pshr.get_org(cadre.get_since_post(emp.empid)) posted_at, cadre.get_since(emp.empid) since_dt, " +
                 "round((sysdate - cadre.get_since(emp.empid))/365.242, 2) span, " +
                 "pshr.get_retddate(emp.empid) DoR, round((pshr.get_retddate(emp.empid)-sysdate)/365.242, 2) Time_To_Ret " +
                "FROM " +
                  "(SELECT * " +
                  "FROM " +
                    "(SELECT rownum AS rno1,      posts.* " +
                    "FROM " +
                      "(SELECT c.loccode,        c.desgcode,        c.indx,        c.rowno,        ml.lvl,        ml.rnum_ml,        ml.locname,        md.hecode,        md.desgtext " +
                      "FROM pshr.mast_desg md, " +
                        "cadre.cadr c " +
                      "INNER JOIN " +
                        "(SELECT rownum AS rnum_ml,          level        AS lvl,          loccode,          locname " +
                        "FROM pshr.mast_loc " +
                          "CONNECT BY prior loccode=locrep " +
                          "START WITH loccode      = " + loccode +
                        ") ml " +
                      "ON c.loccode      = ml.loccode " +
                      "WHERE md.desgcode = c.desgcode " +
                      "AND servcode      = 30 " +
                      "AND gazcode       = 10 " +
                      "ORDER BY ml.rnum_ml, " +
                        "md.hecode,indx " +
                      ") posts " +
                    ") posts2 " +
                  "LEFT OUTER JOIN cadre.cadrmap cm " +
                  "ON cm.rowno = posts2.rowno " +
                  ") postmap " +
                "LEFT OUTER JOIN empperso emp " +
                "ON emp.empid = postmap.empid " +
                "ORDER BY rno1";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        DataGrid dg = new DataGrid();
        dg.DataSource = ds;
        dg.DataBind();
        Response.AddHeader("content-disposition", "attachment;filename=oplist.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter stringwrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlwrite = new System.Web.UI.HtmlTextWriter(stringwrite);
        //htmlwrite.WriteLine("TITLE");
        dg.RenderControl(htmlwrite);
        Response.Write(stringwrite.ToString());
        Response.End();
        dg.Dispose();
    }
}