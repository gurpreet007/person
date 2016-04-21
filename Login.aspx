
<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Login.aspx.cs" Inherits="_Default" ClientIDMode="Static"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div align="center">
    <h2>&nbsp;</h2>
    <p align="center">
        <asp:Login ID="Login1" runat="server" 
            BorderStyle="Solid" BorderWidth="3px" BorderPadding="5" 
            DestinationPageUrl="~/tnp_proposals.aspx" DisplayRememberMe="False" 
            onauthenticate="Login1_Authenticate">
            <TextBoxStyle Width="150px" />
        </asp:Login>
    </p>
    </div>
</asp:Content>
