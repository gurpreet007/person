<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DataEntry.aspx.cs" Inherits="DataEntry" ClientIDMode="Static" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style3
        {
        }
        .style5
        {
        }
        .style7
        {
            height: 33px;
        }
        .style10
        {
            height: 19px;
        }
        .style11
        {
            height: 28px;
        }
        .style13
        {
            height: 26px;
        }
        .style16
        {
            height: 21px;
        }
        .style17
        {
            height: 22px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div align ="center">
    <table style="width: 81%; height: 55px;" frame="void">
        <tr>
            <td align="right" class="style10">
                    <asp:Label ID="lblEmpID" runat="server" Text="EmpID" Font-Bold="True" 
                        Font-Size="X-Small"></asp:Label>
                </td>
            <td align="left" class="style10" colspan="2">
                <asp:TextBox ID="txtEmpID" runat="server" AutoPostBack="True" Height="16px" 
                    MaxLength="6" ontextchanged="txtEmpID_TextChanged" Width="70px"></asp:TextBox>
            </td>
            <td align="right" class="style10" colspan="2">
                    <asp:Label ID="Label4" runat="server" Text="Designation" Font-Bold="True" 
                        Font-Size="X-Small"></asp:Label>
                </td>
            <td align="left" class="style10">
                    <asp:Label ID="lblDesg" runat="server" Font-Names="Arial" Font-Size="Small" 
                        Font-Strikeout="False" ForeColor="Black"></asp:Label>
                </td>
        </tr>
        <tr>
            <td align="right" class="style11">
                    <asp:Label ID="Label3" runat="server" Text="Name" Font-Bold="True" 
                        Font-Size="X-Small"></asp:Label>
                </td>
            <td align="left" class="style11" colspan="2">
                    <asp:Label ID="lblName" runat="server" Font-Bold="False" Font-Names="Arial" 
                        Font-Size="Small" Font-Strikeout="False" ForeColor="Black"></asp:Label>
                </td>
            <td align="right" class="style11" colspan="2">
                    <asp:Label ID="Label2" runat="server" Text="Father Name" Font-Bold="True" 
                        Font-Size="X-Small"></asp:Label>
                </td>
            <td align="left" class="style11">
                    <asp:Label ID="lblFName" runat="server" Font-Names="Arial" Font-Size="Small" 
                        Font-Strikeout="False" ForeColor="Black"></asp:Label>
                </td>
        </tr>
        <tr>
            <td align="right">
                    <asp:Label ID="Label5" runat="server" Text="Current Location" 
                    Font-Bold="True" Font-Size="X-Small"></asp:Label>
                </td>
            <td align="left" colspan="2">
                    <asp:Label ID="lblCLoc" runat="server" Font-Names="Arial" Font-Size="Small" 
                        Font-Strikeout="False" ForeColor="Black"></asp:Label>
                </td>
            <td align="right" colspan="2">
                <asp:Label ID="Label6" runat="server" Font-Bold="True" 
                    Text="Pay Charge Location" Font-Size="X-Small"></asp:Label>
            </td>
            <td align="left">
                    <asp:Label ID="lblPCLoc" runat="server" Font-Names="Arial" Font-Size="Small" 
                        Font-Strikeout="False" ForeColor="Black"></asp:Label>
                </td>
        </tr>
        <tr>
            <td align="right">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td align="left" colspan="5">
                                <asp:Label ID="lblmsg" runat="server" Font-Bold="True" 
                    Font-Size="Small" ForeColor="Red"></asp:Label>
                            </td>
        </tr>
        <tr>
            <td align="center" colspan="6">
                <asp:Panel ID="panEventGrid" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Panel ID="Panel1" runat="server" Height="280px" ScrollBars="Both" 
                                    Wrap="False" Width="900px">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="True" 
                                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                                        CellPadding="3" Font-Size="X-Small" Height="168px" Width="242px">
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnAdd" runat="server" height="26px" onclick="btnAdd_Click" 
                                    style="margin-bottom: 0px" Text="Add" width="93px" />
                                <asp:Button ID="btnEdit" runat="server" height="26px" onclick="btnEdit_Click" 
                                    Text="Edit" width="93px" />
                                <asp:Button ID="btnDelete" runat="server" height="26px" 
                                    onclick="btnDelete_Click" Text="Delete" width="93px" OnClientClick="return confirm('Are you sure to delete?');"/>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="6" class="style3">
                <asp:Panel ID="panEventData" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td align="right" class="style7">
                                <asp:Label ID="Label7" runat="server" Text="Event" Font-Bold="True" 
                                    Font-Size="X-Small"></asp:Label>
                            </td>
                            <td align="left" class="style7">
                                <%--gurpreet--%>
                                <%--<asp:ComboBox ID="drpEvent" runat="server" AutoCompleteMode="SuggestAppend" 
                                    DropDownStyle="DropDownList" height="16px" width="536px" 
                                    AutoPostBack="True" onselectedindexchanged="drpEvent_SelectedIndexChanged" 
                                    Font-Size="Smaller">
                                </asp:ComboBox>--%>
                                <asp:DropDownList ID="drpEvent" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="drpEvent_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label8" runat="server" Text="Date of Relieving" Font-Bold="True" 
                                    Font-Size="X-Small"></asp:Label>
                            </td>
                            <td align="left" class="style5">
                                <asp:TextBox ID="txtDoR" runat="server" height="16px" width="180px" 
                                    Font-Size="Smaller"></asp:TextBox>
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Small" 
                                    Text="Date of Joining"></asp:Label>
                                <asp:TextBox ID="txtDoJ" runat="server" Font-Size="Smaller" height="16px" 
                                    style="margin-bottom: 0px" width="180px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="X-Small" 
                                    Text="Designation"></asp:Label>
                            </td>
                            <td align="left" style="margin-left: 80px">
                                <asp:TextBox ID="txtdesg" runat="server" Font-Size="Smaller" Height="16px" 
                                    Width="536px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="X-Small" 
                                    Text="Posting Location"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtploc" runat="server" Font-Size="Smaller" Height="16px" 
                                    Width="536px" ontextchanged="txtploc_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style17">
                                <asp:Label ID="Label12" runat="server" Text="Pay Charge Location" Font-Bold="True" 
                                    Font-Size="X-Small"></asp:Label>
                            </td>
                            <td align="left" class="style17">
                                <asp:TextBox ID="txtpcloc" runat="server" height="16px" width="536px" ontextchanged="txtpcloc_TextChanged" 
                                    Font-Size="Smaller"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style16">
                                <asp:Label ID="Label16" runat="server" Text="Sanctioned Post" 
                                    Font-Bold="True" Font-Size="X-Small"></asp:Label>
                            </td>
                            <td align="left" class="style16">
                                <%--gurpreet--%>
                                <%-- <asp:ComboBox ID="drpSancDesg" runat="server" height="16px" width="536px" 
                                    Font-Size="Smaller">
                                </asp:ComboBox>--%>
                                <asp:DropDownList ID="drpSancDesg" runat="server" 
                                    onselectedindexchanged="drpEvent_SelectedIndexChanged" Width="536px">
                                </asp:DropDownList>
                                <asp:HiddenField runat="server" ID="hidSancPost" runat="server" 
                                    ClientIDMode="Static" Value="1233"/>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style13">
                                <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Size="X-Small" 
                                    Text="Office Order No"></asp:Label>
                            </td>
                            <td align="left" class="style13">
                                <asp:TextBox ID="txtOoNum" runat="server" Font-Size="Smaller" Height="16px" 
                                    Width="180px"></asp:TextBox>
                                <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="X-Small" 
                                    Text="Date of O/o"></asp:Label>
                                <asp:TextBox ID="txtOoDate" runat="server" Font-Size="Smaller" height="16px" 
                                    width="180px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                &nbsp;</td>
                            <td align="left">
                                <asp:Label ID="lblFilled" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                &nbsp;</td>
                            <td align="left">
                                <asp:Button ID="btnFinal" runat="server" onclick="btnFinal_Click" Text="Add" 
                                    Width="160px" />
                                <asp:Button ID="btnBack" runat="server" onclick="btnBack_Click" Text="Go Back" 
                                    Width="160px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" class="style3">
                &nbsp;</td>
            <td align="center" colspan="2">
                &nbsp;</td>
            <td align="center" colspan="2">
                &nbsp;</td>
        </tr>
    </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            function FillSancDesgs() {
                $("#drpSancDesg").empty();
                $.ajax({
                    type: "POST",
                    url: "DataEntry.aspx/GetSancDesgs2",
                    data: '{"strpcloc":"' + $('#txtpcloc').val() + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $.each(msg.d, function () {
                            $("#drpSancDesg").append($("<option></option>").val(this['Value']).html(this['Text']));
                        });
                        PutSelValue();
                    },
                    error: function (msg, strErr, thrownerr) {
                        alert("An error has occurred during processing your request." + thrownerr);
                        $("#drpSancDesg").append($("<option></option>").val(0).html('---Error---'));
                        PutSelValue();
                    }
                });
                PutSelValue();
            }
            function PutSelValue() {
                $('#hidSancPost').val($('#drpSancDesg').val());
                //alert($('#hidSancPost').val());
            }
            $('#txtOoDate').datepicker(DT_PCKR_OPTS);
            $('#txtDoR').datepicker(DT_PCKR_OPTS);
            $('#txtDoJ').datepicker(DT_PCKR_OPTS);
            doAutoComp('txtploc', 'DataEntry.aspx/GetLocs2');
            doAutoComp('txtpcloc', 'DataEntry.aspx/GetLocs2');
            doAutoComp('txtdesg', 'DataEntry.aspx/GetDesgs2');
            $('#txtpcloc').blur(FillSancDesgs);
            $('#drpSancDesg').change(PutSelValue);
        });
    </script>
</asp:Content>