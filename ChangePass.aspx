<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChangePass.aspx.cs" Inherits="ChangePass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<script type="text/javascript" src="assets/jquery.min.js">
    </script>--%>
    <%--   <script type="text/javascript" src="js/plugins/jquery/jquery.min.js"></script>--%>
    <%-- <script type="text/javascript" src="assets/jquery.validationEngine-en.js"></script>

    <script type="text/javascript" src="assets/jquery.validationEngine.js"></script>

    <link href="assets/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />--%>
    <%--<script type="text/javascript">
        function pageLoad(sender, args) {
            $(document).ready(function() {

                $("#aspnetForm").validationEngine('attach', { promptPosition: "topRight" });
            });

            $("#<%=BtnUpdate.ClientID %>").click(function() {

                var valid = $("#aspnetForm").validationEngine('validate');
                var vars = $("#aspnetForm").serialize();
                if (valid == true) {
                    return true;

                } else {
                    return false;
                }
            });
        }   


    </script>--%>
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Change Password                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <asp:Label ID="lblErrorMessage" runat="server" Text="" CssClass="error-message"></asp:Label>


                    <div class="centered">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="inputdefault">
                                    Old Password<span class="red">*</span></label>
                                <asp:TextBox ID="oldpass" class="form-control" TextMode="Password"
                                    runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="oldpass"
                                    runat="server">Old Password can't left blank</asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    New Password<span class="red">*</span></label>
                                <asp:TextBox ID="pass1" TextMode="Password" runat="server" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="pass1"
                                    runat="server" ErrorMessage="RequiredFieldValidator"> New Password Can't left blank</asp:RequiredFieldValidator>
                                <%--<asp:RegularExpressionValidator ID="regex" runat="server" ControlToValidate="Pass1"
                                                    ErrorMessage="Password Length Minimum 6 or Maximum 20 characters" ValidationExpression="^[0-9A-Za-z_]{6,20}$"></asp:RegularExpressionValidator>--%>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    Confirm Password<span class="red">*</span></label>
                                <asp:TextBox ID="pass2" class="form-control"
                                    TextMode="Password" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="pass1"
                                    runat="server" ErrorMessage="RequiredFieldValidator">confirm New Password can't left blank</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" ControlToValidate="Pass1" ControlToCompare="Pass2"
                                    Type="String" Operator="Equal" Text="Passwords must match!" runat="Server" />
                            </div>
                            <div class="form-group">
                                <asp:Button ID="BtnUpdate" runat="server" Text="Submit" class="btn btn-primary" OnClick="BtnUpdate_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
