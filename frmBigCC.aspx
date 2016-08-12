<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmBigCC.aspx.cs" Inherits="frmCCs2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style>
        #wrapper {margin:auto; text-align:center; background-color:white}
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="wrapper">
        <div>
            <h2>CC Management</h2>
            <br />
        </div>
        <div id="div_options">
            <asp:DropDownList ID="ddOptions" runat="server">
                <asp:ListItem Text="Create" />
                <asp:ListItem Text="Open" />
                <asp:ListItem Text="Search" />
            </asp:DropDownList>
            <asp:Button ID="btnOptions" Text="Go" runat="server" 
                onclick="btnOptions_Click" />
        </div>
        <div id="div_create" runat="server">
            <h3>Create</h3>
            <asp:DropDownList runat="server" ID="ddCreate">
                <asp:ListItem Text="New" />
            </asp:DropDownList>
            <asp:Button ID="btnCreateStart" Text="Create" runat="server" 
                onclick="btnCreateStart_Click"/>
        </div>
        <div id="div_open" runat="server">
            <h3>Open</h3>
            <asp:DropDownList runat="server" ID="ddOpen">
                <asp:ListItem Text="Name1" />
                <asp:ListItem Text="Name2" />
            </asp:DropDownList>
            <asp:Button Text="Go" runat="server" ID="btnOpen" onclick="btnOpen_Click" />
        </div>
        <div id="div_search" runat="server">
            <h3>Search</h3>
            <asp:TextBox runat="server" ID="txtSearch" placeholder="Seach name, tag or data"/>
            <asp:Button Text="Search" runat="server" ID="btnSearch" 
                onclick="btnSearch_Click" />
            <asp:DropDownList runat="server" ID="ddResults">
            </asp:DropDownList>
            <asp:Button Text="Open" runat="server" ID="btnResultOpen" 
                onclick="btnResultOpen_Click" />
        </div>
        <div id="div_data" runat="server">
            <h3 id=heading runat="server">CC</h3>
            <div id="div_data1">
                <p>
                    <asp:Label ID="Label1" Text="Name:" runat="server" />
                    <asp:TextBox ID="CC_name" runat="server"/><br />
                </p>
                <p>
                    <asp:Label ID="Label2" Text="Tags" runat="server" />
                    <asp:TextBox ID="CC_tags" runat="server"/><br />
                </p>
                <p>
                    <asp:Label ID="Label3" Text="Data" runat="server" />
                    <asp:TextBox ID="CC_data" runat="server" TextMode=MultiLine Height="100px" 
                        Width="60%"/>
                </p>
            </div>
            <div id="div_data2">
                <asp:Button ID="btnSave" Text="Save" runat="server" onclick="btnSave_Click" />
                <asp:Button ID="btnSaveAsStart" Text="Save As" runat="server" 
                    onclick="btnSaveAsStart_Click" />
                <div id="div_data2_saveas" runat="server">
                    <asp:TextBox ID="txtSaveAsName" runat="server" placeholder="Save As Name"/>
                    <asp:Button ID="btnSaveAsGo" Text="Go" runat="server" 
                        onclick="btnSaveAsGo_Click" />
                </div>
                <asp:Button ID="btnMakeTemplate" Text="Make Template" runat="server" 
                    onclick="btnMakeTemplate_Click" />
                <asp:Button ID="btnDelete" Text="Delete" runat="server" 
                    OnClientClick="return confirm('Sure to Delete?');" onclick="btnDelete_Click"/>
            </div>
            <div>
                <asp:Label ID="lblmsg" runat="server" />
                <%--<asp:HiddenField runat="server" ID="hidSource" />--%>
                <asp:Label ID="hidSource" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
