<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="redesig.aspx.cs" Inherits="Redesig_Batch"%>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script>
        $(document).ready(function () {
            $('#dt_start').watermark('From Date').datepicker(DT_PCKR_OPTS).inputmask("dd/mm/yyyy");
            $('#dt_end').watermark('To Date').datepicker(DT_PCKR_OPTS).inputmask("dd/mm/yyyy");
            $('#txtEmpID').watermark('Emp ID').inputmask('integer', { rightAlignNumerics: false, allowMinus: false, allowPlus: false });
            $('#txtoonum').watermark('O/o No.');
            $('#txtoodate').watermark('O/o Date').datepicker(DT_PCKR_OPTS).inputmask("dd/mm/yyyy");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div> 
        <ContentTemplate>
            <table width="900px">
                <tr>
                    <td align="center"  colspan="4">
                        <asp:Label ID="Label5" runat="server" Font-Size="Large" ForeColor="#3333CC" 
                            Text="Redesignate"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="200">
                        &nbsp;</td>
                    <td colspan="3">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" >
                        <asp:Label ID="Label4" runat="server" style="text-align: right" 
                            Text="Designation" Width="100px"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="drpDesg" runat="server" Height="20px" Width="102px">
                            <asp:ListItem Value="9052">Sr. XEN</asp:ListItem>
                            <asp:ListItem Value="9050">SE</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="200">
                        &nbsp;</td>
                    <td colspan="3">
                        &nbsp;</td>
                </tr>
                <tr valign="top">
                    <td align="right" valign="top" >
                        <asp:Label ID="Label2" runat="server" 
                            style="text-align: right; margin-left: 0px;" Text="Date of Promotion" 
                            ></asp:Label>
                    </td>
                    <td valign="top" width="100px">
                        <asp:TextBox ID="dt_start" runat="server" Height="17px" Width="100px"></asp:TextBox>
                    </td>
                    <td align="left" valign="top" width="100px">
                        <asp:TextBox ID="dt_end" runat="server" Height="17px" Width="100px"></asp:TextBox>
                    </td>
                    <td valign="top">
                        <asp:Button ID="btnSearch" runat="server" height="23px" 
                            onclick="btnSearch_Click" Text="Search" Width="100px" />
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right" valign="top" width="200px">
                        &nbsp;</td>
                    <td valign="top" width="100px">
                        <asp:RegularExpressionValidator ID="rev_start" runat="server" 
                            ControlToValidate="dt_start" ErrorMessage="!DD-Mon-YYYY" 
                            ValidationExpression="^[0123][0-9]-[a-zA-Z]{3}-[0-9]{4}$" Enabled="False"></asp:RegularExpressionValidator>
                    </td>
                    <td align="left" valign="top" width="100px">
                        <asp:RegularExpressionValidator ID="rev_end" runat="server" 
                            ControlToValidate="dt_end" ErrorMessage="!DD-Mon-YYYY" 
                            ValidationExpression="^[0123][0-9]-[a-zA-Z]{3}-[0-9]{4}$" Enabled="False"></asp:RegularExpressionValidator>
                    </td>
                    <td valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" width="200px">
                        &nbsp;</td>
                    <td colspan="3">
                        <asp:Label ID="lblmsg" runat="server" Text="Label" Visible="False" 
                            Width="600px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" >
                        <asp:Label ID="Label6" runat="server" style="text-align: right" Text="Add Officer" 
                            Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmpID" runat="server" Height="17px" Width="50px"  maxlength="6"
                            onChange="if(!CheckEmpid(this.id)) return false; " ></asp:TextBox>
                    </td>
                        <td>
                            <asp:Button ID="btnShow" runat="server" height="23px" onclick="btnShow_Click" 
                                Text="Show" Width="100px" />
                    </td>
                        <td>
                            <asp:Button ID="btnAdd" runat="server" height="23px" onclick="btnAdd_Click" 
                                Text="Add" Width="100px" />
                    </td>
                </tr>
                <tr>
                    <td align="right" width="200px">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="lblDesgMsg" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="200px">
                        &nbsp;</td>
                    <td colspan="3">
                        <asp:Panel ID="panEmpHist" runat="server" ScrollBars="Auto" Visible="False">
                            <table style="width:100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEmpInfo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvEmpHist" runat="server">
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <ContentTemplate>
                            <asp:GridView ID="gv_redesig" runat="server" AutoGenerateDeleteButton="True" 
                                onrowdeleting="gv_redesig_RowDeleting">
                            </asp:GridView>
                        </ContentTemplate>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="200px">
                        &nbsp;</td>
                    <td align="right" width="100px">
                        &nbsp;</td>
                    <td align="right" width="100px">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" width="200px">
                        &nbsp;</td>
                    <td align="right" width="100px">
                        &nbsp;</td>
                    <td align="right" width="100px">
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="btnPreview" runat="server" onclick="btnPreview_Click" 
                            Text="Preview" Visible="False" Width="100px" />
                    </td>
                </tr>
                <tr>
                    <td align="right" width="200px">
                        &nbsp;</td>
                    <td align="right" >
                        <asp:TextBox ID="txtoonum" runat="server" Height="17px" Visible="False" 
                            Width="100px"></asp:TextBox>
                    </td>
                    <td align="right" width="100px">
                        <asp:TextBox ID="txtoodate" runat="server" Height="17px" 
                            style="margin-left: 0px" Visible="False" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Height="22px" Text="Save" 
                            Visible="False" Width="100px" onclick="btnSave_Click" Enabled="False" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
        </CR:CrystalReportSource>
</div>
</asp:Content>

