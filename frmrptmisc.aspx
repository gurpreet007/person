<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmrptmisc.aspx.cs" Inherits="frmrptmisc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
        }
        .style3
        {
            width: 257px;
        }
        .style4
        {
            width: 187px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table class="style1">
        <tr>
            <td>
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right">
                &nbsp;</td>
            <td class="style4" style="text-align: right">
                <asp:Label ID="Label1" runat="server" Text="Select Case"></asp:Label>
            </td>
            <td class="style3">
                <asp:DropDownList ID="drpotp" runat="server" Height="19px" Width="277px">
                    <asp:ListItem Value="1">Order not Compliance</asp:ListItem>
                    <asp:ListItem Value="2">Officers on Leave</asp:ListItem>
                    <asp:ListItem Value="3">Officers on Deputation</asp:ListItem>
                    <asp:ListItem Value="4">Date of Relieving Empty</asp:ListItem>
                    <asp:ListItem Value="5">Under Suspension</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right">
                &nbsp;</td>
            <td class="style4" style="text-align: right">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right">
                &nbsp;</td>
            <td class="style4" style="text-align: right">
                &nbsp;</td>
            <td class="style3" align="left">
                <asp:Button ID="btnShowData" runat="server" onclick="btnShowData_Click" 
                    Text="Show Data" style="text-align: left" />
            &nbsp;&nbsp;
                <asp:Button ID="btnPrintData" runat="server" onclick="btnPrintData_Click" 
                    Text="Print Data" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right">
                &nbsp;</td>
            <td class="style4" style="text-align: right">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr align="center">
            <td style="text-align: right">
                &nbsp;</td>
            <td class="style2" style="text-align: right" align="center" colspan="3">
                <asp:GridView ID="gvData" runat="server">
                </asp:GridView>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

