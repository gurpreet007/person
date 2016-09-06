<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Disp.aspx.cs" Inherits="Disp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style>
        #wrapper {margin:auto; text-align:center}
        #header {background-color:Silver; width: 90%; margin: auto; color:White; text-align:center; padding:5px}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="wrapper">
        <div id="header">
            <asp:Label ID="lblHeading" runat="server" Text="Upload Disciplinary Cases CSV"></asp:Label>
        </div>
        <div id="lastUp">
            <asp:Label ID="lblLastAt" Text="" runat="server" />
            <asp:LinkButton ID="lnkLastEntries" Text="" runat="server" 
                onclick="lnkLastEntries_Click" />
        </div>
        <div id="divUpload" runat="server">
            <p>
                <asp:FileUpload ID="fupCSV" runat="server" />
            </p>
            <p>
                <asp:Button ID="btnUpload" runat="server" Text="Upload CSV" 
                    onclick="btnUpload_Click" />
            </p>
        </div>
        <div id="divStatus" runat="server" visible="true">
            <p>
                <asp:Label ID="lblStatusText" runat="server" Text="Status: "></asp:Label>
                <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="lblSuccessText" runat="server" Text="Success: "></asp:Label>
                <asp:LinkButton ID="lnkSuccess" runat="server" onclick="lnkSuccess_Click">N/A</asp:LinkButton>
                <br />
                <asp:Label ID="lblErrorText" runat="server" Text="Error: "></asp:Label>
                <asp:LinkButton ID="lnkError" runat="server" onclick="lnkError_Click">N/A</asp:LinkButton>
                <br />
                <asp:GridView ID="gvErrors" runat="server">
                </asp:GridView>
            </p>
        </div>
    </div>
</asp:Content>

