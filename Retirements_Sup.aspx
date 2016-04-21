<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Retirements_Sup.aspx.cs" Inherits="Retire_Batch" %>

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
            $('#dt_start').watermark('From Date').datepicker(DT_PCKR_OPTS).inputmask("dd/mm/yyyy");
            $('#dt_end').watermark('To Date').datepicker(DT_PCKR_OPTS).inputmask("dd/mm/yyyy");
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
                                                Text="Retirement (Superannuation)"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblDOR" runat="server" style="text-align: right" Text="Date of Retirement" ></asp:Label>
                                            <asp:TextBox ID="dt_start" runat="server" Height="17px" 
                                                style="margin-bottom: 0px" Width="100px"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="dt_end" runat="server" Height="17px" Width="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            &nbsp;</td>
                                        <td align="left" width="50%">
                                            <asp:Button ID="btnSearch" runat="server" height="23px" 
                                                onclick="btnSearch_Click" Text="Search" Width="100px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:RegularExpressionValidator ID="rev_start" runat="server" 
                                                ControlToValidate="dt_start" ErrorMessage="!DD-Mon-YYYY" 
                                                ValidationExpression="^[0123][0-9]-[a-zA-Z]{3}-[0-9]{4}$" Enabled="False"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left">
                                            <asp:RegularExpressionValidator ID="rev_end" runat="server" 
                                                ControlToValidate="dt_end" ErrorMessage="!DD-Mon-YYYY" 
                                                ValidationExpression="^[0123][0-9]-[a-zA-Z]{3}-[0-9]{4}$" Enabled="False"></asp:RegularExpressionValidator>
                                        </td>
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
                                style="margin-left: 0px; margin-bottom: 0px;" Visible="False" Width="100px"></asp:TextBox>
                                        </td>
                                        <td>
                            <asp:Button ID="btnSave" runat="server" Height="22px" Text="Save" 
                                Visible="False" Width="100px" onclick="btnSave_Click" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
        
    </div>
</asp:Content>

