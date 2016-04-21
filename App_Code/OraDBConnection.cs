using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Odbc ;
using System.Data.OracleClient;
public class OraDBConnection
{
    private const string constr = "user id=pshr;password=123;"+
        "data source="+
            "(DESCRIPTION="+
                "(ADDRESS_LIST="+
                    "(ADDRESS=(PROTOCOL=TCP)"+
                             "(HOST=127.0.0.1)"+
                             "(PORT=1521)"+
                    ")"+
                ")(CONNECT_DATA=(SERVICE_NAME=xe))"+
            ")";
    
    public static DataSet GetData(String sql)
    {
        OracleDataAdapter adp = new OracleDataAdapter(sql, constr);
        DataSet ds = new DataSet();
        try
        {
            adp.Fill(ds);
            return ds;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            ds.Dispose();
            adp.Dispose();
        }
    }
    public static Boolean ExecQry(string sql)
    {
        OracleConnection con = new OracleConnection(constr);
        OracleCommand cmd = new OracleCommand(sql, con);
        
        //user, time, <sql>
        try
        {
            con.Open();
            
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            con.Close();
            con.Dispose();
            cmd.Dispose();
        }
    }
    public static string GetScalar(string sql)
    {
        OracleConnection con = new OracleConnection(constr);
        OracleCommand cmd = new OracleCommand(sql, con);
        object obj;
        try
        {
            con.Open();

            obj = cmd.ExecuteScalar();
            return (obj==null) ? "" : obj.ToString();
        }
        catch(Exception e)
        {
            throw e;
        }
        finally
        {
            con.Close();
            cmd.Dispose();
            con.Dispose();
        }
    }
    public static void FillGrid(ref System.Web.UI.WebControls.GridView gv, string sql)
    {
        gv.DataSource = GetData(sql);
        gv.DataBind();
    }
}