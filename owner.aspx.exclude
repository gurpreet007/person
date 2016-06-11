<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="owner.aspx.cs" Inherits="owner" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    <asp:TextBox ID="TextBox3" runat="server" Height="100px" TextMode="MultiLine" 
        Width="900px"></asp:TextBox>
    <cc1:TextBoxWatermarkExtender ID="TextBox3_TextBoxWatermarkExtender" 
        runat="server" Enabled="True" TargetControlID="TextBox3" 
        WatermarkText="SELECT WINDOW">
    </cc1:TextBoxWatermarkExtender>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="SELECT" 
        style="font-weight: 700" Width="100px" />
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="UPDATE" 
        style="font-weight: 700" Width="100px" />
    <asp:TextBox ID="TextBox5" runat="server" Visible="False"></asp:TextBox>
    <asp:TextBox ID="TextBox4" runat="server" Visible="False"></asp:TextBox>
    <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
        style="font-weight: 700" Text="XL" Width="100px" />
    <br />
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:TextBox ID="TextBox6" runat="server" Height="100px" Width="900px" 
        TextMode="MultiLine"></asp:TextBox>
    <cc1:TextBoxWatermarkExtender ID="TextBox6_TextBoxWatermarkExtender" 
        runat="server" Enabled="True" TargetControlID="TextBox6" 
        WatermarkText="UPDATE WINDOW">
    </cc1:TextBoxWatermarkExtender>
    <br />
    <asp:Panel ID="Panel2" runat="server" BorderColor="#003366" BorderWidth="3px" 
        Height="576px" ScrollBars="Both" style="margin-top: 0px" Width="1190px">
        <strong></strong>
        <br />
        <br />
        <asp:GridView ID="GridView1" runat="server" 
            BackColor="#0066CC" BorderColor="#000099" BorderStyle="None" BorderWidth="1px" 
            CellPadding="3" CellSpacing="2" onrowcancelingedit="GridView1_RowCancelingEdit" 
            onrowediting="GridView1_RowEditing" onrowupdating="GridView1_RowUpdating" 
            onselectedindexchanged="GridView1_SelectedIndexChanged" Width="1100px">
            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
            <HeaderStyle BackColor="#000066" Font-Bold="True" ForeColor="White" />
            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
            <RowStyle BackColor="#FFF7E7" BorderColor="Black" BorderStyle="Solid" 
                BorderWidth="3px" ForeColor="#8C4510" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#FFF1D4" />
            <SortedAscendingHeaderStyle BackColor="#B95C30" />
            <SortedDescendingCellStyle BackColor="#F1E5CE" />
            <SortedDescendingHeaderStyle BackColor="#93451F" />
        </asp:GridView>
    </asp:Panel>
    <br />
    <br />
</asp:Content>

