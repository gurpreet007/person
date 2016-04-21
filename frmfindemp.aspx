<%--<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="frmfindemp.aspx.vb" Inherits="frmfindemp" %>--%>

<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmfindemp.aspx.vb" Inherits="frmfindemp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Find</title>
    <link href="CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .CssBody
        {
            font-size: small;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" class="CssBody" style="left: 264px; position: relative; top: 48px; border-left-color: #ff3333; border-bottom-color: #ff3333; border-top-style: solid; border-top-color: #ff3333; border-right-style: solid; border-left-style: solid; border-right-color: #ff3333; border-bottom-style: solid;" width="600">
            <tbody>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" class="CssBody" style="position: relative"
                            width="100%">
                            <tr>
                                <td align="left" class="Header1st" style="width: 145px; height: 17px">
                                    <strong>
                                        </strong></td>
                                <td class="Header2nd" style="height: 17px; text-align: center" width="100%">
                                    <asp:Label ID="Label5" runat="server" BackColor="White" BorderColor="Maroon" BorderStyle="Ridge"
                                        BorderWidth="5px" Font-Bold="True" Font-Size="X-Large" ForeColor="#C00000" Text="Find Employee"
                                        Width="296px"></asp:Label>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="ContentBorder" colspan="4">
                        <table border="0" cellpadding="0" cellspacing="10" class="CssBody">
                            <tr>
                                <td style="width: 71px">
                                    First Name</td>
                                <td style="width: 156px; text-align: left">
                                    <asp:TextBox ID="tfName" runat="server" CssClass="txtFont"></asp:TextBox></td>
                                <td>
                                    Middle Name</td>
                                <td>
                                    <asp:TextBox ID="tmName" runat="server" CssClass="txtFont"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 71px">
                                    Last Name</td>
                                <td style="width: 156px; text-align: left">
                                    <asp:TextBox ID="tlName" runat="server" CssClass="txtFont"></asp:TextBox></td>
                                <td>
                                    PAN No</td>
                                <td>
                                    <asp:TextBox ID="tPan" runat="server" CssClass="txtFont"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 71px; text-align: left;">
                                    DOB</td>
                                <td style="text-align: left" colspan="3">
                                    <asp:TextBox ID="txtDOB" runat="server" CssClass="txtFont"></asp:TextBox>&nbsp;(Format 
                                    eg. 05-Aug-2006)</td>
                            </tr>
                            <tr>
                                <td style="height: 18px; width: 71px;">
                                    Designation</td>
                                <td colspan="3" style="height: 18px; text-align: left;">
                                    <asp:DropDownList ID="drpdesg" runat="server" CssClass="DropFont" Width="355px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 71px">
                                    Location</td>
                                <td colspan="3" style="text-align: left">
                                    <asp:DropDownList ID="drploc" runat="server" CssClass="DropFont" Width="352px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 71px">
                                    &nbsp;</td>
                                <td colspan="3">
                                    <asp:Button ID="btnFind" runat="server" CssClass="ButSmall" Text="Find" />
                                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button
                                        ID="btnClear" runat="server" CssClass="ButSmall" Text="Clear" />
                                    &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                                    &nbsp;<asp:HyperLink ID="HyperLink1" runat="server" 
                                        NavigateUrl="~/frmmain.aspx">Back</asp:HyperLink><!--<input type="reset" class="ButSmall" value="Clear">--></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lMsg" runat="server" Font-Bold="True"></asp:Label><br />
                        <br />
                        <asp:DataGrid ID="dgEmpDet" runat="server" AlternatingItemStyle-BackColor="#f8f2ef"
                            BorderColor="#cdc5b1" BorderWidth="1px" CellPadding="5" CssClass="CssBody" HeaderStyle-Font-Bold="True"
                            Width="100%">
                            <AlternatingItemStyle BackColor="#F8F2EF" CssClass="Report_Text" />
                            <ItemStyle CssClass="Report_Text" />
                            <HeaderStyle CssClass="Report_Head" Font-Bold="True" />
                        </asp:DataGrid></td>
                </tr>
            </tbody>
        </table>
    
    </div>
    </form>
</body>
</html>
