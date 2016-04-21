<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmvacancy.aspx.cs" Inherits="frmvacancy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
    .style1
    {
        width: 935px;
    }
    .style2
    {
        width: 14px;
    }
    .style3
    {
        }
    .style4
    {
            width: 433px;
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table class="style1">
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right" class="style3">
                &nbsp;</td>
            <td class="style4" style="text-align: right">
                <asp:Label ID="Label1" runat="server" Text="Select Organisation"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="drporg" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drporg_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right" class="style3">
                &nbsp;</td>
            <td class="style4" style="text-align: right">
                <asp:Label ID="Label2" runat="server" Text="Select Designation"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="drpdesg" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drpdesg_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right" class="style3">
                &nbsp;</td>
            <td class="style4" style="text-align: right">
                <asp:Label ID="Label3" runat="server" Text="Select Branch"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="drpbranch" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drpdesg_SelectedIndexChanged" Height="18px" 
                    Width="87px">
                    <asp:ListItem Value="1">Electrical</asp:ListItem>
                    <asp:ListItem Value="4">Civil</asp:ListItem>
                </asp:DropDownList>
                </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right" class="style3">
                &nbsp;</td>
            <td class="style4" style="text-align: right">
                <asp:Button ID="btnshow" runat="server"  Text="Show " onclick="btnshow_Click" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr align="center">
            <td style="text-align: right" class="style3">
                &nbsp;</td>
            <td class="style2" style="text-align: right" align="center" colspan="3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr align="center">
            <td style="text-align: right" class="style3" colspan="5">
                <asp:GridView ID="gvdata" runat="server">
                </asp:GridView>
            </td>
        </tr>
        <tr align="center">
            <td style="text-align: right" class="style3">
                &nbsp;</td>
            <td class="style2" style="text-align: right" align="center" colspan="3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

