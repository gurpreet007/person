<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Retirements_Vol.aspx.cs" Inherits="Retire_Batch" %>


<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .watermarked 
        {
            border:1px solid #BEBEBE;
            background-color:#F0F8FF;
            color:gray;
        }
    </style>
    <script type="text/javascript" src="./Scripts/JScript.js"></script>
    <script>
        $(document).ready(function () {
            $('#txtEmpID').watermark('Emp ID').inputmask('integer', { rightAlignNumerics: false, allowMinus: false, allowPlus: false });
            $('#txtDor').watermark('DoR').datepicker(DT_PCKR_OPTS).inputmask("dd/mm/yyyy") ;
            $('#txtoonum').watermark('O/o No.');
            $('#txtoodate').watermark('O/o Date').datepicker(DT_PCKR_OPTS).inputmask("dd/mm/yyyy");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div> 
        
       
                                <table style="width:100%;">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="Label5" runat="server" Font-Size="Large" ForeColor="#3333CC" 
                                                Text="Retirement (Voluntary)"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="50%">
                                            <asp:TextBox ID="txtEmpID" runat="server" Height="17px" 
                                                style="margin-bottom: 0px" Width="100px"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btnShow" runat="server" height="23px" onclick="btnShow_Click" 
                                                Text="Show" Width="100px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:RegularExpressionValidator ID="rev_empid" runat="server" 
                                                ControlToValidate="txtEmpID" Display="Dynamic" ErrorMessage="Invalid EmpID" 
                                                ValidationExpression="^[1-9][0-9]{5}$"></asp:RegularExpressionValidator>
                                        </td>
                                              <td align="right">
                                                  &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="Panel1" runat="server">
                                                <table style="width:100%;" align="center">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblEmpInfo" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:GridView ID="gvEmpHist" runat="server">
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:TextBox ID="txtDor" runat="server" Height="17px" Width="100px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnAdd" runat="server" height="23px" onclick="btnAdd_Click" 
                                                Text="Add" Width="100px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:RegularExpressionValidator ID="rev_dor" runat="server" 
                                                ControlToValidate="txtDor" ErrorMessage="!DD-Mon-YYYY" 
                                                ValidationExpression="^[0123][0-9]-[a-zA-Z]{3}-[0-9]{4}$" Enabled="False"></asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDesgMsg" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                <asp:GridView ID="gv_redesig" runat="server">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" onclick="lnkDelete_Click" OnClientClick="return confirm('Are you sure to delete?');">Delete</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                            <asp:TextBox ID="txtoonum" runat="server" Height="17px" Visible="False" 
                                Width="100px"></asp:TextBox>
                                        </td>
                                        <td>
                            <asp:Button ID="btnPreview" runat="server" onclick="btnPreview_Click" 
                                Text="Preview" Visible="False" Width="100px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                            <asp:TextBox ID="txtoodate" runat="server" Height="17px" 
                                style="margin-left: 0px" Visible="False" Width="100px"></asp:TextBox>
                                        </td>
                                        <td>
                            <asp:Button ID="btnSave" runat="server" Height="22px" Text="Save" 
                                Visible="False" Width="100px" onclick="btnSave_Click" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
        
       
    </div>
</asp:Content>

