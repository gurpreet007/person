using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public class Utils
{
    public static Dictionary<string, string> frmaddresses = new Dictionary<string, string>();

    public static int last_cadre_cd;
	public static void ShowMessageBox(System.Web.UI.Page page,string message)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "msgbox", "<script>alert('" + message + "');</script>");
    }

    public static bool UpdateEmpperso(string empid)
    {
        string sql = String.Format("SELECT loccode, desgcode FROM emphistory WHERE empid={0} ORDER BY rowno DESC", empid);
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        System.Text.StringBuilder updatesql = new System.Text.StringBuilder("UPDATE empperso SET ");

        //update posting location
        long val = 0;
        long loccode = 0;
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            val = (row["loccode"] != DBNull.Value) ? Convert.ToInt64(row["loccode"]) : 0;
            if (val != 0 && val != 88888 && val != 99999)
            {
                loccode = val;
                updatesql.Append(String.Format("cloccode = {0},", loccode));
                break;
            }
        }

        //update current designation code
        val = 0;
        long desgcode = 0;
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            val = val = (row["desgcode"] != DBNull.Value) ? Convert.ToInt64(row["desgcode"]) : 0;
            if (val != 0 && val != 8888 && val != 9999)
            {
                desgcode = val;
                updatesql.Append(String.Format("cdesgcode = {0},", desgcode));
                break;
            }
        }

        updatesql.Append(String.Format(" WHERE empid={0}", empid));
        updatesql.Replace(", WHERE", " WHERE");
        sql = updatesql.ToString();
        return OraDBConnection.ExecQry(sql);
    }

    private static bool Generate_Grades(int cad_cd)
    {
        string sql;
        DataSet ds_sel;

        sql = "DELETE FROM cadre.empgrdno";
        OraDBConnection.ExecQry(sql);

        sql = "SELECT s.empid, s.senno, s.sendate, d.cadrecode, d.hecode, e.revcategory, e.branchcode " +
                "FROM pshr.empsen s, pshr.mast_desg d, pshr.empperso e, pshr.mast_cadre x " +
                "WHERE s.empid=e.empid AND e.cdesgcode = d.desgcode AND s.senno BETWEEN '1' AND '999999' AND " +
                "d.cadrecode = (SELECT u.cadregrp FROM mast_cadre u WHERE u.cadrecode = " + cad_cd + ") " +
                "AND d.cadrecode = x.cadrecode " +
                "AND e.branchcode = (SELECT j.branchcode FROM mast_cadre j WHERE j.cadrecode = " + cad_cd + ") " +
                "AND e.recstatus = 10 ORDER BY senno";
        ds_sel = OraDBConnection.GetData(sql);

        if (ds_sel.Tables[0].Rows.Count == 0)
        {
            return false;
        }

        int senno = 0;
        foreach (DataRow drow in ds_sel.Tables[0].Rows)
        {
            senno++;
            sql = string.Format("INSERT INTO cadre.empgrdno VALUES({0},      '0', {1},                                            {2},               {3},   {4},                 {5})",
                                                             drow["empID"], (drow["hecode"] == System.DBNull.Value) ? 99 : drow["hecode"], drow["cadrecode"], senno, drow["revcategory"], drow["branchcode"]);
            OraDBConnection.ExecQry(sql);
        }

        senno = 0;
        sql = " SELECT * FROM cadre.empgrdno ORDER BY hecode, senno";
        ds_sel = OraDBConnection.GetData(sql);
        int grp_cd = 9999;
        foreach (DataRow drow in ds_sel.Tables[0].Rows)
        {
            if (int.Parse(drow["hecode"].ToString()) != grp_cd)
            {
                senno = 1;
                grp_cd = int.Parse(drow["hecode"].ToString());
            }
            else
            {
                senno++;
            }
            sql = "UPDATE cadre.empgrdno SET grdno = " + senno + " WHERE empid = " + drow["empid"];
            OraDBConnection.ExecQry(sql);
        }

        return true;
    }

    private static void Post(int reqdesgcode)
    {
        string sql = "SELECT p.empid FROM pshr.empperso p, cadre.empgrdno s WHERE p.empid  = s.empid";

        //trying to optimize, comment the following line if results are not right
        sql += " AND p.cdesgcode = " + reqdesgcode;

        DataSet ds1 = OraDBConnection.GetData(sql);
        DataSet ds2;
        int desgcode,loccode;
        int pDesg = 0, pLoc = 0;
        System.DateTime dtPost = DateTime.Now;
        System.DateTime dtFirstPost = DateTime.Now;

        foreach (DataRow row1 in ds1.Tables[0].Rows)
        {
            sql = "SELECT * FROM emphistory WHERE empid = " + row1["empID"] + " ORDER BY empid,rowno DESC ";
            ds2 = OraDBConnection.GetData(sql);

            if (ds2.Tables[0].Rows.Count > 0)
            {   
                //get current designation
                foreach (DataRow row2 in ds2.Tables[0].Rows)
                {
                    int.TryParse(row2["desgcode"].ToString(),out desgcode);
                    if (desgcode != 9999 && desgcode != 8888)
                    {
                        pDesg = desgcode;
                        break;
                    }
                }

                //get current location
                foreach (DataRow row2 in ds2.Tables[0].Rows)
                {
                    int.TryParse(row2["loccode"].ToString(), out loccode);
                    if (loccode != 99999 && loccode != 88888)
                    {
                        pLoc = loccode;
                        DateTime.TryParse(row2["Todate"].ToString(),out dtPost);
                        break;
                    }
                }

                sql = "SELECT * FROM emphistory WHERE empid=" + row1["empID"] + " AND desgcode=" + pDesg + " ORDER BY empid,rowno";
                ds2 = OraDBConnection.GetData(sql);

                //get first date in current designation
                foreach (DataRow row2 in ds2.Tables[0].Rows)
                {
                    int.TryParse(row2["desgcode"].ToString(),out desgcode);
                    if (desgcode != 99999 && desgcode != 88888)
                    {
                        DateTime.TryParse(row2["Todate"].ToString(), out dtFirstPost);
                        break;
                    }
                }

                sql = "DELETE FROM pshr.empposting WHERE empid = " + row1["empID"];
                OraDBConnection.ExecQry(sql);


                sql = string.Format("INSERT INTO pshr.empposting (EMPID,         PDESGCODE, PLOCCODE, DTDESG,                                   DTPOST) VALUES " +
                                                                "({0},           {1},       {2},      '{3}',                                    '{4}')",
                                                                  row1["empID"], pDesg,     pLoc,     string.Format("{0:dd-MMM-yyyy}", dtPost), string.Format("{0:dd-MMM-yyyy}", dtFirstPost));
                OraDBConnection.ExecQry(sql);
            }
        }
    }
    public static DataSet GetSeniorityList(int reqdesgcode)
    {
        string sql;

        sql = "SELECT cadrecode FROM pshr.mast_desg WHERE desgcode = " + reqdesgcode;
        int cadrecd = int.Parse(OraDBConnection.GetScalar(sql).ToString());

        Generate_Grades(cadrecd);
        Post(reqdesgcode);

        if (cadrecd != last_cadre_cd)
        {
            Generate_Grades(cadrecd);
            Post(reqdesgcode);
            last_cadre_cd = cadrecd;
        }

        sql = "SELECT p.empid empID,s.senno,p.firstname||' '||p.middlename||' '||p.lastname full_name, " +
                        " r.REVCATEGTEXT, p.dob,p.doj,o.DTPOST,d.desgtext desg, " +
                        " decode(d.servcode,30, b.branchtext,' ') branch, d.hecode , c.cadrename, pshr.get_post(cloccode) as location" +
                        " FROM empperso p, cadre.empgrdno s, empposting o, mast_desg d, mast_cadre c, " +
                        " mast_branch b, mast_loc l, MAST_REVCATEG r WHERE p.empid = s.empid AND " +
                        " s.empid = o.empid AND o.pdesgcode = d.desgcode AND s.senno between 1 AND  900000 AND " +
                        " o.ploccode = l.loccode AND p.branchcode = b.branchcode AND " +
                        " c.cadrecode = d.cadrecode AND p.REVCATEGORY = r.REVCATEGCODE AND " +
                        " d.desgcode = " + reqdesgcode + " ORDER BY senno";
        DataSet ds = OraDBConnection.GetData(sql);
        return ds;
    }

    public static void DownloadFile(String pdfPath, bool autoDelete = true, string content_type = "application/pdf")
    {
        System.IO.FileInfo objFi = new System.IO.FileInfo(pdfPath);
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + objFi.Name);
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AddHeader("Content-Length", objFi.Length.ToString());
        HttpContext.Current.Response.ContentType = content_type;
        HttpContext.Current.Response.WriteFile(objFi.FullName);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.Close();
        if (autoDelete)
        {
            System.IO.File.Delete(pdfPath);
        }
    }

    public static bool DownloadXLS(string sql, string filename, Page pg)
    {
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        if (ds.Tables[0].Rows.Count == 0)
        {
            return false;
        }
        DataGrid dg = new DataGrid();
        dg.DataSource = ds;
        dg.DataBind();
        pg.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        pg.Response.Charset = "";
        pg.Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter stringwrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlwrite = new System.Web.UI.HtmlTextWriter(stringwrite);
        //htmlwrite.WriteLine("TITLE");
        dg.RenderControl(htmlwrite);
        pg.Response.Write(stringwrite.ToString());
        pg.Response.End();
        dg.Dispose();
        return true;
    }
}