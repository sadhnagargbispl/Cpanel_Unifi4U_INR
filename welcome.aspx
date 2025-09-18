<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="welcome.aspx.cs" Inherits="welcome" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <%-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>

    <script type="text/javascript" language="javascript">
        function PrintDiv() {

            var divContents = document.getElementById("ctl00_ContentPlaceHolder1_dvContents").innerHTML;
            var printWindow = window.open('', '', 'height=200,width=400');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(divContents);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.print();

        }
    </script>--%>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content2" runat="server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Welcome
                </h6>
            </div>
            <div id="Div1" class="clearfix gen-profile-box">
                <div class="clearfix gen-profile-box" style="min-height: auto;">
                    <div class="profile-bar clearfix" style="background: #fff;">
                        <div class="clearfix">
                            <div class="centered">
                                <div class="row">
                                    <div class="col-md-12">
                                        <br />
                                        <div class="col-sm-6 pull-none">

                                            <div id="divpay" runat="server">
                                                <div class="form-group" style="display: none">
                                                    <label for="inputdefault">
                                                        <button type="button" class="btn btn-info btn-xs" id="BtnHome" runat="server">
                                                            Home</button>
                                                        <button type="button" class="btn btn-warning btn-xs" id="BtnPrint" runat="server"
                                                            onclick="javascript:PrintDiv();">
                                                            Print</button>
                                                        <%--  <button type="button" class="btn btn-danger btn-xs" id="BtnNewJoin" runat="server">
                                            New Joining
                                        </button>--%>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                                <div id="dvContents" runat="server">
                                                    <div class="container">
                                                        <div class="row">
                                                            <div class="col-md-12" style="display: none;">
                                                                <table>
                                                                    <tr>
                                                                        <td width="50%">
                                                                            <img src="<%= Session["LogoUrl"]%>" class="img-thumbnail" width="120px" height="120px">
                                                                        </td>
                                                                        <td width="50%">
                                                                            <h4>&nbsp;&nbsp; Welcome Letter
                                                                            </h4>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div class="col-md-10">
                                                                <%-- <h5>
                                                        Letter No- Map /<asp:Label ID="LblId" runat="server"></asp:Label>
                                                        /<asp:Label ID="LblYear" runat="server"></asp:Label>
                                                    </h5>--%>
                                                                <p style="margin-bottom: 0px">
                                                                    &nbsp;
                                                                </p>
                                                                <h3 class="text-danger" style="display: none;">
                                                                    <strong><em>Welcome Promoters to ever shine family of Strongold  !</strong>
                                                                </h3>
                                                                <h4 style="display: none;">Date : - <b>
                                                                    <asp:Label ID="lblDoj" runat="server"></asp:Label></b>
                                                                </h4>
                                                                <h4 style="display: none;">Dear <b>
                                                                    <asp:Label ID="LblName" runat="server"></asp:Label>
                                                                    (<asp:Label ID="LblIdno" runat="server"></asp:Label>)</b>
                                                                </h4>
                                                                <p style="display: none;">
                                                                    It gives us immense pleasure to welcome you as a Global Business Promoter of Strongold 
                                                                                            . We wish you great success in your new endeavor. You are starting a journey, where
                                                                                            you will be the promoter of every application of Strongold . You engage in our Service
                                                                                            and agree to be follow the rules &amp; regulations. Your Member Id as specified
                                                                                            below, is to be used in all your communication with the company.
                                                                </p>
                                                                <center>
                                                                    <h4>Your subscription details are as follows
                                                                    </h4>
                                                                </center>
                                                                <center>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <h4>Member Name :
                                                                                </h4>
                                                                            </td>
                                                                            <td>
                                                                                <h4>
                                                                                    <b>
                                                                                        <asp:Label ID="LblName1" runat="server"></asp:Label></b></h4>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <h4>Member Id :
                                                                                </h4>
                                                                            </td>
                                                                            <td>
                                                                                <h4>
                                                                                    <b>
                                                                                        <asp:Label ID="LblIdno1" runat="server"></asp:Label></b></h4>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="display: none;">
                                                                            <td>
                                                                                <h4>Sponsor Id :
                                                                                </h4>
                                                                            </td>
                                                                            <td>
                                                                                <h4>
                                                                                    <b>
                                                                                        <asp:Label ID="LblPlacementid" runat="server"></asp:Label></b></h4>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="display: none;">
                                                                            <td>
                                                                                <h4>Sponsor Name :
                                                                                </h4>
                                                                            </td>
                                                                            <td>
                                                                                <h4>
                                                                                    <b>
                                                                                        <asp:Label ID="LblPlacementName" runat="server"></asp:Label></b></h4>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <h4>Joining Date :
                                                                                </h4>
                                                                            </td>
                                                                            <td>
                                                                                <h4>
                                                                                    <b>
                                                                                        <asp:Label ID="lblDoj1" runat="server"></asp:Label></b></h4>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="display: none;">
                                                                            <td>
                                                                                <h4>Joining Amount :
                                                                                </h4>
                                                                            </td>
                                                                            <td>
                                                                                <h4>
                                                                                    <b>
                                                                                        <asp:Label ID="LblKitAmount" runat="server"></asp:Label></b></h4>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <h4>Password :
                                                                                </h4>
                                                                            </td>
                                                                            <td>
                                                                                <h4>
                                                                                    <b>
                                                                                        <asp:Label ID="lblPassw" runat="server"></asp:Label></b></h4>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td>
                                                                                <h4>T. Password :
                                                                                </h4>
                                                                            </td>
                                                                            <td>
                                                                                <h4>
                                                                                    <b>
                                                                                        <asp:Label ID="lblEPassw" runat="server"></asp:Label></b></h4>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </center>
                                                                <p style="display: none;">
                                                                    Once again welcome to Strongold  family and congratulations on taking your first step
                                                                                            towards a very prosperous and promising future. We are looking forward to have better
                                                                                            business prospective in your promotership.
                                                                </p>
                                                                <p style="display: none;">
                                                                    <br>
                                                                    Thank You!
                                                                </p>
                                                                <p class="text-success" style="display: none;">
                                                                    Best Regards,
                                                                                            <br />
                                                                    <strong>
                                                                        <%=Session["CompName"]%></strong>
                                                                </p>
                                                            </div>
                                                            <%--  <div class="col-md-12">
                                                    <div class="clearfix">
                                                    </div>
                                                    <hr>
                                                    <p>
                                                        &nbsp;
                                                    </p>
                                                </div>--%>
                                                        </div>
                                                    </div>
                                                    <%--</div>--%>
                                                </div>
                                                <hr>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
