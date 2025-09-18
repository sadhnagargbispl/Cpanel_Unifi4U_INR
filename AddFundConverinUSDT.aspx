<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddFundConverinUSDT.aspx.cs" Inherits="AddFundConverinUSDT" %>

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
        <div id="Div1" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Sell USDT-BEP20
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <center>
                        <div class="centered">
                            <div class="col-md-6">
                                <div class="img-responsive">
                                    <img id="ImgQrCode" src="images/QrCodeImage/QrCode.png" runat="server" alt="Image Alternative text"
                                        class="img-thumbnail" />
                                    <div class="qrcode">
                                        <b>
                                            <p id="SpnAddress" runat="server" style="color: darkred; font-size: 0.7em">
                                            </p>
                                        </b><a href="#" target="_blank" onclick="return copyText();">
                                            <asp:Button ID="Button1" class="btn btn-primary" runat="server" Text="Copy" Style="float: none !important; background-color: #eb7c2a; border: 1px solid #eb7c2a;" />
                                        </a>

                                        <br />
                                        <br />
                                        <div class="form-group">
                                            Enter Transaction Hash
                                            <asp:TextBox ID="TxtThxhash" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnRetry" class="btn btn-primary" runat="server" Text="After Deposit Click Here"
                                                Style="float: none !important; background-color: #eb7c2a; border: 1px solid #eb7c2a;" OnClick="btnRetry_Click" />
                                        </div>
                                    </div>
                                    <div class="button-click">
                                    </div>
                                    <br />
                                    <div class="d-block">
                                        <p style="font-size: 0.8em; color: red">
                                            <b>After enter hash click on the "After Deposit Click Here" button. AMOUNT will be deposit
                                                in your account.</b>
                                        </p>
                                        <p style="font-size: 0.8em; color: red">
                                            <b>Kindly Click Button after 60 Seconds of Payment confirmation on blockchain.</b>
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
                                    <asp:HiddenField ID="HdnApiRate" runat="server" />
                                </div>
                            </div>
                        </div>
                    </center>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

