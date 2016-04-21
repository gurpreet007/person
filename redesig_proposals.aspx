<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="redesig_proposals.aspx.cs" Inherits="frmproposalmenu" %>

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
    <table class="style1">
        <tr>
            <td class="style2" align="center" colspan="3">
                            <asp:Label ID="Label5" runat="server" Font-Size="Large" ForeColor="#3333CC" 
                                Text="Redesignation Proposals"></asp:Label>
                        </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                
            </td>
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
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                <asp:GridView ID="gvProposals" runat="server" 
                    AutoGenerateEditButton="True" 
                    onrowediting="gvProposals_RowEditing">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" onclick="lnkDelete_Click" 
                                    OnClientClick="return confirm('Are you sure to delete?');">Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
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
            <td>
                <asp:Button ID="btncproposal" runat="server" onclick="btncproposal_Click" 
                    Text="Create New Proposal" Height="25px" Width="144px" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
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
                <asp:Panel ID="panNewProp" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td colspan="3">
                                <asp:TextBox ID="txtpropname" runat="server" Width="300px"></asp:TextBox>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Button ID="btnCreate" runat="server" onclick="btnCreate_Click" 
                                    Text="Create" />
                                &nbsp;
                                <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                                    Text="Cancel" />
                            </td>
                            <td>
                                &nbsp;</td>
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
        </tr>
    </table>
</asp:Content>

