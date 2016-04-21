<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="rptpplst.aspx.cs" Inherits="rptpplst" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 220px;
        }
        .style3
        {
        }
        .style4
        {
            width: 452px;
        }
        .style5
        {
            width: 130px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    <table style="width:100%;">
        <tr>
            <td class="style2">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td class="style5">
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3" align="center" colspan="2">
                <asp:RadioButtonList ID="optpost" runat="server" 
                     RepeatDirection="Horizontal" AutoPostBack="True" 
                    onselectedindexchanged="optpost_SelectedIndexChanged">
                    <asp:ListItem Value="0" Selected="True">Posting Details</asp:ListItem>
                    <asp:ListItem Value="1">Posting List</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                <asp:Label ID="Label1" runat="server" Text="Enter Employee ID"></asp:Label>
            </td>
            <td class="style4">
                <asp:TextBox ID="txtempid" runat="server" Height="22px" Width="210px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                <asp:Label ID="Label2" runat="server" Text="Select Designation"></asp:Label>
            </td>
            <td class="style4">
                <asp:ComboBox ID="drpdesg" runat="server" AutoCompleteMode="SuggestAppend" 
                                    DropDownStyle="DropDownList" height="20px" width="400px" 
                                    AutoPostBack="True" >
                                </asp:ComboBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                <asp:Label ID="Label3" runat="server" Text="Select Organisation"></asp:Label>
            </td>
            <td class="style4">
                <asp:ComboBox ID="drporg" runat="server" AutoCompleteMode="SuggestAppend" 
                                    DropDownStyle="DropDownList" height="20px" width="400px" 
                                    AutoPostBack="True" >
                                </asp:ComboBox>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                <asp:Label ID="Label4" runat="server" Text="Select Branch"></asp:Label>
            </td>
            <td class="style4">
                <asp:ComboBox ID="drpbranch" runat="server" AutoCompleteMode="SuggestAppend" 
                                    DropDownStyle="DropDownList" height="20px" width="400px" 
                                    AutoPostBack="True" >
                                </asp:ComboBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                <asp:Label ID="Label5" runat="server" Text="Select Category"></asp:Label>
            </td>
            <td class="style4">
                <asp:ComboBox ID="drpcateg" runat="server" AutoCompleteMode="SuggestAppend" 
                                    DropDownStyle="DropDownList" height="20px" width="400px" 
                                    AutoPostBack="True" >
                                </asp:ComboBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                <asp:Button ID="btnshow" runat="server" Text="Show Record" 
                    onclick="btnshow_Click" />
            </td>
            <td class="style4">
                <asp:Button ID="btnclear" runat="server" Text="Clear " 
                    onclick="btnclear_Click" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <br />


</asp:Content>

