using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public class ApplyCRLogin
{
    public string dbName;
    public string serverName;
    public string userID;
    public string passWord;

    public void ApplyInfo(ref ReportDocument _oRpt)
    {
        Database oCRDb = _oRpt.Database;
        Tables oCRTables = oCRDb.Tables;
        TableLogOnInfo oCRTableLogonInfo;
        ConnectionInfo oCRConnectionInfo = new ConnectionInfo();
        oCRConnectionInfo.ServerName = serverName;
        oCRConnectionInfo.UserID = userID;
        oCRConnectionInfo.Password = passWord;

        foreach (CrystalDecisions.CrystalReports.Engine.Table oCRTable in oCRTables)
        {
            oCRTableLogonInfo = oCRTable.LogOnInfo;
            oCRTableLogonInfo.ConnectionInfo = oCRConnectionInfo;
            oCRTable.ApplyLogOnInfo(oCRTableLogonInfo);
            oCRTable.Location = oCRTable.Location.Substring(oCRTable.Location.LastIndexOf('.') + 1);
        }

        //set the sections collection with report sections
        Sections crSections = _oRpt.ReportDefinition.Sections;

        //loop through each section and find all report objects
        //loop through all the report objects to find all subreport objects
        //and set the logoninfo to the subreport
        foreach (Section crSection in crSections)
        {
            ReportObjects crReportObjects = crSection.ReportObjects;
            foreach (ReportObject crReportObject in crReportObjects)
            {
                if (crReportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    //if its a subreport, typecast the reportobject to a subreport object
                    SubreportObject crSubreportObject = (SubreportObject)crReportObject;
                    
                    //open the subreport
                    ReportDocument subRepDoc = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);

                    oCRDb = subRepDoc.Database;
                    oCRTables = oCRDb.Tables;

                    //loop through each table and set the connection info
                    //pass the connection info to the logoninfo object then apply the logoninfo to the subreport
                    foreach(CrystalDecisions.CrystalReports.Engine.Table crTable in oCRTables)
                    {
                        TableLogOnInfo crLogOnInfo = crTable.LogOnInfo;
                        crLogOnInfo.ConnectionInfo = oCRConnectionInfo;
                        crTable.ApplyLogOnInfo(crLogOnInfo);
                        crTable.Location = crTable.Location.Substring(crTable.Location.LastIndexOf(".") + 1);
                    }
                }
            }
        }
    }
}