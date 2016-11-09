<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SMS.aspx.cs" Inherits="SMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <p>
        <asp:Label ID="lbl_txtDump" runat="server" Text="Enter/Paste Comma Separated Phone Numbers:"></asp:Label>
        <br />
        <asp:TextBox ID="txtDump" runat="server" TextMode="MultiLine"></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="btnLoadNums" runat="server" Text="Load Numbers" 
            onclick="btnLoadNums_Click" />
        <br />
        <asp:ListBox ID="lstSMS" runat="server"></asp:ListBox>

    </p>
    <p>
        <asp:Label ID="lbl_txtMessage" runat="server" Text="Enter/Paste SMS Message:"></asp:Label>
        <br />
        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine"></asp:TextBox>        
    </p>
    
    <p>
        <asp:Button ID="btnSend" runat="server" Text="Send SMS" 
            onclick="btnSend_Click" />
        <br />
    </p>
    
</asp:Content>

