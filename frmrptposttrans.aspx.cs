using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.Shared;
using System.Data;
public partial class frmrptposttrans : System.Web.UI.Page
{
    private bool Save()
    {
        string oonum = Session.Contents["oonum"].ToString();
        string oodate = Session.Contents["oodate"].ToString();
        string notes = Session.Contents["notes"].ToString();
        DataSet ds2;
        DataRow row2;
        string pcloccode;
        string sancdesg;
        string sancindx;
        string empid;
        int eventcode;
        string cdesgcode;
        string cloccode;
        string rowno;
        string proposed_rowno;
        string eventhistoryid;
        string sql;
        bool ret;

        Session.Contents["oonum"] = "";
        Session.Contents["oodate"] = "";

        DataSet ds = OraDBConnection.GetData("select * from cadre.cadrmap where status is not null");
        if (ds.Tables[0].Rows.Count < 1)
        {
            Utils.ShowMessageBox(this, "Nothing to save.");
            return false;
        }

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            empid = row["empid"].ToString();
            // T=>CTRP(36) and P=>CPRO(28)
            eventcode = (row["status"].ToString() == "T") ? 36 : 28;
            cdesgcode = row["cdesgcode"].ToString();
            cloccode = row["cloccode"].ToString();
            rowno = OraDBConnection.GetScalar("select nvl(max(rowno),0)+1 from pshr.emphistory where empid=" + empid);

            if (cloccode == "999999999")
            {
                eventcode = 2;
            }

            proposed_rowno = row["proposed_rowno"].ToString();

            if (proposed_rowno.Length == 9)
            {
                pcloccode = cloccode;
                sancdesg = cdesgcode;
                sancindx = "0";
            }
            else
            {
                ds2 = OraDBConnection.GetData("select loccode,desgcode,indx from cadre.cadr where rowno=" + proposed_rowno);
                row2 = ds2.Tables[0].Rows[0];
                pcloccode = row2["loccode"].ToString();
                sancdesg = row2["desgcode"].ToString();
                sancindx = row2["indx"].ToString();
                ds2.Clear();
                ds2.Dispose();
            }
            eventhistoryid = OraDBConnection.GetScalar("select max(eventhistoryid)+1 from pshr.emphistory");

            switch (pcloccode.Substring(0, 3))
            {
                case "101":
                    pcloccode = "101000000";
                    break;
                case "103":
                    pcloccode = "103000000";
                    break;
                case "104":
                    pcloccode = "104000000";
                    break;
                case "105":
                    pcloccode = "105000000";
                    break;
                case "106":
                    pcloccode = "106000000";
                    break;
                case "108":
                    pcloccode = "108000000";
                    break;
            }
            
            //insert into emphistory
            if (proposed_rowno.Length == 9)
            {
                sql = string.Format("insert into pshr.emphistory(empid,eventcode,desgcode,loccode,rowno," +
                                        "eventhistoryid, pcloccode,oonum,odate,status) values " +
                                        "({0},{1},{2},{3},{4},{5},{6},'{7}','{8}',1)",
                                        empid, eventcode, cdesgcode, cloccode, rowno, eventhistoryid, pcloccode, oonum, oodate);
            }
            else
            {
                sql = string.Format("insert into pshr.emphistory(empid,eventcode,desgcode,loccode,rowno," +
                                        "eventhistoryid, pcloccode,sancdesg,sancindx,oonum,odate,status) values " +
                                        "({0},{1},{2},{3},{4},{5},{6},{7},{8},'{9}','{10}',1)",
                                        empid, eventcode, cdesgcode, cloccode, rowno, eventhistoryid, pcloccode, sancdesg, sancindx, oonum, oodate);
            }
            
            ret = OraDBConnection.ExecQry(sql);
            if (ret == false)
            {
                Utils.ShowMessageBox(this, string.Format("Unable to add entry for empid {0} in pshr.emphistory", empid));
                return false;
            }

            //update empperso
            ret = Utils.UpdateEmpperso(empid);
            if (ret == false)
            {
                Utils.ShowMessageBox(this, "Unable to update empperso");
            }
        }
        //update cadrmap
        OraDBConnection.ExecQry("delete from cadre.cadrmap where length(proposed_rowno)=9");
        OraDBConnection.ExecQry("update cadre.cadrmap set rowno=proposed_rowno where proposed_rowno is not null and status is not null");
        OraDBConnection.ExecQry("update cadre.cadrmap set proposed_rowno=null,status=null,cloccode=null,cdesgcode=null,remarks=null,oonum=null,odate=null,sno=null");
        return true;
    }
    private void MakeReport()
    {
        
        string oonum = "-";
        string oodate = "-";
        string notes = "";
        if (Session.Contents["save"].ToString() == "YES")
        {
            oonum = Session.Contents["oonum"].ToString();
            oodate = Session.Contents["oodate"].ToString();
        }
        //notes = Session.Contents["notes"].ToString();
        notes = "1";
        string sql = "select m.sno,'" + oonum + "' as oonum1, '" + oodate + "' as oodate1, '" + notes + "' as notes," +
                    " pshr.get_fullname(e.empid),e.empid,e.dob," +
                    "pshr.get_post(e.cloccode) as old_work_loc,e.cloccode as old_work_loccode,pshr.get_desg(e.cdesgcode) as old_work_desg," +
                    "e.cdesgcode as old_work_desgcode," +
                    "DECODE(m.rowno,0,pshr.get_post(e.cloccode), pshr.get_post(cadre.get_lcode_rno(m.rowno))) AS old_pc_loc," +
                    " DECODE(m.rowno,0,e.cloccode, cadre.get_lcode_rno(m.rowno)) AS old_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.rowno))) AS old_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.rowno)) AS old_pc_desgcode, " +
                    " DECODE(m.rowno,0,'0', cadre.get_indx_rno(m.rowno)) AS old_pc_indx, " +

                    //pshr.get_post(cadre.get_lcode_rno(m.rowno)) as old_pc_loc," +
            //"cadre.get_lcode_rno(m.rowno) as old_pc_loccode,pshr.get_desg(cadre.get_dcode_rno(m.rowno)) as old_pc_desg," +
            //"cadre.get_dcode_rno(m.rowno) as old_pc_desgcode,cadre.get_indx_rno(m.rowno) as old_pc_indx,
                    " pshr.get_post(m.cloccode) as new_work_loc," +
                    "m.cloccode as new_work_loccode,pshr.get_desg(m.cdesgcode) as new_work_desg,m.cdesgcode as new_work_desgcode," +
            //"pshr.get_post(cadre.get_lcode_rno(m.proposed_rowno)) as new_pc_loc,cadre.get_lcode_rno(m.proposed_rowno) as new_pc_loccode," +
                    "decode(length(m.proposed_rowno),9,pshr.get_post(m.proposed_rowno), pshr.get_post(cadre.get_lcode_rno(m.proposed_rowno))) AS new_pc_loc," +
                    "decode(length(m.proposed_rowno),9,m.proposed_rowno, cadre.get_lcode_rno(m.proposed_rowno)) AS new_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno))) AS new_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.proposed_rowno))                               AS new_pc_desgcode, " +

                    //"decode(length(m.proposed_rowno),9,pshr.get_desg(cadre.get_dcode_rno(m.rowno)), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno))) AS new_pc_desg,"+
            //"decode(length(m.proposed_rowno),9,cadre.get_dcode_rno(m.rowno), cadre.get_dcode_rno(m.proposed_rowno)) AS new_pc_desgcode,"+
            ////"pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno)) as new_pc_desg,cadre.get_dcode_rno(m.proposed_rowno) as new_pc_desgcode," +
                    "cadre.get_indx_rno(m.proposed_rowno) as new_pc_indx, m.remarks,'G' as grp,m.proposalno from pshr.empperso e, cadre.cadrmap m where e.empid=m.empid and m.status is not null " +
                    "order by sno";

        OraDBConnection oraCn = new OraDBConnection();
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        string pdfPath;
        if (Session.Contents["save"].ToString() == "YES")
        {
            
           
             pdfPath = Server.MapPath("office_orders\\" + oonum + "\\BEG-I" + oodate + ".pdf");
        }

        else
        {
            //get sysdate
            string sysdate = OraDBConnection.GetScalar("select to_char(sysdate,'dd-mm-yyyy')   from dual");
            string systime = OraDBConnection.GetScalar("select to_char(sysdate,'hh-mm')   from dual");
            pdfPath = Server.MapPath("office_orders\\" + sysdate + systime + ".pdf");
        
        }
        
        
        
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rptposttrans.rpt");

        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();
        CrystalReportViewer1.ReportSource = CrystalReportSource1;
        CrystalReportViewer1.DataBind();


        System.Data.DataSet ds6;
        string sql3 = "select * from cadre.notes_proposal_person where ccnum > 0 order by sno";
        ds6 = OraDBConnection.GetData(sql3);
        CrystalReportSource1.ReportDocument.Subreports["notes"].SetDataSource(ds6.Tables[0]);
        CrystalReportSource1.DataBind();

        System.Data.DataSet ds5;
        string sql2 = "select * from cadre.cclist_proposal_person where ccnum > 0 order by sno";
        ds5 = OraDBConnection.GetData(sql2);
        CrystalReportSource1.ReportDocument.Subreports["cclists"].SetDataSource(ds5.Tables[0]);
        CrystalReportSource1.DataBind();
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);
        DownloadFile(pdfPath);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        MakeReport();
        if (!Page.IsPostBack)
        {
            if (Session.Contents["save"].ToString() == "YES")
            {
                if (Save() == false)
                {
                    Utils.ShowMessageBox(this, "Unable to save");
                    return;
                }
            }
        }
    }
    void DownloadFile(String pdfPath)
    {
        System.IO.FileInfo objFi = new System.IO.FileInfo(pdfPath);
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + objFi.Name);
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AddHeader("Content-Length", objFi.Length.ToString());
        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.WriteFile(objFi.FullName);
        HttpContext.Current.Response.End();
    }


}