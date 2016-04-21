<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    protected void txtEmpID_TextChanged(object sender, EventArgs e)
    {
        string empid = txtEmpID.Text;
        if (empid.Length != 6)
            return;
        string sql = "select firstname,middlename,pshr.get_desg(cdesgcode) as desg,pshr.get_post(cloccode) as loc,fathername from pshr.empperso where empid='" + empid + "'";
        System.Data.DataSet ds = new System.Data.DataSet();
        ds = OraDBConnection.GetData(sql);

        if (ds.Tables[0].Rows.Count != 1)
            return;
            
        lblName.Text = ds.Tables[0].Rows[0]["firstname"].ToString() + " " + ds.Tables[0].Rows[0]["middlename"].ToString();
        lblDesg.Text = ds.Tables[0].Rows[0]["desg"].ToString();
        lblCLoc.Text = ds.Tables[0].Rows[0]["loc"].ToString();
        lblPCLoc.Text = ds.Tables[0].Rows[0]["loc"].ToString();
        lblFName.Text = ds.Tables[0].Rows[0]["fathername"].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style62
        {
            width: 129px;
            height: 24px;
        }
        .style63
        {
            width: 55px;
            height: 24px;
        }
        .style64
        {
            width: 86px;
            height: 24px;
        }
        .style65
        {
            width: 2px;
            height: 24px;
        }
        .style66
        {
            width: 129px;
            height: 17px;
        }
        .style67
        {
            width: 55px;
            height: 17px;
        }
        .style68
        {
            width: 86px;
            height: 17px;
        }
        .style69
        {
            width: 2px;
            height: 17px;
        }
        .style70
        {
            width: 129px;
            height: 42px;
        }
        .style71
        {
            width: 55px;
            height: 42px;
        }
        .style72
        {
            width: 86px;
            height: 42px;
        }
        .style73
        {
            width: 2px;
            height: 42px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center" style="height: 292px; width: 960px; margin-top: 16px">
    
        <table style="width:95%; height: 278px;">
            <tr>
                <td align="right" class="style70">
                    <asp:Label ID="Label1" runat="server" Text="EmpID"></asp:Label>
                </td>
                <td align="left" class="style71">
                    <asp:TextBox ID="txtEmpID" runat="server" ontextchanged="txtEmpID_TextChanged" 
                        AutoPostBack="True" Height="22px" Width="129px"></asp:TextBox>
                </td>
                <td align="right" class="style72">
                    <asp:Label ID="Label4" runat="server" Text="Designation"></asp:Label>
                </td>
                <td class="style73">
                    <asp:Label ID="lblDesg" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" class="style62">
                    <asp:Label ID="Label3" runat="server" Text="Name"></asp:Label>
                </td>
                <td align="left" class="style63">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
                <td align="right" class="style64">
                    <asp:Label ID="Label2" runat="server" Text="Father Name"></asp:Label>
                </td>
                <td class="style65">
                    <asp:Label ID="lblFName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" class="style66">
                    <asp:Label ID="Label5" runat="server" Text="Current Location"></asp:Label>
                </td>
                <td align="left" class="style67">
                    <asp:Label ID="lblCLoc" runat="server"></asp:Label>
                </td>
                <td class="style68" align="right">
                    <asp:Label ID="Label6" runat="server" Text="Pay Charge Location" Height="24px" 
                        Width="145px"></asp:Label>
                </td>
                <td class="style69">
                    <asp:Label ID="lblPCLoc" runat="server"></asp:Label>
                </td>
            </tr>
            </table>
    
    </div>
    </form>
</body>
</html>
