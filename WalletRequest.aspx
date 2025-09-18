<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WalletRequest.aspx.cs" Inherits="WalletRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="assets/jquery.min.js">
    </script>

    <%--   <script type="text/javascript" src="js/plugins/jquery/jquery.min.js"></script>--%>

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

            <%--jq("#<%=CmdSave1.ClientID %>").click(function() {


                var valid = jq("#aspnetForm").validationEngine('validate');
                var vars = jq("#aspnetForm").serialize();
                if (valid == true) {
                    return true;

                } else {
                    return false;
                }
            });--%>
        }


    </script>

    <script type="text/javascript">
        function copyText() {

            var range, selection, worked;
            var element = document.getElementById("ctl00_ContentPlaceHolder1_lblLink");
            if (document.body.createTextRange) {
                range = document.body.createTextRange();
                range.moveToElementText(element);
                range.select();
            } else if (window.getSelection) {
                selection = window.getSelection();
                range = document.createRange();
                range.selectNodeContents(element);
                selection.removeAllRanges();
                selection.addRange(range);
            }

            try {
                document.execCommand('copy');
                alert('link copied');
            }
            catch (err) {
                alert('unable to copy link');
            }
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card" style="padding: 20px;">
        <div class="clearfix gen-profile-box" style="min-height: auto;">
            <div class="profile-bar clearfix" style="background: #000;">
                <div class="clearfix">
                    <br>
                    <div class="row">
                        <div class="col-md-8">
                     <h6>Wallet Request</h6>
                            <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                                <div class="clearfix gen-profile-box" style="min-height: auto;">
                                     
                                    <div class="profile-bar clearfix" style="background: #000;">
                                      
                                        <div class="col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="inputdefault">Enter Amount<span class="red">*</span></label>
                                                <asp:TextBox runat="server" onkeypress="return isNumberKey(event);" MaxLength="8"
                                                    TabIndex="1" ID="TxtAmount" class="form-control validate[required]" Text="0"></asp:TextBox>
                                                <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                            </div>

                                            <div class="form-group">
                                                <label for="inputdefault">Select Paymode<span class="red">*</span></label>
                                                <asp:DropDownList ID="DdlPaymode" runat="server" AutoPostBack="true" CssClass="form-control"
                                                    TabIndex="2" OnSelectedIndexChanged="DdlPaymode_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="form-group" id="divDDno" runat="server" visible="false">
                                                <label for="inputdefault">
                                                    <asp:Label ID="LblDDNo" runat="server" Text="Draft/CHEQUE No. *"></asp:Label>
                                                </label>
                                                <asp:TextBox ID="TxtDDNo" onkeypress="return isNumberKey(event);" class="form-control validate[required]"
                                                    TabIndex="3" runat="server" MaxLength="16" AutoPostBack="true" OnTextChanged="TxtDDNo_TextChanged"></asp:TextBox>
                                            </div>

                                            <div class="form-group" id="divDDDate" runat="server">
                                                <label for="inputdefault">
                                                    <asp:Label ID="LblDDDate" runat="server" Text="Transaction Date *"></asp:Label>
                                                </label>
                                                <asp:TextBox ID="TxtDDDate" runat="server" class="form-control validate[required]" TabIndex="4"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtDDDate"
                                                    Format="dd-MMM-yyyy" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtDDDate"
                                                    ErrorMessage="Invalid Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>

                                                <div class="form-group" id="divImage" runat="server">
                                                    <label for="inputdefault">Scanned Copy:</label>
                                                    <asp:FileUpload runat="server" ID="FlDoc" class="form-control" TabIndex="7" />
                                                    <asp:Label ID="LblImage" runat="server" Visible="false"></asp:Label>
                                                </div>
                                            </div>

                                            <br />
                                            <asp:Button ID="BtnSaveDB" runat="server" Text="Confirmed" class="form-btn" OnClick="BtnSaveDB_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <!-- Genex Business -->
                            <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                                <div class="profile-bar-simple red-border clearfix">
                                    <h6></h6>
                                </div>
                                <div class="col-md-12">
                                    <img class="img-fluid" src="images/QrCodeImage/QrCode.png" alt="QR Code" width="320" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



</asp:Content>

