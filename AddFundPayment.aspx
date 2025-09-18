<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddFundPayment.aspx.cs" Inherits="AddFundPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function copyText() {
            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_SpnAddress");
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
                alert('Address Copied.!');
            }
            catch (err) {
                alert('Unable To Copy Link');
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Add Fund <span>(Deposit Only USDT-BEP20)
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <center>
                        <div class="centered">
                            <div class="col-md-6">
                                <div class="img-responsive">
                                    <img id="ImgQrCode" src="" runat="server" alt="Image Alternative text" class="img-thumbnail" />
                                    <div class="qrcode">
                                        <b>
                                            <p id="SpnAddress" runat="server" style="color: darkred; font-size: 0.7em">
                                            </p>
                                        </b>
                                        <a  href="#" target="_blank" onclick="return copyText();">
                                            <asp:Button ID="Button1" CssClass="btn btn-warning btn-sm p-2" runat="server" Text="Copy"
                                                Style="float: none !important; background-color: #eb7c2a; border: 1px solid #eb7c2a;" />
                                        </a>
                                        <asp:Button ID="btnRetry" CssClass="btn btn-warning btn-sm p-2" runat="server" Text="After Deposit Click Here"
                                            Style="float: none !important; background-color: #eb7c2a; border: 1px solid #eb7c2a;" OnClick="btnRetry_Click" />
                                    </div>
                                    <div class="button-click">
                                    </div>
                                    <div class="d-block">
                                        <p style="font-size: 1em; color: red">
                                            <b>After Deposit click on the "After Deposit Click Here" button. AMOUNT will be deposit
                                                                    in your account.</b>
                                        </p>
                                        <p style="font-size: 1em; color: red">
                                            <b>Kindly click the button 3 minutes after payment confirmation on the blockchain.</b>
                                        </p>
                                    </div>
                                </div>
                                <div style="display: none">
                                    <asp:HiddenField ID="hdnvalueCoin" runat="server" />
                                    <asp:HiddenField ID="hdnStatus" runat="server" />
                                    <asp:HiddenField ID="hdntxnId" runat="server" />
                                    <asp:HiddenField ID="hdntxnout" runat="server" />
                                    <asp:HiddenField ID="hdnMessage" runat="server" />
                                    <asp:HiddenField ID="hdnPaymentId" runat="server" />
                                    <asp:HiddenField ID="OrderId" runat="server" />
                                    <asp:HiddenField ID="fromidno" runat="server" />
                                    <asp:HiddenField ID="privatekey" runat="server" />
                                    <asp:HiddenField ID="hdnresponse" runat="server" />
                                    <asp:HiddenField ID="amountreq" runat="server" />
                                    <asp:HiddenField ID="hdnPayID" runat="server" />
                                    <asp:HiddenField ID="hdncallbckurl" runat="server" />
                                    <asp:HiddenField ID="HdnFormno" runat="server" />
                                    <asp:HiddenField ID="HdnUrl" runat="server" />
                                    <asp:HiddenField ID="HdnAmountApi" runat="server" />
                                </div>
                            </div>
                        </div>
                    </center>
                </div>
            </div>
        </div>
    </div>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.4/jquery.min.js"></script>

    <script type="text/javascript">
        function blink_text() {
            $('.blink').fadeOut(500);
            $('.blink').fadeIn(500);
        }
        setInterval(blink_text, 1000);
    </script>

    <script type="text/javascript">
        $('.ca_copy').on('click', function () {
            debugger;
            copyToClipboard($(this).attr('data-tocopy'));
            let tip = $(this).find('.ca_tooltip.tip');
            let success = $(this).find('.ca_tooltip.success');
            success.show();
            tip.hide();
            setTimeout(function () {
                success.hide();
                tip.show();
            }, 5000);
        })
        function copyToClipboard(text) {
            debugger;
            if (window.clipboardData && window.clipboardData.setData) {
                return clipboardData.setData("Text", text);
            } else if (document.queryCommandSupported && document.queryCommandSupported("copy")) {
                var textarea = document.createElement("textarea");
                textarea.textContent = text;
                textarea.style.position = "fixed";
                document.body.appendChild(textarea);
                textarea.select();
                try {
                    return document.execCommand("copy");
                } catch (ex) {
                    console.warn("Copy to clipboard failed.", ex);
                    return false;
                } finally {
                    document.body.removeChild(textarea);
                }
            }
        }
    </script>

</asp:Content>

