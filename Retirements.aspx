<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Retirements.aspx.cs" Inherits="Retire_Batch" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<style type="text/css">
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
    <div> <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
        
            <ContentTemplate>
                <table width="900px">
                    <tr>
                        <td align="left" width="200" colspan="4">
                         
                            <asp:Panel ID="panSup" runat="server" Width="894px">
                                <table style="width:100%;">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label4" runat="server" style="text-align: right" 
                                                Text="Designation" Visible="False" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpDesg" runat="server" Height="20px" Visible="False" 
                                                Width="102px">
                                                <asp:ListItem Value="9052">Sr. XEN</asp:ListItem>
                                                <asp:ListItem Value="9050">SE</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                              <td>
                                            &nbsp;</td>
                                              <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblDOR" runat="server" style="text-align: right" Text="DoR" 
                                                Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="dt_start" runat="server" Height="17px" 
                                                style="margin-bottom: 0px" Width="100px"></asp:TextBox>
                                            <asp:TextBoxWatermarkExtender ID="dt_start_TextBoxWatermarkExtender" 
                                                runat="server" Enabled="True" TargetControlID="dt_start" 
                                                WatermarkCssClass="watermarked" WatermarkText="Start Range">
                                            </asp:TextBoxWatermarkExtender>
                                            <asp:CalendarExtender ID="dt_start_CalendarExtender" runat="server" 
                                                Enabled="True" Format="dd-MMM-yyyy" TargetControlID="dt_start">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="dt_end" runat="server" Height="17px" Width="100px"></asp:TextBox>
                                            <asp:TextBoxWatermarkExtender ID="dt_end_TextBoxWatermarkExtender" 
                                                runat="server" Enabled="True" TargetControlID="dt_end" 
                                                WatermarkCssClass="watermarked" WatermarkText="End Range">
                                            </asp:TextBoxWatermarkExtender>
                                            <asp:CalendarExtender ID="dt_end_CalendarExtender" runat="server" 
                                                Enabled="True" Format="dd-MMM-yyyy" TargetControlID="dt_end">
                                            </asp:CalendarExtender>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" 
                                                Format="dd-MMM-yyyy" TargetControlID="dt_end">
                                            </asp:CalendarExtender>
                                        </td>
                                              <td>
                                                  <asp:Button ID="btnSearch" runat="server" height="23px" 
                                                      onclick="btnSearch_Click" Text="Search" Width="100px" />
                                        </td>
                                              <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <asp:RegularExpressionValidator ID="rev_start" runat="server" 
                                                ControlToValidate="dt_start" ErrorMessage="!DD-Mon-YYYY" 
                                                ValidationExpression="^[0123][0-9]-[a-zA-Z]{3}-[0-9]{4}$"></asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            <asp:RegularExpressionValidator ID="rev_end" runat="server" 
                                                ControlToValidate="dt_end" ErrorMessage="!DD-Mon-YYYY" 
                                                ValidationExpression="^[0123][0-9]-[a-zA-Z]{3}-[0-9]{4}$"></asp:RegularExpressionValidator>
                                        </td>
                                              <td>
                                            &nbsp;</td>
                                              <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panVol" runat="server" Width="894px">
                                <table style="width:100%;">
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td align="right">
                                            <asp:TextBox ID="txtEmpID" runat="server" Height="17px" Width="100px"></asp:TextBox>
                                            <asp:TextBoxWatermarkExtender ID="txtEmpID_TextBoxWatermarkExtender" 
                                                runat="server" Enabled="True" TargetControlID="txtEmpID" 
                                                WatermarkCssClass="watermarked" WatermarkText="EmpID">
                                            </asp:TextBoxWatermarkExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnShow" runat="server" height="23px" onclick="btnShow_Click" 
                                                Text="Show" Width="100px" />
                                        </td>
                                              <td align="left">
                                                  <asp:TextBox ID="txtDor" runat="server" Height="17px" Width="100px"></asp:TextBox>
                                                  <asp:TextBoxWatermarkExtender ID="txtDor_TextBoxWatermarkExtender" 
                                                      runat="server" Enabled="True" TargetControlID="txtDor" 
                                                      WatermarkCssClass="watermarked" WatermarkText="Enter DOR">
                                                  </asp:TextBoxWatermarkExtender>
                                                  <asp:CalendarExtender ID="txtDor_CalendarExtender" runat="server" 
                                                      Enabled="True" Format="dd-MMM-yyyy" TargetControlID="txtDor">
                                                  </asp:CalendarExtender>
                                        </td>
                                              <td>
                                                  <asp:Button ID="btnAdd" runat="server" height="23px" onclick="btnAdd_Click" 
                                                      Text="Add" Width="100px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td align="right">
                                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" 
                                                runat="server" Enabled="True" TargetControlID="dt_start" 
                                                WatermarkCssClass="watermarked" WatermarkText="Start Range">
                                            </asp:TextBoxWatermarkExtender>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                                                Enabled="True" Format="dd-MMM-yyyy" TargetControlID="dt_start">
                                            </asp:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="rev_empid" runat="server" 
                                                ControlToValidate="txtEmpID" Display="Dynamic" ErrorMessage="Invalid EmpID" 
                                                ValidationExpression="^[1-9][0-9]{5}$"></asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
                                                runat="server" Enabled="True" TargetControlID="dt_end" 
                                                WatermarkCssClass="watermarked" WatermarkText="End Range">
                                            </asp:TextBoxWatermarkExtender>
                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" 
                                                Enabled="True" Format="dd-MMM-yyyy" TargetControlID="dt_end">
                                            </asp:CalendarExtender>
                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" 
                                                Format="dd-MMM-yyyy" TargetControlID="dt_end">
                                            </asp:CalendarExtender>
                                        </td>
                                              <td align="left">
                                                  <asp:RegularExpressionValidator ID="rev_dor" runat="server" 
                                                      ControlToValidate="txtDor" ErrorMessage="!DD-Mon-YYYY" 
                                                      ValidationExpression="^[0123][0-9]-[a-zA-Z]{3}-[0-9]{4}$"></asp:RegularExpressionValidator>
                                        </td>
                                              <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td align="right">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        <td align="left">
                                            <asp:Label ID="lblDesgMsg" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        <td colspan="2">
                                            <asp:Panel ID="Panel1" runat="server">
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
                                                </table>
                                            </asp:Panel>
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
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        <td align="center" colspan="2">
                                            &nbsp;</td>
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
                                <asp:GridView ID="gv_redesig" runat="server">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" onclick="lnkDelete_Click" OnClientClick="return confirm('Are you sure to delete?');">Delete</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
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
                            <asp:Button ID="btnPreview" runat="server" onclick="btnPreview_Click" 
                                Text="Preview" Visible="False" Width="100px" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right" width="200px">
                            <asp:TextBox ID="txtoonum" runat="server" Height="17px" Visible="False" 
                                Width="100px"></asp:TextBox>
                            <asp:TextBoxWatermarkExtender ID="txtoonum_TextBoxWatermarkExtender" 
                                runat="server" Enabled="True" TargetControlID="txtoonum" 
                                WatermarkCssClass="watermarked" WatermarkText="O/o Num">
                            </asp:TextBoxWatermarkExtender>
                        </td>
                        <td align="left" width="100px">
                            <asp:TextBox ID="txtoodate" runat="server" Height="17px" 
                                style="margin-left: 0px" Visible="False" Width="100px"></asp:TextBox>
                            <asp:TextBoxWatermarkExtender ID="txtoodate_TextBoxWatermarkExtender" 
                                runat="server" Enabled="True" TargetControlID="txtoodate" 
                                WatermarkCssClass="watermarked" WatermarkText="O/o Date">
                            </asp:TextBoxWatermarkExtender>
                            <asp:CalendarExtender ID="txtoodate_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd-MMM-yyyy" TargetControlID="txtoodate">
                            </asp:CalendarExtender>
                        </td>
                        <td align="right" width="100px">
                            <asp:Button ID="btnSave" runat="server" Height="22px" Text="Save" 
                                Visible="False" Width="100px" onclick="btnSave_Click" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </ContentTemplate>
        
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
</div>
</asp:Content>

