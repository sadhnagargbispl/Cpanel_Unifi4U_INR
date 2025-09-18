<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Fundwithdrawal.aspx.cs" Inherits="Fundwithdrawal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var jq = $.noConflict();
        jq(document).ready(function () {
            jq(document).bind("contextmenu", function (e) {
                e.preventDefault();
            });
            jq(document).keydown(function (e) {

                if (e.which === 123) {
                    return false;
                }
                if (e.which === 116) {
                    return false;
                }
                if (e.ctrlKey && e.which === 82) {

                    return false;

                }


            });




        });
        function pageLoad(sender, args) {

            jq(document).ready(function () {

                jq("#aspnetForm").validationEngine('attach', { promptPosition: "topRight" });
            });

            jq(document).ready(function () {
                jq("#CmdSave1").click(function () {
                    // Your JavaScript code here
                });
            });


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
    <script type="text/javascript">
        //function isNumberKeypoint(evt) {
        //    var charCode = (evt.which) ? evt.which : event.keyCode;

        //    // Allow digits and a decimal point
        //    if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46)
        //        return false;

        //    return true;
        //}
        function isNumberKeypoint(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Withdrawal
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <div class="centered">
                        <div class="col-md-6">
                            <div class="form-group" style="display: none;">
                                <label for="inputdefault">
                                    Member Id <span style="color: Red;">*</span></label>
                                <asp:TextBox ID="TxtFrxInfra" runat="server" CssClass="form-control" class="form-control validate[required]"></asp:TextBox>
                            </div>
                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                            <asp:Label ID="Label1" runat="server" ForeColor="#D11F7B"></asp:Label>
                            <div class="col-xl-12">
                                <div class="form-group mb-3">
                                    <label for="inputdefault">
                                        Wallet Address <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                    <asp:TextBox ID="TxtWalletAddres" runat="server" Enabled="false" CssClass="form-control"
                                        class="form-control "></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-12">
                                <div class="form-group mb-3">
                                    <label for="inputdefault">
                                        Wallet Type<span style="color: Red;">*</span></label>
                                    <asp:DropDownList ID="ddlWalletType" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="ddlWalletType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="LblWithdrawalConditonIncome" runat="server" ForeColor="red" Text="" Visible="false"></asp:Label>
                                </div>
                            </div>
                            <div class="col-xl-12">
                                <div class="form-group mb-3">
                                    <label for="inputdefault">
                                        Available Fund <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                    <asp:TextBox ID="TxtCredit" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-12">
                                <div class="form-group mb-3">
                                    <label for="inputdefault">
                                        Requested Amount <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                    <asp:TextBox ID="TxtReqAmt" runat="server" MaxLength="8" CssClass="form-control validate[required,custom[number]]"
                                        AutoPostBack="true" OnTextChanged="TxtReqAmt_TextChanged" onkeypress="return isNumberKeypoint(event);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-12">
                                <div class="form-group mb-3">
                                    <label for="inputdefault">
                                        Standard Deduction <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                    <asp:TextBox ID="TxtTds" runat="server" Visible="true" CssClass="form-control validate[required]"
                                        ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-12">
                                <div class="form-group mb-3">
                                    <label for="inputdefault">
                                        Final Withdrawal Amount <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                    <asp:TextBox ID="txtwithdrawls" runat="server" MaxLength="8" CssClass="form-control validate[required]"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-12">
                                <div class="form-group mb-3">
                                    <asp:Button ID="BtnSubmit" runat="server" Text="Send Otp" class="btn btn-warning btn-sm p-2" OnClick="BtnSubmit_Click" />
                                </div>
                            </div>
                            <div runat="server" id="divotp" visible="false">

                                <div class="col-xl-12">
                                    <div class="form-group mb-3">
                                        <label for="inputdefault">
                                            Enter OTP Sent on your E-mail Id.<span style="color: Red; font-weight: bold; font-size: 1.4em">*</span>
                                        </label>
                                        <asp:TextBox ID="TxtOtp" CssClass="form-control validate[required]" runat="server"
                                            autocomplete="off" placeholder="Enter OTP"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="TxtOtp"
                                            runat="server" ValidationGroup="Save">Opt Required
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <br />
                            </div>
                            <div class="form-group">
                                <asp:Button ID="BtnOtp" runat="server" Text="Submit" class="btn btn-warning btn-sm p-2"
                                    Visible="false " ValidationGroup="Save" OnClick="BtnOtp_Click" />
                                <asp:Button ID="ResendOtp" runat="server" Text="Resend Otp" class="btn btn-warning btn-sm p-2"
                                    Visible="false " OnClick="ResendOtp_Click" />
                            </div>
                        </div>
                        <div class="col-md-6" runat="server" id="DivTermsAndCondition" visible="false">
                            <h6>Terms And Condition</h6>
                            <ul>
                                <li style="color: #e2a03f; font-weight: bold">Minimum Request Amount <% =Session["FMinwithdrawl"]%>.</li>
                                <li style="color: #e2a03f; font-weight: bold">Maximum Request Amount <% =Session["MaxWithdrawl"]%>.</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
