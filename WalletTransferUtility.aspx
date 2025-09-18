<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WalletTransferUtility.aspx.cs" Inherits="WalletTransferUtility" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="content1">
    <script type="text/javascript" src="assets/jquery.min.js">
    </script>
    <script type="text/javascript" src="assets/jquery.validationEngine-en.js"></script>
    <script type="text/javascript" src="assets/jquery.validationEngine.js"></script>
    <link href="assets/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />
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

            jq("#<%=cmdSave1.ClientID %>").click(function () {


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
        .style1 {
            height: 15%;
            width: 358px;
        }

        .style2 {
            height: 2px;
            width: 304px;
        }

        .style3 {
            height: 2px;
            width: 358px;
        }
    </style>
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
            margin-left: -2%;
            margin-top: -2%;
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
    <style type="text/css">
        body {
            margin: 0;
            padding: 0;
        }

        .modal1 {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .center1 {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

            .center1 img {
                height: 128px;
                width: 128px;
            }
    </style>
    <script type="text/javascript">
        function myFunction() {

            document.getElementById("ctl00_ContentPlaceHolder1_feedbackpop1").style.display = "none";


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Wallet Transfer
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <div class="col-md-6">
                       
                        <div class="form-group" id="DiMemberId" runat="server" visible="false">
                            <label for="inputdefault">
                                Member Id</label>
                            <asp:TextBox ID="txtMemberId" runat="server" class="form-control validate[required]"
                                AutoPostBack="true"></asp:TextBox>
                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                            <asp:Label ID="lblFormno" runat="server" Visible="false"></asp:Label>
                        </div>
                        <div class="form-group" id="DivMemberName" runat="server" visible="false">
                            <label for="inputdefault">
                                Member Name</label>
                            <asp:HiddenField ID="HdnFormno" runat="server" />
                            <asp:Label ID="LblMobile" runat="server" Visible="false"></asp:Label>
                            <asp:TextBox ID="TxtMemberName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="inputdefault">
                                Wallet Type <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                            <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control " AutoPostBack="true" OnSelectedIndexChanged="ddlVoucherType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label for="inputdefault">
                                Available Balance <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                            <asp:TextBox ID="LblAvailableBal" runat="server" class="form-control validate[required]"
                                AutoPostBack="true" onkeypress="return isNumberKey(event);" ReadOnly="true"></asp:TextBox><asp:Label
                                    ID="Label1" runat="server" Visible="false"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label for="inputdefault">
                                Enter Requested Amount <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                            <asp:TextBox ID="txtAmount" runat="server" class="form-control validate[required]"
                                AutoPostBack="true" onkeypress="return isNumberKey(event);" OnTextChanged="txtAmount_TextChanged" Text="0"></asp:TextBox><asp:Label
                                    ID="LblAmount" runat="server" Visible="false"></asp:Label>
                        </div>

                        <div class="form-group">
                            <label for="inputdefault">
                                Wallet Password <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                            <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" CssClass="form-control validate[required]"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="cmdSave1" runat="server" Text="Submit" class="btn btn-primary" OnClick="cmdSave1_Click" />
                        </div>
                        <div class="form-group ">
                            <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lbladmincharge" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblotp" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblfinal" runat="server" Visible="false"></asp:Label>
                        </div>
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

