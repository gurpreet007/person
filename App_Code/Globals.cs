using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Globals
/// </summary>
public static class Globals
{
    public static string DIR_NUMS
    {
        get
        {
            if (System.Environment.MachineName.ToUpper().Contains("SERVER"))
            {
                return "9646200035,9646200037,9646200026,9646200031,9646200054,9646118786,9646111018";
            }
            else
            {
                return "9646111018";
            }
        }
    }
    public static string DOCS_LOC
    {
        get
        {
            if (System.Environment.MachineName.ToUpper().Contains("SERVER"))
            {
                return "H:\\inetpub\\wwwroot\\uploadordrs\\docs\\";
            }
            else
            {
                return "C:\\inetpub\\wwwroot\\uploadordrs\\docs\\";
            }
        }
    }
}