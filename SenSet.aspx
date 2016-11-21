<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SenSet.aspx.cs" Inherits="SenSet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style>
        #wrapper {margin:auto; text-align:center}
        #info, #update, #divmsg{margin: 10px}
        input[type=text] {width:60px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="wrapper">
        <div id="header">
            <asp:Label ID="lblEmpID" runat="server" Text="Emp ID:"></asp:Label>
            <asp:TextBox ID="txtEmpID" runat="server" MaxLength="6"></asp:TextBox>
            <asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" />
        </div>
        <div id="info">
            <asp:Label ID="lblNameDesg" runat="server" Font-Bold="True"></asp:Label><br />
            <asp:Label ID="lblLoc" runat="server"></asp:Label><br /><br />
            <asp:Label ID="lblSen" runat="server" Font-Bold="True"></asp:Label>
        </div>
        <div id="update" runat="server" visible="false">
            <asp:Label ID="lblNewSen" runat="server" Text="Set New Sen:"></asp:Label>
            <asp:TextBox ID="txtNewSen" runat="server"></asp:TextBox>
            <asp:Button ID="btnUpdate" runat="server" Text="Update Seniority" onclick="btnUpdate_Click"/>
        </div>
        <div id="divmsg" runat="server">
        <asp:Label ID="lblmsg" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>