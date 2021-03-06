﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmproposalmenu.aspx.cs" Inherits="frmproposalmenu" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 199px;
        }
        .style3
        {
            width: 39px;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:HiddenField ID="PropType" runat="server" />
    <table class="style1">
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                <asp:RadioButtonList ID="rbStatus" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="rbStatus_SelectedIndexChanged" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="U">Unsaved</asp:ListItem>
                    <asp:ListItem Value="S">Saved</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                <asp:GridView ID="gvProposals" runat="server" AutoGenerateDeleteButton="True" 
                    AutoGenerateEditButton="True" onrowdeleting="gvProposals_RowDeleting" 
                    onrowediting="gvProposals_RowEditing">
                </asp:GridView>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td colspan="2">
                <asp:Button ID="btncproposal" runat="server" onclick="btncproposal_Click" 
                    Text="Create New Proposal" Height="25px" Width="144px" />
            </td>
            <td class="style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                <asp:Panel ID="panNewProp" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td colspan="3">
                                <asp:TextBox ID="txtpropname" runat="server" Width="200px"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="txtpropname_TBWE" runat="server" 
                                    Enabled="True" TargetControlID="txtpropname" WatermarkCssClass="watermarked" 
                                    WatermarkText="Proposal Name">
                                </asp:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Button ID="btnCreate" runat="server" onclick="btnCreate_Click" 
                                    Text="Create" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                                    Text="Cancel" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

