<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="tnp_proposals.aspx.cs" Inherits="frmproposalmenu" %>

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
                &nbsp;</td>
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
                <asp:GridView ID="gvProposals" runat="server" 
                    AutoGenerateEditButton="True" 
                    onrowediting="gvProposals_RowEditing">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkExport" runat="server" onclick="lnkExport_Click">Export</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this?');" onclick="lnkDelete_Click">Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle BackColor="#99CCFF" />
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
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:DropDownList ID="ddDupProp" runat="server">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddMergeProp" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:TextBox ID="txtpropname" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Button ID="btnCreateDup" runat="server" onclick="btnCreateDup_Click" 
                                    Text="Create" />
                                <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                                    Text="Cancel" />
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Panel ID="Panel1" runat="server" GroupingText="Import Proposal">
                    <asp:FileUpload id="FileUploader" runat="server" />
                    <asp:Button ID="btnPropUp" runat="server" onclick="btnPropUp_Click" 
                    Text="Import" />
                    <br />
                    <asp:Label ID="lblUpFile" runat="server"></asp:Label>
                </asp:Panel>
                <br />
                <br />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>