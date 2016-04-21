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

public partial class frmproposal : System.Web.UI.Page
{
    int prono;

    private void FillNames()
    {
        if (drpSearchby.SelectedValue == "name")
        {
            drpOfficer.Items.Clear();
            string text = txtName.Text;
            string val;
            if (text.Length >= 6)
                val = text.Substring(text.LastIndexOf('(') + 1, 6);
            else
                return;
            drpOfficer.Items.Add(new ListItem(text, val));
        }
        else if (drpSearchby.SelectedValue == "empid")
        {
            drpOfficer.Items.Clear();
            string sql = "select pshr.get_fullname(" + txtEmpid.Text + ") from dual";
            string text = OraDBConnection.GetScalar(sql);
            string val = txtEmpid.Text;
            drpOfficer.Items.Add(new ListItem(text, val));
        }
    }
    private void FillSancDesgs()
    {
        string lcode;
        string sql;
        System.Data.DataSet ds;

        if (txtLoc.Text == "")
            return;
        lcode = ExtractLoccode(txtLoc.Text);
        if (lcode == null)
            return;
        sql = string.Format("select empid, firstname|| ' ' || middlename || ' ' || lastname as name" +
            " from pshr.empperso where empid in (select empid from cadre.cadrmap where rowno in " +
            "(select rowno from cadre.cadr where loccode ='{0}'))", lcode);
        ds = OraDBConnection.GetData(sql);
        drpOfficer.Items.Clear();
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            string text = String.Format("{0} ({1})", row["name"].ToString(), row["empid"].ToString());
            string val = row["empid"].ToString();
            drpOfficer.Items.Add(new ListItem(text, val));
        }
    }
    private void FillGrid()
    {
        string sql;
        DataSet ds;
      
        sql = "select a.* from (" +
              "select Sno,decode(status,'T','Transfer','P','Promotion') as Action," +
              "pshr.get_fullname(empid) as Name,EMPID, " +
              "pshr.get_org(cadre.get_preposting(empid)) as \"Present Location\", " +
              "decode(length(proposed_rowno), 9,pshr.get_post(proposed_rowno),cadre.loc_from_rowno(proposed_rowno) ) as \"Proposed PC Location\", " +
              "pshr.get_post(cloccode) as \"Proposed Location\"," +
              "pshr.get_desg(cdesgcode) as \"Proposed Designation\"," +
              "Remarks from cadre.propcadrmap where status = 'P' and propno =" + prono  +
              " union all " +
              "select Sno,decode(status,'T','Transfer','P','Promotion') as Action," +
              "pshr.get_fullname(empid) as Name,EMPID, " +
              "pshr.get_org(cadre.get_preposting(empid)) as \"Present Location\", " +
              "decode(length(proposed_rowno), 9,pshr.get_post(proposed_rowno),cadre.loc_from_rowno(proposed_rowno) ) as \"Proposed PC Location\", " +
              "pshr.get_post(cloccode) as \"Proposed Location\"," +
              "pshr.get_desg(cdesgcode) as \"Proposed Designation\"," +
              "Remarks from cadre.propcadrmap where status = 'T' and propno =" + prono  +
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
        string sql = string.Format("select empid, pshr.get_fullname(empid) as name, " +
            "pshr.get_post(cadre.get_posting(empid)) as loc from pshr.empperso em where " +
            "empid in (select empid from cadre.propcadrmap where status in ('T','P') and proposed_rowno is null and propno={0})",prono);
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        drpOfficer.Items.Clear();
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            string text = String.Format("{0} ({1}) at {2}", row["name"].ToString(),
                            row["empid"].ToString(), row["loc"].ToString());
            string val = row["empid"].ToString();
            drpOfficer.Items.Add(new ListItem(text, val));
        }
        FillLblLoc();
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
        string dcode = lblvaldesg.Text;
        string status = lblvalstat.Text;
        string branch = lblvalbranch.Text;
        //string val, text;

        if (dcode == null)
        {
            Utils.ShowMessageBox(this, "Please enter this record in cadrmap first");
            return false;
        }
        else if (dcode == "9047")
        {
            dcode = " in(9048)";
        }
        else if (dcode == "9365")
        {
            dcode = " in(9050)";
        }
        else if (dcode == "9366")
        {
            dcode = "in (9052)";
        }
        else if (dcode == "9057" || dcode == "9060" || dcode == "9056")
        {
            dcode = "in (9056,9057,9060) ";
        }
        else
        {
            dcode = " in( " + dcode + ")";
        }

        if (branch != "4")
        {
            branch = "1";
        }

        if (status == "T")
        {
            sql = string.Format("select pshr.get_desg(desgcode) as dname, indx, pshr.get_post(loccode) as lname," +
                                    "'E' as stat, rowno as val,pshr.get_post(loccode) as loc,loccode from cadre.cadr where " +
                                    "desgcode {0} and rowno not in (select rowno from cadre.cadrmap) " +
                                    "and branch = {1}", dcode, branch);
            sql += (filter != "") ? string.Format("and upper(pshr.get_post(loccode)) like upper('%{0}%')", filter) : "";
            sql += " union all ";
            sql += string.Format("select pshr.get_desg(desgcode) as dname, indx, pshr.get_post(loccode) as lname," +
                                    "'F' as stat, rowno as val,pshr.get_post(loccode) as loc,loccode from cadre.cadr where " +
                                    "desgcode {0} and rowno in (select rowno from cadre.cadrmap) " +
                                    "and branch = {1}", dcode, branch);
            sql += (filter != "") ? string.Format("and upper(pshr.get_post(loccode)) like upper('%{0}%')", filter) : "";
            sql += " union all ";
            sql += string.Format("select '0' as dname, 0 as indx, pshr.get_post(loccode) as lname," +
                                    "'S' as stat, loccode as val , pshr.get_post(loccode) as loc,loccode from pshr.mast_loc where " +
                                    "(loccode like '6%' or loccode like '7%' or loccode = '999999999') and loccode <> '601000000'", dcode);
            sql += (filter != "") ? string.Format("and upper(pshr.get_post(loccode)) like upper('%{0}%')", filter) : "";
            sql += " order by stat, loc,indx";
        }
        else if (status == "P")
        {
            sql = string.Format("select pshr.get_desg(desgcode)as dname, indx, pshr.get_post(loccode) as lname," +
                                    "'E' as stat, rowno as val,pshr.get_post(loccode) as loc,loccode from cadre.cadr where " +
                                    "desgcode in (select desgcode from pshr.mast_desg where cadrecode = " +
                                    "(select cadrecode from pshr.mast_desg where desgcode {0}) " +
                                    "and (hecode =(select hecode from pshr.mast_desg where desgcode {0})-1 " +
                                    "or  hecode =(select hecode from pshr.mast_desg where desgcode {0})-2) ) " +
                                    "and rowno not in (select rowno from cadre.cadrmap) " +
                                    "and branch = {1}", dcode, branch);
            sql += (filter != "") ? string.Format("and upper(pshr.get_post(loccode)) like upper('%{0}%')", filter) : "";
            sql += " union all ";
            sql += string.Format("select pshr.get_desg(desgcode) as dname, indx, pshr.get_post(loccode) as lname," +
                                    "'F' as stat, rowno as val, pshr.get_post(loccode) as loc,loccode from cadre.cadr where " +
                                    "desgcode in (select desgcode from pshr.mast_desg where cadrecode = " +
                                    "(select cadrecode from pshr.mast_desg where desgcode {0}) " +
                                    "and (hecode =(select hecode from pshr.mast_desg where desgcode {0})-1 " +
                                    "or  hecode =(select hecode from pshr.mast_desg where desgcode {0})-2) ) " +
                                    "and rowno in (select rowno from cadre.cadrmap) " +
                                    "and branch = {1}", dcode, branch);
            sql += (filter != "") ? string.Format("and upper(pshr.get_post(loccode)) like upper('%{0}%')", filter) : "";
            sql += " union all ";
            sql += string.Format("select  '0' as dname, 0 as indx, pshr.get_post(loccode) as lname," +
                                    "'S' as stat, loccode as val, pshr.get_post(loccode) as loc,loccode from pshr.mast_loc where " +
                                    "(loccode like '6%' or loccode like '7%' or loccode = '999999999') and loccode <> '601000000'", dcode);
            sql += (filter != "") ? string.Format("and upper(pshr.get_post(loccode)) like upper('%{0}%')", filter) : "";
            sql += " order by stat, loc,indx";
        }
        if (sql == "")
        {
            return false;
        }
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        drpLocs.Items.Clear();
        drpLocs.Items.Insert(0, new ListItem("--Select Pay Charge Location--", "-1"));
        //pshr.get_desg(desgcode) || '-' || indx || ' at ' || pshr.get_post(loccode) || ' (F)' as locset,
        string name101 = OraDBConnection.GetScalar("select locabb from pshr.mast_loc where loccode = 101000000");
        string name103 = OraDBConnection.GetScalar("select locabb from pshr.mast_loc where loccode = 103000000");
        string name104 = OraDBConnection.GetScalar("select locabb from pshr.mast_loc where loccode = 104000000");
        string name105 = OraDBConnection.GetScalar("select locabb from pshr.mast_loc where loccode = 105000000");
        string name106 = OraDBConnection.GetScalar("select locabb from pshr.mast_loc where loccode = 106000000");
        string name108 = OraDBConnection.GetScalar("select locabb from pshr.mast_loc where loccode = 108000000");
        int cnt101 = 1;
        int cnt103 = 1;
        int cnt104 = 1;
        int cnt105 = 1;
        int cnt106 = 1;
        int cnt108 = 1;
        ArrayList list101 = new ArrayList();
        ArrayList list103 = new ArrayList();
        ArrayList list104 = new ArrayList();
        ArrayList list105 = new ArrayList();
        ArrayList list106 = new ArrayList();
        ArrayList list108 = new ArrayList();
        ArrayList listOther = new ArrayList();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            string datatext, dataval, loccode;

            loccode = row["loccode"].ToString();


            if (loccode.Substring(0, 3) == "101")
            {
                datatext = row["dname"].ToString() + '-' + (cnt101++).ToString() + " at " + name101 + " (" + row["stat"] + ")";
                dataval = row["val"].ToString();
                list101.Add(new ListItem(datatext, dataval));
            }
            else if (loccode.Substring(0, 3) == "103")
            {
                datatext = row["dname"].ToString() + '-' + (cnt103++).ToString() + " at " + name103 + " (" + row["stat"] + ")";
                dataval = row["val"].ToString();
                list103.Add(new ListItem(datatext, dataval));
            }
            else if (loccode.Substring(0, 3) == "104")
            {
                datatext = row["dname"].ToString() + '-' + (cnt104++).ToString() + " at " + name104 + " (" + row["stat"] + ")";
                dataval = row["val"].ToString();
                list104.Add(new ListItem(datatext, dataval));
            }
            else if (loccode.Substring(0, 3) == "105")
            {
                datatext = row["dname"].ToString() + '-' + (cnt105++).ToString() + " at " + name105 + " (" + row["stat"] + ")";
                dataval = row["val"].ToString();
                list105.Add(new ListItem(datatext, dataval));
            }
            else if (loccode.Substring(0, 3) == "106")
            {
                datatext = row["dname"].ToString() + '-' + (cnt106++).ToString() + " at " + name106 + " (" + row["stat"] + ")";
                dataval = row["val"].ToString();
                list106.Add(new ListItem(datatext, dataval));
            }
            else if (loccode.Substring(0, 3) == "108")
            {
                datatext = row["dname"].ToString() + '-' + (cnt108++).ToString() + " at " + name108 + " (" + row["stat"] + ")";
                dataval = row["val"].ToString();
                list108.Add(new ListItem(datatext, dataval));
            }
            else
            {
                datatext = row["dname"].ToString() + '-' + row["indx"].ToString() + " at " + row["lname"] + " (" + row["stat"] + ")";
                dataval = row["val"].ToString();
                listOther.Add(new ListItem(datatext, dataval));
            }
        }

        foreach (ListItem litem in listOther)
        {
            drpLocs.Items.Add(litem);
            drpLocs.Items[drpLocs.Items.Count - 1].Attributes.Add("style", "background-color:aliceblue;");
        }
        foreach (ListItem litem in list101)
        {
            drpLocs.Items.Add(litem);
            drpLocs.Items[drpLocs.Items.Count - 1].Attributes.Add("style", "background-color:AntiqueWhite;");
        }
        foreach (ListItem litem in list103)
        {
            drpLocs.Items.Add(litem);
            drpLocs.Items[drpLocs.Items.Count - 1].Attributes.Add("style", "background-color:Aqua;");
        }
        foreach (ListItem litem in list104)
        {
            drpLocs.Items.Add(litem);
            drpLocs.Items[drpLocs.Items.Count - 1].Attributes.Add("style", "background-color:Bisque;");
        }
        foreach (ListItem litem in list105)
        {
            drpLocs.Items.Add(litem);
            drpLocs.Items[drpLocs.Items.Count - 1].Attributes.Add("style", "background-color:Azure;");
        }
        foreach (ListItem litem in list106)
        {
            drpLocs.Items.Add(litem);
            drpLocs.Items[drpLocs.Items.Count - 1].Attributes.Add("style", "background-color:Beige;");
        }
        foreach (ListItem litem in list108)
        {
            drpLocs.Items.Add(litem);
            drpLocs.Items[drpLocs.Items.Count - 1].Attributes.Add("style", "background-color:Aquamarine;");
        }
        //listOther.AddRange(list101);
        //listOther.AddRange(list103);
        //listOther.AddRange(list104);
        //listOther.AddRange(list105);
        //listOther.AddRange(list106);
        //listOther.AddRange(list108);
        //drpLocs.DataSource = listOther;
        //drpLocs.DataBind();
        //drpLocs.Items[1].Attributes.Add("style", "background-color:yellow;");
        drpLocs.Items.Add(new ListItem("Retirement-Superannuation","S"));
        drpLocs.Items[drpLocs.Items.Count - 1].Attributes.Add("style", "background-color:LightSlateGray;");
        drpLocs.Items.Add(new ListItem("Retirement-Voluntary Retirement", "V"));
        drpLocs.Items[drpLocs.Items.Count - 1].Attributes.Add("style", "background-color:LightSlateGray;");
        return true;
    }
    private void ClearRightFields()
    {
        txtLocFilter.Text = "";
        drpLocs.Items.Clear();
        txtCLoc.Text = "";
        txtCDesg.Text = "";
        txtRemarks.Text = "";
        lblvaldesg.Text = "";
        lblvalstat.Text = "";
        lblInfo.Text = "";
        lblvalsno.Text = "";
        lblvalbranch.Text = "";
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
        string sql = string.Format("select count(*) from cadre.propcadrmap where rowno={0} and status in ('P','T') " +
                                        "and proposed_rowno is not null", row);
        return OraDBConnection.GetScalar(sql) == "1";
    }
    private void FillLblLoc()
    {
        int empid;
        bool parse = int.TryParse(drpOfficer.SelectedValue, out empid);
        if (parse)
        {
            string loc = OraDBConnection.GetScalar("SELECT pshr.get_post(cloccode) FROM pshr.empperso WHERE empid=" + empid);
            string desg = OraDBConnection.GetScalar("SELECT pshr.get_desg(cdesgcode) FROM pshr.empperso WHERE empid=" + empid);
            lblLoc.Text = loc;
            lbldesg.Text = desg;
        }
    }
    private void FillCurrentDesg()
    {
        string sql;

        //sql = "select desgtext ||'-'|| desgcode from pshr.mast_desg where desgcode = " +
        //        "(select desgcode from cadre.cadr where rowno = " +
        //            "(select rowno from cadre.cadrmap where empid=" + drpOfficer.SelectedValue + "))";

        if (rbTnP.SelectedValue == "T") //Transfer
        {
            sql = "select pshr.get_desg(cdesgcode) from pshr.empperso where empid = " + drpOfficer.SelectedValue;
        }
        else //Promotion
        {
            sql = "select pshr.get_desg(cdesgcode) from pshr.empperso where empid = " + drpOfficer.SelectedValue;
        }
        txtCDesg.Text = OraDBConnection.GetScalar(sql);
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
    private string GetChief(string cloc)
    {
        string cloccode = cloc;
        switch (cloc.Substring(0, 3))
        {
            case "101":
                cloccode = "101000000";
                break;
            case "103":
                cloccode = "103000000";
                break;
            case "104":
                cloccode = "104000000";
                break;
            case "105":
                cloccode = "105000000";
                break;
            case "106":
                cloccode = "106000000";
                break;
            case "108":
                cloccode = "108000000";
                break;
        }
        return cloccode;
    }
    private void ShowHideRightControls(bool shide)
    {
        txtLocFilter.Visible = shide;
        txtCDesg.Visible = shide;
        txtRemarks.Visible = shide;
    }
    private bool Save()
    {
        string oonum = txtOoNum.Text;
        string oodate = txtOoDate.Text;
        string propno = this.prono.ToString();

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
    private void DownloadFile(String pdfPath)
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
    private void MakeReport(bool save = false)
    {
        string oonum = "-";
        string oodate = "-";
        string notes = "1";
        string propno = this.prono.ToString();

        if (save)
        {
            oonum = txtOoNum.Text;
            oodate = txtOoDate.Text;
        }
        string sql = "select m.sno,'" + oonum + "' as oonum1, '" + oodate + "' as oodate1, '" + notes + "' as notes," +
                    " pshr.get_fullname(e.empid),e.empid,e.dob," +
                    "pshr.get_post(e.cloccode) as old_work_loc,e.cloccode as old_work_loccode,pshr.get_desg(e.cdesgcode) as old_work_desg," +
                    "e.cdesgcode as old_work_desgcode," +
                    "DECODE(m.rowno,0,pshr.get_post(e.cloccode), pshr.get_post(cadre.get_lcode_rno(m.rowno))) AS old_pc_loc," +
                    " DECODE(m.rowno,0,e.cloccode, cadre.get_lcode_rno(m.rowno)) AS old_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.rowno))) AS old_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.rowno)) AS old_pc_desgcode, " +
                    " DECODE(m.rowno,0,'0', cadre.get_indx_rno(m.rowno)) AS old_pc_indx, " +
                    " pshr.get_post(m.cloccode) as new_work_loc," +
                    "m.cloccode as new_work_loccode,pshr.get_desg(m.cdesgcode) as new_work_desg,m.cdesgcode as new_work_desgcode," +
                    "decode(length(m.proposed_rowno),9,pshr.get_post(m.proposed_rowno), pshr.get_post(cadre.get_lcode_rno(m.proposed_rowno))) AS new_pc_loc," +
                    "decode(length(m.proposed_rowno),9,m.proposed_rowno, cadre.get_lcode_rno(m.proposed_rowno)) AS new_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno))) AS new_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.proposed_rowno))                               AS new_pc_desgcode, " +
                    "cadre.get_indx_rno(m.proposed_rowno) as new_pc_indx, m.remarks,'G' as grp,m.propno from pshr.empperso e, cadre.propcadrmap m where e.empid=m.empid and m.status is not null " +
                    " AND M.STATUS NOT IN ('S','V') and m.propno=" + propno + 
                    " order by sno";

        OraDBConnection oraCn = new OraDBConnection();
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

        System.Data.DataSet dsnotes;
        sql = "select * from cadre.notes_proposal_person where ccnum > 0 and proposalno = " + propno + " order by sno";
        dsnotes = OraDBConnection.GetData(sql);
        CrystalReportSource1.ReportDocument.Subreports["notes"].SetDataSource(dsnotes.Tables[0]);
        CrystalReportSource1.DataBind();

        System.Data.DataSet dscc;
        sql = "select * from cadre.cclist_proposal_person where ccnum > 0 and proposalno = " + propno + " order by sno";
        dscc = OraDBConnection.GetData(sql);
        CrystalReportSource1.ReportDocument.Subreports["cclists"].SetDataSource(dscc.Tables[0]);
        CrystalReportSource1.DataBind();
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);

        DownloadFile(pdfPath);
    }
    private void Makeproreport(bool save = false)
    {
        string oonum = "-";
        string oodate = "-";
        string notes = "1";
        string propno = this.prono.ToString();

        if (save)
        {
            oonum = txtOoNum.Text;
            oodate = txtOoDate.Text;
        }
        string sql = "select m.sno,'" + oonum + "' as oonum1, '" + oodate + "' as oodate1, '" + notes + "' as notes," +
                    " pshr.get_fullname(e.empid),e.empid,e.dob," +
                    "pshr.get_post(e.cloccode) as old_work_loc,e.cloccode as old_work_loccode,pshr.get_desg(e.cdesgcode) as old_work_desg," +
                    "e.cdesgcode as old_work_desgcode," +
                    "DECODE(m.rowno,0,pshr.get_post(e.cloccode), pshr.get_post(cadre.get_lcode_rno(m.rowno))) AS old_pc_loc," +
                    " DECODE(m.rowno,0,e.cloccode, cadre.get_lcode_rno(m.rowno)) AS old_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.rowno))) AS old_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.rowno)) AS old_pc_desgcode, " +
                    " DECODE(m.rowno,0,'0', cadre.get_indx_rno(m.rowno)) AS old_pc_indx, " +
                    " pshr.get_post(m.cloccode) as new_work_loc," +
                    "m.cloccode as new_work_loccode,pshr.get_desg(m.cdesgcode) as new_work_desg,m.cdesgcode as new_work_desgcode," +
                    "decode(length(m.proposed_rowno),9,pshr.get_post(m.proposed_rowno), pshr.get_post(cadre.get_lcode_rno(m.proposed_rowno))) AS new_pc_loc," +
                    "decode(length(m.proposed_rowno),9,m.proposed_rowno, cadre.get_lcode_rno(m.proposed_rowno)) AS new_pc_loccode," +
                    " DECODE(m.rowno,0,pshr.get_desg(e.cdesgcode), pshr.get_desg(cadre.get_dcode_rno(m.proposed_rowno))) AS new_pc_desg, " +
                    " DECODE(m.rowno,0,e.cdesgcode, cadre.get_dcode_rno(m.proposed_rowno))                               AS new_pc_desgcode, " +
                    "cadre.get_indx_rno(m.proposed_rowno) as new_pc_indx, m.remarks,'G' as grp,m.propno from pshr.empperso e, cadre.propcadrmap m where e.empid=m.empid and m.status is not null " +
                    " AND M.STATUS NOT IN ('S','V') and m.propno=" + propno +
                    " order by sno";

        OraDBConnection oraCn = new OraDBConnection();
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

        CrystalReportSource1.Report.FileName = Server.MapPath("Reports\\rptproposal.rpt");
        CrystalReportSource1.ReportDocument.SetDataSource(ds.Tables[0]);
        CrystalReportSource1.DataBind();

        System.Data.DataSet dsnotes;
        sql = "select * from cadre.notes_proposal_person where ccnum > 0 and proposalno = " + propno + " order by sno";
        dsnotes = OraDBConnection.GetData(sql);
        CrystalReportSource1.ReportDocument.Subreports["notes"].SetDataSource(dsnotes.Tables[0]);
        CrystalReportSource1.DataBind();

        
        CrystalReportSource1.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);

        DownloadFile(pdfPath);
    }
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

    protected void Page_Load(object sender, EventArgs e)
    {
        prono = int.Parse(Session["proposalno"].ToString());
        lblProposalName.Text = Session["proposalname"].ToString() + " (" + Session["proposaldate"].ToString() + ")";
        BypassLogin();
        if (!IsPostBack)
        {
            if ((Session["loginy"] == null) || (Session["loginy"].ToString() != "1"))
            {
                Response.Redirect("Login.aspx");
                return;
            }
            lblInfo.Text = "";

            //string sql = "update cadre.cadrmap set status=null, proposed_rowno=null,remarks=null";
            //if (OraDBConnection.ExecQry(sql) == false)
            //{
            //    Utils.ShowMessageBox(this, "Unable to update cadre.cadrmap");
            //}

            panProposed.Enabled = false;

            //set last cadre code
            Utils.last_cadre_cd = 0;

            //lblEmpid.Visible = true;
            //lblName.Visible = false;
            //lblPosting.Visible = false;
            txtEmpid.Visible = true;
            txtName.Visible = false;
            txtLoc.Visible = false;
        }

            FillGrid();
            
    }
    protected void txtLoc_TextChanged(object sender, EventArgs e)
    {
        panProposed.Enabled = false;
        if (txtLoc.Text == "")
            return;
        if (rbNewOut.SelectedValue == "O")
        {
            FillOutstandings();
        }
        else
        {
            FillSancDesgs();
        }
        FillLblLoc();
        ClearRightFields();
    }
    protected void rbTnP_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbTnP.SelectedValue == "T")
        {
            panMain.GroupingText = "Transfer";
            lblInfo.Text = "Transfer to:";
        }
        else if (rbTnP.SelectedValue == "P")
        {
            panMain.GroupingText = "Promotion";
            lblInfo.Text = "Promote to:";
        }
        ClearRightFields();
    }
    protected void btnSelPresent_Click(object sender, EventArgs e)
    {
        string dcode;
        if (drpOfficer.SelectedValue == null)
        {
            return;
        }
        txtCLoc_TextBoxWatermarkExtender.WatermarkText = "hello";
        ClearRightFields();
        string sql = string.Format("select pshr.get_fullname({0}) as name from dual",
                        drpOfficer.SelectedValue);
        string name = OraDBConnection.GetScalar(sql);

        dcode = OraDBConnection.GetScalar("select cdesgcode from pshr.empperso where empid = " + drpOfficer.SelectedValue);

        //fill labels
        lblvalsno.Text = "";
        lblvalstat.Text = rbTnP.SelectedValue;
        lblvaldesg.Text = dcode;
        lblvalbranch.Text = OraDBConnection.GetScalar("select branchcode from pshr.empperso where empid = " + drpOfficer.SelectedValue);

        if (FillAllLocations() == false) return;

        if (rbTnP.SelectedValue == "T")
        {
            lblInfo.Text = string.Format("Transfer {0} to:", name);
        }
        else
        {
            lblInfo.Text = string.Format("Promote {0} to:", name);
        }
        panProposed.Enabled = true;
        //FillCurrentDesg();
    }
    protected void txtLocFilter_TextChanged(object sender, EventArgs e)
    {
        FillAllLocations(txtLocFilter.Text);
    }
    protected void btnSelProposed_Click(object sender, EventArgs e)
    {

        string cur_row;
        bool in_propcadrmap;
        string sql;
        string sno;
        string outempid;
        string empid = drpOfficer.SelectedValue;
        string cloc = "";
        string cdesg = "";
        string status = lblvalstat.Text;
        string prop_row = drpLocs.SelectedValue;
        string remarks = txtRemarks.Text;
        int propno = prono;
        panPresent.Enabled = true;
        if (!(drpLocs.SelectedValue == "S") && !(drpLocs.SelectedValue == "V"))
        {
            cloc = ExtractLoccode(txtCLoc.Text);
            cdesg = ExtractDesgcode(txtCDesg.Text);
        }

        //get next serial number for current proposal
        if (lblvalsno.Text == "")
        {
            sql = "select nvl(max(sno),0)+1 from cadre.propcadrmap where propno = " + propno;
            sno = OraDBConnection.GetScalar(sql);
        }
        else
        {
            sno = lblvalsno.Text;
        }

        //get current row num of empid
        //if empid is not mapped in cadrmap then put 0 in cur_row
        sql = "select rowno from cadre.cadrmap where empid = " + empid;
        cur_row = OraDBConnection.GetScalar(sql);
        cur_row = (string.IsNullOrEmpty(cur_row)) ? "0" : cur_row;

        //check if empid is in propcadrmap
        //if empid not in propcadrmap then insert in propcadrmap
        //if empid is in propcadrmap (outstanding case) then update propcadrmap
        sql = "select count(*) from cadre.propcadrmap where empid= " + empid + " and propno = " + propno;
        in_propcadrmap = OraDBConnection.GetScalar(sql) == "1";

        //Retirement Handling
        //if drplocs.selectedval == V or S then
        //Check if empid in propcadrmap in current proposal
        //if not then give error
        //if yes then update propcadrmap status=V/S (drpLocs.selectedvalue), remarks=DateofRetirement
        if (prop_row == "S" || prop_row == "V")
        {
            remarks = txtCLoc.Text;
            if (!in_propcadrmap)
            {
                Utils.ShowMessageBox(this, "Can Only Set to Retire Employees in Outstanding");
                return;
            }
            status = drpLocs.SelectedValue;
            sql = string.Format("update cadre.propcadrmap set rowno={0}, status='{1}', remarks='{2}',sno={3} where empid={4} and propno={5}",
                                cur_row, status, remarks, sno, empid, propno);
            if (OraDBConnection.ExecQry(sql) == false)
            {
                Utils.ShowMessageBox(this, "Error updating retirement data in propcadrmap");
                return;
            }
            FillGrid();
            return;
        }

        if (!in_propcadrmap)
        {
            sql = string.Format("insert into cadre.propcadrmap values({0},{1},'{2}',{3},{4},{5},'{6}',{7},{8})",
                                empid, cur_row, status, prop_row, cloc, cdesg, remarks, sno, propno);
            if (OraDBConnection.ExecQry(sql) == false)
            {
                Utils.ShowMessageBox(this, "Error inserting data in propcadrmap");
                return;
            }
        }
        else
        {
            sql = string.Format("update cadre.propcadrmap set status='{0}', proposed_rowno = {1}, cloccode={2}," +
                                "cdesgcode={3},remarks='{4}',sno={5} where empid={6} and propno={7}",
                                status, prop_row, cloc, cdesg, remarks, sno, empid, propno);
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
            sql = "select count(*) from cadre.propcadrmap where empid= '" + outempid + "' and propno = " + propno;
            already_in_propcadrmap = OraDBConnection.GetScalar(sql) == "1";
            if (!already_in_propcadrmap)
            {
                sql = string.Format("insert into cadre.propcadrmap(empid,rowno,propno,status) values({0},{1},{2},'{3}')", outempid, prop_row, propno, status);
                if (OraDBConnection.ExecQry(sql) == false)
                {
                    Utils.ShowMessageBox(this, "Error inserting outstanding entry in propcadrmap");
                    return;
                }
            }
        }
        FillGrid();
        ClearRightFields();
        panProposed.Enabled = false;
        return;
    }
    protected void rbNewOut_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtLoc.Text = "";
        drpOfficer.Items.Clear();
        if (rbNewOut.SelectedValue == "O")
        {
            Label8.Visible = false;
            drpSearchby.Visible = false;
            //lblEmpid.Visible = false;
            txtEmpid.Visible = false;
            //lblName.Visible = false;
            txtName.Visible = false;
            //lblPosting.Visible = false;
            txtLoc.Visible = false;
            lblLoc.Text = "";
            lbldesg.Text = "";
            FillOutstandings();
        }
        else
        {
            drpSearchby.SelectedIndex = 0;
            drpOfficer.Items.Clear();
            Label8.Visible = true;
            drpSearchby.Visible = true;
            //lblPosting.Visible = false;
            txtLoc.Visible = false;
            txtName.Visible = false;
            //lblName.Visible = false;
            //lblEmpid.Visible = true;
            txtEmpid.Text = "";
            txtName.Text = "";
            txtLoc.Text = "";
            txtEmpid.Visible = true;
            FillSancDesgs();
        }
        ClearRightFields();
    }
    protected void btnSave_Click(object sender, EventArgs e) 
    {
        if (txtOoNum.Text.Length < 1)
        {
            Utils.ShowMessageBox(this, "Enter O/o Number");
            return;
        }
        DateTime checkdate;
        if (DateTime.TryParse(txtOoDate.Text, out checkdate) == false)
        {
            Utils.ShowMessageBox(this, "Enter a valid date");
            return;
        }
        if (Save() == false)
        {
            Utils.ShowMessageBox(this, "Error Saving Proposal");
            return;
        }
        //mark proposal as saved
        string sql = string.Format("update cadre.proposals set status='S',oonum='{0}',oodate='{1}' where pno={2}", 
            txtOoNum.Text, txtOoDate.Text, prono);
        if (OraDBConnection.ExecQry(sql) == false)
        {
            Utils.ShowMessageBox(this, "Error marking proposal as saved");
            return;
        }
        MakeReport(true);
    }
    protected void drpOfficer_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLblLoc();
    }
    protected void drpSearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        //lblPosting.Visible = false;
        //lblEmpid.Visible = false;
        //lblName.Visible = false;
        txtLoc.Visible = false;
        txtEmpid.Visible = false;
        txtName.Visible = false;
        if (drpSearchby.SelectedValue == "posting")
        {
            //lblPosting.Visible = true;
            txtLoc.Visible = true;
        }
        else if (drpSearchby.SelectedValue == "name")
        {
            //lblName.Visible = true;
            txtName.Visible = true;
        }
        else if (drpSearchby.SelectedValue == "empid")
        {
            //lblEmpid.Visible = true;
            txtEmpid.Visible = true;
        }
    }
    protected void txtName_TextChanged(object sender, EventArgs e)
    {
        panProposed.Enabled = false;
        if (txtName.Text == "")
            return;
        if (rbNewOut.SelectedValue == "O")
        {
            FillOutstandings();
        }
        else
        {
            FillNames();
        }
        FillLblLoc();
        ClearRightFields();
    }
    protected void txtEmpid_TextChanged(object sender, EventArgs e)
    {
        panProposed.Enabled = false;
        if (txtEmpid.Text == "" || txtEmpid.Text.Length != 6)
            return;
        if (rbNewOut.SelectedValue == "O")
        {
            FillOutstandings();
        }
        else
        {
            FillNames();
        }
        FillLblLoc();
        ClearRightFields();
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
        if (drpLocs.SelectedValue == "V")
        {
            txtCLoc.Text = string.Empty;
            txtCLoc_TextBoxWatermarkExtender.WatermarkText = "Date of Voluntary Retirement (DD-MM-YYYY)";
            ShowHideRightControls(false);
            return;
        }
        else if(drpLocs.SelectedValue == "S")
        {
            sql = "select to_char(pshr.get_retddate(" + drpOfficer.SelectedValue + "),'DD-MM-YYYY') from dual";
            txtCLoc.Text = OraDBConnection.GetScalar(sql); 
            ShowHideRightControls(false);
            return;
        }
        ShowHideRightControls(true);
        int rowno = int.Parse(drpLocs.SelectedValue);
        if (rowno.ToString().Length == 9)
        {
            string name = drpLocs.SelectedItem.Text;
            txtCLoc.Text = name.Substring(0, name.Length - 4) + "-" + drpLocs.SelectedValue;
        }
        else
        {
            sql = "select pshr.get_org(loccode) as locname, loccode, desgcode,pshr.get_desg(desgcode) as desgname from cadre.cadr where rowno = " + rowno;
            System.Data.DataSet ds = OraDBConnection.GetData(sql);
            txtCLoc.Text = ds.Tables[0].Rows[0]["locname"] + "-" + ds.Tables[0].Rows[0]["loccode"];
        }

        //txtCDesg.Text = ds.Tables[0].Rows[0]["desgname"] + "-" + ds.Tables[0].Rows[0]["desgcode"];
        if (lblvalstat.Text == "T")
        {
            txtCDesg.Text = lbldesg.Text + "-" + lblvaldesg.Text;
        }
        else
        {
            DataSet ds = OraDBConnection.GetData("select desgcode,pshr.get_desg(desgcode) as desgtext from cadre.cadr where rowno = " + drpLocs.SelectedValue);
            txtCDesg.Text = ds.Tables[0].Rows[0]["desgtext"] + "-" + ds.Tables[0].Rows[0]["desgcode"];
        }
    }
    protected void gvProposals_RowEditing(object sender, GridViewEditEventArgs e)
    {
        string sno;
        string sql;
        DataSet ds;
        string rowno, loc, desg, remarks, loctext, desgtext, status, name, empid, cposting, newdesg;

        //cancel grid to enter in editing mode
        e.Cancel = true;

        sno = gvProposals.Rows[e.NewEditIndex].Cells[1].Text.ToString();
        if (sno == "&nbsp;")
        {
            Utils.ShowMessageBox(this, "Select this record in outstanding");
            return;
        }

        //disable left panel
        panPresent.Enabled = false;
        panProposed.Enabled = true;

        //get the row to be edited
         sql = "SELECT a.*," +
            "pshr.get_org(a.cloccode)                                                                                      AS loct, " +
            "pshr.get_desg(a.cdesgcode)                                                                                    AS desgt, " +
            "pshr.get_fullname(a.empid)                                                                                    AS name, " +
            "DECODE(a.rowno,0,pshr.get_org(cadre.get_cloccode_empperso(a.empid)),pshr.get_org(cadre.get_posting(a.empid))) AS cposting, " +
            "DECODE(a.rowno,0,cadre.get_cdesgcode_empperso(a.empid),cadre.get_desg_from_id(a.empid))                       AS OldDesg " +
            "FROM (select * from cadre.propcadrmap where sno=" + sno + " and propno = " + prono + ") a";

        //fill the information in the right panel above
        ds = OraDBConnection.GetData(sql);

        loc = ds.Tables[0].Rows[0]["cloccode"].ToString();
        loctext = ds.Tables[0].Rows[0]["loct"].ToString();
        desg = ds.Tables[0].Rows[0]["cdesgcode"].ToString();
        desgtext = ds.Tables[0].Rows[0]["desgt"].ToString();
        remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
        rowno = ds.Tables[0].Rows[0]["proposed_rowno"].ToString();
        status = ds.Tables[0].Rows[0]["status"].ToString();
        name = ds.Tables[0].Rows[0]["name"].ToString();
        empid = ds.Tables[0].Rows[0]["empid"].ToString();
        cposting = ds.Tables[0].Rows[0]["cposting"].ToString();
        newdesg = ds.Tables[0].Rows[0]["OldDesg"].ToString();
        
        //fill labels
        lblvalstat.Text = status;
        lblvaldesg.Text = newdesg;
        lblvalsno.Text = sno;

        //fill officer name in left panel above
        drpOfficer.Items.Clear();
        drpOfficer.Items.Add(new ListItem(name, empid));

        lblvalbranch.Text = OraDBConnection.GetScalar("select branchcode from pshr.empperso where empid = " + drpOfficer.SelectedValue);


        lblLoc.Text = cposting;
        txtEmpid.Text = empid;
        FillAllLocations();
        drpLocs.SelectedIndex = drpLocs.Items.IndexOf(drpLocs.Items.FindByValue(rowno.ToString()));

        txtCLoc.Text = loctext + " -" + loc;
        txtCDesg.Text = desgtext + " -" + desg;
        txtRemarks.Text = remarks;
        lblInfo.Text = (status == "T" ? "Transfer " : "Promote ") + name + " to:";
    }
    protected void btnCreateCC_Click(object sender, EventArgs e)
    {

        string sql;

        sql = "Delete from cadre.cclist_person where sflag = 'D'";
        OraDBConnection.ExecQry(sql);

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
        Makeproreport();
    }
}