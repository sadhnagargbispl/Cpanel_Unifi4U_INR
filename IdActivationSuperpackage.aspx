<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IdActivationSuperpackage.aspx.cs" Inherits="IdActivationSuperpackage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="assets/jquery.min.js">
    </script>

    <%--   <script type="text/javascript" src="js/plugins/jquery/jquery.min.js"></script>--%>

    <script type="text/javascript" src="assets/jquery.validationEngine-en.js"></script>

    <script type="text/javascript" src="assets/jquery.validationEngine.js"></script>

    <link href="assets/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script> --%>


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

    <style type="text/css">
        .feedbackform {
            width: auto;
            height: auto;
            position: absolute;
            top: 100px;
            left: 40%;
            z-index: 9999;
            display: block;
            padding: 15px;
            background: #fff;
            border-radius: 5px;
            border: 1px solid;
        }

            .feedbackform img {
                max-width: 150px;
                display: block;
            }

        .feedbackpop {
            background: url(images/blackbg2.png) repeat;
            position: fixed;
            width: 100%;
            height: 100%;
            display: block;
            z-index: 9999;
        }

        #closeicon a {
            background: url(images/close2.png) no-repeat;
            width: 55px;
            height: 55px;
            display: block;
            margin: -22px -30px 0 0;
            float: right;
            position: absolute;
            right: 0px;
        }

            #closeicon a:hover {
                background: url(images/close2_hover.png) no-repeat;
            }

        #feedbackwrap {
            width: 1000px;
            margin: 0 auto;
            position: relative;
        }

        @media ( max-width: 1000px ) {
            #feedbackwrap {
                width: 100%;
            }

            .feedbackform {
                left: 2%;
                right: 2%;
            }
        }
    </style>
    <script type="text/javascript">

        function checkWAmt() {
            debugger;
            var WAvlBal = '<%= Session["ServiceWallet"] %>';

            var Amt = document.getElementById("<%= txtAmount.ClientID %>").value;
            var Amount = (parseFloat(Amt) % 10);

            var rb = document.getElementById("<%=DDLPaymode.ClientID%>");
            var selectedValue = rb.value;
            if (selectedValue == "1") {
                if (parseFloat(Amt) > parseFloat(WAvlBal)) {
                    alert('Transfer amount could not be greater than available balance.');
                    document.getElementById("<%= txtAmount.ClientID %>").value = '0';
                    return false;
                }
                else if (Amount > 0) {
                    alert('Enter Amount only multiple of Tenth.');
                    document.getElementById("<%= txtAmount.ClientID %>").value = '0';

                    return false;

                }

                else {

                    return true;
                }
            }
            else {
                if (Amount > 0) {
                    alert('Enter Amount only multiple of Tenth.');
                    document.getElementById("<%= txtAmount.ClientID %>").value = '0';

                    return false;

                }

                else {

                    return true;
                }

            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Active with Package
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <div class="centered">
                        <div class="col-md-6">
                            <div class="form-group">
                                <h4>Available Balance:
                                </h4>
                                <h4>USDT Wallet:<span class="red" id="AvailableBal" style="color: Red" runat="server"></span>
                                </h4>
                                <h4>Airdrop Wallet:<span class="red" id="Availableairdrop" style="color: Red" runat="server"></span>
                                </h4>
                            </div>
                            <div class="form-group" id="DiMemberId" runat="server">
                                <label for="inputdefault">
                                    Member Id</label>
                                <asp:TextBox ID="txtMemberId" runat="server" class="form-control validate[required]"
                                    AutoPostBack="true" OnTextChanged="txtMemberId_TextChanged"></asp:TextBox>
                                <asp:Label ID="lblFormno" runat="server" Visible="false"></asp:Label>
                                <asp:HiddenField ID="hdnMacadrs" runat="server" />
                                <asp:HiddenField ID="HdnTopupSeq" runat="server" />
                                <asp:HiddenField ID="HdnMemberMacAdrs" runat="server" />
                                <asp:HiddenField ID="HdnMemberTopupseq" runat="server" />
                                <asp:HiddenField ID="MemberStatus" runat="server" />
                                <asp:HiddenField ID="hdnFormno" runat="server" />
                                <asp:HiddenField ID="hdnemail" runat="server" />
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <div class="form-group" id="DivMemberName" runat="server">
                                        <label for="inputdefault">
                                            Member Name</label>
                                        <asp:Label ID="LblMobile" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="TxtMemberName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtMemberId" EventName="TextChanged" />

                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="form-group" style="display: none">
                                <label for="inputdefault">
                                    Payment Type</label>
                                <asp:DropDownList ID="DDLPaymode" runat="server" class="form-control" AutoPostBack="true">
                                    <asp:ListItem Text="Wallet" Value="1"></asp:ListItem>

                                </asp:DropDownList>
                            </div>

                            <div class="form-group" id="DivCurrency" runat="server" visible="false">
                                <label for="inputdefault">
                                    Currency <span class="red">*</span></label>
                                <asp:DropDownList ID="ddlcurrency" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" id="Div1" runat="server">
                                <label for="inputdefault">
                                    Package</label>
                                <asp:DropDownList ID="CmbKit" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CmbKit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <asp:Label ID="LblAmount" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="LblAmountUse" runat="server" Visible="false"></asp:Label>

                            <div class="form-group" runat="server">
                                <label for="inputdefault">
                                    Request Amount <span class="red"><span style="color: #5c13b1 !important; font-weight: bolder; font-size: 0.9em;">*</span></span></label>
                                <asp:TextBox ID="TextBox1" runat="server" class="form-control" AutoPostBack="true" Text="0" onkeypress="return isNumberKey(event);" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    Amount (Airdrop Wallet)  <span class="red"><span style="color: #5c13b1 !important; font-weight: bolder; font-size: 0.9em;">(You Can Use Upto 25% Amount From Airdrop Wallet Here)</span></span></label>
                                <asp:TextBox ID="TxtPointWallet" runat="server" class="form-control" aria-describedby="emailHelp" placeholder="Enter Amount"
                                    AutoPostBack="true" ReadOnly="true" Text="0" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    Amount (USDT Wallet)  <span class="red"><span style="color: #5c13b1 !important; font-weight: bolder; font-size: 0.9em;">(You Can Use Remaining Amount From USDT Wallet Here)</span></span></label>
                                <asp:TextBox ID="txtAmount" runat="server" class="form-control" aria-describedby="emailHelp" placeholder="Enter Amount" 
                                    AutoPostBack="true" ReadOnly="true" Text="0" onkeypress="return isNumberKey(event);"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label for="inputdefault">Wallet Password</label>
                                <asp:TextBox ID="TxtTransPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Button ID="cmdSave1" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Validation" OnClick="cmdSave1_Click" />
                            </div>
                            <div class="form-group ">
                                <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

