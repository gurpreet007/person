using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.Shared;
using System.Web.Services;
using System.Text;
using System.Data.OleDb;

public partial class frmproposal : System.Web.UI.Page
{
    #region DataMembers
    int PRONO;
    private enum rowTypes
    {
        NORMAL,
        SPECIAL_LOC,
        LEAVE_RET_EVENT,
    };
    #endregion

    #region Methods
    private void FillGrid()
    {
        string sql;
        DataSet ds;
      
        sql = "select a.* from (" +
              "select Sno,decode(status,'T','Transfer','P','Promotion') as Action," +
              "pshr.get_fullname(empid) as Name,EMPID, " +
              "case when last_event = 17 then 'On Reinstatement' else cadre.get_org_plants(cadre.get_preposting(empid)) end as \"Present Location\", " +
              "disp_left as Left_Display, "+
              "decode(length(proposed_rowno), 9,pshr.get_post(proposed_rowno), "+
              "cadre.get_org_plants(cadre.loccode_from_rowno(proposed_rowno))) as \"Proposed PC Location\", " +
              "pshr.get_post(cloccode) as \"Proposed Location\"," +
              "pshr.get_desg(cdesgcode) as \"Proposed Designation\"," +
              "disp_right as Right_Display, " +
              "Remarks,newempid, prvcomment from cadre.propcadrmap where status = 'P' and propno =" + PRONO +
              " union all " +
              "select Sno,decode(status,'T','Transfer','P','Promotion') as Action," +
              "pshr.get_fullname(empid) as Name,EMPID, " +
              "case when last_event = 17 then 'On Reinstatement' else cadre.get_org_plants(cadre.get_preposting(empid)) end as \"Present Location\", " +
              "disp_left as Left_Display, " +
              "decode(length(proposed_rowno), 9,pshr.get_post(proposed_rowno), "+
              "cadre.get_org_plants(cadre.loccode_from_rowno(proposed_rowno))) as \"Proposed PC Location\", " +
              "cadre.get_org_plants(cloccode) as \"Proposed Location\"," +
              "pshr.get_desg(cdesgcode) as \"Proposed Designation\"," +
              "disp_right as Right_Display, " +
              "Remarks,newempid,prvcomment from cadre.propcadrmap where status in ('T','O') and propno =" + PRONO +
              ") a order by a.sno";

        ds = OraDBConnection.GetData(sql);
        gvProposals.DataSource = ds;
        gvProposals.DataBind();
    }
    private void FillOutstandings()
    {
        //string sql = "select empid, pshr.get_fullname(empid) as name, " +
        //    "get_post(cadre.get_posting(empid)) as loc from pshr.empperso em where " +
        //    "empid in (select empid from cadre.cadrmap where status = '" + rbTnP.SelectedValue + "' and proposed_rowno is null) ";
        //string sql = string.Format("select empid, pshr.get_fullname(empid) as name, " +
        //    "pshr.get_post(cadre.get_posting(empid)) as loc from pshr.empperso em where " +
        //    "empid in (select empid from cadre.propcadrmap where status in ('T','P') and proposed_rowno is null and propno={0})",prono);

        string sql = string.Format("select empid, pshr.get_fullname(empid) as name from pshr.empperso em where " +
            "empid in (select empid from cadre.propcadrmap where status in ('T','P','O') and proposed_rowno is null and propno={0})", PRONO);

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        drpOfficer.Items.Clear();
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            string text = String.Format("{0} ({1})", row["name"].ToString(), row["empid"].ToString());
            string val = row["empid"].ToString();
            drpOfficer.Items.Add(new ListItem(text, val));
        }
        //FillLblLoc();
        //txtName.Visible = false;
        //lblName.Visible = false;
        //lblPosting.Visible = false;
        //txtEmpid.Visible = false;
        //lblEmpid.Visible = false;
        //txtLoc.Visible = false;
        //Label8.Visible = false;
        //drpSearchby.Visible = false;
        //drpOfficer.Visible = true;
    }
    private string ExtractDesgcode(string designation)
    {
        //get desgcode from entry of type "desgtext-desgcode"
        //if (Regex.IsMatch(designation, @"['!@#$%^*~`]+") == true)
        if (Regex.IsMatch(designation, @"^[\w\s,-/&()]+$") == false)
        {
            Utils.ShowMessageBox(this, "Invalid Designation");
            return null;
        }
        string dcode = designation.Split('-')[designation.Split('-').Length - 1];
        if (isExistsDesg(dcode))
            return dcode;
        else
            return null;
    }
    private bool isValidDesgcode(string designation)
    {
        //get desgcode from entry of type "desgtext-desgcode"
        //if (Regex.IsMatch(designation, @"['!@#$%^*~`]+") == true)
        if (Regex.IsMatch(designation, @"^[\w\s,-/&]+$") == false)
        {
            return false;
        }
        string dcode = designation.Split('-')[designation.Split('-').Length - 1];
        if (isExistsDesg(dcode))
            return true;
        else
            return false;
    }
    private string ExtractLoccode(string location)
    {
        //get loccode from entry of type "locname-loccode"
        //^[\w\s,-]+$
        //if (Regex.IsMatch(location, @"['!@#$%^*~`]+") == true)
        if (Regex.IsMatch(location, @"^[\w\s,-/&.()]+$") == false)
        {
            Utils.ShowMessageBox(this, "Invalid Location");
            return null;
        }
        string lcode = location.Split('-')[location.Split('-').Length - 1];
        if (isExistsLoc(lcode))
            return lcode;
        else
            return null;
    }
    private bool isExistsDesg(string dcode)
    {
        string sql = string.Format("select count(*) from pshr.mast_desg where desgcode = '{0}'", dcode);
        string count = OraDBConnection.GetScalar(sql);
        return (count != null && count == "1");
    }
    private bool isExistsLoc(string lcode)
    {
        string sql = string.Format("select count(*) from pshr.mast_loc where loccode = '{0}'", lcode);
        string count = OraDBConnection.GetScalar(sql);
        return (count != null && count == "1");
    }
    private bool FillAllLocations(string filter = "")
    {
        string sql = "";
        string dcode = hidolddesgcode.Value;
        string status = hidStatus.Value;
        string branch = hidbranch.Value;
        string vacfilter = drpFilter.SelectedValue;
        string curdesg = string.Empty;
        string filterexp = string.Empty;
        if (dcode == null)
        {
            Utils.ShowMessageBox(this, "Please enter this record in cadrmap first");
            return false;
        }

        curdesg = dcode;
        //to show higher posts also in case of transfer
        //to handle cases like AEE at paycharge of Sr.XEN
        if (vacfilter == "H")
        {
            dcode = OraDBConnection.GetScalar("select nextdesg from cadre.nextdesg where desg = " + curdesg);
            if (string.IsNullOrEmpty(dcode))
            {
                Utils.ShowMessageBox(this, "Unable to find Next Higher Designation");
                return false;
            }
            //reset vacfilter to make it behave as ALL case
            //vacfilter = "A";
        }
        else
        {
            switch (dcode)
            {
                case "9047":
                    dcode = " in (9048) ";
                    break;
                case "9365":
                    dcode = " in (9050) ";
                    break;
                case "9366":
                    dcode = " in (9052) ";
                    break;
                //case "9056":
                case "9057":
                case "9060":
                    dcode = " in (9056,9057) ";
                    break;
                default:
                    dcode = " in (" + dcode + ") ";
                    break;
            }
        }


        //if branch is not civil(4) then consider branch as elec(1)
        if (branch != "4")
        {
            branch = "1";
        }

        //create filter expression
        filterexp = (filter != "") ? string.Format(" and upper(locname) like upper('%{0}%')", filter.Replace(" ","%")) : "";
        
        if (status == "T" || status == "O")
        {
            if (vacfilter == "A" || vacfilter == "E")
            {
                //Empty Locations, val = rowno
                sql = string.Format("select pshr.get_desg(desgcode) as dname, indx, locabb as lname," +
                                        "'E' as stat, rowno as val, c.loccode, null as empid from cadre.cadr c, pshr.mast_loc ml where " +
                                        "desgcode {0} and (hia is null or hia = 0) and rowno not in (select rowno from cadre.cadrmap) " +
                                        "and c.loccode=ml.loccode and branch = {1} {2}", dcode, branch, filterexp);
            }
            if (vacfilter == "A" || vacfilter == "F")
            {
                //Filled Locations, val = rowno
                sql += (!string.IsNullOrEmpty(sql)) ? " union all " : "";
                sql += string.Format("select pshr.get_desg(desgcode) as dname, indx, locabb as lname," +
                                        "'F' as stat, c.rowno as val, c.loccode, cm.empid as empid from cadre.cadr c,cadre.cadrmap cm, pshr.mast_loc ml where " +
                                        "desgcode {0} and (hia is null or hia = 0) and c.rowno in (select rowno from cadre.cadrmap) " +
                                        "and c.loccode=ml.loccode and branch = {1} and c.rowno = cm.rowno {2}", dcode, branch,filterexp);
            }
            if (vacfilter == "H")
            {
                sql = string.Format("select pshr.get_desg(desgcode) as dname, indx, locabb as lname, 'E' as stat, " +
                                        "rowno as val, c.loccode, null as empid from cadre.cadr c, pshr.mast_loc ml where " +
                                        "desgcode in ({0}) and rowno not in (select rowno from cadre.cadrmap) " +
                                        "and c.loccode=ml.loccode and (hia is null or hia = 0) and branch = {1} {2}", dcode, branch, filterexp);
                sql += " union all ";
                sql += string.Format("select pshr.get_desg(desgcode) as dname, indx, locabb as lname, 'F' as stat, " +
                                        "c.rowno as val, c.loccode, cm.empid as empid from cadre.cadr c, cadre.cadrmap cm, pshr.mast_loc ml where " +
                                        "desgcode in ({0}) and c.rowno in (select rowno from cadre.cadrmap) " +
                                        "and c.loccode=ml.loccode and (hia is null or hia = 0) and branch = {1} and c.rowno = cm.rowno {2}", dcode, branch, filterexp);
            }
        }
        else if (status == "P")
        {
            //special handling in case of AAE -> show both AE and AEE sanctioned locations
            string nextdesg = ((new string[] { "9499", "9500", "9535", "9544", "9067" , "9088", "9501", "9077"}).Contains(curdesg)) ? "9057,9056" : string.Empty;
            
            if(string.IsNullOrEmpty(nextdesg))
            {
                nextdesg = OraDBConnection.GetScalar("select nextdesg from cadre.nextdesg where desg = " + curdesg);
                if (string.IsNullOrEmpty(nextdesg))
                {
                    Utils.ShowMessageBox(this, "Unable to find Next Higher Designation");
                    return false;
                }
            }
            if (vacfilter == "A" || vacfilter == "E")
            {
                //Empty Locations, val = rowno
                sql = string.Format("select pshr.get_desg(desgcode) as dname, indx, locabb as lname, 'E' as stat, " +
                                        "rowno as val, c.loccode, null as empid from cadre.cadr c, pshr.mast_loc ml where " +
                                        "desgcode in ({0}) and rowno not in (select rowno from cadre.cadrmap) " +
                                        "and c.loccode=ml.loccode and (hia is null or hia = 0) and branch = {1} {2}", nextdesg, branch, filterexp);
            }
            if (vacfilter == "A" || vacfilter == "F")
            {
                //Filled Locations, val = rowno
                sql += (!string.IsNullOrEmpty(sql)) ? " union all " : "";
                sql += string.Format("select pshr.get_desg(desgcode) as dname, indx, locabb as lname, 'F' as stat, " +
                                        "c.rowno as val, c.loccode, cm.empid as empid from cadre.cadr c, cadre.cadrmap cm, pshr.mast_loc ml where " +
                                        "desgcode in ({0}) and c.rowno in (select rowno from cadre.cadrmap) " +
                                        "and c.loccode=ml.loccode and (hia is null or hia = 0) and branch = {1} and c.rowno = cm.rowno {2}", nextdesg, branch, filterexp);
            }
        }
        else
        {
            Utils.ShowMessageBox(this, "Error getting sanctioned locations");
            return false;
        }

        //show special locations, val = loccode
        if (vacfilter == "A" || vacfilter == "S")
        {
            sql += (!string.IsNullOrEmpty(sql)) ? " union all " : "";
            sql += string.Format("select  '0' as dname, 0 as indx, locabb as lname," +
                                    "'S' as stat, loccode as val, loccode,null as empid from pshr.mast_loc where " +
                                    "(loccode like '6%' or loccode like '7%') and loccode <> '601000000' {0}", filterexp);
        }
        sql += " order by stat, loccode,indx";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        drpLocs.Items.Clear();
        drpLocs.Items.Insert(0, new ListItem("--Select Pay Charge Location--", "-1"));

        string name103 = "O&M/GGSSTP, ROPAR";
        string name104 = "O&M/GHTP, LEHRA-MOHABBAT";
        string name105 = "O&M/GNDTP, BATHINDA";
        string name106 = "CE / THERMAL DESIGN, PATIALA";
        string name108 = "T.S., PTA.";

        int cnt103 = 1;
        int cnt104 = 1;
        int cnt105 = 1;
        int cnt106 = 1;
        int cnt108 = 1;
        
        ArrayList list103 = new ArrayList();
        ArrayList list104 = new ArrayList();
        ArrayList list105 = new ArrayList();
        ArrayList list106 = new ArrayList();
        ArrayList list108 = new ArrayList();
        ArrayList listOther = new ArrayList();

        string datatext, dataval, loccode;
        StringBuilder sbDataText = new StringBuilder(300);
        ListItem litem2;
        string loc_org;
        string back_color;

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            sbDataText.Clear();
            loccode = row["loccode"].ToString();
            loc_org = loccode.Substring(0, 3);

            if (loc_org == "103")
            {
                sbDataText.Append(row["dname"]).Append('-').Append(cnt103++).Append(" at ").Append(name103).Append(" (").Append(row["stat"]).Append(") ").Append(row["empid"]);
                back_color = "background-color:Aqua;";
            }
            else if (loc_org == "104")
            {
                sbDataText.Append(row["dname"]).Append('-').Append(cnt104++).Append(" at ").Append(name104).Append(" (").Append(row["stat"]).Append(") ").Append(row["empid"]);
                back_color = "background-color:Bisque;";
            }
            else if (loc_org == "105")
            {
                sbDataText.Append(row["dname"]).Append('-').Append(cnt105++).Append(" at ").Append(name105).Append(" (").Append(row["stat"]).Append(") ").Append(row["empid"]);
                back_color =  "background-color:Azure;";
            }
            else if (loc_org == "106")
            {
                sbDataText.Append(row["dname"]).Append('-').Append(cnt106++).Append(" at ").Append(name106).Append(" (").Append(row["stat"]).Append(") ").Append(row["empid"]);
                back_color ="background-color:Beige;";
            }
            else if (loc_org == "108")
            {
                sbDataText.Append(row["dname"]).Append('-').Append(cnt108++).Append(" at ").Append(name108).Append(" (").Append(row["stat"]).Append(") ").Append(row["empid"]);
                back_color = "background-color:Aquamarine;";
            }
            else
            {
                sbDataText.Append(row["dname"]).Append('-').Append(row["indx"]).Append(" at ").Append(row["lname"]).Append(" (").Append(row["stat"]).Append(") ").Append(row["empid"]);
                back_color = "background-color:aliceblue;";
            }
            litem2 = new ListItem(sbDataText.ToString(), row["val"].ToString());
            litem2.Attributes.Add("style", back_color);
            drpLocs.Items.Add(litem2);
        }

        if (vacfilter != "H")
        {
            //add events, val = EV_<eventcode>
            sql = "select event || ' ('|| EREF ||')' evtext, 'EV_' || EVENTCODE as evval from mast_event " +
                    "where egrp in ('RETD','LEAVE') and rdate =1 order by egrp,eref";
            ds.Clear();
            ds = OraDBConnection.GetData(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                datatext = row["evtext"].ToString();
                dataval = row["evval"].ToString();

                drpLocs.Items.Add(new ListItem(datatext, dataval));
                drpLocs.Items[drpLocs.Items.Count - 1].Attributes.Add("style", "background-color:LightSlateGray;");
            }
        }
        return true;
    }
    private void ClearRightFields()
    {
        //show newEmpid textbox only if non-gaz id is selected
        txtNewEmpid.Visible = !(hidEmpID.Value.StartsWith("10"));
        txtLocFilter.Text = string.Empty;
        txtCLoc.Text = string.Empty;
        txtCDesg.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        txtSno.Text = string.Empty;
        hidolddesgcode.Value = string.Empty;
        hidStatus.Value = string.Empty;
        lblInfo.Text = string.Empty;
        hidsno.Value = string.Empty;
        hidbranch.Value = string.Empty;
        txtPrvComment.Text = string.Empty;
        txtDispLeft.Text = string.Empty;
        txtDispRight.Text = string.Empty;
        drpLocs.Items.Clear();
    }
    private bool isFilled(string row)
    {
        string sql = "select count(*) from cadre.cadrmap where rowno=" + row;
        return OraDBConnection.GetScalar(sql) == "1";
    }
    private bool isAlreadyProposed(string row, int propno)
    {
        string sql = string.Format("select count(*) from cadre.propcadrmap where proposed_rowno = {0} and empid <> {1} and propno={2}",
            row, drpOfficer.SelectedValue,propno);
        return OraDBConnection.GetScalar(sql) == "1";
    }
    private bool isOtherRowUpdated(string row)
    {
        string sql = string.Format("select count(*) from cadre.propcadrmap where rowno={0} and status in ('P','T','O') " +
                                        "and proposed_rowno is not null", row);
        return OraDBConnection.GetScalar(sql) == "1";
    }
    private void ReassignLocation()
    {
        string sql;

        sql = "update cadre.cadrmap set proposed_rowno=null, cloccode=null, cdesgcode=null, remarks = null, oonum=null, odate=null where empid=";
        OraDBConnection.ExecQry(sql);
    }
    private void BypassLogin()
    {
        Session["loginy"] = "1";
        Session["username"] = "cadre";
        //Response.Redirect("./DataEntry.aspx");
        //Response.Redirect("./tnp.aspx");
    }
    private void ShowHideRightControls(bool shide)
    {
        txtLocFilter.Visible = shide;
        txtCDesg.Visible = shide;
        txtRemarks.Visible = shide;
    }
    private bool Save_Temp()
    {
        string oonum = txtOoNum.Text;
        string oodate = txtOoDate.Text;
        string propno = this.PRONO.ToString();

        string empid, eventcode = "0", cdesgcode, cloccode, rowno, proposed_rowno;
        string pcloccode, sancdesg, sancindx, eventhistoryid;
        string sql;
        bool ret;
        DataSet ds2;
        DataRow row2;

        DataSet ds = OraDBConnection.GetData("select * from cadre.propcadrmap where propno=" + propno);
        if (ds.Tables[0].Rows.Count < 1)
        {
            Utils.ShowMessageBox(this, "Nothing to save.");
            return false;
        }

        //delete old and possibly incomplete change made to emphistory due to previous saving of this office order
        OraDBConnection.ExecQry("delete from emphistory where oonum='" + oonum + "' and odate = '" + oodate + "'");

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            empid = row["empid"].ToString();
            switch (row["status"].ToString())
            {
                case "P":
                    eventcode = "28";
                    break;
                case "T":
                    eventcode = "36";
                    break;
                case "O":
                    eventcode = "37";
                    break;
                case "S":
                    eventcode = "12";
                    break;
                case "V":
                    eventcode = "13";
                    break;
            }
            cdesgcode = row["cdesgcode"].ToString();
            cloccode = row["cloccode"].ToString();
            rowno = OraDBConnection.GetScalar("select nvl(max(rowno),0)+1 from pshr.emphistory where empid=" + empid);

            if (cloccode == "999999999")
            {
                eventcode = "2";
            }

            proposed_rowno = row["proposed_rowno"].ToString();
            if (string.IsNullOrEmpty(proposed_rowno))
            {
                //handling for retiree
                pcloccode = string.Empty;
                sancdesg = string.Empty;
                sancindx = string.Empty;
            }
            else if (proposed_rowno.Length == 9)
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

            if (!string.IsNullOrEmpty(pcloccode))
            {
                switch (pcloccode.Substring(0, 3))
                {
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
            }
            //insert into emphistory
            if (string.IsNullOrEmpty(proposed_rowno))
            {
                //in case of retirement
                sql = string.Format("insert into pshr.emphistory(empid,eventcode,rowno," +
                                        "eventhistoryid,oonum,odate,status) values " +
                                        "({0},{1},{2},{3},'{4}','{5}',1)",
                                        empid, eventcode, rowno, eventhistoryid, oonum, oodate);
            }
            else if (proposed_rowno.Length == 9)
            {
                //in case of transfer/promotion at Special Locations
                sql = string.Format("insert into pshr.emphistory(empid,eventcode,desgcode,loccode,rowno," +
                                        "eventhistoryid, pcloccode,oonum,odate,status) values " +
                                        "({0},{1},{2},{3},{4},{5},{6},'{7}','{8}',1)",
                                        empid, eventcode, cdesgcode, cloccode, rowno, eventhistoryid, pcloccode, oonum, oodate);
            }
            else
            {
                //in case of normal location
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
        }
        return true;
    }
    private void ChangeTables(string empid)
    {
        string sql;
        string oonum, postrel, postjoin, eventcode, loccode, desgcode;
        string jloccode, jdesgcode, jindx;
        DateTime odate;

        System.Data.DataSet ds = new System.Data.DataSet();
        System.Data.DataRow drow;

        oonum = txtOoNum.Text;
        //get values
        sql = string.Format("select * from cadre.chargereport where oonum = '{0}' and empid = {1} order by oodate desc", oonum, empid);
        ds = OraDBConnection.GetData(sql);

        if (ds.Tables[0].Rows.Count != 1)
        {
            //lMsg0.Text = "No Pending Row";
            return;
        }
        drow = ds.Tables[0].Rows[0];

        
        //check and get o/o date
        if (!Convert.IsDBNull(drow["oodate"].ToString()))
        {
            odate = (DateTime)drow["oodate"];
        }
        else
        {
            //lblMsg.Text = "Invalid O/o Date";
            return;
        }
        
        postrel = drow["postrel"].ToString();
        postjoin = drow["postjoin"].ToString();
        eventcode = drow["eventcode"].ToString();
        loccode = drow["loccode"].ToString();
        desgcode = drow["desgcode"].ToString();

        ds.Clear();

        //postjoin is empty it means we are dealing with a special location
        if (string.IsNullOrEmpty(postjoin))
        {
            jloccode = string.Empty;
            jdesgcode = string.Empty;
            jindx = string.Empty;

            //delete entry from cadrmap
            sql = "delete from cadre.cadrmap where empid = " + empid;
            OraDBConnection.ExecQry(sql);
        }
        else
        {
            //get the trifecta from newrowno
            sql = "select loccode, desgcode, indx from cadre.cadr where rowno =" + postjoin;
            ds = OraDBConnection.GetData(sql);

            if (ds.Tables[0].Rows.Count != 1)
            {
                //lMsg0.Text = "Unable to get join row in cadre";
                return;
            }
            drow = ds.Tables[0].Rows[0];
            jloccode = drow["loccode"].ToString();
            jdesgcode = drow["desgcode"].ToString();
            jindx = drow["indx"].ToString();

            //update cadrmap
            sql = "delete from cadre.cadrmap where empid = " + empid + " or rowno = " + postjoin;
            OraDBConnection.ExecQry(sql);
            sql = string.Format("insert into cadre.cadrmap(empid,rowno) values ({0},{1})", empid, postjoin);
            OraDBConnection.ExecQry(sql);
        }

        //update empperso
        sql = string.Format("update pshr.empperso set cloccode = {0}, cdesgcode = {1} where empid = {2}",
            loccode, desgcode, empid);
        OraDBConnection.ExecQry(sql);

        //insert new row in emphistory, keeping fromdate and todate empty
        sql = string.Format("insert into pshr.emphistory(empid,eventcode,desgcode,loccode,rowno, " +
                    "eventhistoryid, pcloccode,sancdesg,sancindx,oonum,odate,status) values " +
                    "({0},{1},{2},{3},(select nvl(max(rowno),0)+1 from pshr.emphistory where empid={0}), " +
                    "(select max(eventhistoryid)+1 from pshr.emphistory),'{4}','{5}','{6}','{7}', " +
                    "to_date('{8}','DD-MON-YYYY hh:mi:ss AM'),1)",
                    empid, eventcode, desgcode, loccode, jloccode, jdesgcode, jindx, oonum,
                    odate.ToString("dd-MMM-yyyy hh:mm:ss tt"));
        OraDBConnection.ExecQry(sql);
    }
    private bool Save()
    {
        string empid, eventcode, cdesgcode, cloccode, proposed_rowno,oldrowno, newempid;
        string pcloccode, sancdesg, sancindx, sql, rstatus, remDate = string.Empty;
        bool ret = false;
        DataSet ds;
        rowTypes rowType;

        string oonum = txtOoNum.Text;
        string oodate = txtOoDate.Text;
        int[] leave_events = new int[]{1,2,3,4,5,6,7,8,9,62,63,86,98};
        int[] retd_events = new int[]{11,12,13,14,15,16,89};

        //check if any outstanding entry is pending
        string out_count = OraDBConnection.GetScalar("select count(*) from cadre.propcadrmap where "+
            "status in ('T','P','O') and proposed_rowno is null and propno="+PRONO);
        if (out_count != "0")
        {
            Utils.ShowMessageBox(this, "Outstanding entries are pending.");
            return false;
        }

        
        ds = OraDBConnection.GetData("select * from cadre.propcadrmap where propno=" + this.PRONO.ToString());
        if (ds.Tables[0].Rows.Count < 1)
        {
            Utils.ShowMessageBox(this, "Nothing to save.");
            return false;
        }

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            empid = row["empid"].ToString();
            rstatus = row["status"].ToString().Trim();

            if (rstatus == "P")
            {
                eventcode = "28"; //CPRO
            }
            else if (rstatus == "T")
            {
                eventcode = "36"; //CTRP
            }
            else if (rstatus == "O")
            {
                eventcode = "37"; //CTRO
            }
            else if (rstatus.StartsWith("EV_"))
            {
                eventcode = rstatus.Split('_')[1];
            }
            else
            {
                //give error or continue?
                continue;
            }
            cdesgcode = row["cdesgcode"].ToString();
            cloccode = row["cloccode"].ToString();

            //if location is "On Leave" then set eventcode to LELS
            if (cloccode == "999999999")
            {
                eventcode = "2";
            }

            oldrowno = row["rowno"].ToString();
            proposed_rowno = row["proposed_rowno"].ToString();
            newempid = row["newempid"].ToString();
            if (string.IsNullOrEmpty(proposed_rowno))
            {
                //handling for retirement,leave events
                pcloccode = string.Empty;
                sancdesg = string.Empty;
                sancindx = string.Empty;
                remDate = row["remarks"].ToString();
                rowType = rowTypes.LEAVE_RET_EVENT;
            }
            else if (proposed_rowno.Length == 9)
            {
                pcloccode = cloccode;
                sancdesg = cdesgcode;
                sancindx = "0";
                rowType = rowTypes.SPECIAL_LOC;
            }
            else
            {
                DataRow row2;
                DataSet ds2;
                ds2 = OraDBConnection.GetData("select loccode,desgcode,indx from cadre.cadr where rowno=" + proposed_rowno);
                row2 = ds2.Tables[0].Rows[0];
                pcloccode = row2["loccode"].ToString();
                sancdesg = row2["desgcode"].ToString();
                sancindx = row2["indx"].ToString();
                ds2.Clear();
                ds2.Dispose();
                rowType = rowTypes.NORMAL;
            }

            if (!string.IsNullOrEmpty(pcloccode))
            {
                //to mark at the disposal of Chief -
                //the following organizations
                switch (pcloccode.Substring(0, 3))
                {
                    case "103": //GGSSTP Ropar
                        pcloccode = "103000000";
                        break;
                    case "104": //GHTP LM
                        pcloccode = "104000000";
                        break;
                    case "105": //Thermal Design
                        pcloccode = "105000000";
                        break;
                    case "106": //GNDTP Bathinda
                        pcloccode = "106000000";
                        break;
                    case "108": //Transmission Systems
                        pcloccode = "108000000";
                        break;
                }
            }

            //check if this employee is already under transfer
            //sql = string.Format("select count(*) from cadre.chargereport where status is null and empid = {0}", empid);
            //if (OraDBConnection.GetScalar(sql) != "0")
            //{
            //    //remove old T & P entries from chargereport table cancelling old orders
            //    sql = "delete from cadre.chargereport where status is null and empid = " + empid;
            //    OraDBConnection.ExecQry(sql);
            //}

            sql = string.Empty;
            if (rowType == rowTypes.LEAVE_RET_EVENT)
            {
                //in case of leave event set cloccode = 999999999 (On Leave)
                if(leave_events.Contains(int.Parse(eventcode)))
                {
                    cloccode = "999999999";
                }

                //remarks column should contain date in the case of leave/retd events
                DateTime outDate;
                if (DateTime.TryParse(remDate, out outDate))
                {
                    sql = string.Format("insert into cadre.chargereport(empid, oonum, oodate, postrel, eventcode, loccode, desgcode, eventdate) " +
                    "values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                    empid, oonum, oodate, oldrowno, eventcode, cloccode, cdesgcode, remDate);
                }
                else
                {
                    Utils.ShowMessageBox(this, string.Format("Invalid Date for empid " + empid));
                    return false;
                }
            }
            else if (rowType == rowTypes.SPECIAL_LOC)
            {
                sql = string.Format("insert into cadre.chargereport(empid, oonum, oodate, postrel, eventcode, loccode, desgcode,newempid) " +
                   "values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                   empid, oonum, oodate, oldrowno, eventcode, cloccode, cdesgcode, newempid);
            }
            else if(rowType == rowTypes.NORMAL)
            {
                //in case of normal location
                sql = string.Format("insert into cadre.chargereport(empid, oonum, oodate, postrel, postjoin, eventcode, loccode, desgcode, newempid) " +
                    "values({0},'{1}','{2}',{3},{4},{5},{6},{7},'{8}')",
                    empid,oonum,oodate,oldrowno,proposed_rowno,eventcode,cloccode,cdesgcode, newempid);
            }

            if (sql != string.Empty)
            {
                ret = OraDBConnection.ExecQry(sql);
            }
            if (ret == false)
            {
                Utils.ShowMessageBox(this, string.Format("Unable to add entry for empid " + empid));
                return false;
            }

            //change the cadrmap, empperso and emphistory tables for this empid
            //keeping fromdate and todate blank
            ChangeTables(empid);
        }
        return true;
    }
    private void MakeReport(bool save = false)
    {
        string oonum = "-";
        string oodate = "-";
        string endono = "-";
        string notes = "1";
        string propno = this.PRONO.ToString();
        string bignote=string.Empty;
        if (save)
        {
            oonum = txtOoNum.Text;
            oodate = txtOoDate.Text;
            endono = txtEndorsNo.Text;
        }
        if (ddBigNotes.SelectedValue != "")
        {
            bignote = OraDBConnection.GetScalar(string.Format("select data from cadre.bignotes where name = '{0}'", ddBigNotes.SelectedValue));
        }
        string sql = "select m.sno,'" + oonum + "' as oonum1, '" + oodate + "' as oodate1, '" + notes + "' as notes," +
                    "'" + endono + "' as endono, " +
                    ddFSizePrev.SelectedValue + " as fsize, " +
                    "'" + bignote + "' as bignote, "+
                    "(select count(*) from cadre.propcadrmap where propno = " + propno + ") TotCount, " +
                    "(select count(*) from cadre.propcadrmap where status = 'P' and propno = " + propno + ") PCount, " +
                    " pshr.get_fullname(e.empid) as fullname,to_char(e.empid) as empid,to_char(e.dob,'dd-mm-yyyy') as dob," +
                    "pshr.get_post(e.cloccode) as old_work_loc,e.cloccode as old_work_loccode,pshr.get_desg(e.cdesgcode) as old_work_desg," +
                    "e.cdesgcode as old_work_desgcode," +
                    " DECODE(m.rowno,0,pshr.get_post(e.cloccode), pshr.get_post(cadre.get_lcode_rno(m.rowno))) AS old_pc_loc," +
                    " DECODE(m.rowno,0,e.cloccode, cadre.get_lcode_rno(m.rowno)) AS old_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.rowno))) AS old_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.rowno)) AS old_pc_desgcode, " +
                    " DECODE(m.rowno,0,'0', cadre.get_indx_rno(m.rowno)) AS old_pc_indx, " +
                    " cadre.get_org_plants(m.cloccode) as new_work_loc," +
                    "m.cloccode as new_work_loccode,pshr.get_desg(m.cdesgcode) as new_work_desg,m.cdesgcode as new_work_desgcode," +
                    "decode(length(m.proposed_rowno),9,pshr.get_post(m.proposed_rowno), cadre.get_org_plants(cadre.get_lcode_rno(m.proposed_rowno))) AS new_pc_loc," +
                    "decode(length(m.proposed_rowno),9,m.proposed_rowno, cadre.get_lcode_rno(m.proposed_rowno)) AS new_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno))) AS new_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.proposed_rowno))                               AS new_pc_desgcode, " +
                    "cadre.get_indx_rno(m.proposed_rowno) as new_pc_indx, m.remarks,'G' as grp,m.propno, to_char(m.newempid) as newempid, m.status, "+
                    "m.disp_left, m.disp_right, "+
                    "pshr.get_soccat(e.empid) as categ " +
                    "from pshr.empperso e, cadre.propcadrmap m "+
                    "where e.empid=m.empid and m.status is not null " +
                    " AND M.STATUS NOT IN ('S','V') and m.propno=" + propno +
                    " AND m.cloccode is not null" +
                    " order by sno";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        string pdfPath;
        if (save)
        {
            pdfPath = Server.MapPath("office_orders\\" + oonum + "-BEG-I" + oodate + ".pdf");
        }
        else
        {
            pdfPath = Server.MapPath("office_orders\\preview-"+propno +"-"+ DateTime.Now.ToString("yyyyMMdd-HHmmssfff") + ".pdf");
        }

        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rptposttrans.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();

        //System.Data.DataSet dsnotes;
        //sql = "select * from cadre.notes_proposal_person where ccnum > 0 and proposalno = " + propno + " order by sno";
        //dsnotes = OraDBConnection.GetData(sql);
        //CrystalReportSource1.ReportDocument.Subreports["notes"].SetDataSource(dsnotes.Tables[0]);
        //CrystalReportSource1.DataBind();

        System.Data.DataSet dscc;
        sql = "select * from cadre.cclist_proposal_person where ccnum > 0 and proposalno = " + propno + " order by sno";
        dscc = OraDBConnection.GetData(sql);
        CrystalReportSource1.ReportDocument.Subreports["cclists"].SetDataSource(dscc.Tables[0]);
        CrystalReportSource1.DataBind();
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);

        Utils.DownloadFile(pdfPath);
    }
    private void Makeproreport(bool save = false)
    {
        string oonum = "-";
        string oodate = "-";
        string notes = "1";
        string propno = this.PRONO.ToString();
        string approver = ddApprover.SelectedValue;

        if (save)
        {
            oonum = Regex.Escape(txtOoNum.Text);
            oodate = txtOoDate.Text;
        }
        string sql = "select m.sno,'" + oonum + "' as oonum1, '" + oodate + "' as oodate1, '" + notes + "' as notes,"+
                    "'" + approver + "' as approver, " +
                    "'" + ddPropLineMode.SelectedValue + "' as proplinemode, " +
                    "'" + txtPropLine.Text + "' as proplinetext, " +
                    "'" + ddPropLastLineMode.SelectedValue + "' as proplastlinemode, " +
                    "'" + txtPropLastLine.Text + "' as proplastlinetext, " +
                    ddFSize.SelectedValue + " as fsize, " +
                    "(select count(*) from cadre.propcadrmap where propno = " + propno + ") TotCount, " +
                    "(select count(*) from cadre.propcadrmap where status = 'P' and propno = "+propno +") PCount, " +
                    " pshr.get_fullname(e.empid) as fullname,to_char(e.empid) as empid, to_char(m.newempid) as newempid, to_char(e.dob,'dd-mm-yyyy') as dob," +
                    "pshr.get_post(e.cloccode) as old_work_loc,e.cloccode as old_work_loccode,pshr.get_desg(e.cdesgcode) as old_work_desg," +
                    "e.cdesgcode as old_work_desgcode," +
                    "DECODE(m.rowno,0,pshr.get_post(e.cloccode), pshr.get_post(cadre.get_lcode_rno(m.rowno))) AS old_pc_loc," +
                    " DECODE(m.rowno,0,e.cloccode, cadre.get_lcode_rno(m.rowno)) AS old_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.rowno))) AS old_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.rowno)) AS old_pc_desgcode, " +
                    " DECODE(m.rowno,0,'0', cadre.get_indx_rno(m.rowno)) AS old_pc_indx, " +
                    " cadre.get_org_plants(m.cloccode) as new_work_loc," +
                    "m.cloccode as new_work_loccode,pshr.get_desg(m.cdesgcode) as new_work_desg,m.cdesgcode as new_work_desgcode," +
                    "decode(length(m.proposed_rowno),9,pshr.get_post(m.proposed_rowno), cadre.get_org_plants(cadre.get_lcode_rno(m.proposed_rowno))) AS new_pc_loc," +
                    "decode(length(m.proposed_rowno),9,m.proposed_rowno, cadre.get_lcode_rno(m.proposed_rowno)) AS new_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno))) AS new_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.proposed_rowno))                               AS new_pc_desgcode, " +
                    "cadre.get_indx_rno(m.proposed_rowno) as new_pc_indx, m.remarks,'G' as grp,m.propno, m.newempid,m.status,m.disp_left, m.disp_right "+
                    "from pshr.empperso e, cadre.propcadrmap m where e.empid=m.empid and m.status is not null " +
                    " AND M.STATUS NOT IN ('S','V') and m.propno=" + propno + 
                    " AND m.cloccode is not null" +
                    " order by sno";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        string pdfPath;
        if (save)
        {
            pdfPath = Server.MapPath("office_orders\\" + oonum + "-BEG-I" + oodate + ".pdf");
        }
        else
        {
            pdfPath = Server.MapPath("office_orders\\proposal_" + propno + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
        }

        CrystalReportSource1.ReportDocument.Refresh();
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rptproposal.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();
        
        //System.Data.DataSet dsnotes;
        //sql = "select * from cadre.notes_proposal_person where ccnum > 0 and proposalno = " + propno + " order by sno";
        //dsnotes = OraDBConnection.GetData(sql);
        //CrystalReportSource1.ReportDocument.Subreports["notes"].SetDataSource(dsnotes.Tables[0]);
        //CrystalReportSource1.DataBind();

        
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);

        Utils.DownloadFile(pdfPath);
    }
    private void Makeproreport_pc(bool save = false)
    {
        string oonum = "-";
        string oodate = "-";
        string notes = "1";
        string propno = this.PRONO.ToString();
        string approver = ddApprover.SelectedValue;

        if (save)
        {
            oonum = Regex.Escape(txtOoNum.Text);
            oodate = txtOoDate.Text;
        }
        string sql = "select m.sno,'" + oonum + "' as oonum1, '" + oodate + "' as oodate1, '" + notes + "' as notes," +
                    "'" + approver + "' as approver, " +
                    "'" + ddPropLineMode.SelectedValue + "' as proplinemode, " +
                    "'" + txtPropLine.Text + "' as proplinetext, " +
                    "'" + ddPropLastLineMode.SelectedValue + "' as proplastlinemode, " +
                    "'" + txtPropLastLine.Text + "' as proplastlinetext, " +
                    ddFSize.SelectedValue + " as fsize, "+
                    "(select count(*) from cadre.propcadrmap where propno = " + propno + ") TotCount, " +
                    "(select count(*) from cadre.propcadrmap where status = 'P' and propno = " + propno + ") PCount, " +
                    " pshr.get_fullname(e.empid),to_char(e.empid) as empid,e.dob," +
                    "pshr.get_post(e.cloccode) as old_work_loc,e.cloccode as old_work_loccode,pshr.get_desg(e.cdesgcode) as old_work_desg," +
                    "e.cdesgcode as old_work_desgcode," +
                    "DECODE(m.rowno,0,pshr.get_post(e.cloccode), pshr.get_post(cadre.get_lcode_rno(m.rowno))) AS old_pc_loc," +
                    " DECODE(m.rowno,0,e.cloccode, cadre.get_lcode_rno(m.rowno)) AS old_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.rowno))) AS old_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.rowno)) AS old_pc_desgcode, " +
                    " DECODE(m.rowno,0,'0', cadre.get_indx_rno(m.rowno)) AS old_pc_indx, " +
                    " cadre.get_org_plants(m.cloccode) as new_work_loc," +
                    "m.cloccode as new_work_loccode,pshr.get_desg(m.cdesgcode) as new_work_desg,m.cdesgcode as new_work_desgcode," +
                    "decode(length(m.proposed_rowno),9,pshr.get_post(m.proposed_rowno), cadre.get_org_plants(cadre.get_lcode_rno(m.proposed_rowno))) AS new_pc_loc," +
                    "decode(length(m.proposed_rowno),9,m.proposed_rowno, cadre.get_lcode_rno(m.proposed_rowno)) AS new_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno))) AS new_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.proposed_rowno))                               AS new_pc_desgcode, " +
                    "cadre.get_indx_rno(m.proposed_rowno) as new_pc_indx, m.remarks,m.prvcomment, 'G' as grp,m.propno, m.newempid,m.status,m.disp_left, m.disp_right " +
                    "from pshr.empperso e, cadre.propcadrmap m where e.empid=m.empid and m.status is not null " +
                    " AND M.STATUS NOT IN ('S','V') and m.propno=" + propno +
                    " AND m.cloccode is not null" +
                    " order by sno";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        string pdfPath;
        if (save)
        {
            pdfPath = Server.MapPath("office_orders\\" + oonum + "-BEG-I" + oodate + ".pdf");
        }
        else
        {
            pdfPath = Server.MapPath("office_orders\\proposal-" + propno + "-" + DateTime.Now.ToString("yyyyMMdd-HHmmssfff") + ".pdf");
        }

        CrystalReportSource1.ReportDocument.Refresh();
        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rptproposal_pc.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();

        //System.Data.DataSet dsnotes;
        //sql = "select * from cadre.notes_proposal_person where ccnum > 0 and proposalno = " + propno + " order by sno";
        //dsnotes = OraDBConnection.GetData(sql);
        //CrystalReportSource1.ReportDocument.Subreports["notes"].SetDataSource(dsnotes.Tables[0]);
        //CrystalReportSource1.DataBind();


        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);

        Utils.DownloadFile(pdfPath);
    }
    private bool ShowInfo(string empid)
    {
        string sql = "select pshr.get_fullname(e.empid) as name, " +
            "e.cdesgcode as cdesgcode, pshr.get_desg(e.cdesgcode) as cdesg,  i.photo2 as photo, " +
            "e.cloccode as cloccode, pshr.get_org(e.cloccode) as cloc, " +
            "cm.rowno as rowno, nvl(cadre.get_mapping_text(e.empid),'(None)') as maptext " +
            "from pshr.empperso e left outer join img_pshr.img i on e.empid = i.empid " +
            "left outer join cadre.cadrmap cm " +
            "on cm.empid = e.empid where e.empid = " + empid;
        DataSet ds = OraDBConnection.GetData(sql);
        if (ds.Tables[0].Rows.Count != 1)
        {
            return false;
        }
        DataRow drow = ds.Tables[0].Rows[0];

        lblInfoName.Text = drow["name"].ToString();
        lblInfoDesg.Text = drow["cdesg"].ToString();
        lblInfoWLoc.Text = drow["cloc"].ToString();
        lblInfoPCLoc.Text = drow["maptext"].ToString();

        hidEmpID.Value = empid;
        hidWDesgCode.Value = drow["cdesgcode"].ToString();
        hidWLoccode.Value = drow["cloccode"].ToString();
        hidPCRowNo.Value = drow["rowno"].ToString();
        //load photo
        if (!Convert.IsDBNull(ds.Tables[0].Rows[0]["photo"]))
        {
            byte[] barr = (byte[])ds.Tables[0].Rows[0]["photo"];
            string base64str = Convert.ToBase64String(barr);
            imgEmpPhoto.ImageUrl = string.Format("data:image/gif;base64,{0}", base64str);
        }
        ClearRightFields();
        return true;
    }
    private void ClearInfo(bool clearMsgLbls = true)
    {
        lblInfoDesg.Text = string.Empty;
        lblInfoName.Text = string.Empty;
        lblInfoPCLoc.Text = string.Empty;
        lblInfoWLoc.Text = string.Empty;
        imgEmpPhoto.ImageUrl = null;
        hidEmpID.Value = string.Empty;
        hidWDesgCode.Value = string.Empty;
        hidWLoccode.Value = string.Empty;
        hidPCRowNo.Value = string.Empty;
        if (clearMsgLbls)
        {
            lblMsgNew.Text = string.Empty;
            lblMsgOut.Text = string.Empty;
        }
    }
    private bool CheckForUnderTransfer(string empid, ref Label lblMsg)
    {
        //check if this employee is already under transfer with status is null i.e. neither relieving or joining in progress
        string sql = string.Format("select status, oonum, to_char(oodate,'dd-mm-yyyy') as oodate from cadre.chargereport " +
            "where nvl(status,'null') <> 'JRA' and empid = {0} order by oodate desc", empid);

        DataSet ds = OraDBConnection.GetData(sql);

        if (ds.Tables[0].Rows.Count == 0)
        {
            return true;
        }
        string status = ds.Tables[0].Rows[0]["status"].ToString();
        string oodate = ds.Tables[0].Rows[0]["oodate"].ToString();
        string oonum = ds.Tables[0].Rows[0]["oonum"].ToString();

        if (ds.Tables[0].Rows.Count > 1)
        {
            lblMsg.Text = "Multiple under transfer rows found. Aborting";
            return false;
        }
        //if status is null then under transfer procedure is not yet started.
        //can safely cancel the last order
        else if (string.IsNullOrEmpty(status))
        {
            lblMsg.Text = String.Format("Already u/t. O/o No. {0} Dt. {1}", oonum, oodate);
            return true;
        }
        //if relieving or joining procedure is already started then cannot safely cancel the prev. order, hence abort
        else
        {
            string statusDesc = string.Empty;
            switch (status)
            {
                case "RRS":
                    statusDesc = "Relieving Req. Submitted";
                    break;
                case "RRA":
                    statusDesc = "Relieving Req. Accepted";
                    break;
                case "JRS":
                    statusDesc = "Joining Req. Submitted";
                    break;
            }

            lblMsg.Text = String.Format("Already u/t. O/o No. {0} Dt. {1}. Status {2}. Aborting",
                ds.Tables[0].Rows[0]["oonum"], ds.Tables[0].Rows[0]["oodate"], statusDesc);
            return false;
        }
    }
    private bool CheckOONum()
    {
        DateTime checkdate;

        if (txtOoNum.Text.Length < 1)
        {
            Utils.ShowMessageBox(this, "Enter O/o Number");
            return false;
        }
        if (txtEndorsNo.Text.Length < 1)
        {
            Utils.ShowMessageBox(this, "Enter Endorsement Number");
            return false;
        }
        if (DateTime.TryParse(txtOoDate.Text, out checkdate) == false)
        {
            Utils.ShowMessageBox(this, "Enter a valid date");
            return false;
        }

        //check if oonum is unique
        string sql = string.Format("select count(*) from cadre.tp_proposals where oonum = '{0}' and pno != '{1}'", txtOoNum.Text, PRONO);
        if (OraDBConnection.GetScalar(sql) != "0")
        {
            Utils.ShowMessageBox(this, "This Office Order Number already exists for a different proposal");
            return false;
        }
        return true;
    }
    private void FillBigNotes()
    {
        string sql = "select name, name as txtval from cadre.bignotes where type='N' order by addedon desc";
        //string sql = "select name, name || '(' || tags || ')' as txtval from cadre.bignotes where type='N' order by addedon desc";
        ddBigNotes.DataSource = OraDBConnection.GetData(sql);
        ddBigNotes.DataTextField = "txtval";
        ddBigNotes.DataValueField = "name";
        ddBigNotes.DataBind();
        ddBigNotes.Items.Insert(0, new ListItem("Select Note", ""));
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        string str_propno = Session["proposalno"] as string;
        if (str_propno == null)
        {
            //Response.Redirect("Login.aspx");
            return;
        }
        PRONO = int.Parse(str_propno);
        lblProposalName.Text = Session["proposalname"].ToString() + " (" + Session["proposaldate"].ToString() + ")";
        BypassLogin();
        if (!IsPostBack)
        {
            if ((Session["loginy"] == null) || (Session["loginy"].ToString() != "1"))
            {
                //Response.Redirect("Login.aspx");
                return;
            }
            lblInfo.Text = "";

            //string sql = "update cadre.cadrmap set status=null, proposed_rowno=null,remarks=null";
            //if (OraDBConnection.ExecQry(sql) == false)
            //{
            //    Utils.ShowMessageBox(this, "Unable to update cadre.cadrmap");
            //}

            //propose.Enabled = false;
            divpropose.Disabled = true;
            //set last cadre code
            Utils.last_cadre_cd = 0;

            //lblEmpid.Visible = true;
            //lblName.Visible = false;
            //lblPosting.Visible = false;
            //txtEmpid.Visible = true;
            //txtName.Visible = false;
            //txtLoc.Visible = false;

            string sqlPropLine = "select proplinemode, proplinetext, LASTLINEMODE, LASTLINETEXT,bignote from cadre.tp_proposals where pno = " + PRONO;
            DataRow drow = OraDBConnection.GetData(sqlPropLine).Tables[0].Rows[0];
            if (drow["proplinemode"].ToString() == "A")
            {
                ddPropLineMode.SelectedIndex = 0;
            }
            else
            {
                ddPropLineMode.SelectedIndex = 1;
                txtPropLine.Text = drow["proplinetext"].ToString();
                txtPropLine.Visible = true;
            }
            if (drow["LASTLINEMODE"].ToString() == "N")
            {
                ddPropLastLineMode.SelectedIndex = 0;
            }
            else
            {
                ddPropLastLineMode.SelectedIndex = 1;
                txtPropLastLine.Text = drow["LASTLINETEXT"].ToString();
                txtPropLastLine.Visible = true;
            }

            //check if already saved
            string sql = "select status from cadre.tp_proposals where pno = " + PRONO;
            btnSave.Enabled = !(OraDBConnection.GetScalar(sql) == "S");

            FillBigNotes();
            if (drow["bignote"].ToString() != "")
            {
                ddBigNotes.SelectedIndex = ddBigNotes.Items.IndexOf(ddBigNotes.Items.FindByValue(drow["bignote"].ToString()));
            }
        }

        FillGrid();
        FillOutstandings();
    }
    protected void txtLocFilter_TextChanged(object sender, EventArgs e)
    {
        //if filter contains 6 numbers then make the row containing that empid as currently selected item 
        //if no such row exists, show message that no such empid is found
        //if filter is anything else then call FindAllLocations function
        string filter = txtLocFilter.Text;
        drpLocs.SelectedIndex = 0;
        if (filter.Length == 6 && new Regex("[1-9][0-9]{5}").IsMatch(filter))
        {
            foreach (ListItem litem in drpLocs.Items)
            {
                if (litem.Text.Contains(filter))
                {
                    drpLocs.ClearSelection();
                    litem.Selected = true;
                    return;
                }
            }
            Utils.ShowMessageBox(this, "No such Empid");
        }
        else if (filter != "Filter")
        {
            FillAllLocations(filter);
        }
    }
    protected void btnSelProposed_Click(object sender, EventArgs e)
    {
        string cur_row;
        string sql;
        string outempid;
        string empid = hidEmpID.Value;
        string cloc = "";
        string cdesg = "";
        string olddesgcode = hidWDesgCode.Value;
        string oldloccode = hidWLoccode.Value;
        //newEmpid is required when promoting JE to AE, 
        //user will search for JE using its old non-gazetted empid 
        //and then enter a new empid for AE in this field
        string newEmpid = txtNewEmpid.Text;
        string status = hidStatus.Value;
        string prop_row = drpLocs.SelectedValue;
        string remarks = txtRemarks.Text;
        string sno = string.Empty;
        int propno = PRONO;
        bool in_propcadrmap;
        string lastEvent = string.Empty;
        string prvComment = string.Empty;
        string displeft = txtDispLeft.Text;
        string dispright = txtDispRight.Text;

        //own intereset
        if (cbOwnInterest.Checked)
        {
            status = "O";
        }
        txtEmpid.Text = "";
        //lblLoc.Text = "";
        //lbldesg.Text = "";

        //panPresent.Enabled = true;
        divchoose.Disabled = false;

        if (!string.IsNullOrEmpty(txtPrvComment.Text))
        {
            prvComment = txtPrvComment.Text;
        }

        //newEmpID should be either blank 
        //or contain an EmpID starting with 10 in case actual empid is not starting with 10
        if (!empid.StartsWith("10"))
        {
            //empid is not starting with 10 so it is of JE, 
            //checking if newEmpID starts with 10
            //also checking for valid length (6 digits)
            if (!(newEmpid.StartsWith("10") && newEmpid.Length == 6))
            {
                Utils.ShowMessageBox(this, "Enter a valid New Empid");
                return;
            }
            //check if newempid not already allocated to any other officer
            sql = "select count(*) from pshr.empperso where empid = " + newEmpid;
            if (OraDBConnection.GetScalar(sql) != "0")
            {
                Utils.ShowMessageBox(this, "The New Empid specified is already in use");
                return;
            }
        }
        else
        {
            //if empid is already of gazetted officer, i.e. starts with 10
            //then New Empid is irrelevent
            newEmpid = "";
        }

        //check if proposed row is already assigned to someone in this proposal
        sql = string.Format("select count(*) from cadre.propcadrmap where proposed_rowno='{0}' and propno={1} and empid <> '{2}'",prop_row, PRONO, empid);
        if(OraDBConnection.GetScalar(sql) != "0")
        {
            Utils.ShowMessageBox(this, "The selected proposed row already exists in this proposal");
            return;
        }

        //if its not an event
        //then get cloccode and cdesgcode of proposed posting
        if(prop_row.StartsWith("EV_") == false)
        {
            cloc = ExtractLoccode(txtCLoc.Text);
            cdesg = ExtractDesgcode(txtCDesg.Text);
            if (cdesg == null)
            {
                return;
            }
        }
       
        //get last event from emphistory
        //will be handy in case of events like Suspension, Leave etc
        sql = string.Format("select eventcode from emphistory where empid = {0} "+
            "and rowno = (select max(rowno) from emphistory where empid = {0})", empid);
        lastEvent = OraDBConnection.GetScalar(sql);

        //get next serial number for current proposal
        sno = (txtSno.Text == "") ? 
            OraDBConnection.GetScalar("select nvl(max(sno),0)+1 from cadre.propcadrmap where propno = " + propno) 
            : txtSno.Text;

        //get current row num of empid
        //if empid is not mapped in cadrmap then put 0 in cur_row
        //max function is used so that atleast one row is returned if empid is not found in cadremap
        sql = "select nvl(max(rowno),0) from cadre.cadrmap where empid = " + empid;
        cur_row = OraDBConnection.GetScalar(sql);

        //check if empid is in propcadrmap
        //if empid not in propcadrmap then insert in propcadrmap
        //if empid is already in propcadrmap (outstanding case) then update propcadrmap
        sql = "select count(*) from cadre.propcadrmap where empid= " + empid + " and propno = " + propno;
        in_propcadrmap = OraDBConnection.GetScalar(sql) == "1";

        //Event Handling
        //if drplocs.selectedval i.e. prop_row contains an eventcode then
        //check if empid in propcadrmap for current proposal
        //if not then give error as event handling is only supported on outstanding cases
        //if yes then update propcadrmap with status=EV_<eventcode>, remarks=DateofEvent
        if (prop_row.StartsWith("EV_"))
        {
            if (!in_propcadrmap)
            {
                Utils.ShowMessageBox(this, "Can Only Apply Events to Employees in Outstanding");
                return;
            }

            //IMP: remarks contain date of event
            remarks = txtCLoc.Text;
            
            //IMP: in case of events the status is event number in the form of "EV_<eventcode>"
            status = prop_row;

            sql = string.Format(
                "update cadre.propcadrmap set rowno={0}, status='{1}', remarks='{2}',sno={3} where empid={4} and propno={5}",
                                cur_row, status, remarks, sno, empid, propno);

            if (OraDBConnection.ExecQry(sql) == false)
            {
                Utils.ShowMessageBox(this, "Error updating event data in propcadrmap");
                return;
            }
            FillGrid();
            return;
        }
        //If record not in propcadrmap i.e. Normal case handling
        if (!in_propcadrmap)
        {
            sql = string.Format("insert into cadre.propcadrmap(empid, rowno,status, proposed_rowno, cloccode, " +
                                "cdesgcode, remarks, sno, propno, newempid, last_event, olddesgcode, oldloccode, prvcomment, " +
                                "disp_left, disp_right) " +
                                " values({0}, {1}, '{2}', {3}, {4}," +
                                "'{5}', '{6}', {7}, {8}, '{9}', '{10}'," +
                                "{11}, {12}, '{13}', '{14}', '{15}')",
                                empid, cur_row, status, prop_row, cloc,
                                cdesg, remarks, sno, propno, newEmpid, lastEvent,
                                olddesgcode, oldloccode, prvComment, displeft, dispright);
            if (OraDBConnection.ExecQry(sql) == false)
            {
                Utils.ShowMessageBox(this, "Error inserting data in propcadrmap");
                return;
            }
        }
        //If record exists in propcadrmap then complete the info in propcadrmap
        //i.e. this record was already in outstanding
        else
        {
            sql = string.Format("update cadre.propcadrmap set status='{0}', proposed_rowno = {1}, cloccode={2}," +
                                "cdesgcode={3},remarks='{4}',sno={5}, newempid='{6}', last_event={7}, "+
                                "olddesgcode={8}, oldloccode={9}, prvcomment = '{10}', " +
                                "disp_left = '{11}', disp_right = '{12}' "+
                                "where empid={13} and propno={14}",
                                status, prop_row, cloc, cdesg, remarks, sno, newEmpid, lastEvent,
                                olddesgcode, oldloccode, prvComment, displeft, dispright, 
                                empid, propno);
            if (OraDBConnection.ExecQry(sql) == false)
            {
                Utils.ShowMessageBox(this, "Error updating data in propcadrmap");
                return;
            }
        }

        //outstanding handling
        //if proposed row is mapped in cadrmap then:
        //a. get outstanding's empid from cadrmap
        //b. insert outstanding's entry in propcadrmap
        sql = "select empid from cadre.cadrmap where rowno = " + prop_row;
        outempid = OraDBConnection.GetScalar(sql);

        if (!string.IsNullOrEmpty(outempid))
        {
            bool already_in_propcadrmap;

            //check if this outstanding entry is already entered into propcadrmap
            //(this could happen if empid which is going to be outstanding in this case have already been selected
            //and changed its posting in a previous case hence putting the record already in propcadrmap)
            sql = "select count(*) from cadre.propcadrmap where empid= '" + outempid + "' and propno = " + propno;
            already_in_propcadrmap = OraDBConnection.GetScalar(sql) == "1";
            if (!already_in_propcadrmap)
            {
                sql = string.Format("insert into cadre.propcadrmap(empid,rowno,propno,status) values({0},{1},{2},'{3}')",
                    outempid, prop_row, propno, status);
                if (OraDBConnection.ExecQry(sql) == false)
                {
                    Utils.ShowMessageBox(this, "Error inserting outstanding entry in propcadrmap");
                    return;
                }
            }

            //add outstanding entry as displacedid
            sql = string.Format("update cadre.propcadrmap set displacedid='{0}' where empid={1} and propno={2}", outempid, empid, propno);
            if (OraDBConnection.ExecQry(sql) == false)
            {
                Utils.ShowMessageBox(this, "Unable to add displaced id");
                return;
            }
        }
        FillGrid();
        ClearRightFields();
        //panProposed.Enabled = false;
        divpropose.Disabled = true;
        FillOutstandings();
        return;
    }
    protected void btnGenOO_Click(object sender, EventArgs e) 
    {
        if (CheckOONum())
        {
            MakeReport(true);
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    if (Session["trans_cclist"].ToString() != "1")
        //    {
        //        Utils.ShowMessageBox(this, "Create CC List First");
        //        return;
        //    }
        //}
        //catch 
        //{
        //    Utils.ShowMessageBox(this, "Error: Create CC List First");
        //    return;
        //}
        string sql = string.Format("update cadre.tp_proposals set bignote = '{0}' where pno = {1}", ddBigNotes.SelectedValue, PRONO);
        OraDBConnection.ExecQry(sql);
        MakeReport();
    }
    protected void drpLocs_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql;

        drpLocs.BackColor = System.Drawing.Color.White;
        if (drpLocs.SelectedIndex == 0)
        {
            drpLocs.BackColor = System.Drawing.Color.AliceBlue;
            txtCLoc.Text = "";
            txtCDesg.Text = "";
            return;
        }
        if(drpLocs.SelectedValue == "EV_RSUP ")
        {
            sql = "select to_char(pshr.get_retddate(" + drpOfficer.SelectedValue + "),'DD-MON-YYYY') from dual";
            txtCLoc.Text = OraDBConnection.GetScalar(sql); 
            ShowHideRightControls(false);
            return;
        }
        else if (drpLocs.SelectedValue.StartsWith("EV_"))
        {
            txtCLoc.Text = System.DateTime.Now.ToString("d-MMM-yyyy");
            ShowHideRightControls(false);
            return;
        }
       
        ShowHideRightControls(true);
        int rowno = int.Parse(drpLocs.SelectedValue);

        //handle special locations
        if (rowno.ToString().Length == 9)
        {
            string name = drpLocs.SelectedItem.Text;
            txtCLoc.Text = name.Substring(0, name.Length - 4) + "-" + drpLocs.SelectedValue;
        }
        else
        {
            //sql = "select pshr.get_org(loccode) as locname, loccode, desgcode,pshr.get_desg(desgcode) as desgname from cadre.cadr where rowno = " + rowno;
            sql = "select cm.empid, pshr.get_org(loccode) as locname, loccode, desgcode,pshr.get_desg(desgcode) as desgname "+
                "from cadre.cadr c left outer join cadre.cadrmap cm on c.rowno=cm.rowno where c.rowno = " + rowno;
            System.Data.DataSet ds = OraDBConnection.GetData(sql);
            txtCLoc.Text = ds.Tables[0].Rows[0]["locname"] + "-" + ds.Tables[0].Rows[0]["loccode"];
            if ( ds.Tables[0].Rows[0]["empid"] == DBNull.Value )
            {
                if (txtRemarks.Text == "")
                {
                    txtRemarks.Text = "i) Against a vacant post";
                }
                else
                {
                    txtRemarks.Text += Environment.NewLine + "ii) Against a vacant post";
                }
            }
        }

        if (hidStatus.Value == "T" || hidStatus.Value == "O")
        {
            txtCDesg.Text = lblInfoDesg.Text + "-" + hidWDesgCode.Value;
            //txtCDesg.Text = lbldesg.Text + "-" + lblvaldesg.Text;
        }
        else if (hidStatus.Value == "P")
        {
            DataSet ds = OraDBConnection.GetData("select desgcode,pshr.get_desg(desgcode) as desgtext from cadre.cadr where rowno = " + drpLocs.SelectedValue);
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtCDesg.Text = ds.Tables[0].Rows[0]["desgtext"] + "-" + ds.Tables[0].Rows[0]["desgcode"];
            }
            else
            {
                //for special cases, get nextdesg from cadre.nextdesg
                sql = "select nextdesg as dval, pshr.get_desg(nextdesg) as dtext from cadre.nextdesg where desg = " + hidolddesgcode.Value;
                DataSet ds_nextdesg = OraDBConnection.GetData(sql);
                if (ds_nextdesg.Tables[0].Rows.Count != 1)
                {
                    Utils.ShowMessageBox(this, "Error getting Next Higher Designation");
                    return;
                }
                string dval = ds_nextdesg.Tables[0].Rows[0]["dval"].ToString();
                string dtext = ds_nextdesg.Tables[0].Rows[0]["dtext"].ToString();
                txtCDesg.Text = dtext + " -" + dval;
                ds_nextdesg.Clear();
                ds_nextdesg.Dispose();
            }
        }
    }
    protected void btnCreateCC_Click(object sender, EventArgs e)
    {

        string sql;

        //sql = "Delete from cadre.cclist_person where sflag = 'D'";
        //OraDBConnection.ExecQry(sql);

        sql = "Insert into cadre.cclist_person (sno, loc, ccnum, sflag ) values( (select max(sno)+1 from cadre.cclist_person), (SELECT wm_concat(DISTINCT(' '  || REPLACE" +
            " (pshr.get_circle(cloccode),',',' '))) FROM (SELECT (SELECT loccode FROM cadre.cadr WHERE rowno = s.rowno ) AS cloccode FROM cadre.propcadrmap s WHERE proposed_rowno IS NOT NULL " +
            " UNION SELECT cloccode FROM cadre.propcadrmap s WHERE proposed_rowno IS NOT NULL) a) , -1,'D' )";
        OraDBConnection.ExecQry(sql);

        ClientScript.RegisterStartupScript(Page.GetType(),"ccpage", "<script type='text/javascript'>detailedresults=window.open('cc.aspx','_new');</script>");
        Session["trans_cclist"] = "1";
    }
    protected void btncreatenotes_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(Page.GetType(), "notespage", "<script type='text/javascript'>detailedresults=window.open('frmnotes.aspx','_new');</script>");
        Session["trans_notes"] = "1";
    }
    protected void btnPrintProposal_Click(object sender, EventArgs e)
    {
        string sql = string.Empty;
        if (ddPropLineMode.SelectedIndex == 1)
        {
            sql = string.Format("update cadre.tp_proposals set proplinemode = 'M', proplinetext = '{0}' where pno = '{1}'",
                txtPropLine.Text, PRONO);
            OraDBConnection.ExecQry(sql);
        }
        
        if (ddPropLastLineMode.SelectedIndex == 1)
        {
            sql = string.Format("update cadre.tp_proposals set LASTLINEMODE = 'M', LASTLINETEXT = '{0}' where pno = '{1}'",
                txtPropLastLine.Text, PRONO);
            OraDBConnection.ExecQry(sql);
        }
       
        Makeproreport();
    }
    protected void drpFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillAllLocations();
    }
    protected void gvProposals_RowEditing(object sender, GridViewEditEventArgs e)
    {
        string sql;
        DataRow drow;
        string sno;
        string empid;
        //cancel grid to enter in editing mode
        e.Cancel = true;

        sno = gvProposals.Rows[e.NewEditIndex].Cells[1].Text.ToString();
        empid = gvProposals.Rows[e.NewEditIndex].Cells[4].Text.ToString();
        if (sno == "&nbsp;")
        {
            //Utils.ShowMessageBox(this, "Select this record in outstanding");
            //panPresent.Enabled = true;
            //panProposed.Enabled = false;
            divchoose.Disabled = false;
            divpropose.Disabled = true;
            ClearInfo();
            ShowInfo(empid);
            return;
        }

        //disable left panel
        //panPresent.Enabled = false;
        //panProposed.Enabled = true;
        divchoose.Disabled = true;
        divpropose.Disabled = false;
        //get the row to be edited
        sql = "select pshr.get_fullname(pm.empid) as name, pm.empid, " +
            "olddesgcode, pshr.get_desg(olddesgcode) as olddesgtext, " +
            "oldloccode, pshr.get_org(oldloccode) as oldloctext, " +
            "rowno as oldrowno, cadre.get_mapping_text_from_rowno(rowno) as oldmaptext," +
            "proposed_rowno as newrowno, cadre.get_mapping_text_from_rowno(proposed_rowno) as newmaptext, " +
            "cloccode as newloccode, pshr.get_org(cloccode) as newloctext, " +
            "cdesgcode as newdesgcode, pshr.get_desg(cdesgcode) as newdesgtext," +
            "remarks, sno, status, newempid, prvcomment,disp_left, disp_right, i.photo2 as photo " +
            "from cadre.propcadrmap pm left outer join img_pshr.img i on i.empid = pm.empid "+
            "where pm.empid = " + empid + " and propno = " + PRONO;

        drow = OraDBConnection.GetData(sql).Tables[0].Rows[0];

        //fill fields
        hidStatus.Value = drow["status"].ToString();
        hidolddesgcode.Value = drow["olddesgcode"].ToString();
        hidsno.Value = drow["sno"].ToString();

        lblInfoName.Text = drow["name"].ToString();
        lblInfoDesg.Text = drow["olddesgtext"].ToString();
        lblInfoWLoc.Text = drow["oldloctext"].ToString();
        lblInfoPCLoc.Text = drow["oldmaptext"].ToString();
        hidEmpID.Value = drow["empid"].ToString();
        hidWDesgCode.Value = drow["olddesgcode"].ToString();
        hidWLoccode.Value = drow["oldloccode"].ToString();
        hidPCRowNo.Value = drow["oldrowno"].ToString();

        //load photo
        if (!Convert.IsDBNull(drow["photo"]))
        {
            byte[] barr = (byte[])drow["photo"];
            string base64str = Convert.ToBase64String(barr);
            imgEmpPhoto.ImageUrl = string.Format("data:image/gif;base64,{0}", base64str);
        }

        hidbranch.Value = OraDBConnection.GetScalar("select branchcode from pshr.empperso where empid = " + hidEmpID.Value);

        FillAllLocations();
        drpLocs.SelectedIndex = drpLocs.Items.IndexOf(drpLocs.Items.FindByValue(drow["newrowno"].ToString()));

        txtCLoc.Text = drow["newloctext"].ToString() + " -" + drow["newloccode"].ToString();
        txtCDesg.Text = drow["newdesgtext"].ToString() + " -" + drow["newdesgcode"].ToString();
        txtRemarks.Text = drow["remarks"].ToString();
        txtSno.Text = drow["sno"].ToString();
        txtPrvComment.Text = drow["prvcomment"].ToString();
        txtDispLeft.Text = drow["disp_left"].ToString();
        txtDispRight.Text = drow["disp_right"].ToString();
        lblInfo.Text = (drow["status"].ToString() == "P" ? "Promote " : "Transfer ") + drow["name"].ToString() + " to:";
    }
    protected void gvProposals_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string empid;
        string sql;

        //cancel grid to enter in editing mode
        e.Cancel = true;

        empid = gvProposals.Rows[e.RowIndex].Cells[4].Text.ToString();
        sql = string.Format("delete from cadre.propcadrmap where propno = {0} and empid = {1}", PRONO, empid);
        OraDBConnection.ExecQry(sql);
        FillGrid();
    }
    protected void btnSelNewID_Click(object sender, EventArgs e)
    {
        string empid = txtEmpid.Text;
        //panProposed.Enabled = false;
        divpropose.Disabled = true;
        ClearInfo();
        if (empid == "" || empid.Length != 6)
        {
            lblMsgNew.Text = "Invalid empid";
            return;
        }
        if (ShowInfo(empid) == false)
        {
            lblMsgNew.Text = "No such empid";
            return;
        }

        //if (CheckForUnderTransfer(empid, ref lblMsgNew) == false)
        //{
        //    ClearInfo(false);
        //    return;
        //}
    }
    protected void btnSelOutID_Click(object sender, EventArgs e)
    {
        if (drpOfficer.SelectedValue == "")
        {
            //lblMsgOut.Text = "No Outstanding";
            return;
        }
        string empid = drpOfficer.SelectedValue;
        ClearInfo();
        ShowInfo(empid);

        //if (CheckForUnderTransfer(empid, ref lblMsgOut) == false)
        //{
        //    ClearInfo(false);
        //    return;
        //}
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        string empid = hidEmpID.Value;

        if (string.IsNullOrEmpty(empid)) return;
        if (!empid.StartsWith("10"))
        {
            Utils.ShowMessageBox(this, "Can only promote this officer");
            return;
        }

        ClearRightFields();

        DataSet ds = OraDBConnection.GetData(
            "select cdesgcode,branchcode from pshr.empperso where empid = " +
            empid);

        hidsno.Value = "";
        hidStatus.Value = "T";
        hidolddesgcode.Value = ds.Tables[0].Rows[0]["cdesgcode"].ToString();
        hidbranch.Value = ds.Tables[0].Rows[0]["branchcode"].ToString();

        ds.Dispose();

        FillAllLocations();

        lblInfo.Text = string.Format("Transfer {0} to:", empid);
        //panProposed.Enabled = true;
        divpropose.Disabled = false;
        cbOwnInterest.Visible = true;
    }
    protected void btnPromote_Click(object sender, EventArgs e)
    {
        string empid = hidEmpID.Value;

        if (string.IsNullOrEmpty(empid)) return;

        ClearRightFields();

        DataSet ds = OraDBConnection.GetData(
            "select cdesgcode,branchcode from pshr.empperso where empid = " +
            empid);

        hidsno.Value = "";
        hidStatus.Value = "P";
        hidolddesgcode.Value = ds.Tables[0].Rows[0]["cdesgcode"].ToString();
        hidbranch.Value = ds.Tables[0].Rows[0]["branchcode"].ToString();

        ds.Dispose();

        FillAllLocations();

        lblInfo.Text = string.Format("Promote {0} to:", empid);
        txtRemarks.Text = "i) On Promotion";
        //panProposed.Enabled = true;
        divpropose.Disabled = false;
        cbOwnInterest.Visible = false;
    }
    protected void ddPropLineMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPropLine.Visible = ddPropLineMode.SelectedIndex == 1;
        if (ddPropLastLineMode.SelectedIndex == 0)
        {
            OraDBConnection.ExecQry(string.Format("update cadre.tp_proposals set proplinemode = 'A' where pno = '{0}'",PRONO));
        }
    }
    protected void ddPropLastLineMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPropLastLine.Visible = ddPropLastLineMode.SelectedIndex == 1;
        if (ddPropLastLineMode.SelectedIndex == 0)
        {
            OraDBConnection.ExecQry(string.Format("update cadre.tp_proposals set LASTLINEMODE = 'N' where pno = '{0}'",PRONO));
        }
    }
    protected void btnXLSProp_Click(object sender, EventArgs e)
    {
        string sql = "select pc.sno, pc.empid, pshr.get_fullname(pc.empid),status, " +
                        "pshr.get_desg(pc.olddesgcode) as pres_desg, " +
                        "pshr.get_org(pc.oldloccode) as pres_loc, " +
                        "cadre.get_mapping_text_from_rowno(rowno) as old_mapping, " +
                        "pshr.get_desg(pc.cdesgcode) as new_desg, " +
                        "pshr.get_org(pc.cloccode) as new_loc, " +
                        "cadre.get_mapping_text_from_rowno(proposed_rowno) as new_mapping, " +
                        "remarks, pc.PRVCOMMENT as priv_comm " +
                        "from cadre.propcadrmap pc " +
                        "where pc.PROPNO= " + PRONO + " " +
                        "order by pc.sno";
        Utils.DownloadXLS(sql, "prop_" + PRONO + ".xls", this);
    }
    protected void btnPDFProp_Click(object sender, EventArgs e)
    {
        string sql = string.Empty;
        if (ddPropLineMode.SelectedIndex == 1)
        {
            sql = string.Format("update cadre.tp_proposals set proplinemode = 'M', proplinetext = '{0}' where pno = '{1}'",
                txtPropLine.Text, PRONO);
            OraDBConnection.ExecQry(sql);
        }
        if (ddPropLastLineMode.SelectedIndex == 1)
        {
            sql = string.Format("update cadre.tp_proposals set LASTLINEMODE = 'M', LASTLINETEXT = '{0}' where pno = '{1}'",
                txtPropLastLine.Text, PRONO);
            OraDBConnection.ExecQry(sql);
        }
        Makeproreport_pc();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sql;

        //check if already saved
        sql = "select status from cadre.tp_proposals where pno = " + PRONO;
        if (OraDBConnection.GetScalar(sql) == "S")
        {
            Utils.ShowMessageBox(this, "Office order already saved");
            return;
        }

        if (CheckOONum() == false)
        {
            return;
        }

        if (Save())
        {
            sql = string.Format("update cadre.tp_proposals set status='S',oonum='{0}',oodate='{1}' where pno={2}",
            txtOoNum.Text, txtOoDate.Text, PRONO);

            if (!OraDBConnection.ExecQry(sql))
            {
                Utils.ShowMessageBox(this, "Error marking propsal as saved");
            }
            btnSave.Enabled = false;
            Utils.ShowMessageBox(this, "Order Saved and Implemented");
        }
        else
        {
            Utils.ShowMessageBox(this, "Error saving proposal");
        }
    }
    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetDesgs(string prefixText, int count, string contextKey)
    {
        prefixText = prefixText.Replace(' ', '%');

        if (Regex.IsMatch(prefixText, @"['!@#$^*~`]+") == true)
            return null;

        string sql = "select desgtext ||'-'|| desgcode from pshr.mast_desg where gazcode = 10 and " +
                        "upper(desgtext) like upper('%" + prefixText + "%')";

        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        List<string> list = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            list.Add(ds.Tables[0].Rows[i][0].ToString());
        return list.ToArray();
    }
    [WebMethod]
    public static string GetDesgs2()
    {
        StringBuilder sbLocs = new StringBuilder();
        string sql = "select desgtext ||'-'|| desgcode as desg from pshr.mast_desg where gazcode = 10 order by desgtext";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        foreach (System.Data.DataRow drow in ds.Tables[0].Rows)
        {
            sbLocs.AppendFormat("{0}:", drow["desg"]);
        }
        return sbLocs.ToString();
    }
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetLocs(string prefixText, int count, string contextKey)
    {
        prefixText = prefixText.Replace(' ', '%');

        if (Regex.IsMatch(prefixText, @"['!@#$^*~`]+") == true)
            return null;

        string sql = "select locname ||'-'|| loccode from pshr.mast_loc where " +
                        "upper(locname) like upper('%" + prefixText + "%')";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        List<string> list = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            list.Add(ds.Tables[0].Rows[i][0].ToString());
        return list.ToArray();
    }
    [WebMethod]
    public static string GetLocs2()
    {
        StringBuilder sbLocs = new StringBuilder();
        string sql = "select loccode,locname from pshr.mast_loc where aloc=1 order by locname";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        foreach (System.Data.DataRow drow in ds.Tables[0].Rows)
        {
            sbLocs.AppendFormat("{0}-{1}:", drow["locname"], drow["loccode"]);
        }
        return sbLocs.ToString();
    }
    [WebMethod]
    public static string GetNames2()
    {
        StringBuilder sbLocs = new StringBuilder();
        //string sql = "select pshr.get_fullname(empid) || '(' || empid || ')' as name from pshr.empperso where " +
        //                "recstatus = 10 and empid like '1%' order by pshr.get_fullname(empid),empid";
        string sql = "select pshr.get_fullname(empid) || '(' || empid || ')' as name from pshr.empperso where " +
                        "recstatus = 10 order by pshr.get_fullname(empid),empid";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        foreach (System.Data.DataRow drow in ds.Tables[0].Rows)
        {
            sbLocs.AppendFormat("{0}:", drow["name"]);
        }
        return sbLocs.ToString();
    }
    [WebMethod]
    public static string GiveCount()
    {
        return "123";
    }
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetNames(string prefixText, int count, string contextKey)
    {
        prefixText = prefixText.Replace(' ', '%');
        prefixText = prefixText.ToUpper();
        if (Regex.IsMatch(prefixText, @"['!@#$^*~`]+") == true)
            return null;
        string sql = "select pshr.get_fullname(empid) || '(' || empid || ')' from pshr.empperso where " +
                        "pshr.get_fullname(empid) like '%" + prefixText + "%' " +
                        "and recstatus = 10 " +
                        "and empid like '1%' " +
                        "order by pshr.get_fullname(empid),empid";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        List<string> list = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            list.Add(ds.Tables[0].Rows[i][0].ToString());
        return list.ToArray();
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string sql = "select sno,empid, pshr.get_fullname(empid) as name, " +
                        "pshr.get_desg(olddesgcode) || '-' || pshr.get_org(oldloccode) as oldloc, " +
                        "pshr.get_desg(CDESGCODE) || '-' || pshr.get_org(cloccode) as newloc," +
                        "decode(status,'P','Promotion','Transfer') as status," +
                        "remarks from cadre.propcadrmap where propno = " + PRONO + " order by sno";
        Utils.DownloadXLS(sql, "entries.xls", this);
    }
    protected void lnkImport_Click(object sender, EventArgs e)
    {
        int empid;
        int sno;
        string remarks;
        string sql;
        string filepath = Server.MapPath("office_orders\\" + FileUploader.FileName);

        if (FileUploader.HasFile)
            try
            {
                FileUploader.SaveAs(filepath);
            }
            catch (Exception ex)
            {
                Utils.ShowMessageBox(this,"Error: " + ex.Message);
            }
        else
        {
            return;
        }

        String connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filepath);

        OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
        DataSet ds = new DataSet();

        adapter.Fill(ds, "tab");

        DataTable data = ds.Tables["tab"];

        foreach (DataRow drow in data.Rows)
        {
            sno = Int32.Parse(drow["sno"].ToString());
            empid = Int32.Parse(drow["empid"].ToString());
            remarks = drow["remarks"].ToString();

            sql = String.Format("update cadre.propcadrmap set sno={0}, remarks = '{1}' where empid = '{2}' and propno = {3}", sno, remarks, empid, PRONO);
            OraDBConnection.ExecQry(sql);
        }

        FillGrid();
        Utils.ShowMessageBox(this, "Entries Uploaded");
    }
    #endregion
}
