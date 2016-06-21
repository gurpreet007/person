<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="LocatePost.aspx.cs" Inherits="LocatePost" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8">
	<title>Locate Post</title>
</head>
<body>
    <form runat="server">
        <div id="wrapper">
            <asp:DropDownList runat="server" ID="ddDirect" AutoPostBack="true"
                onselectedindexchanged="ddDirect_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Label Text="" ID="lblLoccode" runat="server" />
            <br />
            <asp:DropDownList runat="server" ID="ddOrg" AutoPostBack="true" 
                onselectedindexchanged="ddOrg_SelectedIndexChanged">
            </asp:DropDownList>
            <br />
            <asp:DropDownList runat="server" ID="ddCircle" AutoPostBack="true" 
                onselectedindexchanged="ddCircle_SelectedIndexChanged">
            </asp:DropDownList>
            <br />
            <asp:DropDownList runat="server" ID="ddDiv" AutoPostBack="true" 
                onselectedindexchanged="ddDiv_SelectedIndexChanged">
            </asp:DropDownList>
            <br />
            <asp:DropDownList runat="server" ID="ddSubDiv" AutoPostBack="true" 
                onselectedindexchanged="ddSubDiv_SelectedIndexChanged">
            </asp:DropDownList>
            <br />
            <br />
            <asp:ListBox ID="ddPost" runat="server">
            </asp:ListBox>
            <br />
            <asp:Button ID="btnCopyPost" Text="Copy Post" runat="server" 
                onclick="btnCopyPost_Click" />
        </div>
    </form>
</body>
</html>