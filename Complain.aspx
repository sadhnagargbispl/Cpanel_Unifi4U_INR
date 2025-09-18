<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Complain.aspx.cs" Inherits="Complain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        //$(document).ready(function() { $('[id$=chkSelectAll]').click(function() { $("[id$='chkSelect']").attr('checked', this.checked); }); });
        //    function reset() {
        //        $("[id$='chkSelect']").prop('checked', false);
        //    }

        function CountChar() {
            var SmsTxt = document.getElementById('<%=TxtDesc.ClientID%>').value;

            var cnt = SmsTxt.length;
            var smsNo;
            var cnt_;
            cnt_ = cnt;
            if (cnt > 500) {
                smsNo = cnt / 500;
                cnt = cnt % 500;
                cnt_ = cnt + '/' + parseInt(smsNo);
            }

            document.getElementById("remainingC").innerHTML = 500 - cnt_;
        }
    </script>

    <script type="text/javascript" src="assets/jquery.min.js">
    </script>

    <%--   <script type="text/javascript" src="js/plugins/jquery/jquery.min.js"></script>--%>

    <script type="text/javascript" src="assets/jquery.validationEngine-en.js"></script>

    <script type="text/javascript" src="assets/jquery.validationEngine.js"></script>

    <link href="assets/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var jq = $.noConflict();
        function pageLoad(sender, args) {
            jq(document).ready(function () {

                jq("#aspnetForm").validationEngine('attach', { promptPosition: "topRight" });
            });

            jq("#<%=BtnSubMit.ClientID %>").click(function () {


                var valid = jq("#aspnetForm").validationEngine('validate');
                var vars = jq("#aspnetForm").serialize();
                if (valid == true) {
                    return true;

                } else {
                    return false;
                }
            });




        }




    </script>

    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Raise Ticket
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <div class="centered">
                        <div class="clr">
                            <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        </div>
                        <div id="DivError" runat="server" visible="false">
                            <span id="spanError" runat="server"></span>
                        </div>
                        <div class="row" id="divSuccess" runat="server">
                            <div class="col-sm-12">
                                <asp:Label ID="LblCompalin" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="Lblgroup" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="col-sm-12">
                                <div class="col-sm-6">
                                    Username :
                                    <asp:TextBox ID="TxtDirectSeller" Enabled="false" runat="server" cssclass="form-control"
                                        PlaceHolder="Username" AutoPostBack="true" OnTextChanged="TxtDirectSeller_TextChanged"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-12" style="display: none;">
                                <div class="col-sm-3">
                                    Name : *
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="TxtName" Enabled="false" runat="server" class="form-control" PlaceHolder="Name"
                                        ValidationGroup="Save"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-12" style="display: none;">
                                <div class="col-sm-6">
                                    Mobile No : *
                                </div>
                                <div class="col-sm-6">
                                    <%--<input type="email" class="form-control" id="email" placeholder="Mobile No:">--%>
                                    <asp:TextBox ID="TxtMobl" Enabled="false" runat="server" class="form-control" PlaceHolder="Mobile No"
                                        ValidationGroup="Save"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-12" style="display: none;">
                                <div class="col-sm-6">
                                    Email ID : *
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="TxtEmail" Enabled="false" runat="server" class="form-control" PlaceHolder="Email Id"
                                        ValidationGroup="Save"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div class="col-sm-6">
                                    Nature of Grievance:
                                    <asp:DropDownList ID="CmbCmplntType" runat="server" placeholder="Nature of Grievance "
                                        class="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-12" style="display: none;">
                                <div class="col-sm-6">
                                    Subject : *
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="TxtSubject" runat="server" CssClass="form-control validate[required]"
                                        PlaceHolder="Subject" ValidationGroup="Save" Text="aa"></asp:TextBox>
                                    <%--  <input type="email" class="form-control" id="email" placeholder="Subject">--%>
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div class="col-sm-6">
                                    Description : *
                                    <asp:TextBox ID="TxtDesc" runat="server" TextMode="MultiLine" CssClass="form-control validate[required]"
                                        placeholder="Description" ValidationGroup="Save" onkeyup="CountChar();" MaxLength="500"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div class="col-sm-6">
                                    <br />
                                    <asp:Button ID="BtnSubMit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Save" OnClick="BtnSubMit_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Save" />
                    <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                    ControlToValidate="TxtSubject" ErrorMessage="Enter Subject" Display="None" />
                                &nbsp;
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                    ControlToValidate="TxtDesc" ErrorMessage="Enter Description" Display="None" />--%>
                    &nbsp;
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
