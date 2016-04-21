<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmproposal.aspx.cs" Inherits="frmproposal" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        body
        {
            background: #b6b7bc;
            font-size: .80em;
            font-family: "Helvetica Neue" , "Lucida Grande" , "Segoe UI" , Arial, Helvetica, Verdana, sans-serif;
            margin: 0px;
            padding: 0px;
            color: #696969;
            height: 198px;
        }
        .style2
        {
            height: 26px;
        }
        .style4
        {
            height: 32px;
        }
        .unwatermarked 
        {
        }
        .watermarked 
        {
	        height:20px;
	        width:150px;
	        padding:2px 0 0 2px;
	        border:1px solid #BEBEBE;
	        background-color:#F0F8FF;
	        color:gray;
        }
        .style7
        {
            height: 55px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="panMain" runat="server" GroupingText="Transfer">
        <table style="width:100%;">
            <tr>
                <td>
                    </td>
                <td colspan="4" align="center">
                    <asp:Label ID="lblProposalName" runat="server" Font-Size="Large"></asp:Label>
                    <br />
                </td>
                <td>
                    </td>
            </tr>
            <tr>
                <td class="style7">
                    <br />
                    <br />
                    <br />
                    <br />
                </td>
                <td class="style7" colspan="2">
                    <asp:Panel ID="panPresent" runat="server" GroupingText="Present Post" 
                        height="310px" width="427px" style="margin-top: 0px">
                        <table style="width:100%; height: 182px;">
                            <tr>
                                <td align="center">
                                    <asp:RadioButtonList ID="rbTnP" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="rbTnP_SelectedIndexChanged" 
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Value="T">Transfer</asp:ListItem>
                                        <asp:ListItem Value="P">Promotion</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:RadioButtonList ID="rbNewOut" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="rbNewOut_SelectedIndexChanged" 
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Value="N">New</asp:ListItem>
                                        <asp:ListItem Value="O">Outstanding</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label8" runat="server" Text="Search by"></asp:Label>
                                    &nbsp;
                                    <asp:DropDownList ID="drpSearchby" runat="server" AutoPostBack="True" 
                                        Height="25px" onselectedindexchanged="drpSearchby_SelectedIndexChanged">
                                        <asp:ListItem Value="empid">Empid</asp:ListItem>
                                        <asp:ListItem Value="name">Name</asp:ListItem>
                                        <asp:ListItem Value="posting">Posting</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txtEmpid" runat="server" AutoPostBack="True" 
                                        CssClass="unwatermarked" height="25px" ontextchanged="txtEmpid_TextChanged" 
                                        width="294px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtEmpid_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="txtEmpid" 
                                        WatermarkCssClass="watermarked" WatermarkText="EmpID">
                                    </asp:TextBoxWatermarkExtender>
                                    <asp:AutoCompleteExtender ID="txtEmpid_AutoCompleteExtender" runat="server" 
                                        DelimiterCharacters="" Enabled="True" ServiceMethod="GetLocs" ServicePath="" 
                                        TargetControlID="txtEmpid" UseContextKey="True">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txtName" runat="server" AutoPostBack="True" 
                                        CssClass="unwatermarked" height="25px" ontextchanged="txtName_TextChanged" 
                                        width="294px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtName_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="txtName" 
                                        WatermarkCssClass="watermarked" WatermarkText="Name">
                                    </asp:TextBoxWatermarkExtender>
                                    <asp:AutoCompleteExtender ID="txtName_AutoCompleteExtender" runat="server" 
                                        DelimiterCharacters="" Enabled="True" ServiceMethod="GetNames" ServicePath="" 
                                        TargetControlID="txtName" UseContextKey="True">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txtLoc" runat="server" AutoPostBack="True" 
                                        CssClass="unwatermarked" height="25px" ontextchanged="txtLoc_TextChanged" 
                                        width="294px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtLoc_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="txtLoc" 
                                        WatermarkCssClass="watermarked" WatermarkText="Posting Location">
                                    </asp:TextBoxWatermarkExtender>
                                    <asp:AutoCompleteExtender ID="txtLoc_AutoCompleteExtender" runat="server" 
                                        DelimiterCharacters="" Enabled="True" ServiceMethod="GetLocs" ServicePath="" 
                                        TargetControlID="txtLoc" UseContextKey="True">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:DropDownList ID="drpOfficer" runat="server" AutoPostBack="True" 
                                        height="25px" onselectedindexchanged="drpOfficer_SelectedIndexChanged" 
                                        width="294px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblLoc" runat="server"></asp:Label>
                                    <asp:Label ID="lbldesg" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSelPresent" runat="server" onclick="btnSelPresent_Click" 
                                        Text="Select" Width="136px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td class="style7" colspan="2">
                    <asp:Panel ID="panProposed" runat="server" Enabled="False" 
                        GroupingText="Proposed Post" Height="310px" Width="427px">
                        <table style="width:100%; height: 3px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblInfo" runat="server" Text="Transfer 'Name' to:"></asp:Label>
                                    <asp:Label ID="lblvalstat" runat="server"></asp:Label>
                                    <asp:Label ID="lblvaldesg" runat="server"></asp:Label>
                                    <asp:Label ID="lblvalsno" runat="server"></asp:Label>
                                    <asp:Label ID="lblvalbranch" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="style2">
                                    <asp:TextBox ID="txtLocFilter" runat="server" AutoPostBack="True" 
                                        CssClass="unwatermarked" Height="25px" ontextchanged="txtLocFilter_TextChanged" 
                                        width="274px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtLocFilter_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="txtLocFilter" 
                                        WatermarkCssClass="watermarked" WatermarkText="Filter">
                                    </asp:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="style2">
                                    <asp:DropDownList ID="drpLocs" runat="server" AutoPostBack="True" 
                                        BackColor="AliceBlue" height="25px" 
                                        onselectedindexchanged="drpLocs_SelectedIndexChanged" width="274px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="style2">
                                    <asp:TextBox ID="txtCLoc" runat="server" AutoPostBack="True" 
                                        CssClass="unwatermarked" height="25px" width="274px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtCLoc_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="txtCLoc" 
                                        WatermarkCssClass="watermarked" WatermarkText="Pay Charge Location">
                                    </asp:TextBoxWatermarkExtender>
                                    <asp:AutoCompleteExtender ID="txtCLoc_AutoCompleteExtender" runat="server" 
                                        DelimiterCharacters="" Enabled="True" ServiceMethod="GetLocs" ServicePath="" 
                                        TargetControlID="txtCLoc" UseContextKey="True">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="style2">
                                    <asp:TextBox ID="txtCDesg" runat="server" CssClass="unwatermarked" 
                                        height="25px" width="274px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtCDesg_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="txtCDesg" 
                                        WatermarkCssClass="watermarked" WatermarkText="Pay Charge Designation">
                                    </asp:TextBoxWatermarkExtender>
                                    <asp:AutoCompleteExtender ID="txtCDesg_AutoCompleteExtender" runat="server" 
                                        DelimiterCharacters="" Enabled="True" ServiceMethod="GetDesgs" ServicePath="" 
                                        TargetControlID="txtCDesg" UseContextKey="True">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="unwatermarked" 
                                        height="25px" width="274px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtRemarks_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="txtRemarks" 
                                        WatermarkCssClass="watermarked" WatermarkText="Remarks">
                                    </asp:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSelProposed" runat="server" height="26px" 
                                        onclick="btnSelProposed_Click" Text="Select" width="136px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td class="style7">
                </td>
            </tr>
            <tr>
                <td colspan="6" align="center">
                    <asp:Panel ID="panProposals" runat="server">
                        <table style="width:100%;">
                            <tr align="center">
                                <td align="center">
                                    <asp:GridView ID="gvProposals" runat="server" HorizontalAlign="Left" 
                                        AutoGenerateEditButton="True" onrowediting="gvProposals_RowEditing" 
                                          >
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                    <asp:TextBox ID="txtNotes" runat="server" Height="16px" TextMode="MultiLine" 
                        Width="871px" Visible="False">Notes:-</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                    &nbsp;<asp:Panel ID="panControls" runat="server">
                        <table style="width:100%;">
                            <tr align="center">
                                <td align="center">
                                    <asp:Button ID="btncreatenotes" runat="server" height="23px" 
                                        onclick="btncreatenotes_Click" Text="Insert Notes" width="150px" />
                                </td>
                                <td align="center">
                                    <asp:Button ID="btnPrintProposal" runat="server" height="23px" 
                                        Text="Print Proposal" UseSubmitBehavior="False" width="150px" 
                                        onclick="btnPrintProposal_Click" />
                                </td>
                                <td align="center">
                                    <asp:Button ID="btnCreateCC" runat="server" EnableTheming="True" height="23px" 
                                        onclick="btnCreateCC_Click" Text="Create CC List" width="150px" />
                                </td>
                                <td align="center">
                                    <asp:Button ID="btnPreview" runat="server" height="23px" 
                                        onclick="btnPreview_Click" Text="Preview Report" UseSubmitBehavior="False" 
                                        width="150px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    &nbsp;</td>
                <td align="center" colspan="2">
                    <asp:Panel ID="panReports" runat="server">
                        <table style="width:100%;">
                            <tr>
                                <td align="center" class="style4">
                                    &nbsp;<asp:TextBox ID="txtOoNum" runat="server" CssClass="unwatermarked" 
                                        Height="25px" Width="78px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtOoNum_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="txtOoNum" 
                                        WatermarkCssClass="watermarked" WatermarkText="O/o Number">
                                    </asp:TextBoxWatermarkExtender>
                                    &nbsp;<asp:TextBox ID="txtOoDate" runat="server" CssClass="unwatermarked" 
                                        Height="25px" width="78px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtOoDate_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="txtOoDate" 
                                        WatermarkCssClass="watermarked" WatermarkText="O/o Date">
                                    </asp:TextBoxWatermarkExtender>
                                    <asp:CalendarExtender ID="txtOoDate_CalendarExtender" runat="server" 
                                        Enabled="True" Format="dd-MMM-yyyy" TargetControlID="txtOoDate">
                                    </asp:CalendarExtender>
                                    <asp:Button ID="btnSave" runat="server" height="26px" onclick="btnSave_Click" 
                                        Text="Save and Generate O/o" UseSubmitBehavior="False" width="203px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;</td>
                <td align="center" colspan="2">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                        <Report FileName="Reports\\rptposttrans.rpt">
                        </Report>
                    </CR:CrystalReportSource>
                </td>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

