<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Redesig_Batch.aspx.cs" Inherits="Redesig_Batch" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

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
        <table width="900px">
            <tr>
                <td width="200" align="right" >
                <asp:Label ID="Label4" runat="server" Text="Designation" style="text-align: right" 
                        Width="100px"></asp:Label>
                </td>
                <td  colspan="3">
                    <asp:DropDownList ID="drpDesg" runat="server" Width="102px" Height="20px">
                    <asp:ListItem Value="9052">Sr. XEN</asp:ListItem>
                    <asp:ListItem Value="9050">SE</asp:ListItem>
                </asp:DropDownList>
                    </td>
                
            </tr>
            <tr valign="top">
            <td width="200px" align="right" valign="top">

                <asp:Label ID="Label2" runat="server" Text="DoP" style="text-align: right" 
                    Width="100px"></asp:Label>

            </td>
             <td width="100px" valign="top">

                <asp:TextBox ID="dt_start" runat="server" Width="100px" 
                     Height="17px"></asp:TextBox>

                 <asp:TextBoxWatermarkExtender ID="dt_start_TextBoxWatermarkExtender" 
                     runat="server" Enabled="True" TargetControlID="dt_start" 
                     WatermarkText="Start Range" WatermarkCssClass="watermarked">
                 </asp:TextBoxWatermarkExtender>
                 <asp:CalendarExtender ID="dt_start_CalendarExtender" runat="server" 
                     Enabled="True" Format="dd-MMM-yyyy" TargetControlID="dt_start">
                 </asp:CalendarExtender>

            </td>
             <td width="100px" align="left" valign="top">

                <asp:TextBox ID="dt_end" runat="server" Width="100px" 
                     Height="17px"></asp:TextBox>
                 <asp:TextBoxWatermarkExtender ID="dt_end_TextBoxWatermarkExtender" 
                     runat="server" Enabled="True" TargetControlID="dt_end" 
                     WatermarkText="End Range" WatermarkCssClass="watermarked">
                 </asp:TextBoxWatermarkExtender>
                <asp:CalendarExtender ID="dt_end_CalendarExtender" runat="server" 
                    Enabled="True" Format="dd-MMM-yyyy" TargetControlID="dt_end">
                </asp:CalendarExtender>

            </td>
             <td valign="top" >

                <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" 
                    Text="Search" Width="100px" height="23px"/>

                </td>
            </tr>
            <tr valign="top">
            <td width="200px" align="right" valign="top">

                &nbsp;</td>
             <td width="100px" valign="top">

                 <asp:RegularExpressionValidator ID="rev_start" runat="server" 
                     ControlToValidate="dt_start" ErrorMessage="!DD-Mon-YYYY" 
                     ValidationExpression="^[0123][0-9]-[a-zA-Z]{3}-[0-9]{4}$"></asp:RegularExpressionValidator>

            </td>
             <td width="100px" align="left" valign="top">

                 <asp:RegularExpressionValidator ID="rev_end" runat="server" 
                     ControlToValidate="dt_end" ErrorMessage="!DD-Mon-YYYY" 
                     ValidationExpression="^[0123][0-9]-[a-zA-Z]{3}-[0-9]{4}$"></asp:RegularExpressionValidator>

            </td>
             <td valign="top" >

                 &nbsp;</td>
            </tr>
            <tr>
            <td width="200px" align="right">

                &nbsp;</td>
             <td  colspan="3">

                <asp:Label ID="lblmsg" runat="server" Text="Label" Visible="False" Width="600px"></asp:Label>

            </td>
            </tr>
            <tr>
            <td  align="center" colspan="4">

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gv_redesig" runat="server">
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
            </tr>
            </table>
        <asp:Panel ID="Panel1" runat="server" Width="900px" Visible="False">
            <table width="900px">
                <tr>
                    <td width="200px" align="right">
                        &nbsp;</td>
                    <td width="100px" align="right">
                        &nbsp;</td>
                    <td width="100px" align="right">
                        <asp:Button ID="btnPreview" runat="server" onclick="btnPreview_Click" 
                            Text="Preview" Width="100px" />
                    </td>
                    <td >
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width="200px" align="right">
                        <asp:TextBox ID="txtoonum" runat="server" Height="17px" Width="100px"></asp:TextBox>
                        <asp:TextBoxWatermarkExtender ID="txtoonum_TextBoxWatermarkExtender" 
                            runat="server" Enabled="True" TargetControlID="txtoonum" 
                            WatermarkCssClass="watermarked" WatermarkText="O/o Num">
                        </asp:TextBoxWatermarkExtender>
                    </td>
                    <td width="100px" align="left">
                        <asp:TextBox ID="txtoodate" runat="server" Width="100px" Height="17px" 
                            style="margin-left: 0px"></asp:TextBox>
                        <asp:TextBoxWatermarkExtender ID="txtoodate_TextBoxWatermarkExtender" 
                            runat="server" Enabled="True" TargetControlID="txtoodate" 
                            WatermarkCssClass="watermarked" WatermarkText="O/o Date">
                        </asp:TextBoxWatermarkExtender>
                        <asp:CalendarExtender ID="txtoodate_CalendarExtender" runat="server" 
                            Enabled="True" Format="dd-MMM-yyyy" TargetControlID="txtoodate">
                        </asp:CalendarExtender>
                    </td>
                    <td width="100px" align="right">
                        <asp:Button ID="btnSave" runat="server" Height="22px" Text="Save" 
                            Width="100px" />
                    </td>
                    <td >
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                    Enabled="True" Format="dd-MMM-yyyy" TargetControlID="dt_end">
                        </asp:CalendarExtender>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
</div>
</asp:Content>

