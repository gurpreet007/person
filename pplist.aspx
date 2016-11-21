<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="pplist.aspx.cs" Inherits="pplist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style>
        #wrapper {margin:auto; text-align:center}
        #chkBySen{margin: 10px}
        #divInfo, #divControl, #divMsg{margin: 30px}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="wrapper">
        <div id="divInfo">
            <asp:Label ID="lblDesg" runat="server" Text="Select Designation"></asp:Label>
            <asp:DropDownList ID="drpDesg" runat="server"></asp:DropDownList><br />
            <asp:CheckBox ID="chkBySen" text="Order by Seniority Number" runat="server" />
        </div>
        <div id="divControl">
            <asp:Button ID="btnShow" runat="server" Text="PP List" onclick="btnShow_Click" />
        </div>
        <div id="divMsg" runat="server">
            <asp:Label ID="lblmsg" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>