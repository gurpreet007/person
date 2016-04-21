<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NonCadreLoc.aspx.cs" Inherits="NonCadreLoc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 198px;
        }
        .style2
        {
            width: 552px;
        }
        .style3
        {
            width: 151px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width:100%;" width="50">
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style1">
                <br />
                <br />
                <br />
            </td>
            <td class="style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style1">
                <asp:Label ID="Label3" runat="server" Text="Enter Post Name to be  Created" 
                    style="font-weight: 700"></asp:Label>
            </td>
            <td class="style2">
                <asp:TextBox ID="txtNew" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style1">
                <asp:Label ID="Label5" runat="server" Text="Select Type of Post to Create" 
                    style="font-weight: 700"></asp:Label>
            </td>
            <td class="style2">
                <asp:DropDownList ID="drploctype" runat="server" Width="300px" 
                    AutoPostBack="True" onselectedindexchanged="drploctype_SelectedIndexChanged">
                    <asp:ListItem Value="0">--Select Post Type --</asp:ListItem>
                    <asp:ListItem Value="20">CE Level</asp:ListItem>
                    <asp:ListItem Value="30">SE Level</asp:ListItem>
                    <asp:ListItem Value="40">Xen Level</asp:ListItem>
                    <asp:ListItem Value="50">AE-AEE Level</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style1">
                <asp:Label ID="Label4" runat="server" Text="Select Reporting Location" 
                    style="font-weight: 700"></asp:Label>
            </td>
            <td class="style2">
                <asp:DropDownList ID="drpLocs" runat="server" Width="300px">
                </asp:DropDownList>
                <asp:Button ID="btnDel" runat="server" Text="Delete Selected" 
                    onclick="btnDel_Click" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style1">
                <asp:Label ID="Label2" runat="server" Text="Filter Location" 
                    style="font-weight: 700"></asp:Label>
            </td>
            <td class="style2">
                <asp:TextBox ID="txtFilter" runat="server" Width="300px" AutoPostBack="True" 
                    ontextchanged="txtFilter_TextChanged"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                <asp:CheckBox ID="chkShowAll" runat="server" AutoPostBack="True" 
                    Font-Bold="True" oncheckedchanged="chkShowAll_CheckedChanged" 
                    Text="Show All Locations" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                <asp:Label ID="lblmsg" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                <asp:Button ID="btnAdd" runat="server" Text="Add Post" 
                    onclick="btnAdd_Click" height="26px" width="138px" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
    <br />
    <br />
</asp:Content>

