using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Services;
using System.Collections;

public partial class DataEntry : System.Web.UI.Page
{
    private const string TEXT_BTN_ADD = "Add";
    private const string TEXT_BTN_EDIT = "Edit";
    private const string TEXT_BTN_DELETE = "Delete";

    //private enum MEvents
    //{
    //    CTRP = 36,
    //    CPRO = 28,
        
    //    //Retirement
    //    RMIN = 11,
    //    RSUP = 12,
    //    RVOR = 13,
    //    RPRB = 14,
    //    REGN = 15,
    //    REXP = 16,
    //    RMIS = 89,

    //    //Leave
    //    LELS = 2,
    //    LMTL =3,
    //    LJON = 10,
    //    LJONRC = 72,
    //    LJONTR = 73,
    //    LJONPR = 74,
    //    LJONSP = 75,
    //    LJONRT = 76,
    //    LJONEX = 77,
    //    JELWS = 87,
    //};

    private void FillSancDesgs(string lcode)
    {
        //string sql = "select pshr.get_desg(desgcode)||'-'||indx||'-'||substr(cadre.get_branch(branch),1,2)||'-'||ptype "
        //                + "as desg,desgcode,indx from cadre.cadr where loccode ='" + lcode + "' order by scaleid desc";
        //removed md.gazcode=10
        string sql = @"SELECT a.desg  || nvl2(b.empid,'('  ||b.empid  ||')',' ') AS desg,a.desgcode AS desgcode
                          FROM
                            (SELECT pshr.get_desg(cd.desgcode) ||'-'||cd.indx||'-'||SUBSTR(cadre.get_branch(cd.branch),1,2)||'-'||cd.ptype AS desg,
                              cd.desgcode||'-'||cd.indx as desgcode,
                              cd.indx as indx,
                              cd.rowno rowno,
                              md.hecode as hc
                            FROM cadre.cadr cd ,PSHR.mast_desg md
                            WHERE cd.loccode ='" + lcode + @"'
                            and (hia is null or hia = 0) and cd.desgcode=md.desgcode
                            ORDER BY hc,indx
                          ) a
                        LEFT OUTER JOIN cadre.cadrmap b ON a.rowno = b.rowno";
        using (System.Data.DataSet ds = OraDBConnection.GetData(sql))
        {
            drpSancDesg.Items.Clear();
            drpSancDesg.DataSource = ds.Tables[0];
            drpSancDesg.DataTextField = "desg";
            drpSancDesg.DataValueField = "desgcode";
            drpSancDesg.DataBind();
        }
    }
    private void FillEventList()
    {
        string sql = "SELECT eref || ' (' || event || ')' AS eventname, "+
            "eventcode FROM pshr.mast_event order by eventname";
        using(System.Data.DataSet ds = OraDBConnection.GetData(sql))
        {
            drpEvent.Items.Clear();
            drpEvent.DataSource = ds.Tables[0];
            drpEvent.DataTextField = "eventname";
            drpEvent.DataValueField = "eventcode";
            drpEvent.DataBind();
        }
        LockSelectiveEventData();   
    }
    private void ClearEventData()
    {
        txtDoR.Text = "";
        txtDoJ.Text = "";
        txtdesg.Text = "";
        txtploc.Text = "";
        txtpcloc.Text = "";
        drpSancDesg.Items.Clear();
        txtOoNum.Text = "";
        txtOoDate.Text = "";
        lblmsg.Text = "";
    }
    private void LockEventData(bool lockit)
    {
        //reverse boolean, so if user calls LockEventData(true) it really locks controls
        //and vice versa
        lockit = !lockit;

        //lock controls
        drpEvent.Enabled = lockit;
        txtDoJ.Enabled = lockit;
        txtDoR.Enabled = lockit;
        txtdesg.Enabled = lockit;
        txtploc.Enabled = lockit;
        txtpcloc.Enabled = lockit;
        drpSancDesg.Enabled = lockit;
        txtOoDate.Enabled = lockit;
        txtOoNum.Enabled = lockit;
    }
    private void LockSelectiveEventData()
    {
        if (btnFinal.Text == TEXT_BTN_DELETE)
            return;
        string sql = "SELECT rdate,jdate,desg,loc FROM pshr.mast_event WHERE " +
                           "eventcode=" + drpEvent.SelectedValue;
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            System.Data.DataRow row = ds.Tables[0].Rows[0];

            txtDoR.Enabled = (row["rdate"].ToString() == "1");
            txtDoR.BackColor = (row["rdate"].ToString() == "1") ? Color.White : Color.LightGray;

            txtDoJ.Enabled = (row["jdate"].ToString() == "1");
            txtDoJ.BackColor = (row["jdate"].ToString() == "1") ? Color.White : Color.LightGray;

            txtdesg.Enabled = (row["desg"].ToString() == "1");
            txtdesg.BackColor = (row["desg"].ToString() == "1") ? Color.White : Color.LightGray;

            drpSancDesg.Enabled = txtpcloc.Enabled = txtploc.Enabled = (row["loc"].ToString() == "1");
            drpSancDesg.BackColor = txtpcloc.BackColor = txtploc.BackColor = 
                (row["loc"].ToString() == "1") ? Color.White : Color.LightGray;

            txtOoDate.Enabled = txtOoNum.Enabled = true;
            txtOoDate.BackColor = txtOoNum.BackColor = Color.White;
        }
    }
    private void FillEventData()
    {
        long empid = Convert.ToInt64(txtEmpID.Text.Trim());
        int rowno = Convert.ToInt32(GridView1.SelectedRow.Cells[1].Text.Trim());
        string sql = String.Format("SELECT eventcode,to_char(fromdate,'DD-MON-YYYY') as fromdate,to_char(todate,'DD-MON-YYYY') as todate," +
                                    "get_desg(desgcode) ||'-'|| desgcode AS desg,get_post(loccode) ||'-'|| loccode AS ploc," +
                                    "get_post(pcloccode) ||'-'|| pcloccode AS pcloc,pcloccode, pshr.get_desg(sancdesg) AS sancdesgt," +
                                    "sancdesg,sancindx,oonum,to_char(odate,'DD-MON-YYYY') as odate " +
                                    "FROM emphistory WHERE empid={0} AND rowno={1}", empid, rowno);
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        if (ds.Tables[0].Rows.Count != 1)
        {
            lblmsg.Text = "Error: More than one row found";
            return;
        }
        System.Data.DataRow row = ds.Tables[0].Rows[0];
        //fill eventcode
        drpEvent.SelectedIndex = drpEvent.Items.IndexOf(drpEvent.Items.FindByValue(row["eventcode"].ToString()));

        //fill date of relieving
        txtDoR.Text = row["fromdate"].ToString();

        //fill date of joining
        txtDoJ.Text = row["todate"].ToString();

        //fill desgcode
        txtdesg.Text = (row["desg"].ToString() == "-") ? "" : row["desg"].ToString();

        //fill posting location
        txtploc.Text = (row["ploc"].ToString() == "-") ? "" : row["ploc"].ToString();

        //fill pay charge location
        txtpcloc.Text = (row["pcloc"].ToString() == "-") ? "" : row["pcloc"].ToString();

        //fill hidden field hidsancpost with sanc desg and indx
        hidSancPost.Value = row["sancdesg"].ToString() + "-" + row["sancindx"].ToString();
        
        //fill sanctioned designation
        FillSancDesgs(row["pcloccode"].ToString());
        drpSancDesg.SelectedIndex = drpSancDesg.Items.IndexOf(drpSancDesg.Items.FindByValue(hidSancPost.Value));

        //fill O/o Num
        txtOoNum.Text = row["oonum"].ToString();

        //fill O/o Date
        txtOoDate.Text = row["odate"].ToString();
    }
    private void ClearBasicInfo()
    {
        lblName.Text = "";
        lblDesg.Text = "";
        lblCLoc.Text = "";
        lblPCLoc.Text = "";
        lblFName.Text = "";
        lblmsg.Text = "";
    }
    private void FillBasicInfo(string empid)
    {
        string sql = string.Format("select pshr.get_fullname({0}) as name,pshr.get_desg(cdesgcode) as desg,pshr.get_post(cloccode) as loc," +
                        "fathername from pshr.empperso where empid='{0}'", empid);
        System.Data.DataSet ds = new System.Data.DataSet();
        ds = OraDBConnection.GetData(sql);

        if (ds.Tables[0].Rows.Count != 1)
            return;

        lblName.Text = ds.Tables[0].Rows[0]["name"].ToString();
        lblDesg.Text = ds.Tables[0].Rows[0]["desg"].ToString();
        lblCLoc.Text = ds.Tables[0].Rows[0]["loc"].ToString();
        lblFName.Text = ds.Tables[0].Rows[0]["fathername"].ToString();

        //get pay charge location from cadrmap
        //sql = string.Format("SELECT get_post(loccode) FROM cadre.cadr WHERE rowno IN "+
        //                    "(SELECT rowno FROM cadre.cadrmap WHERE empid={0})", empid);
        sql = string.Format("select pshr.get_post(pcloccode) from emphistory where empid = {0}" +
            " and rowno = (select max(rowno) from pshr.emphistory where empid={0})", empid);
        lblPCLoc.Text = OraDBConnection.GetScalar(sql);
    }
    private void FillEventGrid(string empid)
    {
        string sql = string.Format("SELECT rowno as \"Row #\",get_event(eventcode) as \"Event\", " +
                                "to_char(fromdate,'DD-MM-YYYY') \"From Date\", " +
                                "to_char(todate,'DD-MM-YYYY') \"To Date\", get_desg(desgcode) as \"Desg\", " +
                                "get_post(loccode) as \"Posting Loc\", pshr.get_post(pcloccode) as \"Pay Charge Loc\", " +
                                "pshr.get_desg(sancdesg)||'-'||sancindx as \"Sanctioned Post\","+
                                "(select cm.empid from cadre.cadr c,cadre.cadrmap cm where c.loccode=pcloccode and "+
                                "c.desgcode=sancdesg and c.indx = sancindx and c.rowno=cm.rowno) as \"Mapped Empid\","+
                                "oonum as \"O/o Num\"," +
                                "to_char(odate,'dd-Mon-yyyy') as \"O/o Date\" from pshr.emphistory " +
                                "where empid='{0}' order by rowno", empid);
        OraDBConnection.FillGrid(ref GridView1, sql);
    }
    private bool isDepu()
    {
        //cpro=28, crdg=38, cdfs=45
        if (isValidLoc(txtpcloc.Text) == false || isValidLoc(txtploc.Text) == false)
            return false;
        string pcode = ((txtploc.Text.Split('-')).Length >= 2) ? (txtploc.Text.Split('-')[txtploc.Text.Split('-').Length - 1]) : null;
        bool depuloc = (!pcode.StartsWith("601") && (pcode[0] == '7' || pcode[0] == '6'));
        bool depuevent = (drpEvent.SelectedValue == "28" || drpEvent.SelectedValue == "38" || drpEvent.SelectedValue == "45");
        bool ondepu = (depuevent && depuloc);
        return ondepu;
    }
    private string DoAdd()
    {
        bool ondepu = isDepu();

        //fill desgcode if it is filled
        string dcode = ((txtdesg.Text.Split('-')).Length >= 2) ? (txtdesg.Text.Split('-')[txtdesg.Text.Split('-').Length - 1]) : null;

        //fill posting location if it is filled
        string pcode = ((txtploc.Text.Split('-')).Length >= 2) ? (txtploc.Text.Split('-')[txtploc.Text.Split('-').Length - 1]) : null;

        //fill pay charge location if it is filled
        string pccode = "";
        if (!ondepu)
        {
            pccode = ((txtpcloc.Text.Split('-')).Length >= 2) ? (txtpcloc.Text.Split('-')[txtpcloc.Text.Split('-').Length - 1]) : null;
        }

        long empid = Convert.ToInt64(txtEmpID.Text.Trim());
        //string sancpost = drpSancDesg.SelectedValue;
        string sancpost = hidSancPost.Value;
        string sql;

        hidSancPost.Value = "";

        //get next rowno
        string rno="-1";
        sql = "select nvl(max(rowno),0)+1 AS rowno from pshr.emphistory where empid = " + empid;
        rno = OraDBConnection.GetScalar(sql);

        string sancdesg = "";
        string sancindx = "";
        if (!ondepu)
        {
            //sancpost (if filled) consists of "desgcode-indx",
            sancdesg = (sancpost.Split('-').Length == 2) ? sancpost.Split('-')[0] : "0";
            sancindx = (sancpost.Split('-').Length == 2) ? sancpost.Split('-')[1] : "0";

            if (pccode == null || pccode.Length != 9)
            {
                //lblmsg.Text = "Select a pay charge location";
                //return "-1";
                pccode = "88888";
            }
        }

        ////if eventgroup is 'Retd' then vacant cadrmap
        //VacantCadrMap(empid, int.Parse(drpEvent.SelectedValue));

        //insert event into emphistory
        sql = String.Format("insert into pshr.emphistory (empid, eventcode, fromdate, todate, desgcode, loccode, rowno, " +
                                "pcloccode, eventhistoryid, sancdesg, sancindx, oonum,odate,status) values ({0},  {1}," +
                                "'{2}','{3}','{4}','{5}','{6}','{7}',(select max(eventhistoryid)+1 from pshr.emphistory)," +
                                "'{8}','{9}','{10}','{11}',1)",
                                empid, drpEvent.SelectedValue, txtDoR.Text, txtDoJ.Text, dcode, pcode, rno, pccode,
                                sancdesg, sancindx, txtOoNum.Text, txtOoDate.Text);

        bool retval = OraDBConnection.ExecQry(sql);

        lblmsg.Text = retval ? "Event added successfully" : "Unable to add event";
        return rno;
    }
    private string DoEdit()
    {
        bool ondepu = isDepu();
        
        long empid = Convert.ToInt64(txtEmpID.Text.Trim());
        int rno = Convert.ToInt32(GridView1.SelectedRow.Cells[1].Text.Trim());

        //fill desgcode if it is filled
        string dcode = ((txtdesg.Text.Split('-')).Length >= 2) ? (txtdesg.Text.Split('-')[txtdesg.Text.Split('-').Length - 1]) : null;

        //fill posting location if it is filled
        string pcode = ((txtploc.Text.Split('-')).Length >= 2) ? (txtploc.Text.Split('-')[txtploc.Text.Split('-').Length - 1]) : null;

        //fill pay charge location if it is filled
        string pccode = "";
        if (!ondepu)
        {
            pccode = ((txtpcloc.Text.Split('-')).Length >= 2) ? (txtpcloc.Text.Split('-')[txtpcloc.Text.Split('-').Length - 1]) : null;
        }

        //string sancpost = drpSancDesg.SelectedValue;
        string sancpost = hidSancPost.Value;
        string sql;

        //sancpost (if filled) consists of "desgcode-indx",
        string sancdesg = "";
        string sancindx = "";
        if (!ondepu)
        {
            sancdesg = (sancpost.Split('-').Length == 2) ? sancpost.Split('-')[0] : "";
            sancindx = (sancpost.Split('-').Length == 2) ? sancpost.Split('-')[1] : "";
        }

        //update event in emphistory
        sql = String.Format("UPDATE pshr.emphistory SET eventcode='{0}', fromdate='{1}', todate='{2}',  desgcode='{3}', loccode='{4}', pcloccode='{5}'," +
                                "sancdesg='{6}', sancindx='{7}', oonum='{8}',   odate='{9}' WHERE empid={10} AND rowno={11}", drpEvent.SelectedValue,
                                txtDoR.Text, txtDoJ.Text, dcode, pcode, pccode, sancdesg, sancindx, txtOoNum.Text, txtOoDate.Text, empid, rno);

        bool retval = OraDBConnection.ExecQry(sql);
        lblmsg.Text = retval ? "Event Edited Successfully" : "Unable to edit event";
        return rno.ToString();
    }
    private bool ResetRownos(long empid)
    {
        string sql;
        bool res;
        sql = @"BEGIN
              DELETE FROM cadre.temp_setrowno WHERE empid = " + empid +@";
              INSERT INTO cadre.temp_setrowno
              SELECT empid,
                rowno,
                rownum
              FROM
                (SELECT rowno,empid FROM emphistory WHERE empid = " +empid+ @" ORDER BY rowno
                );
              UPDATE emphistory eh
              SET eh.rowno =
                (SELECT tsr.newrowno
                FROM CADRE.temp_setrowno tsr
                WHERE tsr.empid  = eh.empid
                AND tsr.oldrowno = eh.rowno
                )
              WHERE eh.empid="+empid+ @";
            END;";
        res = OraDBConnection.ExecQry(sql);
        return true;
    }
    private string DoDelete()
    {
        long empid = Convert.ToInt64(txtEmpID.Text.Trim());
        int rno = Convert.ToInt32(GridView1.SelectedRow.Cells[1].Text.Trim());

        string sql = string.Format("DELETE FROM emphistory WHERE empid={0} AND rowno={1}", empid, rno);
        bool retval = OraDBConnection.ExecQry(sql);

        ResetRownos(empid);
        lblmsg.Text = retval ? "Event Deleted Successfully" : "Unable to delete event";
        return rno.ToString();
    }
    private void VacantCadrMap(long empid, int eventcode)
    {
        if (eventcode >= 11 && eventcode <= 16)
        {
            string sql = "delete from cadre.cadrmap where empid=" + empid.ToString();
            OraDBConnection.ExecQry(sql);
        }
    }
    private void GetLastPostingInfo(string empid, out string loccode, out string desgcode, 
        out string pcloccode, out string sancdesg, out string sancindx, out int eventid)
    {
        string val = string.Empty;
        string sql = String.Format("SELECT loccode, desgcode, pcloccode, sancdesg, sancindx,eventcode FROM "+
            "emphistory WHERE empid={0} ORDER BY rowno DESC", empid);
        
        loccode = desgcode = pcloccode = sancdesg = sancindx = string.Empty;
        eventid = 0;
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        //get current loccode
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            val = (!string.IsNullOrEmpty(row["loccode"].ToString())) ? row["loccode"].ToString() : string.Empty;
            if (val != string.Empty && val != "88888" && val != "99999")
            {
                loccode = val;
                break;
            }
        }

        //get current desgcode
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            val = (!string.IsNullOrEmpty(row["desgcode"].ToString())) ? row["desgcode"].ToString() : string.Empty;
            if (val != string.Empty && val != "8888" && val != "9999")
            {
                desgcode = val;
                break;
            }
        }

        //get combination of pay charge loccode/sancdesg/sancindx
        string valpcloc, valsancdesg, valsancindx;
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            valpcloc = (!string.IsNullOrEmpty(row["pcloccode"].ToString())) ? row["pcloccode"].ToString() : string.Empty;
            valsancdesg = (!string.IsNullOrEmpty(row["sancdesg"].ToString())) ? row["sancdesg"].ToString() : string.Empty;
            valsancindx = (!string.IsNullOrEmpty(row["sancindx"].ToString())) ? row["sancindx"].ToString() : string.Empty;
            if (valpcloc != string.Empty && valpcloc != "88888" && valpcloc != "99999" && valpcloc != "77777" &&
                valsancdesg != string.Empty && valsancdesg != "8888" && valsancdesg != "9999" &&
                valsancindx != string.Empty && valsancindx != "88888" && valsancindx != "99999")
            {
                pcloccode = valpcloc;
                sancdesg = valsancdesg;
                sancindx = valsancindx;
                break;
            }
        }

        //get last eventid
        string evntid="";
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            val = (!string.IsNullOrEmpty(row["eventcode"].ToString())) ? row["eventcode"].ToString() : string.Empty;
            if (val != string.Empty && val != "99")
            {
                evntid = val;
                break;
            }
        }
        eventid = int.Parse(evntid);
    }
    private void UpdateCadrMap(string empid)
    {
        string sql = String.Format("SELECT loccode, desgcode, pcloccode, sancdesg, sancindx FROM emphistory WHERE empid={0} ORDER BY rowno DESC", empid);
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        //if for current event pcloc is 0 in mast_event then delete entry from cadrmap
        //and exit

        string pclocval = OraDBConnection.GetScalar(string.Format("SELECT pcloc FROM pshr.mast_event WHERE eventcode={0}", drpEvent.SelectedValue));
        if (pclocval == "0")
        {
            OraDBConnection.ExecQry("delete from cadre.cadrmap where empid=" + empid);
            return;
        }

        //get pay charge loccode
        long val = 0;
        long pcloccode = 0;
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            val = (row["pcloccode"] != DBNull.Value) ? Convert.ToInt64(row["pcloccode"]) : 0;
            if (val != 0 && val != 88888 && val != 99999)
            {
                pcloccode = val;
                break;
            }
        }

        //get sanctioned designation code
        val = 0;
        long sancdesgcode = 0;
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            val = (row["sancdesg"] != DBNull.Value) ? Convert.ToInt64(row["sancdesg"]) : 0;
            if (val != 0 && val != 88888 && val != 99999)
            {
                sancdesgcode = val;
                break;
            }
        }

        //get sanctioned designation indx
        val = 0;
        long sancindx = 0;
        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
        {
            val = (row["sancindx"] != DBNull.Value) ? Convert.ToInt64(row["sancindx"]) : 0;
            if (val != 0 && val != 88888 && val != 99999)
            {
                sancindx = val;
                break;
            }
        }

        //if pcloccode,sancdesg and sancindx are all zero then -
        //- delete empid from cadrmap (if it exists) and return
        if (pcloccode == 0 && sancdesgcode == 0 && sancindx == 0)
        {
            sql = String.Format("DELETE from cadre.cadrmap where empid={0}", empid);
            OraDBConnection.ExecQry(sql);
            return;
        }

        //get rowno from cadr given pcloccode,sancdesg and sancindx
        sql = String.Format("SELECT rowno FROM cadre.cadr WHERE loccode={0} AND desgcode={1} AND indx={2}", pcloccode, sancdesgcode, sancindx);
        string rowno = OraDBConnection.GetScalar(sql);

        if (rowno == "" || rowno == null)
        {
            //Removed error to support empty fields
            ///Utils.ShowMessageBox(this,"Error updating CadrMap (invalid rowno)");
            return;
        }

        long lrowno = Convert.ToInt64(rowno);
        long lempid = Convert.ToInt64(empid);

        //check if rowno already exists, if yes then delete that row
        sql = "SELECT count(*) FROM cadre.cadrmap WHERE rowno=" + lrowno;
        if (OraDBConnection.GetScalar(sql) != "0")
        {
            //delete this rowno's entry from cadrmap
            OraDBConnection.ExecQry("delete from cadre.cadrmap where rowno = " + lrowno);
        }

        sql = "SELECT count(*) FROM cadre.cadrmap WHERE empid=" + lempid;
        string empidcount = OraDBConnection.GetScalar(sql);

        if (empidcount == "0")    //if empid doesn't exist, insert in cadrmap
        {
            sql = string.Format("INSERT INTO cadre.cadrmap(empid,rowno) VALUES({0},{1})", lempid, lrowno);
            if (OraDBConnection.ExecQry(sql) == false)
                Utils.ShowMessageBox(this, "Unable to insert into CadrMap");
        }
        else if (empidcount == "1")  //if empid exists only once then update it.
        {
            sql = string.Format("UPDATE cadre.cadrmap SET rowno={0} WHERE empid={1}", lrowno, lempid);
            if (OraDBConnection.ExecQry(sql) == false)
                Utils.ShowMessageBox(this, "Unable to update CadrMap");
        }
        else
        {
            Utils.ShowMessageBox(this, "Duplicate empids in cadrmap");
        }
    }
    private void GoBack()
    {
        panEventData.Visible = false;
        panEventGrid.Visible = true;
        LockEventData(false);
        FillEventGrid(txtEmpID.Text);
        txtEmpID.Enabled = true;
        hidSancPost.Value = "";
    }
    private bool DoValidations()
    {
        //if deletion is to be done then no need to validate
        if (btnFinal.Text == TEXT_BTN_DELETE)
            return true;

        //validations stopped for data entry
        return true;

        //bool ondepu = isDepu();

        //string sql = string.Format("SELECT rdate,jdate,desg,loc,pcloc,onum FROM pshr.mast_event WHERE eventcode={0}", drpEvent.SelectedValue);
        //System.Data.DataSet ds = OraDBConnection.GetData(sql);
        //if (ds != null && ds.Tables[0].Rows.Count == 1)
        //{
        //    System.Data.DataRow row = ds.Tables[0].Rows[0];

        //    if (row["rdate"].ToString() == "1")
        //    {
        //        if (isValidDate(txtDoR.Text) == false)
        //        {
        //            Utils.ShowMessageBox(this,"Please fill a valid Date of Relieving");
        //            return false;
        //        }
        //    }

        //    if (row["jdate"].ToString() == "1")
        //    {
        //        if (isValidDate(txtDoJ.Text) == false)
        //        {
        //            Utils.ShowMessageBox(this, "Please fill a valid Date of Joining");
        //            return false;
        //        }
        //    }

        //    if (row["desg"].ToString() == "1")
        //    {
        //        if (isValidDesg(txtdesg.Text) == false)
        //        {
        //            Utils.ShowMessageBox(this, "Please fill a valid designation");
        //            return false;
        //        }
        //    }

        //    if (row["loc"].ToString() == "1")
        //    {
        //        if (isValidLoc(txtploc.Text) == false)
        //        {
        //            Utils.ShowMessageBox(this, "Please fill a valid posting location");
        //            return false;
        //        }
        //    }

        //    if (row["pcloc"].ToString() == "1")
        //    {
        //        if (!ondepu && isValidLoc(txtpcloc.Text) == false)
        //        {
        //            Utils.ShowMessageBox(this, "Please fill a valid pay charge location");
        //            return false;
        //        }
        //    }

        //    if (row["pcloc"].ToString() == "1")
        //    {
        //        if (!ondepu && isValidDesg(drpSancDesg.SelectedValue) == false)
        //        {
        //            Utils.ShowMessageBox(this, "Please fill a valid sanctioned designation");
        //            return false;
        //        }
        //    }

        //    if (row["onum"].ToString() == "1")
        //    {
        //        if (isValidDesg(txtOoNum.Text) == false)
        //        {
        //            Utils.ShowMessageBox(this, "Please fill a valid office order number");
        //            return false;
        //        }
        //    }

        //    if (row["onum"].ToString() == "1")
        //    {
        //        if (isValidDate(txtOoDate.Text) == false)
        //        {
        //            Utils.ShowMessageBox(this, "Please fill a valid office order date");
        //            return false;
        //        }
        //    }
        //}
        //return true;
    }
    private bool isValidDesg(string desg)
    {
        if (desg == "" || Regex.IsMatch(desg, @"['!@#$%^*~`]+") == true)
            return false;

        return true;
    }
    private bool isValidLoc(string loc)
    {
        if (loc == "" || Regex.IsMatch(loc, @"['!@#$%^*~`]+") == true)
            return false;

        return true;
    }
    private bool isValidOONum(string oonum)
    {
        if (oonum == "" || Regex.IsMatch(oonum, @"['!@#$%^*~`]+") == true)
            return false;

        return true;
    }
    private bool isValidDate(string date)
    {
        DateTime temp;
        return (DateTime.TryParse(date, out temp));
    }
    private void DoSancDesg()
    {
        //get loccode from entry of type "locname-loccode"
        //^[\w\s,-]+$
        if (Regex.IsMatch(txtpcloc.Text, @"['!@#$%^*~`]+") == true)
        {
            Utils.ShowMessageBox(this, "Invalid Location");
            return;
        }
        string lcode = txtpcloc.Text.Split('-')[txtpcloc.Text.Split('-').Length - 1];
        if (lcode.Length != 9)
            return;
        FillSancDesgs(lcode);
    }
    private void DeleteFromCadrMap_ID(string empid)
    {
        string sql = "DELETE FROM cadre.cadrmap WHERE empid=" + empid;
        OraDBConnection.ExecQry(sql);
    }
    private void DeleteFromCadrMap_ROWNO(string rowno)
    {
        string sql = "DELETE FROM cadre.cadrmap WHERE rowno=" + rowno;
        OraDBConnection.ExecQry(sql);
    }
    private string GetRowNo(string pcloccode, string sancdesg, string sancindx)
    {
        //get the rowno of other person
        string sql = string.Format("select rowno from cadre.cadr where loccode='{0}' and desgcode='{1}' and indx='{2}'",
                        pcloccode, sancdesg, sancindx);
        return OraDBConnection.GetScalar(sql);
    }
    private void InsertIntoCadrMap(string empid, string rowno)
    {
        string sql;
        sql = string.Format("INSERT INTO cadre.cadrmap(empid,rowno) VALUES('{0}','{1}')", empid, rowno);
        OraDBConnection.ExecQry(sql);
    }
    private void UpdateEmpperso(string empid, int eventid, string cloccode, string cdesgcode)
    {
        string sql;
        string recstatus = "10";
        if (eventid == (int)EVENTS.RETD.RSUP)
        {
            recstatus = "20";
        }
        sql = string.Format("UPDATE EMPPERSO SET cloccode = '{0}', cdesgcode = '{1}', recstatus = '{2}' WHERE empid = '{3}'", cloccode, cdesgcode, recstatus, empid);
        OraDBConnection.ExecQry(sql);
    }
    private bool UpdatePosting(string rno, string action)
    {
        string empid = txtEmpID.Text;
        string sql;
        string dor, doj;
        int eventid;
        string pcloccode, sancdesg, sancindx;
        string loccode, desgcode;
        
        System.Data.DataRow drow;

        GetLastPostingInfo(empid, out loccode, out desgcode, out pcloccode, out sancdesg, out sancindx, out eventid);

        if (action == TEXT_BTN_DELETE)
        {
            string cadr_rowno;
            sql = string.Format("select eventcode from pshr.emphistory where rowno = (select max(rowno) from emphistory where empid  = '{0}') and empid = '{0}'",empid);
            eventid = int.Parse(OraDBConnection.GetScalar(sql));

            DeleteFromCadrMap_ID(empid);
            cadr_rowno = GetRowNo(pcloccode, sancdesg, sancindx);
            if (!string.IsNullOrEmpty(cadr_rowno))
            {
                DeleteFromCadrMap_ROWNO(cadr_rowno);
                InsertIntoCadrMap(empid, cadr_rowno);
            }
            //return true;
        }
        else if (action == TEXT_BTN_ADD || action == TEXT_BTN_EDIT)
        {
            drow = OraDBConnection.GetData("select * from pshr.emphistory where empid=" + empid + " and rowno=" + rno).Tables[0].Rows[0];
            dor = drow["fromdate"].ToString();
            doj = drow["todate"].ToString();
            eventid = int.Parse(drow["eventcode"].ToString());

            //if both DOR and DOJ are empty
            if (dor == string.Empty && doj == string.Empty)
            {
                //if both DOR and DOJ are not filled then we don't need to change posting
                //exit in this case
                return true;
            }
            //if DOR is filled and DOJ is empty
            else if (dor != string.Empty && doj == string.Empty)
            {
                DeleteFromCadrMap_ID(empid);

                //if leave events and LDI is not empty
                //then delete Secondary empid from cadrmap 
                //and insert new primary empid in cadrmap
                if (eventid == (int)EVENTS.LEAVE.LELS ||
                    eventid == (int)EVENTS.LEAVE.LMTL)
                {
                    if (!string.IsNullOrEmpty(pcloccode) &&
                        !string.IsNullOrEmpty(sancdesg) &&
                        !string.IsNullOrEmpty(sancindx))
                    {
                        string rowno = GetRowNo(pcloccode, sancdesg, sancindx);
                        if (!string.IsNullOrEmpty(rowno))
                        {
                            //delete secondary empid using rowno
                            DeleteFromCadrMap_ROWNO(rowno);
                            //insert primary empid,rowno combination
                            //InsertIntoCadrMap(empid, rowno);
                        }
                    }
                }

            }
            else if (doj != string.Empty)
            {
                //delete empid from cadrmap
                DeleteFromCadrMap_ID(empid);

                //empty rowno, delete old person
                //enter in cadrmap(P,R)
                if (!string.IsNullOrEmpty(pcloccode) &&
                    !string.IsNullOrEmpty(sancdesg) &&
                    !string.IsNullOrEmpty(sancindx))
                {
                    string rowno = GetRowNo(pcloccode, sancdesg, sancindx);
                    if (!string.IsNullOrEmpty(rowno))
                    {
                        //delete secondary empid using rowno
                        DeleteFromCadrMap_ROWNO(rowno);
                        //insert primary empid,rowno combination
                        InsertIntoCadrMap(empid, rowno);
                    }
                }
            }
        }
        UpdateEmpperso(empid, eventid, loccode, desgcode);
        return true;
    }
    //private bool UpdatePosting2(string rno,string action)
    //{
    //    string empid = txtEmpID.Text;
    //    string sql;
    //    string dor, doj;
    //    int eventid;
    //    string pcloccode, sancdesg, sancindx;
    //    string loccode, desgcode;
    //    System.Data.DataRow drow;

    //    GetLastPostingInfo(empid, out loccode, out desgcode, out pcloccode, out sancdesg, out sancindx, out eventid);

    //    if (action == TEXT_BTN_DELETE)
    //    {
    //        //releiving so delete entry from cadrmap
    //        DeleteFromCadrMap_ID(empid);
    //    }
    //    drow = OraDBConnection.GetData("select * from pshr.emphistory where empid=" + empid + " and rowno=" + rno).Tables[0].Rows[0];
    //    dor = drow["fromdate"].ToString();
    //    doj = drow["todate"].ToString();
    //    eventid = drow["eventcode"].ToString();

    //    //if both DOR and DOJ are empty
    //    if (dor == string.Empty && doj == string.Empty)
    //    {
    //        //if both DOR and DOJ are not filled then we don't need to change posting
    //        //exit in this case
    //        return true;
    //    }
    //    //if DOR is filled and DOJ is empty
    //    else if (dor != string.Empty && doj == string.Empty)
    //    {
    //        //releiving so delete entry from cadrmap
    //        DeleteFromCadrMap_ID(empid);

    //        //if leave events and LDI set is not empty
    //        //then delete Secondary empid from cadrmap 
    //        //and insert new primary empid in cadrmap
    //        if (eventid == (int)EVENTS.LEAVE.LELS ||
    //            eventid == (int)EVENTS.LEAVE.LMTL)
    //        {
    //            if (!string.IsNullOrEmpty(pcloccode) &&
    //                !string.IsNullOrEmpty(sancdesg)  &&
    //                !string.IsNullOrEmpty(sancindx))
    //            {
    //                string rowno = GetRowNo(pcloccode, sancdesg, sancindx);
    //                if (!string.IsNullOrEmpty(rowno))
    //                {
    //                    //delete secondary empid using rowno
    //                    DeleteFromCadrMap_ROWNO(rowno);
    //                    //insert primary empid,rowno combination
    //                    InsertIntoCadrMap(empid, rowno);
    //                }
    //            }
    //        }
    //        //Retirement
    //        if (eventid == EVENTS.RETD.RSUP.ToString() || eventid == EVENTS.RETD.RVOR.ToString() ||
    //            eventid == EVENTS.RETD.RMIN.ToString() || eventid == EVENTS.RETD.RMIS.ToString() ||
    //            eventid == EVENTS.RETD.RPRB.ToString() || eventid == EVENTS.RETD.REGN.ToString() ||
    //            eventid == EVENTS.RETD.REXP.ToString())
    //        {
    //            //update empperso set recstatus=20
    //            sql = "update empperso set recstatus=20 where empid = " + empid;
    //            OraDBConnection.ExecQry(sql);
    //        }
    //        //if not retirement event then update empperso with current loccode and current desgcode
    //        else
    //        {
    //            sql = string.Format("update empperso set cloccode='{0}',cdesgcode='{1}' where empid = {2}", loccode, desgcode, empid);
    //            OraDBConnection.ExecQry(sql);
    //        }
    //    }
    //    else if (doj != string.Empty)
    //    {
    //    }
    //    return true;
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if ((Session["loginy"] == null) || (Session["loginy"].ToString() != "1"))
            {
                Response.Redirect("Login.aspx");
                return;
            }
            FillEventList();
            panEventGrid.Visible = false;
            panEventData.Visible = false;
        }
    }
    protected void txtEmpID_TextChanged(object sender, EventArgs e)
    {
        string sql;
        string empid;

        if (Session["loginy"] == null || 
            Session["loginy"].ToString().Length == 0 ||
            Session["loginy"].ToString() != "1")
        {
            Response.Redirect("Login.aspx");
            return;
        }

        empid = txtEmpID.Text;

        //clear past info
        ClearBasicInfo();
        ClearEventData();
        panEventGrid.Visible = false;
        panEventData.Visible = false;

        //if empid doesn't exist then return
        sql = string.Format("SELECT count(*) FROM empperso WHERE empid='{0}'", empid);
        if (OraDBConnection.GetScalar(sql) != "1")
        {
            return;
        }
        FillBasicInfo(empid);
        FillEventGrid(empid);
        panEventGrid.Visible = true;
        panEventData.Visible = false;
    }
    protected void txtpcloc_TextChanged(object sender, EventArgs e)
    {
        DoSancDesg();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        panEventGrid.Visible = false;
        panEventData.Visible = true;
        btnFinal.Text = TEXT_BTN_ADD;
        ClearEventData();
        txtEmpID.Enabled = false;
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (GridView1.SelectedRow == null)
        {
            return;
        }

        ClearEventData();
        FillEventData();
        panEventGrid.Visible = false;
        panEventData.Visible = true;
        btnFinal.Text = TEXT_BTN_EDIT;
        txtEmpID.Enabled = false;
        LockSelectiveEventData();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (GridView1.SelectedRow == null)
            return;
        DoDelete();
        FillEventGrid(txtEmpID.Text);
        return;
        //ClearEventData();
        //FillEventData();
        //LockEventData(true);
        //panEventGrid.Visible = false;
        //panEventData.Visible = true;
        //btnFinal.Text = TEXT_BTN_DELETE;
        //txtEmpID.Enabled = false;
    }
    protected void btnFinal_Click(object sender, EventArgs e)
    {
        string rno = "-1";
        string action = string.Empty;
        if(DoValidations()==false)
            return;
        switch (btnFinal.Text)
        {
            case TEXT_BTN_ADD:
                rno = DoAdd();
                break;
            case TEXT_BTN_EDIT:
                rno = DoEdit();
                break;
            case TEXT_BTN_DELETE:
                rno = DoDelete();
                break;
        }
        
        //bool retval = Utils.UpdateEmpperso(txtEmpID.Text);
        //if (retval == false)
        //{
        //    Utils.ShowMessageBox(this, "Unable to update Empperso");
        //}
        //UpdateCadrMap(txtEmpID.Text);
        action = btnFinal.Text;

        bool retval = UpdatePosting(rno,action);
        if (retval == false)
        {
            Utils.ShowMessageBox(this, "Unable to update posting");
        }
        FillBasicInfo(txtEmpID.Text);
        GoBack();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        GoBack();
    }
    protected void drpEvent_SelectedIndexChanged(object sender, EventArgs e)
    {
        LockSelectiveEventData();
    }
    protected void txtploc_TextChanged(object sender, EventArgs e)
    {
        txtpcloc.Text = txtploc.Text;
        DoSancDesg();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Locations(string prefixText, int count, string contextKey)
    {
        return default(string[]);
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static string[] Get_Locations(string prefixText)
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
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static string[] Get_Designations(string prefixText)
    {

        prefixText = prefixText.Replace(' ', '%');

        if (Regex.IsMatch(prefixText, @"['!@#$^*~`]+") == true)
            return null;

        //string sql = "select desgtext ||'-'|| desgcode from pshr.mast_desg where gazcode  =10 and " +
        //                "upper(desgtext) like upper('%" + prefixText + "%')";
        string sql = "select desgtext ||'-'|| desgcode from pshr.mast_desg where upper(desgtext) like upper('%" + prefixText + "%')";
        System.Data.DataSet ds1 = OraDBConnection.GetData(sql);
        List<string> list = new List<string>();
        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            list.Add(ds1.Tables[0].Rows[i][0].ToString());

        }
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
    public static string GetDesgs2()
    {
        StringBuilder sbLocs = new StringBuilder();
        //string sql = "select desgcode, desgtext from pshr.mast_desg where gazcode=10 order by hecode";
        string sql = "select desgcode, desgtext from pshr.mast_desg order by hecode";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);
        foreach (System.Data.DataRow drow in ds.Tables[0].Rows)
        {
            sbLocs.AppendFormat("{0}-{1}:", drow["desgtext"], drow["desgcode"]);
        }
        return sbLocs.ToString();
    }
    [WebMethod()]
    public static ArrayList GetSancDesgs2(string strpcloc)
    {
        ArrayList lstArrLanguage = new ArrayList();

        //get loccode from entry of type "locname-loccode"
        string lcode = strpcloc.Split('-')[strpcloc.Split('-').Length - 1];
        if (lcode.Length != 9)
            return null;
        //string sql = "select a.desg || nvl2(b.empid,'('||b.empid||')',' ') as desg,a.desgcode as desgcode,a.indx as indx from "+
        //            "(select pshr.get_desg(desgcode)||'-'||indx||'-'||substr(cadre.get_branch(branch),1,2)||'-'||ptype as desg, "+
        //            "desgcode,indx,rowno from cadre.cadr where loccode ='" + lcode + 
        //            "' order by scaleid desc) a left outer join cadre.cadrmap b on a.rowno = b.rowno";

//        string sql = @"SELECT a.desg  || nvl2(b.empid,'('  ||b.empid  ||')',' ') AS desg,a.desgcode AS desgcode
//                          FROM
//                            (SELECT pshr.get_desg(cd.desgcode) ||'-'||cd.indx||'-'||SUBSTR(cadre.get_branch(cd.branch),1,2)||'-'||cd.ptype AS desg,
//                              cd.desgcode||'-'||cd.indx as desgcode,
//                              cd.indx as indx,
//                              cd.rowno rowno,
//                              md.hecode as hc
//                            FROM cadre.cadr cd ,PSHR.mast_desg md
//                            WHERE cd.loccode ='" + lcode + @"'
//                            and cd.desgcode=md.desgcode
//                            and md.gazcode = 10
//                            ORDER BY hc,indx
//                          ) a
//                        LEFT OUTER JOIN cadre.cadrmap b ON a.rowno = b.rowno";
        string sql = @"SELECT a.desg  || nvl2(b.empid,'('  ||b.empid  ||')',' ') AS desg,a.desgcode AS desgcode
                          FROM
                            (SELECT pshr.get_desg(cd.desgcode) ||'-'||cd.indx||'-'||SUBSTR(cadre.get_branch(cd.branch),1,2)||'-'||cd.ptype AS desg,
                              cd.desgcode||'-'||cd.indx as desgcode,
                              cd.indx as indx,
                              cd.rowno rowno,
                              md.hecode as hc
                            FROM cadre.cadr cd ,PSHR.mast_desg md
                            WHERE cd.loccode ='" + lcode + @"'
                            and cd.desgcode=md.desgcode
                            ORDER BY hc,indx
                          ) a
                        LEFT OUTER JOIN cadre.cadrmap b ON a.rowno = b.rowno";
        System.Data.DataSet ds = OraDBConnection.GetData(sql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (System.Data.DataRow row in ds.Tables[0].Rows)
            {
                string text = row["desg"].ToString();
                string val = row["desgcode"].ToString();
                lstArrLanguage.Add(new ListItem(text, val));
            }
        }
        else
        {
            lstArrLanguage.Add(new ListItem("---Empty---", "0"));
        }

        return lstArrLanguage;
    }
}