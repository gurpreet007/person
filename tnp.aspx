<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="tnp.aspx.cs" Inherits="frmproposal" ClientIDMode="Static"%>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style>
        #wrapper {margin:auto; text-align:center}
        #header {background-color:Silver; width: 90%; margin: auto; color:White; text-align:center; padding:5px}
        #divchoose {display: inline-block;  width:30%; padding:5px; background-color:White; color:Black; text-align:left; padding-left:30px; border-right-style:dotted; border-right-width:1px}
        #divpropose {display: inline-block; width:50%; padding:5px; background-color:White; color:Black; vertical-align:top; height:100%}
        #grid {margin: auto; width:90%; clear: both; padding:5px; background-color:White; color:Black}
        #options {margin: auto; width:90%; clear: both; padding:5px; background-color:Silver; color:White; border-bottom-style:dotted}
        #prop_controls {margin: auto; width:90%; clear: both; padding:5px; background-color:Silver; color:White; border-bottom-style:dotted}
        #cancel_order {margin: auto; width:90%; clear: both; padding:5px; background-color:Silver; color:White; border-bottom-style:dotted}
        #saveactions {margin: auto; width:90%; clear: both; padding:5px; background-color:Silver; color:White; border-bottom-style:dotted}
        #editbignote {margin: auto; width:90%; clear: both; padding:5px; background-color:Silver; color:White; border-bottom-style:dotted}
        #prev_controls {margin: auto;width:90%; clear: both; padding:5px; background-color:Silver; color:White; border-bottom-style:dotted}
        #genoo_controls {margin: auto;width:90%; clear: both; padding:5px; background-color:Silver; color:White;border-bottom-style:dotted}
        #priv_controls {margin: auto;width:90%; clear: both; padding:5px; background-color:Silver; color:White}
        #otp_controls {margin: auto;width:90%; clear: both; padding:5px; background-color:Silver; color:White;border-bottom-style:dotted}
        #hidd_fields {margin: auto;width:90%; clear: both; padding:5px; background-color:White; color:Black}
        #imgEmpPhoto {width: 138px; height: 170px}
        #grid {overflow:auto}
        #drpLocs {width:80%;max-width:90%; overflow:hidden}
        #txtPropLine {width:80%; height:40px}
        #txtOoNum{width:150px;}
        #txtEndorsNo{width:150px;}
        #txtOTP{width:60px}
        #lnkExport {margin-left:10px; margin-right:60px}
        #lnkImport {margin-right:10px}
        #locatePost{border-style:solid;border-width:1px}
        input[type=text].divsave {width:80px;}
        input[type=text].divpropose {width:80%;}
        textarea {width:80%;}
        .msg {color:Red}
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="wrapper">
        <div id="header">
            <asp:Label ID="lblProposalName" runat="server" Text="Label"></asp:Label>
        </div>
        <div id="divchoose" runat="server">
            <h2>Choose</h2>
            <p>
                <asp:TextBox ID="txtEmpid" runat="server"></asp:TextBox>
                <asp:Button ID="btnSelNewID" runat="server" Text="Show Info" onclick="btnSelNewID_Click"/>
                <br />
                <asp:Label ID="lblMsgNew" class="msg" runat="server" Text=""></asp:Label>
                <br />
                <asp:DropDownList ID="drpOfficer" runat="server"> </asp:DropDownList>
                <asp:Button ID="btnSelOutID" runat="server" Text="Show Info" onclick="btnSelOutID_Click"/>
                <br />
                <asp:Label ID="lblMsgOut" class="msg" runat="server" Text=""></asp:Label>
            </p>
            <strong>Name: </strong>
            <asp:Label ID="lblInfoName" runat="server" Text=""></asp:Label>
            <br />
            <strong>Desg: </strong>
            <asp:Label ID="lblInfoDesg" runat="server" Text=""></asp:Label>
            <br />
            <strong>Loc: </strong>
            <asp:Label ID="lblInfoWLoc" runat="server" Text=""></asp:Label>
            <br />
            <strong>Mapping: </strong>
            <asp:Label ID="lblInfoPCLoc" runat="server" Text=""></asp:Label>
            <br />
            <asp:Image ID="imgEmpPhoto" runat="server" />
            <p>
                <asp:Button ID="btnTransfer" runat="server" Text="Transfer" onclick="btnTransfer_Click"/>
                <asp:Button ID="btnPromote" runat="server" Text="Promote" onclick="btnPromote_Click"/><br />
                <asp:Button ID="btnChangePC" runat="server" Text="Change PC" 
                    onclick="btnChangePC_Click" style="height: 26px" /><br />
                <asp:Button ID="btnCanOrd" runat="server" Text="Cancel Order" 
                    onclick="btnCanOrd_Click" />
            </p>
        </div>
        <div id="divpropose"  runat="server">
            <h2>Propose</h2>
            <asp:Label ID="lblInfo" runat="server" Text="Transfer 'Name' to:"></asp:Label>
            <br />
            <asp:DropDownList ID="drpFilter" runat="server" AutoPostBack="true" onselectedindexchanged="drpFilter_SelectedIndexChanged">
                <asp:ListItem Value="A">Show All Posts</asp:ListItem>
                <asp:ListItem Value="E">Show Empty</asp:ListItem>
                <asp:ListItem Value="F">Show Filled</asp:ListItem>
                <asp:ListItem Value="S">Show Special</asp:ListItem>
                <asp:ListItem Value="H">Show Higher Posts</asp:ListItem>
            </asp:DropDownList>
            <asp:CheckBox ID="cbOwnInterest" runat="server" Text="Own Interest"/> 
            <asp:LinkButton ID="lnkLocatePost" Text="Locate Post" runat="server" />
            <asp:LinkButton ID="lnkPastePost" Text="Paste Post" runat="server" 
                onclick="lnkPastePost_Click" />
            <br />
            <asp:TextBox ID="txtLocFilter" runat="server" AutoPostBack="True" CssClass="divpropose unwatermarked" ontextchanged="txtLocFilter_TextChanged"></asp:TextBox>
            <br />
            <asp:DropDownList ID="drpLocs" runat="server" AutoPostBack="true" class="divpropose" onselectedindexchanged="drpLocs_SelectedIndexChanged"></asp:DropDownList>
            <br />
            <asp:TextBox ID="txtCLoc" runat="server" AutoPostBack="True" CssClass="divpropose unwatermarked"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtCDesg" runat="server" CssClass="divpropose unwatermarked"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="divpropose unwatermarked"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtPrvComment" runat="server" CssClass="divpropose unwatermarked"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtSno" runat="server" CssClass="divpropose unwatermarked"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtNewEmpid" runat="server" CssClass="divpropose unwatermarked" Visible=false></asp:TextBox>
            <br />
            <asp:TextBox ID="txtDispLeft" runat="server" TextMode="MultiLine" CssClass="divpropose unwatermarked"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtDispRight" runat="server" TextMode="MultiLine" CssClass="divpropose unwatermarked"></asp:TextBox>
            <br />
            <asp:TextBox ID="txtSysRemarks" runat="server" TextMode="MultiLine" CssClass="divpropose unwatermarked"></asp:TextBox>
            <br />
            <asp:Button ID="btnSelProposed" runat="server" Text="Select" onclick="btnSelProposed_Click"/>
        </div>
        <div id="grid">
            <asp:GridView ID="gvProposals" runat="server" HorizontalAlign="Left" 
                AutoGenerateEditButton="True" AutoGenerateDeleteButton="True" 
                Font-Size="Small" Width="100%" onrowdeleting="gvProposals_RowDeleting" 
                onrowediting="gvProposals_RowEditing">
                <HeaderStyle BackColor="Silver" ForeColor="White" />
            </asp:GridView>
        </div>
        <div id="options">
            <asp:LinkButton ID="lnkOnSaveAct" runat="server" onclick="lnkOnSaveAct_Click">4 On Save Actions</asp:LinkButton>
            <asp:Button Text="Auto Arrange" ID="btnAutoArrange" runat="server" 
                onclick="btnAutoArrange_Click" />
            <asp:LinkButton ID = "lnkExport" Text="Export Entries" runat="server" 
                onclick="lnkExport_Click" />
            <asp:FileUpload ID="FileUploader" runat="server" Width="200px" />
            <asp:LinkButton ID = "lnkImport" Text="Import Entries" runat="server" 
                onclick="lnkImport_Click" />
        </div>
        <div id="cancel_order" runat="server" visible=false>
            <asp:DropDownList ID="drpCanOrders" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnCanOrder" runat="server" Text="Cancel This Order" 
                onclick="btnCanOrder_Click" />
        </div>
        <div id="saveactions" runat="server" visible=false>
            <asp:DropDownList ID="drpSaveActions" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnDelSaveAction" runat="server" Text="Delete Action" 
                onclick="btnDelSaveAction_Click" />
        </div>
        <div id="prop_controls">
            <strong>Proposal Report: </strong>
            <asp:DropDownList ID="ddPropLineMode" runat="server" AutoPostBack="True"
                onselectedindexchanged="ddPropLineMode_SelectedIndexChanged">
                <asp:ListItem Value="A">Auto 1st Line</asp:ListItem>
                <asp:ListItem Value="M">Manual</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddPropLastLineMode" runat="server" AutoPostBack="True"
                onselectedindexchanged="ddPropLastLineMode_SelectedIndexChanged">
                <asp:ListItem Value="A">No Last Line</asp:ListItem>
                <asp:ListItem Value="M">Manual Last Line</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddApprover" runat="server" style="margin-left: 0px" Width="80px">
                <asp:ListItem Selected="True">CMD / PSPCL</asp:ListItem>
                <asp:ListItem>Dir. Admin. / PSPCL</asp:ListItem>
                <asp:ListItem>Dir. Dist. / PSPCL</asp:ListItem>
                <asp:ListItem>Dir. Comm. / PSPCL</asp:ListItem>
                <asp:ListItem>Dir. Finance / PSPCL</asp:ListItem>
                <asp:ListItem>Dir. Gen. / PSPCL</asp:ListItem>
             </asp:DropDownList>
             <asp:DropDownList ID="ddFSize" runat="server">
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem Selected="True">11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
                <asp:ListItem>13</asp:ListItem>
                <asp:ListItem>14</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
              </asp:DropDownList>
            <asp:Button ID="btnProp" runat="server" Text="PDF" 
                onclick="btnPrintProposal_Click"/>
            <asp:Button ID="btnPDFProp" runat="server" Text="PDF Pvt Comm" onclick="btnPDFProp_Click"/>
            <asp:Button ID="btnXLSProp" runat="server" Text="XLS" onclick="btnXLSProp_Click"/>
            <%--<br />--%>
            <asp:TextBox ID="txtPropLine" runat="server" TextMode="MultiLine" placeholder="First Line" Visible=false></asp:TextBox>
            <%--<br />--%>
            <asp:TextBox ID="txtPropLastLine" runat="server" TextMode="MultiLine" placeholder="Last Line" Visible=false></asp:TextBox>
        </div>
        <div id="prev_controls">
            <strong>Preview Report: </strong>
            <%--<a href="frmnotes.aspx" target="_blank">Insert Notes</a>--%>
            <asp:DropDownList runat="server" ID="ddBigNotes">
            </asp:DropDownList>
            <asp:LinkButton ID="lnkEditBigNote" runat="server" 
                onclick="lnkEditBigNote_Click">Edit BigNote</asp:LinkButton>
            <asp:DropDownList runat="server" ID="ddBigCC">
            </asp:DropDownList>
            <%--<a href="CC.aspx" target="_blank">Insert CC List</a>--%>
            <asp:DropDownList ID="ddFSizePrev" runat="server">
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem Selected="True">12</asp:ListItem>
                <asp:ListItem>13</asp:ListItem>
                <asp:ListItem>14</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
              </asp:DropDownList>
            <asp:Button ID="btnPreview" runat="server" Text="Preview" onclick="btnPreview_Click"/>
        </div>
        <div id="editbignote" runat="server" visible=false>
            <asp:TextBox ID="txtBigNote" runat="server" TextMode=MultiLine Height="100px" Width="60%"></asp:TextBox>
            <asp:Button ID="btnSaveBigNote" runat="server" Text="Save" 
                onclick="btnSaveBigNote_Click" />
        </div>
        <div id="genoo_controls">
            <strong>Generate O/o: </strong>
            <asp:TextBox ID="txtOoNum" runat="server" CssClass="divsave unwatermarked" 
                Width="100px"></asp:TextBox>
            <asp:TextBox ID="txtOoDate" runat="server" CssClass="divsave unwatermarked"></asp:TextBox>
            <asp:TextBox ID="txtEndorsNo" runat="server" CssClass="divsave unwatermarked" 
                Width="100px"></asp:TextBox>
            <asp:Button ID="btnGenOO" runat="server" Text="Generate O/o" 
                onclick="btnGenOO_Click"/>
       </div>
       <div id="otp_controls">
            <strong>OTP Controls:</strong>
            <asp:Button ID="btnSendOTP" runat="server" Text="Send OTP" 
                onclick="btnSendOTP_Click"/>
            <asp:TextBox ID="txtOTP" runat="server"></asp:TextBox>
            <asp:Button ID="btnCheckOTP" runat="server" Text="Enable Privileged Controls" 
                onclick="btnCheckOTP_Click" />
        </div>
        <div id="priv_controls" runat="server">
        <strong>Privileged Controls: </strong>
            <asp:Button ID="btnSave" runat="server" Text="Save" 
                onclick="btnSave_Click" OnClientClick="if ( !confirm('Are you sure you want to save this proposal?')) return false;"
                />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnUpload" runat="server" Text="Upload" 
                onclick="btnUpload_Click" 
                OnClientClick="if ( !confirm('Are you sure you want to upload this proposal?')) return false;"
                />
        </div>
        <div id="hidd_fields">
            <asp:HiddenField ID="hidEmpID" runat="server" />
            <asp:HiddenField ID="hidWDesgCode" runat="server" />
            <asp:HiddenField ID="hidWLoccode" runat="server" />
            <asp:HiddenField ID="hidPCRowNo" runat="server" />
            <asp:HiddenField ID="hidStatus" runat="server" />
            <asp:HiddenField ID="hidolddesgcode" runat="server" />
            <asp:HiddenField ID="hidsno" runat="server" />
            <asp:HiddenField ID="hidbranch" runat="server" />
            <asp:ScriptManager ID="ScriptManager1" runat="server"> </asp:ScriptManager>
            <cr:crystalreportsource ID="CrystalReportSource1" runat="server">
                <Report FileName="Reports\\rptposttrans.rpt"> </Report>
            </cr:crystalreportsource>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtEmpid').watermark('EMPID');
            $('#txtLocFilter').watermark('Filter');
            $('#txtCLoc').watermark('Working Location');
            $('#txtCDesg').watermark('Working Designation');
            $('#txtRemarks').watermark('Remarks');
            $('#txtPrvComment').watermark('Prv. Comments');
            $('#txtDispLeft').watermark('Left Text (Optional)');
            $('#txtDispRight').watermark('Right Text (Optional)');
            $('#txtSysRemarks').watermark('Place for System Remarks');
            $('#txtSno').watermark('Serial No. (Optional)').inputmask({ "mask": "9", "repeat": "3", "greedy": false });
            $('#txtOoNum').watermark('O/o Number');
            $('#txtOoDate').watermark('O/o Date');
            $('#txtEndorsNo').watermark('Endor. No.');
            $('#txtEventDate').watermark('Event Date');
            $('#txtNewEmpid').watermark('New EmpID (For JE to AE)');
            doAutoComp('txtCLoc', 'tnp.aspx/GetLocs2');
            doAutoComp('txtCDesg', 'tnp.aspx/GetDesgs2');
            $('#txtOoDate').datepicker(DT_PCKR_OPTS);
            $('#txtEventDate').datepicker(DT_PCKR_OPTS);
            var winLocateObjRef = null;
            $('#lnkLocatePost').click(function () {
                if (winLocateObjRef == null || winLocateObjRef.closed) {
                    myWin = window.open("./LocatePost.aspx", "LocatePost", "width=600, height=400, scrollbars=no, resizable=no, status=yes, dialog=yes, modal=yes");
                    myWin.focus();
                    return false;
                }
                else {
                    winLocateObjRef.focus();
                };
            });
        });
    </script>
</asp:Content>

