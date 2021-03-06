﻿using System;
using System.Data; 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

public partial class frmproposalmenu : System.Web.UI.Page
{
    string myaddress = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["loginy"] == null || Session["loginy"].ToString() != "1")
        {
            Response.Redirect("./Login.aspx");
        }
        if (!IsPostBack)
        {
            FillGrid();
            panNewProp.Visible = false;
            txtpropname.Text = "";
        }
    }
    private void FillGrid()
    {
        string sql;
        DataSet ds;
        if (rbStatus.SelectedValue == "U")
        {
            sql = "select pno as \"Proposal_No\",pname as \"Proposal_Name\", " +
                "to_char(pdate,'DD-Mon-YYYY') as \"Creation_Date\" , " +
                "(select count(*) from cadre.propcadrmap where propno = pno) as entry_count" +
                " from cadre.tp_proposals " +
                " WHERE status='" + rbStatus.SelectedValue + "' order by pno";
            ds = OraDBConnection.GetData(sql);
            gvProposals.DataSource = ds;
            gvProposals.DataBind();
            gvProposals.Visible = true;
            gvProposals_Saved.Visible = false;
        }
        else
        {
            sql = "select pno as \"Proposal_No\",pname as \"Proposal_Name\", " +
                "to_char(pdate,'DD-Mon-YYYY') as \"Creation_Date\" , " +
                "(select count(*) from cadre.propcadrmap where propno = pno) as entry_count,oonum, " +
                "to_char(oodate,'dd-Mon-yyyy') as oodate " +
                " from cadre.tp_proposals " +
                " WHERE status='" + rbStatus.SelectedValue + "' order by pno desc";
            ds = OraDBConnection.GetData(sql);
            gvProposals_Saved.DataSource = ds;
            gvProposals_Saved.DataBind();
            gvProposals.Visible = false;
            gvProposals_Saved.Visible = true;
        }
    }
    protected void btncproposal_Click(object sender, EventArgs e)
    {
        string sql = "select pno, pname || '(' || pno || ')' as propname from cadre.tp_proposals where status='U'";
        DataSet ds = OraDBConnection.GetData(sql);
        
        ddDupProp.DataSource = ds;
        ddDupProp.DataTextField = "propname";
        ddDupProp.DataValueField = "pno";
        ddDupProp.DataBind();
        ddDupProp.Items.Insert(0, new ListItem("Duplicate","0"));

        ddMergeProp.DataSource = ds;
        ddMergeProp.DataTextField = "propname";
        ddMergeProp.DataValueField = "pno";
        ddMergeProp.DataBind();
        ddMergeProp.Items.Insert(0, new ListItem("Merge","0"));

        panNewProp.Visible = true;
    }
    protected void gvProposals_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Session["proposalno"] = gvProposals.Rows[e.NewEditIndex].Cells[3].Text.ToString();
        Session["proposalname"] = gvProposals.Rows[e.NewEditIndex].Cells[4].Text.ToString();
        Session["proposaldate"] = gvProposals.Rows[e.NewEditIndex].Cells[5].Text.ToString();
        Response.Redirect("~/tnp.aspx");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        panNewProp.Visible = false;
        txtpropname.Text = "";
    }
    protected void rbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private bool isValidDS(DataSet ds)
    {
        //check column count - should be 25 columns (8+17)
        if (ds.Tables.Count != 1 || ds.Tables[0].Columns.Count != 27)
        {
            return false;
        }
        //check all column names
        if (ds.Tables[0].Columns[0].Caption != "PNO" ||
            ds.Tables[0].Columns[1].Caption != "PNAME" || 
            ds.Tables[0].Columns[2].Caption != "PDATE" || 
            ds.Tables[0].Columns[3].Caption != "OONUM" || 
            ds.Tables[0].Columns[4].Caption != "OODATE" || 
            ds.Tables[0].Columns[5].Caption != "STATUS" ||
            ds.Tables[0].Columns[6].Caption != "PROPLINEMODE" || 
            ds.Tables[0].Columns[7].Caption != "PROPLINETEXT" ||
            ds.Tables[0].Columns[8].Caption != "LASTLINEMODE" ||
            ds.Tables[0].Columns[9].Caption != "LASTLINETEXT" || 
            ds.Tables[0].Columns[10].Caption != "EMPID" || 
            ds.Tables[0].Columns[11].Caption != "ROWNO" || 
            ds.Tables[0].Columns[12].Caption != "STATUS0" || 
            ds.Tables[0].Columns[13].Caption != "PROPOSED_ROWNO" || 
            ds.Tables[0].Columns[14].Caption != "CLOCCODE" || 
            ds.Tables[0].Columns[15].Caption != "CDESGCODE" || 
            ds.Tables[0].Columns[16].Caption != "REMARKS" || 
            ds.Tables[0].Columns[17].Caption != "SNO" ||
            ds.Tables[0].Columns[18].Caption != "PROPNO" ||
            ds.Tables[0].Columns[19].Caption != "NEWEMPID" ||
            ds.Tables[0].Columns[20].Caption != "DISPLACEDID" ||
            ds.Tables[0].Columns[21].Caption != "LAST_EVENT" ||
            ds.Tables[0].Columns[22].Caption != "OLDDESGCODE" ||
            ds.Tables[0].Columns[23].Caption != "OLDLOCCODE" ||
            ds.Tables[0].Columns[24].Caption != "PRVCOMMENT" ||
            ds.Tables[0].Columns[25].Caption != "DISP_LEFT" ||
            ds.Tables[0].Columns[26].Caption != "DISP_RIGHT"
           )
        {
            return false;
        }
        return true;
    }
    private bool DBInsert(DataSet ds)
    {
        string pno, pname, pdate, oonum, oodate, status, proplinemode, proplinetext, lastlinemode, lastlinetext;
        string empid, rowno, status0, proposedrowno, cloccode, cdesgcode, remarks;
        string sno, propno, newempid, displacedid, last_event, olddesgcode, oldloccode; 
        string prvcomment, disp_left, disp_right;
        DataRow firstrow;
        string sql;
        bool ret;

        if (ds.Tables[0].Rows.Count > 0)
        {
            firstrow = ds.Tables[0].Rows[0];
            pno = firstrow["PNO"].ToString().Trim();
            pname = firstrow["PNAME"].ToString().Trim();
            pdate = firstrow["PDATE"].ToString().Trim();
            oonum = (firstrow["OONUM"].ToString().Trim() == "-1") ? null : firstrow["OONUM"].ToString().Trim();   //can be -1
            oodate = (firstrow["OODATE"].ToString().Trim() == "01-Jan-2001") ? null : firstrow["OODATE"].ToString().Trim(); //can be 01-Jan-2001 == null
            status = firstrow["STATUS"].ToString().Trim();
            proplinemode = firstrow["PROPLINEMODE"].ToString().Trim();
            proplinetext = (firstrow["PROPLINETEXT"].ToString().Trim() == "-1") ? null : firstrow["PROPLINETEXT"].ToString().Trim();   //can be -1
            lastlinemode = firstrow["LASTLINEMODE"].ToString().Trim();
            lastlinetext = (firstrow["LASTLINETEXT"].ToString().Trim() == "-1") ? null : firstrow["LASTLINETEXT"].ToString().Trim();   //can be -1
            //check if proposal exists on server
            sql = "select count(*) as cnt from cadre.tp_proposals where pno = " + pno;
            if (OraDBConnection.GetScalar(sql) != "0")
            {
                Utils.ShowMessageBox(this, "Proposal already exists");
                return false;
            }
            //insert the proposal info in cadre.tp_proposals
            sql = string.Format("insert into cadre.tp_proposals values ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                    pno, pname, pdate, oonum, oodate, status, proplinemode, proplinetext, lastlinemode, lastlinetext);
            ret = OraDBConnection.ExecQry(sql);
            if (ret == false)
            {
                Utils.ShowMessageBox(this, "Error saving proposal info");
                return false;
            }

            //add proposal rows
            foreach (DataRow prow in ds.Tables[0].Rows)
            {
                empid = prow["EMPID"].ToString().Trim();
                rowno = (prow["ROWNO"].ToString().Trim() == "-1") ? null : prow["ROWNO"].ToString().Trim();   //can be -1
                status0 = (prow["STATUS0"].ToString().Trim() == "-1") ? null : prow["STATUS0"].ToString().Trim();   //can be -1
                proposedrowno = (prow["PROPOSED_ROWNO"].ToString().Trim() == "-1") ? null : prow["PROPOSED_ROWNO"].ToString().Trim();   //can be -1
                cloccode = (prow["CLOCCODE"].ToString().Trim() == "-1") ? null : prow["CLOCCODE"].ToString().Trim();   //can be -1
                cdesgcode = (prow["CDESGCODE"].ToString().Trim() == "-1") ? null : prow["CDESGCODE"].ToString().Trim();   //can be -1
                remarks = (prow["REMARKS"].ToString().Trim() == "-1") ? null : prow["REMARKS"].ToString().Trim();    //can be -1
                sno = (prow["SNO"].ToString().Trim() == "-1") ? null : prow["SNO"].ToString().Trim();    //can be -1
                propno = prow["PROPNO"].ToString().Trim();
                newempid = (prow["NEWEMPID"].ToString().Trim() == "-1") ? null : prow["NEWEMPID"].ToString().Trim(); //can be -1
                displacedid = (prow["DISPLACEDID"].ToString().Trim() == "-1") ? null : prow["DISPLACEDID"].ToString().Trim(); //can be -1
                last_event = (prow["LAST_EVENT"].ToString().Trim() == "-1") ? null : prow["LAST_EVENT"].ToString().Trim(); //can be -1
                olddesgcode = (prow["OLDDESGCODE"].ToString().Trim() == "-1") ? null : prow["OLDDESGCODE"].ToString().Trim(); //can be -1
                oldloccode = (prow["OLDLOCCODE"].ToString().Trim() == "-1") ? null : prow["OLDLOCCODE"].ToString().Trim(); //can be -1
                prvcomment = (prow["PRVCOMMENT"].ToString().Trim() == "-1") ? null : prow["PRVCOMMENT"].ToString().Trim(); //can be -1
                disp_left = (prow["DISP_LEFT"].ToString().Trim() == "-1") ? null : prow["DISP_LEFT"].ToString().Trim(); //can be -1
                disp_right = (prow["DISP_RIGHT"].ToString().Trim() == "-1") ? null : prow["DISP_RIGHT"].ToString().Trim(); //can be -1

                sql = string.Format("insert into cadre.propcadrmap values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')",
                    empid, rowno, status0, proposedrowno, cloccode, cdesgcode, remarks, sno, propno, newempid, displacedid, last_event, olddesgcode,oldloccode, prvcomment, disp_left, disp_right);
                ret = OraDBConnection.ExecQry(sql);
                if (ret == false)
                {
                    Utils.ShowMessageBox(this, "Error saving proposal for empid: " + empid);
                    return false;
                }
            }
        }
        return true;
    }
    protected void btnPropUp_Click(object sender, EventArgs e)
    {
        string filepath = Server.MapPath("office_orders\\" + FileUploader.FileName);
        if (FileUploader.HasFile)
            try
            {
                FileUploader.SaveAs(filepath);
            }
            catch (Exception ex)
            {
                lblUpFile.Text = "ERROR: " + ex.Message.ToString();
            }
        else
        {
            lblUpFile.Text = "You have not specified a file.";
        }


        DataSet ds = new DataSet();
        ds.ReadXml(filepath);
        System.IO.File.Delete(filepath);
        if (!isValidDS(ds))
        {
            Utils.ShowMessageBox(this, "Invalid XML");
            return;
        }
        if (DBInsert(ds))
        {
            Utils.ShowMessageBox(this, "Successfully Imported XML");
            FillGrid();
            return;
        }
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        int indx = ((GridViewRow)(((LinkButton)sender).Parent.Parent)).RowIndex;
        int selprop = int.Parse(gvProposals.Rows[indx].Cells[3].Text);

        string sql = @"SELECT   pr.pno,  pr.pname,
                          TO_CHAR(pr.pdate,'dd-Mon-yyyy')                     AS pdate,
                          NVL(pr.oonum,'-1')                                  AS oonum,
                          TO_CHAR(NVL(pr.oodate,'01-Jan-2001'),'dd-Mon-yyyy') AS oodate,
                          pr.status,  pr.proplinemode,  
                          NVL(pr.proplinetext,'-1') as proplinetext,
                          pr.lastlinemode,
                          NVL(pr.lastlinetext,'-1') as lastlinetext,
                          pcm.empid,  
                          NVL(pcm.rowno,-1) as rowno,  
                          NVL(pcm.status,'-1') as status0,
                          NVL(pcm.proposed_rowno,'-1') as proposed_rowno,  
                          NVL(pcm.cloccode,-1) as cloccode,  
                          NVL(pcm.cdesgcode,-1) as cdesgcode,
                          NVL(pcm.remarks,'-1') AS remarks,
                          NVL(pcm.sno,-1) as sno,  
                          pcm.propno,  
                          NVL(pcm.newempid,-1) AS newempid,
                          NVL(pcm.displacedid,-1) as displacedid,  
                          NVL(pcm.last_event,-1) as last_event,
                          NVL(pcm.olddesgcode,-1) as olddesgcode,  
                          NVL(pcm.oldloccode,-1) as oldloccode,
                          NVL(pcm.PRVCOMMENT,'-1') as prvcomment, 
                          NVL(pcm.DISP_LEFT,'-1') as disp_left,
                          NVL(pcm.DISP_RIGHT,'-1') as disp_right
                        FROM cadre.tp_proposals pr,  cadre.propcadrmap pcm
                        WHERE pcm.propno=pr.pno AND propno      =" + selprop;

        DataSet ds = OraDBConnection.GetData(sql);
        if (ds.Tables[0].Rows.Count < 1)
        {
            Utils.ShowMessageBox(this, "There should be atleast one entry in the proposal");
            return;
        }
        string filepath = Server.MapPath("office_orders\\saved_propno_" + selprop.ToString() + ".xml");
        ds.WriteXml(filepath);
        //Utils.DownloadFile(filepath, false, "text/xml");
        Utils.DownloadFile(filepath, true);
    }
    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        string oonum = ((System.Web.UI.WebControls.GridViewRow)((((System.Web.UI.Control)(sender)).Parent).Parent)).Cells[7].Text;
        string propno = ((System.Web.UI.WebControls.GridViewRow)((((System.Web.UI.Control)(sender)).Parent).Parent)).Cells[3].Text;

        string sql = "SELECT m.sno,  p.OONUM  AS oonum1,  to_char(p.OODATE,'dd-MM-yyyy')  AS oodate1,  '1' AS notes,  p.ENDONUM  AS endono,10  AS fsize," +
              "(SELECT COUNT(*) FROM cadre.propcadrmap WHERE propno = 163  )                                                                   AS TotCount," +
              "(SELECT COUNT(*) FROM cadre.propcadrmap WHERE status = 'P' AND propno = 163  )                                                  AS PCount," +
              "pshr.get_fullname(e.empid)                                                                                                      AS fullname," +
              "TO_CHAR(e.empid)                                                                                                                AS empid," +
              "TO_CHAR(e.dob,'dd-mm-yyyy')                                                                                                     AS dob," +
              "pshr.get_post(e.cloccode)                                                                                                       AS old_work_loc," +
              "e.cloccode                                                                                                                      AS old_work_loccode," +
              "pshr.get_desg(e.cdesgcode)                                                                                                      AS old_work_desg," +
              "e.cdesgcode                                                                                                                     AS old_work_desgcode," +
              "DECODE(m.rowno,0,pshr.get_post(e.cloccode), pshr.get_post(cadre.get_lcode_rno(m.rowno)))                                        AS old_pc_loc," +
              "DECODE(m.rowno,0,e.cloccode, cadre.get_lcode_rno(m.rowno))                                                                      AS old_pc_loccode," +
              "DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.rowno)))                                       AS old_pc_desg," +
              "DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.rowno))                                                                     AS old_pc_desgcode," +
              "DECODE(m.rowno,0,'0', cadre.get_indx_rno(m.rowno))                                                                              AS old_pc_indx," +
              "cadre.get_org_plants(m.cloccode)                                                                                                AS new_work_loc," +
              "m.cloccode                                                                                                                      AS new_work_loccode," +
              "pshr.get_desg(m.cdesgcode)                                                                                                      AS new_work_desg," +
              "m.cdesgcode                                                                                                                     AS new_work_desgcode," +
              "DECODE(LENGTH(m.proposed_rowno),9,pshr.get_post(m.proposed_rowno), cadre.get_org_plants(cadre.get_lcode_rno(m.proposed_rowno))) AS new_pc_loc," +
              "DECODE(LENGTH(m.proposed_rowno),9,m.proposed_rowno, cadre.get_lcode_rno(m.proposed_rowno))                                      AS new_pc_loccode," +
              "DECODE(m.proposed_rowno,0,pshr.get_desg(m.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno)))                     AS new_pc_desg," +
              "DECODE(m.proposed_rowno,0,m.cdesgcode, cadre.get_dcode_rno(m.proposed_rowno))                                                   AS new_pc_desgcode," +
              "cadre.get_indx_rno(m.proposed_rowno)                                                                                            AS new_pc_indx," +
              "(select data from cadre.bignotes where name = p.bignote)                                                                        AS BIGNOTE," +
              "(select data from cadre.bigcc where name = p.bigcc)                                                                             AS BIGCC," +
              "m.sysremarks" +
              "|| m.remarks                                                                                                                    AS remarks," +
              "'G'                                                                                                                             AS grp," +
              "m.propno," +
              "TO_CHAR(m.newempid)                                                                                                             AS newempid," +
              "m.status," +
              "m.disp_left," +
              "m.disp_right," +
              "pshr.get_soccat(e.empid)                                                                                                        AS categ " +
            "FROM pshr.empperso e," +
              "cadre.propcadrmap m," +
              "cadre.tp_proposals p " +
            "WHERE e.empid     =m.empid " +
            "AND m.propno = p.pno " +
            "AND m.status     IS NOT NULL " +
            "AND M.STATUS NOT IN ('S','V') " +
            "AND m.propno      =" + propno + " " +
            "AND m.cloccode   IS NOT NULL " +
            "ORDER BY sno";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        string pdfPath;
        pdfPath = Server.MapPath("office_orders\\" + oonum.Replace("\\", "").Replace("/", "") + "-BEG-I.pdf");
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rptposttrans.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();

        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);

        Utils.DownloadFile(pdfPath);
    }
    protected void lnkDownloadPrv_Click(object sender, EventArgs e)
    {
        string oonum = ((System.Web.UI.WebControls.GridViewRow)((((System.Web.UI.Control)(sender)).Parent).Parent)).Cells[7].Text;
        string propno = ((System.Web.UI.WebControls.GridViewRow)((((System.Web.UI.Control)(sender)).Parent).Parent)).Cells[3].Text;

        string sql = "SELECT m.sno,  p.OONUM  AS oonum1,  to_char(p.OODATE,'dd-MM-yyyy')  AS oodate1,  '1' AS notes,  p.ENDONUM  AS endono,10  AS fsize," +
              "(SELECT COUNT(*) FROM cadre.propcadrmap WHERE propno = 163  )                                                                   AS TotCount," +
              "(SELECT COUNT(*) FROM cadre.propcadrmap WHERE status = 'P' AND propno = 163  )                                                  AS PCount," +
              "pshr.get_fullname(e.empid)                                                                                                      AS fullname," +
              "TO_CHAR(e.empid)                                                                                                                AS empid," +
              "TO_CHAR(e.dob,'dd-mm-yyyy')                                                                                                     AS dob," +
              "pshr.get_post(e.cloccode)                                                                                                       AS old_work_loc," +
              "e.cloccode                                                                                                                      AS old_work_loccode," +
              "pshr.get_desg(e.cdesgcode)                                                                                                      AS old_work_desg," +
              "e.cdesgcode                                                                                                                     AS old_work_desgcode," +
              "DECODE(m.rowno,0,pshr.get_post(e.cloccode), pshr.get_post(cadre.get_lcode_rno(m.rowno)))                                        AS old_pc_loc," +
              "DECODE(m.rowno,0,e.cloccode, cadre.get_lcode_rno(m.rowno))                                                                      AS old_pc_loccode," +
              "DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.rowno)))                                       AS old_pc_desg," +
              "DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.rowno))                                                                     AS old_pc_desgcode," +
              "DECODE(m.rowno,0,'0', cadre.get_indx_rno(m.rowno))                                                                              AS old_pc_indx," +
              "cadre.get_org_plants(m.cloccode)                                                                                                AS new_work_loc," +
              "m.cloccode                                                                                                                      AS new_work_loccode," +
              "pshr.get_desg(m.cdesgcode)                                                                                                      AS new_work_desg," +
              "m.cdesgcode                                                                                                                     AS new_work_desgcode," +
              "DECODE(LENGTH(m.proposed_rowno),9,pshr.get_post(m.proposed_rowno), cadre.get_org_plants(cadre.get_lcode_rno(m.proposed_rowno))) AS new_pc_loc," +
              "DECODE(LENGTH(m.proposed_rowno),9,m.proposed_rowno, cadre.get_lcode_rno(m.proposed_rowno))                                      AS new_pc_loccode," +
              "DECODE(m.proposed_rowno,0,pshr.get_desg(m.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno)))                     AS new_pc_desg," +
              "DECODE(m.proposed_rowno,0,m.cdesgcode, cadre.get_dcode_rno(m.proposed_rowno))                                                   AS new_pc_desgcode," +
              "cadre.get_indx_rno(m.proposed_rowno)                                                                                            AS new_pc_indx," +
              "(select data from cadre.bignotes where name = p.bignote)                                                                        AS BIGNOTE," +
              "(select data from cadre.bigcc where name = p.bigcc)                                                                             AS BIGCC," +
              "m.sysremarks" +
              "|| m.remarks                                                                                                                    AS remarks," +
              "'G'                                                                                                                             AS grp," +
              "m.propno," +
              "m.prvcomment," +
              "TO_CHAR(m.newempid)                                                                                                             AS newempid," +
              "m.status," +
              "m.disp_left," +
              "m.disp_right," +
              "pshr.get_soccat(e.empid)                                                                                                        AS categ " +
            "FROM pshr.empperso e," +
              "cadre.propcadrmap m," +
              "cadre.tp_proposals p " +
            "WHERE e.empid     =m.empid " +
            "AND m.propno = p.pno " +
            "AND m.status     IS NOT NULL " +
            "AND M.STATUS NOT IN ('S','V') " +
            "AND m.propno      =" + propno + " " +
            "AND m.cloccode   IS NOT NULL " +
            "ORDER BY sno";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        string pdfPath;
        pdfPath = Server.MapPath("office_orders\\" + oonum.Replace("\\", "").Replace("/", "") + "-BEG-I.pdf");
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rptposttrans_pc.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();

        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);

        Utils.DownloadFile(pdfPath);
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        int indx = ((GridViewRow)(((LinkButton)sender).Parent.Parent)).RowIndex;
        int selprop = int.Parse(gvProposals.Rows[indx].Cells[3].Text);

        //string prono = gvProposals.Rows[e.RowIndex].Cells[2].Text.ToString();
        string sql = string.Empty;

        sql = "delete from cadre.propcadrmap where propno =" + selprop;
        OraDBConnection.ExecQry(sql);

        //gurpreet
        //sql = "delete from cadre.cclist_proposal_person  where proposalno =" + prono;
        //OraDBConnection.ExecQry(sql);

        //sql = "delete from cadre.notes_proposal_person  where proposalno =" + prono;
        //OraDBConnection.ExecQry(sql);

        sql = "delete from cadre.tp_proposals  where pno =" + selprop;
        OraDBConnection.ExecQry(sql);
        FillGrid();
    }
    protected void btnCreateDup_Click(object sender, EventArgs e)
    {
        string propname = txtpropname.Text;
        int dupNo=int.Parse(ddDupProp.SelectedValue);
        int mergeNo=int.Parse(ddMergeProp.SelectedValue);

        if (string.IsNullOrEmpty(propname))
        {
            Utils.ShowMessageBox(this, "Enter Proposal Name");
        }
        if (dupNo == 0 && mergeNo == 0)
        {
            //status: U-Unsaved, S-Saved
            //type: TP-Transfer/Promotion, RD-Redesignation
            string sql = "insert into cadre.tp_proposals(pno,pname,pdate,status,proplinemode, lastlinemode) values" +
                         "((select nvl(max(pno),0)+1 from cadre.tp_proposals)," +
                         "'" + propname + "',sysdate,'U','A','N')";

            bool ret = false;
            try
            {
                ret = OraDBConnection.ExecQry(sql);
                if (ret == false)
                {
                    throw new Exception("Error in Creating Proposal");
                }
            }
            catch (Exception ex)
            {
                Utils.ShowMessageBox(this, ex.Message);
                return;
            }
        }
        else if (dupNo > 0 && mergeNo == 0)
        {
            string sql_prop_create = "insert into cadre.tp_proposals(pno,pname,pdate,status,proplinemode) values" +
                     "((select nvl(max(pno),0)+1 from cadre.tp_proposals)," +
                     "'" + propname + "',sysdate,'U','A')";
            string sql_prop_dup = "insert into cadre.propcadrmap " +
                                    "select empid, rowno, status, proposed_rowno, cloccode, cdesgcode, remarks, sno, " +
                                    "(select max(pno) from cadre.tp_proposals), " +
                                    "newempid, displacedid, last_event, olddesgcode, oldloccode, prvcomment," +
                                    "DISP_LEFT, DISP_RIGHT,  SYSREMARKS,  FLAG_OWNINT " +
                                    "from cadre.propcadrmap where propno = " + dupNo;
            //string sql_cc_dup = "insert into CADRE.CCLIST_PROPOSAL_PERSON " +
            //                        "select sno, (select max(pno) from cadre.tp_proposals), ccnum, loc from " +
            //                        "CADRE.CCLIST_PROPOSAL_PERSON where PROPOSALNO= " + dupNo;
            //string sql_notes_dup = "insert into CADRE.NOTES_PROPOSAL_PERSON " +
            //                        "select sno, (select max(pno) from cadre.tp_proposals), ccnum, notes from " +
            //                        "CADRE.NOTES_PROPOSAL_PERSON where PROPOSALNO= " + dupNo;
            bool ret = false;

            try
            {
                ret = OraDBConnection.ExecQry(sql_prop_create);
                if (ret == false)
                {
                    throw new Exception("Error in Creating Proposal Entry");
                }
                ret = OraDBConnection.ExecQry(sql_prop_dup);
                if (ret == false)
                {
                    throw new Exception("Error in Duplicating Proposal");
                }
                //ret = OraDBConnection.ExecQry(sql_cc_dup);
                //if (ret == false)
                //{
                //    throw new Exception("Error in Duplicating CC List");
                //}
                //ret = OraDBConnection.ExecQry(sql_notes_dup);
                //if (ret == false)
                //{
                //    throw new Exception("Error in Duplicating Notes");
                //}
            }
            catch (Exception ex)
            {
                Utils.ShowMessageBox(this, ex.Message);
                return;
            }

        }
        else if (dupNo > 0 && mergeNo > 0)
        {
            string sql_prop_create = "insert into cadre.tp_proposals(pno,pname,pdate,status,proplinemode) values" +
                                     "((select nvl(max(pno),0)+1 from cadre.tp_proposals)," +
                                     "'" + propname + "',sysdate,'U','A')";
            string sql_prop_dup = "insert into cadre.propcadrmap " +
                                    "select empid, rowno, status, proposed_rowno, cloccode, cdesgcode, remarks, sno, " +
                                    "(select max(pno) from cadre.tp_proposals), " +
                                    "newempid, displacedid, last_event, olddesgcode, oldloccode, prvcomment, disp_left, disp_right, sysremarks, flag_ownint " +
                                    "from cadre.propcadrmap where propno = " + dupNo;
            string sql_prop_merge = "insert into cadre.propcadrmap " +
                                    "select empid, rowno, status, proposed_rowno, cloccode, cdesgcode, remarks, "+
                                    "sno+(select max(sno) from cadre.propcadrmap where propno = (select max(pno) from cadre.tp_proposals)), "+
                                    "(select max(pno) from cadre.tp_proposals), " +
                                    "newempid, displacedid, last_event, olddesgcode, oldloccode, prvcomment, "+
                                    "disp_left, disp_right, sysremarks, flag_ownint " +
                                    "from cadre.propcadrmap where propno = " + mergeNo;
            //string sql_cc_dup = "insert into CADRE.CCLIST_PROPOSAL_PERSON " +
            //                        "select sno, (select max(pno) from cadre.tp_proposals), ccnum, loc from " +
            //                        "CADRE.CCLIST_PROPOSAL_PERSON where PROPOSALNO= " + dupNo;
            //string sql_notes_dup = "insert into CADRE.NOTES_PROPOSAL_PERSON " +
            //                        "select sno, (select max(pno) from cadre.tp_proposals), ccnum, notes from " +
            //                        "CADRE.NOTES_PROPOSAL_PERSON where PROPOSALNO= " + dupNo;
            bool ret = false;

            try
            {
                ret = OraDBConnection.ExecQry(sql_prop_create);
                if (ret == false)
                {
                    throw new Exception("Error in Creating Proposal Entry");
                }
                ret = OraDBConnection.ExecQry(sql_prop_dup);
                if (ret == false)
                {
                    throw new Exception("Error in Duplicating Proposal");
                }
                ret = OraDBConnection.ExecQry(sql_prop_merge);
                if (ret == false)
                {
                    throw new Exception("Error in Merging Proposal Entry");
                }
                //ret = OraDBConnection.ExecQry(sql_cc_dup);
                //if (ret == false)
                //{
                //    throw new Exception("Error in Duplicating CC List");
                //}
                //ret = OraDBConnection.ExecQry(sql_notes_dup);
                //if (ret == false)
                //{
                //    throw new Exception("Error in Duplicating Notes");
                //}
            }
            catch (Exception ex)
            {
                Utils.ShowMessageBox(this, ex.Message);
                return;
            }
        }
        panNewProp.Visible = false;
        txtpropname.Text = "";
        txtpropname.Text = "";
        FillGrid();
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        Session["proposalno"] = ((System.Web.UI.WebControls.GridViewRow)((((System.Web.UI.Control)(sender)).Parent).Parent)).Cells[3].Text;
        Session["proposalname"] = ((System.Web.UI.WebControls.GridViewRow)((((System.Web.UI.Control)(sender)).Parent).Parent)).Cells[4].Text;
        Session["proposaldate"] = ((System.Web.UI.WebControls.GridViewRow)((((System.Web.UI.Control)(sender)).Parent).Parent)).Cells[5].Text;
        Response.Redirect("~/tnp.aspx");
    }
}