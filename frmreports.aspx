<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmreports.aspx.cs" Inherits="frmreports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
            width: 225px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    <table style="width:100%;">
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
            <td class="style1" align="center" colspan="4">
                <asp:RadioButtonList ID="optpost" runat="server" 
                     RepeatDirection="Horizontal" AutoPostBack="True" 
                    onselectedindexchanged="optpost_SelectedIndexChanged" 
                    style="height: 26px; width: 367px">
                    <asp:ListItem Value="0" Selected="True">Posting Details</asp:ListItem>
                    <asp:ListItem Value="1">PP List</asp:ListItem>
                    <asp:ListItem Value="2">Vacancy List</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="style1" align="center" colspan="4">
                <asp:RadioButtonList ID="rbWise" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="rbWise_SelectedIndexChanged" 
                    RepeatDirection="Horizontal" Enabled="False" Visible="False">
                    <asp:ListItem Selected="True" Value="DW">Designation Wise</asp:ListItem>
                    <asp:ListItem Value="LW">Location Wise</asp:ListItem>
                </asp:RadioButtonList>
            </td>
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
                <%--<asp:ComboBox ID="drpdesg" runat="server" AutoCompleteMode="SuggestAppend" 
                                    DropDownStyle="DropDownList" height="20px" width="400px" 
                                    AutoPostBack="True" >
                                </asp:ComboBox>--%>
                <asp:DropDownList ID="drpdesg" runat="server">
                </asp:DropDownList>
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
                <%--<asp:ComboBox ID="drporg" runat="server" AutoCompleteMode="SuggestAppend" 
                                    DropDownStyle="DropDownList" height="20px" width="400px" 
                                    AutoPostBack="True" >
                                </asp:ComboBox>--%>&nbsp;<asp:DropDownList ID="drporg" 
                    runat="server">
                </asp:DropDownList>
                <asp:CheckBox ID="chksloc" runat="server" Text="Incl. Sub Locations" />
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                &nbsp;</td>
            <td class="style4">
                <asp:RadioButtonList ID="chkorgopt" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="chkorgopt_SelectedIndexChanged1" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True">Zone</asp:ListItem>
                    <asp:ListItem>Circle</asp:ListItem>
                    <asp:ListItem>Division</asp:ListItem>
                    <asp:ListItem>SubDivision</asp:ListItem>
                    <asp:ListItem>All</asp:ListItem>
                </asp:RadioButtonList>
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
                <%--<asp:ComboBox ID="drpbranch" runat="server" AutoCompleteMode="SuggestAppend" 
                                    DropDownStyle="DropDownList" height="20px" width="400px" 
                                    AutoPostBack="True" >
                                </asp:ComboBox>--%>
                <asp:DropDownList ID="drpbranch" runat="server">
                </asp:DropDownList>
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
                <%--<asp:ComboBox ID="drpcateg" runat="server" AutoCompleteMode="SuggestAppend" 
                                    DropDownStyle="DropDownList" height="20px" width="400px" 
                                    AutoPostBack="True" >
                                </asp:ComboBox>--%>
                <asp:DropDownList ID="drpcateg" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                <asp:Button ID="btnshowpanel" runat="server" Text="Show Panel" 
                    onclick="btnshowpanel_Click" />
            </td>
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
            <td class="style2" align="right">
                <asp:DropDownList ID="drpDesgCat" runat="server">
                    <asp:ListItem Value="0">Eng. Gaz.</asp:ListItem>
                    <asp:ListItem Value="10">Accounts</asp:ListItem>
                    <asp:ListItem Value="20">Technical</asp:ListItem>
                    <asp:ListItem Value="30">Engineering</asp:ListItem>
                    <asp:ListItem Value="40">General</asp:ListItem>
                    <asp:ListItem Value="99">Others</asp:ListItem>
                    <asp:ListItem Value="100">All</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="style5">
                <asp:Button ID="btnGetDesg" runat="server" onclick="btnGetDesg_Click" 
                    Text="Get Desgs" />
            </td>
            <td class="style4">
                <asp:Button ID="btnOPList" runat="server" CausesValidation="False" 
                    onclick="btnOPList_Click" Text="Generate Mapping List" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2" align="right">
                <asp:TextBox ID="txtDesgFilter" runat="server" Width="120px">Filter</asp:TextBox>
            </td>
            <td class="style5">
                <asp:DropDownList ID="drpDesgs" runat="server" Width="150px">
                    <asp:ListItem Selected="True" 
                        Value="9047, 9048, 9365, 9050, 9366, 9052, 9056, 9057, 9060">All Engg. Gaz.</asp:ListItem>
                    <asp:ListItem Value="9047, 9048">EIC, CE</asp:ListItem>
                    <asp:ListItem Value="9047">EIC</asp:ListItem>
                    <asp:ListItem Value="9048">CE</asp:ListItem>
                    <asp:ListItem Value="9365, 9050">Dy. CE, SE</asp:ListItem>
                    <asp:ListItem Value="9365">Dy. CE</asp:ListItem>
                    <asp:ListItem Value="9050">SE</asp:ListItem>
                    <asp:ListItem Value="9366, 9052">Addl. SE, Sr. XEN</asp:ListItem>
                    <asp:ListItem Value="9366">Addl. SE</asp:ListItem>
                    <asp:ListItem Value="9052">Sr. XEN</asp:ListItem>
                    <asp:ListItem Value="9056, 9057, 9060">AEE, AE, AE(OT)</asp:ListItem>
                    <asp:ListItem Value="9056">AEE</asp:ListItem>
                    <asp:ListItem Value="9057">AE</asp:ListItem>
                    <asp:ListItem Value="9060">AE (OT)</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="style4">
                <asp:Button ID="btnOPList0" runat="server" CausesValidation="False" 
                    onclick="btnOPList0_Click" Text="Generate OP List" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <br />

</asp:Content>

