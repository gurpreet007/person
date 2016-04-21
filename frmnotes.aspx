<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmnotes.aspx.cs" Inherits="frmnotes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
 <style type="text/css">
        body
        {
            background: #b6b7bc;
            font-size: .80em;
            font-family: "Helvetica Neue" , "Lucida Grande" , "Segoe UI" , Arial, Helvetica, Verdana, sans-serif;
            margin: 0px;
            padding: 0px;
            color: #696969;
            height: 198px;
        }
        .style2
        {
        }
        .unwatermarked
        {
            height: 18px;
            width: 148px;
        }
        .watermarked
        {
            height: 20px;
            width: 150px;
            padding: 2px 0 0 2px;
            border: 1px solid #BEBEBE;
            background-color: #F0F8FF;
            color: gray;
        }
        .style3
        {
            height: 49px;
        }
    </style>
    <%--<style type="text/css">
        .style1
        {
        }
        .style2
        {
            width: 129px;
        }
    </style>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<table style="width: 100%;">
        <tr>
            <td class="style2">
                &nbsp;
                </td>
            <td class="style1" colspan="2">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;
                </td>
            <td class="style1" colspan="2">
                <asp:Label ID="Label1" runat="server" Text="Available Notes" Width="150px"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Notes to be shown in reports"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2" colspan="3" rowspan="2">
                &nbsp;
                <asp:ListBox ID="lstAll" runat="server" Height="180px" Width="491px"></asp:ListBox>
            </td>
            <td>
                <br />
                <br />
                <asp:Button ID="btnAdd" runat="server" Height="26px" OnClick="btnAdd_Click" Text="----&gt;"
                    Width="69px" />
            </td>
            <td rowspan="2">
                <asp:ListBox ID="lstCC" runat="server" Height="180px" Style="margin-right: 0px" 
                    Width="301px">
                </asp:ListBox>
            </td>
            <td>
                <br />
                <br />
                <br />
                <asp:Button ID="btnUp" runat="server" Height="30px" OnClick="btnUp_Click" Text="^"
                    Width="37px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnRemove" runat="server" Height="26px" OnClick="btnRemove_Click"
                    Text="&lt;----" Width="69px" />
            </td>
            <td>
                <asp:Button ID="btnDown" runat="server" Height="32px" OnClick="btnDown_Click" Text="v"
                    Width="37px" />
            </td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;
                <asp:Button ID="btnAddnote" runat="server" Height="31px" OnClick="btnAddLoc_Click"
                    Text="Add Note" Width="118px" />
            </td>
            <td align="center" class="style3">
                <asp:Button ID="btnRemnote" runat="server" Height="28px" OnClick="btnRemLoc_Click"
                    Text="Remove Note" Width="118px" />
            </td>
            <td class="style3">
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btneditnote" runat="server" Height="30px" OnClick="btneditloc_Click"
                    Text="Edit Note" Width="114px" />
            &nbsp;&nbsp;&nbsp;
            </td>
            <td class="style3">
                &nbsp;
            </td>
            <td class="style3">
                &nbsp;
            </td>
            <td class="style3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2" align="right">
                &nbsp;
                <asp:Label ID="lbllstsno" runat="server"></asp:Label>
            </td>
            <td class="style1" colspan="2" rowspan="2">
                <asp:TextBox ID="txtNewnote" runat="server" OnTextChanged="txtNewnote_TextChanged"
                    ToolTip="Search Location" Width="299px" Height="42px" TextMode="MultiLine"></asp:TextBox>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;
            </td>
            <td class="style1" colspan="2">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnDone" runat="server" Height="27px" OnClick="btnDone_Click" Text="Done"
                    Width="69px" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>


</asp:Content>

