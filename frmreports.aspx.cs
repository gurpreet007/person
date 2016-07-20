using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using OfficeOpenXml;
using System.IO;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Data;

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
        btnAllOO.Text = "All Office Orders";
        btnAllOO_PC.Text = "All Office Orders" + Environment.NewLine + "with Private Comments";
        btnAllOO_PC.Height = 46;
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
    private bool CreateOpListWorkSheet(string loccode, ExcelWorksheet worksheet)
    {
        //string loccode = drporg.SelectedValue;
        string locname = worksheet.Name;
        string sql = string.Empty;
        const int POS_HECODE = 15;
        const int POS_EMPID = 6;
        const int POS_CIRCLE = 2;
        const int POS_DIVISION = 3;
        const int NUM_COLS = 16;

        int col = 1;
        string hecode_val = string.Empty;
        string empid_val = string.Empty;
        //merge rows vars
        string latest_circle = string.Empty;
        int latest_circle_startrow = 0;
        int latest_circle_endrow = 0;
        string latest_division = string.Empty;
        int latest_division_startrow = 0;
        int latest_division_endrow = 0;
        //end merge rows vars

        const int startRow = 5;
        int row = startRow;

        btnOPList0.Enabled = false;
        sql = "SELECT " +
                "pshr.get_org_abb(get_repcode(loccode,'Z')) as Zone, pshr.get_org_abb(get_repcode(loccode,'C')) as Circle," +
                "pshr.get_org_abb(get_repcode(loccode,'D')) as Division, pshr.get_org_abb(get_repcode(loccode,'S')) as SubDivision," +
                "desgtext||'-'||indx as desg, nvl(emp.empid,0), " +
                "NVL(pshr.get_fullname(emp.empid),'Vacant') as name, to_char(emp.dob,'dd-Mon-yyyy') as dob, pshr.get_qual(emp.empid) qual, " +
                 "pshr.get_org_abb(cadre.get_since_post(emp.empid)) posted_at, to_char(cadre.get_since(emp.empid),'dd-Mon-yyyy') since_dt, " +
                 "round((sysdate - cadre.get_since(emp.empid))/365.242, 2) span, " +
                 "to_char(pshr.get_retddate(emp.empid),'dd-Mon-yyyy') DoR, "+
                 "round((pshr.get_retddate(emp.empid)-sysdate)/365.242, 2) Time_To_Ret, hecode, "+
                 "decode(branch,4, 'Civil',99,'Not Known',88,'Not Relevent', 'Electrical') as Branch " +
                "FROM " +
                  "(SELECT * " +
                  "FROM " +
                    "(SELECT rownum AS rno1,      posts.* " +
                    "FROM " +
                      "(SELECT c.loccode, c.desgcode, c.indx, c.rowno, ml.lvl, ml.rnum_ml, ml.locname, md.hecode, md.desgtext,c.branch " +
                      "FROM pshr.mast_desg md, " +
                        "cadre.cadr c " +
                      "INNER JOIN " +
                        "(SELECT rownum AS rnum_ml,          level        AS lvl,          loccode,          locname " +
                        "FROM pshr.mast_loc " +
                          "CONNECT BY loctype<>20 and prior loccode=locrep " +
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

        

        // get handle to the existing worksheet
        //ExcelWorksheet worksheet;

        if (worksheet == null)
            return false;

        //main header
        worksheet.Cells["A1"].Value = "Oplist for " + locname;
        using (ExcelRange r = worksheet.Cells["A1:G1"])
        {
            r.Merge = true;
            r.Style.Font.SetFromFont(new Font("Britannic Bold", 22, FontStyle.Italic));
            r.Style.Font.Color.SetColor(Color.White);
            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
        }

        //headings
        worksheet.Cells["A4"].Value = "Zone";
        worksheet.Cells["B4"].Value = "Circle";
        worksheet.Cells["C4"].Value = "Division";
        worksheet.Cells["D4"].Value = "SubDivision";
        worksheet.Cells["E4"].Value = "Desg";
        worksheet.Cells["F4"].Value = "Empid";
        worksheet.Cells["G4"].Value = "Name";
        worksheet.Cells["H4"].Value = "DOB";
        worksheet.Cells["I4"].Value = "Qual";
        worksheet.Cells["J4"].Value = "Posting";
        worksheet.Cells["K4"].Value = "Since";
        worksheet.Cells["L4"].Value = "Span";
        worksheet.Cells["M4"].Value = "DOR";
        worksheet.Cells["N4"].Value = "ToR";
        worksheet.Cells["O4"].Value = "HC";
        worksheet.Cells["P4"].Value = "Branch";
        worksheet.Cells["A4:P4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        worksheet.Cells["A4:P4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        worksheet.Cells["A4:P4"].Style.Font.Bold = true;

        //actual data rows
        DataRowCollection drows = OraDBConnection.GetData(sql).Tables[0].Rows;
        if (drows.Count == 0)
            return false;

        foreach (DataRow drow in drows)
        {
            col = 1;
            foreach (object item in drow.ItemArray)
            {
                if (item != null)
                    worksheet.Cells[row, col].Value = item;
                col++;
            }
            hecode_val = worksheet.Cells[row, POS_HECODE].Value.ToString();
            empid_val = worksheet.Cells[row, POS_EMPID].Value.ToString();
            //merge circle rows
            if (row == startRow)
            {
                latest_circle = worksheet.Cells[row, POS_CIRCLE].Value.ToString();
                latest_circle_startrow = row;
            }
            else if (latest_circle != worksheet.Cells[row, POS_CIRCLE].Value.ToString())
            {
                //set end row
                latest_circle_endrow = row - 1;

                if (!string.IsNullOrWhiteSpace(latest_circle))
                {
                    //merge cols
                    worksheet.Cells[latest_circle_startrow, POS_CIRCLE, latest_circle_endrow, POS_CIRCLE].Merge = true;
                    worksheet.Cells[latest_circle_startrow, POS_CIRCLE, latest_circle_endrow, POS_CIRCLE].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                //set new latest_circle
                latest_circle = worksheet.Cells[row, POS_CIRCLE].Value.ToString();
                //reinit startrow
                latest_circle_startrow = row;
            }
            //end merge circle rows

            //merge div rows
            if (row == startRow)
            {
                latest_division = worksheet.Cells[row, POS_DIVISION].Value.ToString();
                latest_division_startrow = row;
            }
            else if (latest_division != worksheet.Cells[row, POS_DIVISION].Value.ToString())
            {
                //set end row
                latest_division_endrow = row - 1;
                if (!string.IsNullOrWhiteSpace(latest_division))
                {
                    //merge cols
                    worksheet.Cells[latest_division_startrow, POS_DIVISION, latest_division_endrow, POS_DIVISION].Merge = true;
                    worksheet.Cells[latest_division_startrow, POS_DIVISION, latest_division_endrow, POS_DIVISION].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                //set new latest_div
                latest_division = worksheet.Cells[row, POS_DIVISION].Value.ToString();
                //reinit startrow
                latest_division_startrow = row;
            }
            //end merge div rows

            if (hecode_val == "1" || hecode_val == "2")
            {
                ExcelRange r = worksheet.Cells[row, 1, row, NUM_COLS];
                r.Style.Font.SetFromFont(new Font("Britannic Bold", 10, FontStyle.Regular));
                r.Style.Font.Color.SetColor(Color.Red);
            }
            else if (hecode_val == "3" || hecode_val == "4")
            {
                ExcelRange r = worksheet.Cells[row, 1, row, NUM_COLS];
                r.Style.Font.SetFromFont(new Font("Britannic Bold", 10, FontStyle.Regular));
                r.Style.Font.Color.SetColor(Color.Blue);
            }
            else if (hecode_val == "5" || hecode_val == "6")
            {
                ExcelRange r = worksheet.Cells[row, 1, row, NUM_COLS];
                r.Style.Font.SetFromFont(new Font("Britannic Bold", 10, FontStyle.Regular));
                r.Style.Font.Color.SetColor(Color.Magenta);
            }
            else if (hecode_val == "7" || hecode_val == "8" || hecode_val == "9")
            {
                ExcelRange r = worksheet.Cells[row, 1, row, NUM_COLS];
                r.Style.Font.SetFromFont(new Font("Britannic Bold", 10, FontStyle.Regular));
                r.Style.Font.Color.SetColor(Color.Black);
            }

            //if (string.IsNullOrWhiteSpace(empid_val))
            //{
            //    ExcelRange r = worksheet.Cells[row, 5, row, NUM_COLS];
            //    r.Style.Font.SetFromFont(new Font("Britannic Bold", 10, FontStyle.Regular));
            //    r.Style.Font.Color.SetColor(Color.Purple);
            //}

            row++;
        }
        //merge last set of circle
        //set end row
        latest_circle_endrow = row - 1;
        //merge cols
        if (!string.IsNullOrWhiteSpace(latest_circle))
        {
            worksheet.Cells[latest_circle_startrow, POS_CIRCLE, latest_circle_endrow, POS_CIRCLE].Merge = true;
        }

        //merge last set of division
        //set end row
        latest_division_endrow = row - 1;
        //merge cols
        if (!string.IsNullOrWhiteSpace(latest_division))
        {
            worksheet.Cells[latest_division_startrow, POS_DIVISION, latest_division_endrow, POS_DIVISION].Merge = true;
        }

        //Merge Zone Column
        worksheet.Cells["A5:A" + (row - 1)].Merge = true;
        worksheet.Cells["A5:A" + (row - 1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //zone
        worksheet.Column(1).Width = 25;
        //circle
        worksheet.Column(2).Width = 25;
        //div
        worksheet.Column(3).Width = 25;
        //subdiv
        worksheet.Column(4).Width = 30;
        //desg
        worksheet.Column(5).Width = 30;
        //empid
        worksheet.Column(6).Width = 7;
        //name
        worksheet.Column(7).Width = 20;
        //dob
        worksheet.Column(8).Width = 10;
        //qual
        worksheet.Column(9).Width = 15;
        //posting
        worksheet.Column(10).Width = 30;
        //since
        worksheet.Column(11).Width = 10;
        //span
        worksheet.Column(12).Width = 5;
        //dor
        worksheet.Column(13).Width = 10;
        //ToR
        worksheet.Column(14).Width = 5;
        //hecode
        worksheet.Column(15).Width = 3;
        //branch
        worksheet.Column(16).Width = 12;

        worksheet.Row(1).Height = 40;

        worksheet.Column(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(4).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(5).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(6).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(7).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(8).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(9).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(10).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(11).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(12).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(13).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(14).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(15).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        worksheet.Column(16).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        return true;
    }
    private void TestOPList()
    {
        string file = Server.MapPath("office_orders\\auto_oplist.xlsx");
        bool retVal;
        if (File.Exists(file)) File.Delete(file);
        FileInfo newFile = new FileInfo(file);
        ExcelWorksheet sheet;
        ExcelWorksheet origSheet;
        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
        {
            sheet = xlPackage.Workbook.Worksheets.Add("one");
            sheet.Cells[1, 1, 10, 10].Value = "1";
            sheet = xlPackage.Workbook.Worksheets.Add("two");
            sheet.Cells[1, 1, 10, 10].Value = "2";
            sheet = xlPackage.Workbook.Worksheets.Add("three");
            origSheet = xlPackage.Workbook.Worksheets[1];
            sheet.Cells[1, 1, 10, 10].Value = origSheet.Cells[1, 1, 10, 10].Value;
            origSheet = xlPackage.Workbook.Worksheets[2];
            sheet.Cells[11, 1, 20, 10].Value = origSheet.Cells[1, 1, 10, 10].Value;
            xlPackage.Save();
        }
        btnOPList0.Enabled = true;
        Utils.DownloadFile(file, true, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
    protected void btnOPList0_Click(object sender, EventArgs e)
    {
        //TestOPList();
        //return;
        string file = Server.MapPath("office_orders\\auto_oplist.xlsx");
        string sql = "select loccode,locabb from pshr.mast_loc where loccode like '___000000' and aloc=1 and loctype in (11,20) order by locabb";
        DataSet ds = OraDBConnection.GetData(sql);
        bool retVal;
        if (File.Exists(file)) File.Delete(file);
        FileInfo newFile = new FileInfo(file);
        ExcelWorksheet sheet;
        const int NUM_COLS=16;

        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
        {
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                sheet = xlPackage.Workbook.Worksheets.Add(drow["locabb"].ToString());
                retVal = CreateOpListWorkSheet(drow["loccode"].ToString(), sheet);
                if (!retVal)
                {
                    xlPackage.Workbook.Worksheets.Delete(sheet);
                }
            }
            
            //combined sheet
            ExcelWorksheet combined = xlPackage.Workbook.Worksheets.Add("Combined");
            ExcelWorksheet origSheet;
            
            //headings
            xlPackage.Workbook.Worksheets[1].Cells[4, 1, 4, NUM_COLS].Copy(combined.Cells[1,1]);

            //sheets loop
            for (int i = 1, currRow = 2; i <= xlPackage.Workbook.Worksheets.Count - 1; i++)
            {
                origSheet = xlPackage.Workbook.Worksheets[i];
                for (int r = 5; origSheet.Cells[r, 1].Value != null; r++, currRow++)
                {
                    origSheet.Cells[r, 1, r, NUM_COLS].Copy(combined.Cells[currRow, 1]);
                }
            }

            xlPackage.Save();
        }
        btnOPList0.Enabled = true;
        Utils.DownloadFile(file, true, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
    protected void btnAllOO_Click(object sender, EventArgs e)
    {
        string empid = txtempid.Text;
        string sql = "select m.sno,tpp.oonum as oonum, "+
            "to_char(tpp.oodate,'dd-mm-yyyy') as oodate, "+
            "'12' as fsize, pshr.get_fullname(m.empid),to_char(m.empid) as empid,pshr.get_dob(m.empid) as dob,"+
            "pshr.get_post(m.oldloccode) as old_work_loc,m.oldloccode as old_work_loccode,pshr.get_desg(m.olddesgcode) as old_work_desg, m.olddesgcode as old_work_desgcode,"+
            "DECODE(m.rowno,0,pshr.get_post(m.oldloccode), pshr.get_post(cadre.get_lcode_rno(m.rowno))) AS old_pc_loc,"+
            "DECODE(m.rowno,0,m.oldloccode, cadre.get_lcode_rno(m.rowno)) AS old_pc_loccode,"+
            "DECODE(m.rowno,0,pshr.get_desg(m.olddesgcode), pshr.get_desg(cadre.get_dcode_rno(m.rowno))) AS old_pc_desg, "+
            "DECODE(m.rowno,0,m.olddesgcode, cadre.get_dcode_rno(m.rowno)) AS old_pc_desgcode, "+
            "DECODE(m.rowno,0,'0', cadre.get_indx_rno(m.rowno)) AS old_pc_indx,  cadre.get_org_plants(m.cloccode) as new_work_loc,"+
            "m.cloccode as new_work_loccode,pshr.get_desg(m.cdesgcode) as new_work_desg,m.cdesgcode as new_work_desgcode,"+
            "decode(length(m.proposed_rowno),9,pshr.get_post(m.proposed_rowno), cadre.get_org_plants(cadre.get_lcode_rno(m.proposed_rowno))) AS new_pc_loc,"+
            "decode(length(m.proposed_rowno),9,m.proposed_rowno, cadre.get_lcode_rno(m.proposed_rowno)) AS new_pc_loccode,"+
            "DECODE(m.rowno,0,pshr.get_desg(m.olddesgcode), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno))) AS new_pc_desg, "+
            "DECODE(m.rowno,0,m.olddesgcode, cadre.get_dcode_rno(m.proposed_rowno)) AS new_pc_desgcode, "+
            "cadre.get_indx_rno(m.proposed_rowno) as new_pc_indx, m.remarks,'G' as grp,m.propno, to_char(m.newempid) as newempid, m.status, "+
            "m.disp_left, m.disp_right, pshr.get_soccat(m.empid) as categ "+
            "from cadre.propcadrmap m, cadre.tp_proposals tpp  "+
            "where m.propno = tpp.pno and m.status is not null AND M.STATUS NOT IN ('S','V') "+
            "and m.propno in (select propno from cadre.propcadrmap where empid = "+empid+") and tpp.status='S' " +
            "AND m.cloccode is not null order by oodate,sno";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        string pdfPath;
        pdfPath = Server.MapPath("office_orders\\all_orders_"+empid+".pdf");

        CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\all_rptposttrans.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();

        CrystalReportSource1.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfPath);
        Utils.DownloadFile(pdfPath);
    }
    protected void btnAllOO_PC_Click(object sender, EventArgs e)
    {
        string empid = txtempid.Text;
        string sql = "select m.sno,tpp.oonum as oonum, " +
            "to_char(tpp.oodate,'dd-mm-yyyy') as oodate, " +
            "'12' as fsize, pshr.get_fullname(m.empid),to_char(m.empid) as empid,pshr.get_dob(m.empid) as dob," +
            "pshr.get_post(m.oldloccode) as old_work_loc,m.oldloccode as old_work_loccode,pshr.get_desg(m.olddesgcode) as old_work_desg, m.olddesgcode as old_work_desgcode," +
            "DECODE(m.rowno,0,pshr.get_post(m.oldloccode), pshr.get_post(cadre.get_lcode_rno(m.rowno))) AS old_pc_loc," +
            "DECODE(m.rowno,0,m.oldloccode, cadre.get_lcode_rno(m.rowno)) AS old_pc_loccode," +
            "DECODE(m.rowno,0,pshr.get_desg(m.olddesgcode), pshr.get_desg(cadre.get_dcode_rno(m.rowno))) AS old_pc_desg, " +
            "DECODE(m.rowno,0,m.olddesgcode, cadre.get_dcode_rno(m.rowno)) AS old_pc_desgcode, " +
            "DECODE(m.rowno,0,'0', cadre.get_indx_rno(m.rowno)) AS old_pc_indx,  cadre.get_org_plants(m.cloccode) as new_work_loc," +
            "m.cloccode as new_work_loccode,pshr.get_desg(m.cdesgcode) as new_work_desg,m.cdesgcode as new_work_desgcode," +
            "decode(length(m.proposed_rowno),9,pshr.get_post(m.proposed_rowno), cadre.get_org_plants(cadre.get_lcode_rno(m.proposed_rowno))) AS new_pc_loc," +
            "decode(length(m.proposed_rowno),9,m.proposed_rowno, cadre.get_lcode_rno(m.proposed_rowno)) AS new_pc_loccode," +
            "DECODE(m.rowno,0,pshr.get_desg(m.olddesgcode), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno))) AS new_pc_desg, " +
            "DECODE(m.rowno,0,m.olddesgcode, cadre.get_dcode_rno(m.proposed_rowno)) AS new_pc_desgcode, " +
            "cadre.get_indx_rno(m.proposed_rowno) as new_pc_indx, m.remarks,'G' as grp,m.propno, to_char(m.newempid) as newempid, m.status, " +
            "m.disp_left, m.disp_right, m.prvcomment, pshr.get_soccat(m.empid) as categ " +
            "from cadre.propcadrmap m, cadre.tp_proposals tpp  " +
            "where m.propno = tpp.pno and m.status is not null AND M.STATUS NOT IN ('S','V') " +
            "and m.propno in (select propno from cadre.propcadrmap where empid = " + empid + ") and tpp.status='S' " +
            "AND m.cloccode is not null order by oodate,sno";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        string pdfPath;
        pdfPath = Server.MapPath("office_orders\\all_orders_" + empid + ".pdf");

        CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\all_rptposttrans_pc.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();

        CrystalReportSource1.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfPath);
        Utils.DownloadFile(pdfPath);
    }
    protected void btnVacancy_Click(object sender, EventArgs e)
    {
        string sql = string.Empty;

        sql = "select pshr.get_org_abb(GET_REPCODE(loccode,'Z')) as Zone, pshr.get_org(loccode) loc, pshr.get_desg(desgcode) desg, indx " +
                "from cadre.cadr natural join pshr.mast_desg " +
                "where servcode=30 and gazcode=10 and rowno not in (select rowno from cadre.cadrmap)" +
                "order by hecode, loccode";

        Utils.DownloadXLS(sql, "vacancy_list.xls", this);
    }
    protected void btnAOOSummary_Click(object sender, EventArgs e)
    {
        string sql = string.Empty;
        string empid = txtempid.Text;

        if (string.IsNullOrWhiteSpace(empid))
        {
            return;
        }
        sql = "select cr.empid, pshr.get_fullname(cr.empid) as name, cr.oonum,cr.oodate, " +
                "cadre.get_mapping_text_from_rowno(cr.postrel) as postrel, " +
                "cadre.get_mapping_text_from_rowno(cr.postjoin) as postjoin," +
                "pshr.get_org(cr.loccode) wloc, pshr.get_desg(cr.desgcode) wdesg," +
                "decode(cr.eventcode, 36, 'Trans_Pub',37,'Trans_Own',28, 'Pro') as event,cr.status," +
                "remarks,sysremarks, prvcomment " +
                "from cadre.propcadrmap pcm inner join cadre.tp_proposals tpp on pcm.propno = tpp.pno " +
                "inner join cadre.chargereport cr on cr.empid= pcm.empid and cr.oonum=tpp.oonum " +
                "where pcm.empid in (" + empid + ")" +
                "order by empid, oodate";
        Utils.DownloadXLS(sql, "off_order_summary" + empid + ".xls", this);
    }
}
