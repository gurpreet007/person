Imports System.Data
Partial Class frmfindemp
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Len(Session("UID")) = 0 Then
            'Response.Redirect("Default.aspx")
        End If
        lMsg.Text = ""
        If Not IsPostBack Then
            Dim oraCn As New OraDBconnection
            Dim ds As New DataSet
            Dim sql As String

            sql = "select desgtext, desgcode  from pshr.mast_desg order by desgtext"
            ds = OraDBConnection.GetData(sql)
            drpdesg.Items.Clear()
            drpdesg.DataSource = ds.Tables(0)
            drpdesg.DataTextField = "desgtext"
            drpdesg.DataValueField = "desgcode"
            drpdesg.DataBind()

            'OraDBConnection.
            'oraCn.FillDrp(ds, drpdesg)
            ds.Clear()
            ds.Dispose()
            drpdesg.SelectedIndex = Me.drpdesg.Items.IndexOf(Me.drpdesg.Items.FindByText(""))
            ds = New DataSet()
            sql = "select locname, loccode from pshr.mast_loc where loccode <> 77777 order by loccode"
            ds = OraDBConnection.GetData(sql)
            'oraCn.FillData(sql, ds)
            drploc.Items.Clear()
            drploc.DataSource = ds.Tables(0)
            drploc.DataTextField = "locname"
            drploc.DataValueField = "loccode"
            drploc.DataBind()

            'oraCn.FillDrp(ds, drploc)
            ds.Clear()
            ds.Dispose()


        End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        tfName.Text = ""
        tmName.Text = ""
        tlName.Text = ""
        txtDOB.Text = ""
        tPan.Text = ""
        drpdesg.SelectedIndex = Me.drpdesg.Items.IndexOf(Me.drpdesg.Items.FindByText(""))
        drploc.SelectedIndex = 0
    End Sub

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        If Len(tfName.Text.Trim & tmName.Text.Trim & tlName.Text.Trim & tPan.Text & drploc.SelectedItem.Text.Trim & drpdesg.SelectedItem.Text.Trim & txtDOB.Text) = 0 Then
            'lMsg.Text = "Full name can not be less than 3 alphabets"
            lMsg.Text = "All fields can not be empty"
            dgEmpDet.Visible = False
            Exit Sub
        End If
        dgEmpDet.Visible = True

        Dim oraCn As New OraDBconnection
        Dim ds As New DataSet
        Dim sql As String

        sql = ""
        sql = sql & "select empid, firstname || ' ' || middlename || ' ' || lastname emp_name, fathername father_name, gpfno,panno,desgtext designation, locname location,to_char(dob,'dd-Mon-yyyy') Date_Of_Birth from pshr.empperso a, pshr.mast_desg b, pshr.mast_loc c where a.cdesgcode=b.desgcode and a.cloccode=c.loccode and a.recstatus=10 "
        If tfName.Text <> "" Then sql = sql & "and upper(firstname) like upper('" & tfName.Text & "%') "
        If tmName.Text <> "" Then sql = sql & "and upper(middlename) like upper('" & tmName.Text & "%') "
        If tlName.Text <> "" Then sql = sql & "and upper(lastname) like upper('" & tlName.Text & "%') "
        If tPan.Text <> "" Then sql = sql & "and upper(panno) like upper('" & tPan.Text & "%') "
        If drpdesg.SelectedItem.Text <> "" Then sql = sql & "and cdesgcode like '" & drpdesg.SelectedValue & "%' "
        If drploc.SelectedItem.Text <> "" Then sql = sql & "and cloccode like '" & drploc.SelectedValue & "%' "
        If txtDOB.Text <> "" And IsDate(txtDOB.Text) Then
            Dim d As Date
            d = txtDOB.Text
            txtDOB.Text = Format(d, "dd-MMM-yyyy")
            sql = sql & "and dob = '" & txtDOB.Text & "' "
        End If
        sql = sql & " order by a.empid "

        'oraCn.FillData(sql, ds)
        ds = OraDBConnection.GetData(sql)
        dgEmpDet.DataSource = ds
        dgEmpDet.DataBind()
        ds.Clear()
        ds.Dispose()

        If dgEmpDet.Items.Count > 1 Then
            lMsg.Text = dgEmpDet.Items.Count.ToString & " Matches found"
        Else
            lMsg.Text = dgEmpDet.Items.Count.ToString & " Match found"
        End If
    End Sub
End Class
