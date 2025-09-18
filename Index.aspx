<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.4/Chart.js"></script>--%>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <style>
        .tblpadding {
            padding-top: 5px !important;
            padding-bottom: 0px !important;
        }

        .tblpadding1 {
            padding-top: 5px !important;
            padding-bottom: 0px !important;
            text-align: right;
        }

        .infocolor {
            background-color: #ff2b3f;
        }
    </style>
    <style type="text/css">
        .infoclr {
            background-color: #ffe3ed;
        }

        .backclr {
            background-color: #efefef;
        }

        .Background {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .Popup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 500px;
            height: 550px;
        }

        .lbl {
            font-size: 16px;
            font-style: italic;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function copyText() {

            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_lblLink");
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
    <script type="text/javascript">
        function copyText1() {
            debugger;
            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_lblLink1");
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
    <div class="row">
        <div class="col-md-8">
            <div class="row">
                <div class="col-md-4">

                    <!-- Profile pic -->
                    <div class="clearfix gen-profile-box">
                        <div class="profile-bar clearfix" style="margin-bottom: 0px;">
                            <div class="clearfix">
                                <div class="profile-bar-simple red-border clearfix">
                                    <center>
                                        <h6>MY Profile
                                        </h6>
                                    </center>
                                </div>
                            </div>
                            <div style="height: 0px; overflow: hidden">
                                <input type="file" name="ctl00$ContentPlaceHolder1$fuMember" id="ctl00_ContentPlaceHolder1_fuMember"
                                    onchange="LoadingFile(this);">
                            </div>
                            <div class="user-profile-pic">
                                <a id="aimage" runat="server">
                                    <asp:Image ID="Image2" runat="server" Style="border-radius: 30%; border: 3px solid #984485; box-shadow: 3px 1px 0px #ddd"
                                        class="rounded-circle" />
                                </a>
                                <ajaxToolkit:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panl1" TargetControlID="aImage"
                                    CancelControlID="Button2" BackgroundCssClass="Background">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none">
                                    <iframe style="width: 450px; height: 450px;" id="irm1" src="ProfilePicUploader.aspx"
                                        runat="server"></iframe>
                                    <br />
                                    <asp:Button ID="Button2" runat="server" Text="Close" />
                                </asp:Panel>
                            </div>
                            <div class="row">
                                <p>
                                    &nbsp;&nbsp;&nbsp;
                                </p>
                                <div class="profile-basic" style="width: 90%">
                                    <p>
                                        <b>
                                            <asp:Label ID="LblMemName" runat="server"></asp:Label></b>
                                        <br>
                                        <asp:Label ID="LblMemId" runat="server"></asp:Label>
                                    </p>
                                    <div class="col-md-12">
                                        <div class="row tab-box" style="margin: 17px 0;">
                                            <div class="col-lg-6 col-sm-12 tab-bg">
                                                <span class="tab-lable">Rank </span>
                                            </div>
                                            <div class="col-lg-6 col-sm-12">
                                                <span class="tab-field" id="SpanRank" runat="server"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-lg-12 col-sm-12">
                                <div id="dvUnpaidIncome">
                                    <div class="row">
                                        <div class="clearfix gen-profile-box" style="min-height: 273px;">
                                            <div class="profile-bar-simple blue-border">
                                                <center>
                                                    <h6>My Incentive</h6>
                                                </center>
                                            </div>
                                            <div class="profile-features-1">
                                                <asp:Repeater ID="RptIncome" runat="server">
                                                    <ItemTemplate>
                                                        <div class="col-md-12">
                                                            <div class="row tab-box" style="margin: 17px 0;">
                                                                <div class="col-lg-6 col-sm-12 tab-bg">
                                                                    <span class="tab-lable">
                                                                        <%#Eval("Name")%>
                                                                    </span>
                                                                </div>
                                                                <div class="col-lg-6 col-sm-12">
                                                                    <span class="tab-field">
                                                                        <%#Eval("Income")%>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix gen-profile-box">
                        <div class="profile-bar clearfix" style="margin-bottom: 0px;">
                            <div class="clearfix">
                                <div class="profile-bar-simple red-border clearfix">
                                    <center>
                                        <h6>Level Achieved
                                        </h6>
                                    </center>
                                </div>
                            </div>
                            <div class="row">
                                <p>
                                    &nbsp;&nbsp;&nbsp;
                                </p>
                                <div class="profile-basic" style="width: 90%; min-height: unset !important;">
                                    <p style="display: none;">
                                        <b>
                                            <asp:Label ID="Label1" runat="server"></asp:Label></b>
                                        <br>
                                        <asp:Label ID="Label2" runat="server"></asp:Label>
                                    </p>
                                    <div class="col-md-12">
                                        <div class="row tab-box" style="margin: 0; border: none;">

                                            <div class="col-lg-12 col-sm-12">
                                                <center>
                                                    <span id="LblOpenLevel" runat="server" style="font-weight: bold; font-size: 1.5em; text-align: center; color: #00254e;"></span>
                                                </center>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <div class="col-md-8">
                    <div class="clearfix gen-profile-box" id="Div1">
                        <div class="profile-features-1">
                            <div class="col-md-12">
                                <div id="Div10">
                                    <div class="profile-bar-simple red-border clearfix">
                                        <h6>Dash Board Quick Link
                                        </h6>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-12" align="center">
                                            <a id="A1" href="profile.aspx" runat="server">
                                                <img src="assets/1profile.png" width="60"></a>
                                            <% if (Session["ActiveStatus"].ToString() == "Y")
                                                {%>
                                            <a id="A7" href="ProfileUp.aspx" runat="server"></a>
                                            <% }%>
                                            <a id="A2" href="Mydirects.aspx" runat="server">
                                                <img src="assets/2mydirects.png" width="60">

                                            </a>
                                            <a id="A6" href="Complain.aspx"
                                                    runat="server">
                                                    <img src="assets/6complain.png" width="60">

                                            </a> 
                                           <%-- <a id="A3" href="#"
                                                        runat="server">
                                                        <img src="assets/package-purchase%20(4).png" width="60"></a>--%>
                                            <%--<img src="assets/package-purchase.png" />--%>
                                        </div>
                                        <div class="news-separator">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix gen-profile-box" id="Div2">
                        <div class="profile-features-1">
                            <div class="col-md-12">
                                <div id="Div3">
                                    <div class="profile-bar-simple red-border clearfix">
                                        <h6>OUR PORTAL
                                        </h6>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-12" align="center">

                                            <a id="A4" href="UtilityServicesRedirect.aspx"
                                                runat="server" target="_blank">
                                                <img src="assets/utility.png" width="60"></a>

                                            <a id="A9" href="StoreRedirect.aspx" runat="server" target="_blank" style="text-decoration: none;">
                                                <img src="assets/storeportal.png" width="60">
                                            </a>

                                            <a id="A10" href="PetroRedirect.aspx" runat="server" target="_blank">
                                                <img src="assets/petrocard.png" width="60">
                                            </a>
                                        </div>
                                        <div class="news-separator">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix gen-profile-box" id="Div11">
                        <div class="profile-features-1">
                            <div class="col-md-12">
                                <div id="Div12">
                                    <div class="profile-bar-simple red-border clearfix">
                                        <h6>Wallet Summary
                                        </h6>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <table id="table3" class="table table-hover table-mc-light-blue table-bordered">
                                                <thead>
                                                    <th style="text-align: center !important">Wallet Name
                                                    </th>
                                                    <th style="text-align: right !important">Credit
                                                    </th>
                                                    <th style="text-align: right !important">Debit
                                                    </th>
                                                    <th style="text-align: right !important">Balance
                                                    </th>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="gvBalance" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="text-align: center !important">
                                                                    <%#Eval("WalletName")%>
                                                                </td>
                                                                <td style="text-align: right !important">
                                                                    <%#Eval("Credit")%>
                                                                </td>
                                                                <td style="text-align: right !important">
                                                                    <%#Eval("Debit")%>
                                                                </td>
                                                                <td style="text-align: right !important">
                                                                    <%#Eval("Balance")%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix gen-profile-box" id="Div13">
                        <div class="profile-features-1">
                            <div class="col-md-12">
                                <div id="Div14">
                                    <div class="profile-bar-simple red-border clearfix">
                                        <h6>MY REFERRAL LINK
                                        </h6>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="table-columns">
                                            <div class="table-responsive">
                                                <table class="table table-hover table-bordered">
                                                    <tbody>
                                                        <tr>
                                                            <td id="Td1" style="color: red; font-weight: bold;" runat="server">REFERRAL
                                                            </td>
                                                            <td>
                                                                <a runat="server" target="_blank" id="aRfLink">
                                                                    <asp:Label ID="lblLink" Style="color: blue;" runat="server"></asp:Label></a>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Button ID="btncopy" CssClass="btn btn-primary" OnClientClick="return copyText();"
                                                                    runat="server" Text="Copy" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                    <tbody id="Tbody1" runat="server" style="display: none">
                                                        <tr>
                                                            <td style="color: red; font-weight: bold;">RIGHT
                                                            </td>
                                                            <td>
                                                                <a runat="server" target="_blank" id="aRfLink1">
                                                                    <asp:Label ID="lblLink1" Style="" runat="server"></asp:Label></a>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Button ID="Button1" CssClass="btn btn-primary" OnClientClick="return copyText1();"
                                                                    runat="server" Text="Copy" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix gen-profile-box" id="Div15">
                        <div class="profile-features-1">
                            <div class="col-md-12">
                                <div id="Div16">
                                    <div class="profile-bar-simple red-border clearfix">
                                        <h6>MEMBER STATISTICS
                                        </h6>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:GridView ID="gv" runat="server" CssClass="table table-bordered table-striped "
                                            AutoGenerateColumns="false" ShowHeader="true">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-CssClass="tblpadding" HeaderText="" HeaderStyle-BackColor="#ff2b3f">
                                                    <ItemTemplate>
                                                        <%#Eval("Name")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="tblpadding1" HeaderText="Direct" HeaderStyle-BackColor="#ff2b3f">
                                                    <ItemTemplate>
                                                        <%#Eval("LeftRegistration")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="tblpadding1" HeaderText="Indirect" HeaderStyle-BackColor="#ff2b3f">
                                                    <ItemTemplate>
                                                        <%#Eval("RightRegistration")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="tblpadding1" HeaderText="Total" HeaderStyle-BackColor="#ff2b3f">
                                                    <ItemTemplate>
                                                        <%#Eval("TotalRegistration")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

        <%--            <div class="clearfix gen-profile-box" id="divPetrocard" runat="server" visible="false">
                        <div class="profile-features-1">
                            <div class="col-md-12">
                                <div>
                                    <div class="profile-bar-simple red-border clearfix">
                                        <h6>Petro Card Sale
                                        </h6>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridPetroCardselect" runat="server" CssClass="table table-bordered table-striped "
                                            AutoGenerateColumns="true" ShowHeader="true">
                                            <Columns>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>--%>



                    <!-- Genex Business -->

                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="clearfix gen-profile-box" id="Div4">
                <div class="profile-features-1">
                    <div class="col-md-12">
                        <div id="Div5" style="height: 200px;">
                            <div class="profile-bar-simple red-border clearfix">
                                <h6>Information
                                </h6>
                            </div>
                            <div class="col-md-12">
                                <marquee behavior="scroll" direction="up" onmouseover="this.stop();" onmouseout="this.start();"
                                    style="height: 130px;">
                                    <asp:Repeater runat="server" ID="RptNews">
                                        <ItemTemplate>
                                            <p style="margin-bottom: 5px; margin-top: 5px;"><small><%#Eval("NewsDetail")%></small></p>
                                            <p><span class="news-date pull-left"><i class="fa fa-calendar" aria-hidden="true"></i><%#Eval("NewsDate")%> </span></p>
                                            <br />
                                            </p>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </marquee>
                                <div class="news-separator">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix gen-profile-box" id="dvVPRequest">
                <div class="profile-features-1">
                    <div class="profile-bar-simple red-border clearfix">
                        <h6>My Active Direct
                        </h6>
                    </div>
                    <div id="dvRequestDetails" class="col-md-12" style="font-size: 9px">
                        <div class="table-responsive">
                            <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="true" CssClass="table datatable"
                                AllowPaging="true" PageSize="10" OnPageIndexChanging="RptDirects_PageIndexChanging">
                                <Columns>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <p class="btn btn-default pull-right">
                        View More
                    </p>
                    <div class="news-separator">
                    </div>
                </div>
            </div>
            <%--<div class="clearfix gen-profile-box" id="dvVPRequest">
                <div class="profile-features-1">
                    <div class="profile-bar-simple red-border clearfix">
                        <h6>Income Status
                        </h6>
                    </div>
                    <div id="dvRequestDetails" class="col-md-12" style="font-size: 9px">
                        <div class="widget-stat card mb-3 p-3 color-4">
                            <div class="d-flex justify-content-between mt-3">
                                <div class="name">Stacking </div>
                                <div class="imgicon"><asp:Label ID="TotalInv" class="imgicon" runat="server" Text="0"></asp:Label>/<asp:Label ID="lblinvest" class="imgicon" runat="server" Text="0"></asp:Label></div>
                            </div>
                            <asp:Label ID="lbltotincome" runat="server" Text="0" Visible="false"></asp:Label>
                            <asp:Label ID="lblinv" runat="server" Text="0" Visible="false" Style="font-size: 9px;"></asp:Label>
                            <div class="progress mt-2 mb-2">
                                <div class="progress-bar" role="progressbar" style="width: 25%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100" id="divprogress" runat="server">
                                    <asp:Label ID="lblachieve" runat="server" Text="0"></asp:Label>
                                </div>
                            </div>
                            <div class="d-flex justify-content-between">
                                <div class="name">Bonuses</div>
                                <div class="imgicon">
                                    <asp:Label ID="lblbalance" runat="server" Text="0"></asp:Label>
                                </div>
                            </div>
                        </div>


                        <div class="widget-stat card mb-3 p-3 color-4">
                            <div class="d-flex justify-content-between mt-3">
                                <div class="name">InvestMent </div>
                                <div class="imgicon"><asp:Label ID="TotalInvestment" class="imgicon" runat="server" Text="0"></asp:Label>/<asp:Label ID="lblinvests" class="imgicon" runat="server" Text="0"></asp:Label></div>
                            </div>
                            <asp:Label ID="Label3" runat="server" Text="0" Visible="false"></asp:Label>
                            <asp:Label ID="lblinvs" runat="server" Text="0" Visible="false" Style="font-size: 9px;"></asp:Label>
                            <div class="progress mt-2 mb-2">
                                <div class="progress-bar" role="progressbar" style="width: 25%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100" id="divprogressbar" runat="server">
                                    <asp:Label ID="lblachieves" runat="server" Text="0"></asp:Label>
                                </div>
                            </div>
                            <div class="d-flex justify-content-between">
                                <div class="name">Bonuses</div>
                                <div class="imgicon">
                                    <asp:Label ID="lblbalances" runat="server" Text="0"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="news-separator">
                    </div>
                </div>
            </div>--%>
        </div>
        <div id="dvRLoader">
        </div>
    </div>

    <script src="popupassets/jquery-1.12.4.min.js"></script>

    <!-- Library - Js -->

    <script src="popupassets/popper.min.js"></script>

    <script src="popupassets/lib.js"></script>

    <script src="popupassets/jquery.flagstrap.min.js"></script>

    <script type="text/javascript" src="popupassets/jquery.themepunch.tools.min.js"></script>

    <script type="text/javascript" src="popupassets/jquery.themepunch.revolution.min.js"></script>

    <script src="js/functions1.js"></script>

    </div>
    <asp:Repeater ID="RptPopup" runat="server">
        <ItemTemplate>
            <%#Eval("DivStart")%>
        </ItemTemplate>
    </asp:Repeater>

    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <script type="text/javascript" src="popupassets/jquery.fancybox.pack.js"></script>

    <script type="text/javascript" src="popupassets/jquery.fancybox.pack1.js"></script>

    <link rel="stylesheet" href="popupassets/jquery.fancybox.css" type="text/css" media="screen" />

    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>

    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $(".fbox").fancybox({
                openEffect: 'elastic',
                closeEffect: 'elastic'
            });
        });
    </script>

    <script type="text/javascript">

        var delay = 1000;

        setTimeout(function () {

            $(document).ready(function () {
                $("#First").fancybox({
                    helpers: {

                    },

                    'afterClose': function () {
                        $("#Second").fancybox().trigger("click");
                    }
                }).trigger('click');

            });
        }, delay);


    </script>

    <script type="text/javascript" src="assets/jquery.dataTables.min.js"></script>

    <script type="text/javascript" src="assets/tableExport.js"></script>

    <script type="text/javascript">

        function pageLoad(sender, args) {


            $(document).ready(function () {
                $('#customers2').DataTable();

            });
        }
    </script>

    <script type="text/javascript" src="https://canvasjs.com/assets/script/jquery-1.11.1.min.js"></script>

    <script src="https://canvasjs.com/assets/script/canvasjs.min.js"></script>

</asp:Content>
