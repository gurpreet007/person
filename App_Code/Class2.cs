using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using System.Data.Odbc;
using System.Data.OleDb;
using System.Text;
using System.IO;
//using ICSharpCode.SharpZipLib.Zip;
using System.Data.OracleClient;
//using Oracle.DataAccess.Client;
/// <summary>
/// Summary description for Class2
/// </summary>
public class Class2
{
    private static String connectionString()
    {
        return "user id=pshr;password=PinkWater2010;data source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.10.1.99)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=pshr)))";
    }

    
    public static Boolean execute(String sql)
    {
        OracleConnection con = new OracleConnection(connectionString());
        OracleCommand cmd = new OracleCommand(sql, con);
        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }
        catch (Exception e)
        {
            //MessageBox.Show("error executing query:\n" + e.ToString());
            return false;
        }
    }
	 public static void dirpdf(string pdfpath)
    {
        System.IO.FileInfo objfi = new System.IO.FileInfo(pdfpath);
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + objfi.Name);
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AddHeader("Content-Lenght", objfi.Length.ToString());
        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.WriteFile(objfi.FullName);
        HttpContext.Current.Response.End();
            
    }
 	 public static void open__file2(string file__path,string new_name)
    {

        FileInfo objFi = new FileInfo(file__path);
        HttpContext.Current.Response.Clear();
        new_name = new_name+".xls";
       // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + objFi.Name);
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename="+new_name);
        HttpContext.Current.Response.Charset = "";

        HttpContext.Current.Response.AddHeader("Content-Length", objFi.Length.ToString());


        HttpContext.Current.Response.ContentType = "application/octet-stream";

        HttpContext.Current.Response.WriteFile(objFi.FullName);

        HttpContext.Current.Response.End();
    }
    public static String getScaler(String sql)
    {
        OracleConnection con = new OracleConnection(connectionString());
        OracleCommand cmd = new OracleCommand(sql, con);
        String s;
        con.Open();
        
        s = "" + cmd.ExecuteScalar();
        con.Close();
        con.Dispose();
        return s;
    }

    public static DataSet getResut(String sql)
    {
        OracleDataAdapter adp = new OracleDataAdapter(sql, connectionString());
        DataSet ds = new DataSet();
        adp.Fill(ds);
        return ds;
    }
    public static void SetFocus(Control control)
    {
        //StringBuilder sb = new StringBuilder();
        
        //sb.Append("\r\n<script language='JavaScript'>\r\n");
        //sb.Append("<!--\r\n");
        //sb.Append("function SetFocus()\r\n");
        //sb.Append("{\r\n");
        //sb.Append("\tdocument.");

        //Control p = control.Parent;
        //while (!(p is System.Web.UI.HtmlControls.HtmlForm)) p = p.Parent;

        //sb.Append(p.ClientID);
        //sb.Append("['");
        //sb.Append(control.UniqueID);
        //sb.Append("'].focus();\r\n");
        //sb.Append("}\r\n");
        //sb.Append("window.onload = SetFocus;\r\n");
        //sb.Append("// -->\r\n");
        //sb.Append("</script>");

        //control.Page.RegisterClientScriptBlock("SetFocus", sb.ToString());

    }


    public static void AlertMsg(System.Web.UI.Page senderPage, string alertMsg)
    {
        string strScript;
        strScript = "<script language=JavaScript> alert('" + alertMsg + "') </script>";
        if (!(senderPage.IsStartupScriptRegistered("OnClick")))
            senderPage.RegisterStartupScript("OnClick", strScript);
    }
    public static void Show(string message)
    {
        // Cleans the message to allow single quotation marks 
        string cleanMessage = message.Replace("'", "\'");
        string script = "<script type='text/javascript'>alert('" + cleanMessage + "');</script>";

        // Gets the executing web page 
        Page page = HttpContext.Current.CurrentHandler as Page;

        // Checks if the handler is a Page and that the script isn't allready on the Page 
        //if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        //{
        //    page.ClientScript.RegisterClientScriptBlock(typeof(alert), "alert", script);
        //}
    }
    public static String NumberToText(int number)
    {
        if (number == 0) return "Zero";

        if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";

        int[] num = new int[4];
        int first = 0;
        int u, h, t;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (number < 0)
        {
            sb.Append("Minus ");
            number = -number;
        }

        string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ", 
"Five " ,"Six ", "Seven ", "Eight ", "Nine "};

        string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", 
"Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};

        string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", 
"Seventy ","Eighty ", "Ninety "};

        string[] words3 = { "Thousand ", "Lakh ", "Crore " };

        num[0] = number % 1000; // units 
        num[1] = number / 1000;
        num[2] = number / 100000;
        num[1] = num[1] - 100 * num[2]; // thousands 
        num[3] = number / 10000000; // crores 
        num[2] = num[2] - 100 * num[3]; // lakhs 

        for (int i = 3; i > 0; i--)
        {
            if (num[i] != 0)
            {
                first = i;
                break;
            }
        }


        for (int i = first; i >= 0; i--)
        {
            if (num[i] == 0) continue;

            u = num[i] % 10; // ones 
            t = num[i] / 10;
            h = num[i] / 100; // hundreds 
            t = t - 10 * h; // tens 

            if (h > 0) sb.Append(words0[h] + "Hundred ");

            if (u > 0 || t > 0)
            {
                //if (h > 0 || i == 0) sb.Append("and "); 

                if (t == 0)
                    sb.Append(words0[u]);
                else if (t == 1)
                    sb.Append(words1[u]);
                else
                    sb.Append(words2[t - 2] + words0[u]);
            }

            if (i != 0) sb.Append(words3[i - 1]);

        }
        return sb.ToString().TrimEnd();
    }
    public static String numset(string n)
    {
        string r;
        int i = n.Length;
        if (i == 2 || i < 2)
        {
            r = n;
            return r.ToString().TrimEnd();
        }

        r = n.Insert(i - 3, ",");
        if (i == 4 || i < 4)
        {
            if (r.StartsWith(","))
            {
                r = r.Substring(1);
            }

            return r.ToString().TrimEnd();
        }
        for (int b = i - 5; i > 0; i--)
        {

            r = r.Insert(b, ",");
            b = b - 2;
            if (b < 0)
            {
                if (r.StartsWith(","))
                {
                    r = r.Substring(1);
                }
                
                break;

            }
        }
        return r.ToString().TrimEnd();
        


    }


    public static void Convert_to_dat(String sql,string file_name)
    {
        string gg = sql;

        DataSet ds = Class2.getResut(gg);
        //dataAdapter.Fill(ds, "t394");
        DataTable dt = ds.Tables[0];
        int[] maxLengths = new int[dt.Columns.Count];
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            maxLengths[i] = dt.Columns[i].ColumnName.Length;
            foreach (DataRow row in dt.Rows)
            {
                if (!row.IsNull(i))
                {
                    int length = row[i].ToString().Length;
                    if (length > maxLengths[i])
                    { maxLengths[i] = length; }
                }
            }
        }

        using (StreamWriter sw = new StreamWriter("c:/bankcd/" + file_name+"/"+file_name + ".dat", false))
        {
            for (int i = 1; i < dt.Columns.Count; i++)
            { 
                sw.Write(dt.Columns[i].ColumnName.PadRight(maxLengths[i] + 2)); 
            }
            //sw.WriteLine();
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!row.IsNull(i))
                    { sw.Write(row[i].ToString().PadRight(maxLengths[i] + 2)); }
                    else { sw.Write(new string(' ', maxLengths[i] + 2)); }
                }
                sw.WriteLine();
            }
            sw.Close();
        }
    }
    public static void convert_to_doc(string sql,string file_name)
    {
        string gg = sql;

        DataSet ds = Class2.getResut(gg);
        //dataAdapter.Fill(ds, "t394");
        DataTable dt = ds.Tables[0];
        int[] maxLengths = new int[dt.Columns.Count];
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            maxLengths[i] = dt.Columns[i].ColumnName.Length;
            foreach (DataRow row in dt.Rows)
            {
                if (!row.IsNull(i))
                {
                    int length = row[i].ToString().Length;
                    if (length > maxLengths[i])
                    { maxLengths[i] = length; }
                }
            }
        }
        using (StreamWriter sw = new StreamWriter("c:/bankcd/" + file_name + "/" + file_name + ".doc", false))
        
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            { sw.Write(dt.Columns[i].ColumnName.PadRight(maxLengths[i] + 2)); }
            sw.WriteLine();
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!row.IsNull(i))
                    { sw.Write(row[i].ToString().PadRight(maxLengths[i] + 2)); }
                    else { sw.Write(new string(' ', maxLengths[i] + 2)); }
                }
                sw.WriteLine();
            }
            string y = Class2.numset(Class2.getScaler("select sum(approved_amount) TOTAL from bank"));
            sw.WriteLine("-------------------------------------------------------------------------------------------------");

            sw.Write("TOTAL AMOUNT RELEASED (Rs.):-                                  " + y);
            sw.Close();
        }
    }
    
    public static void open__file(string file__path)
    {
       
        FileInfo objFi = new FileInfo(file__path);
        HttpContext.Current.Response.Clear();

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + objFi.Name);

        HttpContext.Current.Response.Charset = "";

        HttpContext.Current.Response.AddHeader("Content-Length", objFi.Length.ToString());

        HttpContext.Current.Response.ContentType = "application/octet-stream";

        HttpContext.Current.Response.WriteFile(objFi.FullName);

        HttpContext.Current.Response.End();

      
    }
 public static  DataRow getRowResut(String sql)
    {
        OracleDataAdapter adp = new OracleDataAdapter(sql, connectionString());
        DataTable dt = new DataTable();
        adp.Fill(dt);
        return dt.Rows[0];
    }
    //public static void StartZip(string directory, string zipfile_path)
    //{

    //    // the directory you need to zip 
    //    string[] filenames = Directory.GetFiles(directory);

    //    // path which the zip file built in 
    //    ZipOutputStream s = new ZipOutputStream(File.Create(zipfile_path));
    //    foreach (string filename in filenames)
    //    {
    //        FileStream fs = File.OpenRead(filename);
    //        byte[] buffer = new byte[fs.Length];
    //        fs.Read(buffer, 0, buffer.Length);
    //        ZipEntry entry = new ZipEntry(filename);
    //        s.PutNextEntry(entry);
    //        s.Write(buffer, 0, buffer.Length);
    //        fs.Close();

    //    }
    //    s.SetLevel(5);
    //    s.Finish();
    //    s.Close();
    //}
 
     public static void showmsg(System.Web.UI.Page page, string message)
    {
        page.RegisterStartupScript("key2","<script>alert('" + message + "'); </script>");
    }
 	public static String NumberToText2(Int64 number)
    {
        if (number == 0) return "Zero";

        if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";

        Int64[] num = new Int64[4];
        Int64 first = 0;
        Int64 u, h, t;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (number < 0)
        {
            sb.Append("Minus ");
            number = -number;
        }

        string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ", 
"Five " ,"Six ", "Seven ", "Eight ", "Nine "};

        string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", 
"Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};

        string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", 
"Seventy ","Eighty ", "Ninety "};

        string[] words3 = { "Thousand ", "Lakh ", "Crore " };

        num[0] = number % 1000; // units 
        num[1] = number / 1000;
        num[2] = number / 100000;
        num[1] = num[1] - 100 * num[2]; // thousands 
        num[3] = number / 10000000; // crores 
        num[2] = num[2] - 100 * num[3]; // lakhs 

        for (Int64 i = 3; i > 0; i--)
        {
            if (num[i] != 0)
            {
                first = i;
                break;
            }
        }


        for (Int64 i = first; i >= 0; i--)
        {
            if (num[i] == 0) continue;

            u = num[i] % 10; // ones 
            t = num[i] / 10;
            h = num[i] / 100; // hundreds 
            t = t - 10 * h; // tens 

            if (h > 0) sb.Append(words0[h] + "Hundred ");

            if (u > 0 || t > 0)
            {
                //if (h > 0 || i == 0) sb.Append("and "); 

                if (t == 0)
                    sb.Append(words0[u]);
                else if (t == 1)
                    sb.Append(words1[u]);
                else
                    sb.Append(words2[t - 2] + words0[u]);
            }

            if (i != 0) sb.Append(words3[i - 1]);

        }
        return sb.ToString().TrimEnd();
    }


}// end class

