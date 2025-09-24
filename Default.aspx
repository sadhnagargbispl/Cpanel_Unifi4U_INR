<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>
        <%=Session["Title"].ToString ()%></title>
    <%--<title>
        Welcome To My Club Happiness </title>--%>

    <script type="text/javascript">
        window.history.forward();
        function noBack() {
            window.history.forward();
        }
    </script>

    <link href="LoginCss/css/style.css" rel="stylesheet" type="text/css" />
    <link href="LoginCss/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Kurale" rel="stylesheet" />
    <link rel="stylesheet" href="Login/css/flexslider-min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.3.0/css/font-awesome.css">

    <script type="text/javascript" src="highslide/highslide-full.js"></script>

    <link rel="stylesheet" type="text/css" href="highslide/highslide.css" />

    <script type="text/javascript">
        hs.graphicsDir = 'highslide/graphics/';
        hs.align = 'center';
        hs.transitions = ['expand', 'crossfade'];
        hs.fadeInOut = true;
        hs.dimmingOpacity = 0.8;
        hs.outlineType = 'rounded-white';
        hs.marginTop = 60;
        hs.marginBottom = 40;
        hs.numberPosition = '';
        hs.wrapperClassName = 'custom';
        hs.width = 600;
        hs.height = 500;
        hs.number = 'Page %1 of %2';
        hs.captionOverlay.fade = 0;

        // Add the slideshow providing the controlbar and the thumbstrip

    </script>

    <!--Slider-in icons-->
    <!--[if lt IE 9]>
<script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->

    <script language="javascript" type="text/javascript">
        function PutCursor() {
            document.loginform.Txtuid.focus();
        }
    </script>

    <script type="text/javascript">
        function disableBackButton() {
            window.history.forward();
        }
        setTimeout("disableBackButton()", 0);
    </script>

    <script type="text/javascript">

        history.pushState(null, null, location.href);
        window.onpopstate = function () {
            history.go(1);
        };

    </script>



    <style>
        .back-image {
            background-color: #ecf0fa !important;
            background-image: url(https://img.freepik.com/free-vector/modern-abstract-white-minimal-background_84443-8318.jpg) !important;
            background-position: center;
            min-height: 100vh;
            background-size: cover;
        }


        .main {
            padding: 25px 25px 25px;
            max-width: 600px !important;
            margin: 0px auto;
            background: #0075a0 !important;
            -webkit-box-shadow: 3px 17px 43px 6px rgba(0, 0, 0, 0.75);
            -moz-box-shadow: 3px 17px 43px 6px rgba(0,0,0,0.75);
            box-shadow: 1px 6px 8px 0px rgba(0, 0, 0, 0.75);
            border-radius: 10px;
        }

        .right-side a, .left-side a {
            font-size: 14px;
            color: #ffffff !important;
        }
    </style>


</head>
<body oncontextmenu="return false;">
    <form id="loginform" runat="server">
        <div class="back-image">
            <!---728x90--->
            <style>
                .loginlogo {
                    width: 200px;
                    background: white;
                    padding: 5px;
                    border-radius: 7px;
                }
            </style>
            <p>
                &nbsp;
            </p>
            <div class="main ">
                <div class="headder-icon">
                </div>
                <p align="center">
                    <img src="<%=Session["LogoUrl"].ToString ()%>" class="loginlogo">
                </p>
                <div class="its-sign-in">
                    <h2 class="">Sign in Here</h2>
                </div>

                <div class="form-left-w3l ">
                    <input type="text" runat="server" id="Txtuid" name="uid" placeholder="Distributor/Customer ID"
                        required="">
                </div>
                <div class="form-left-w3l ">
                    <input type="password" runat="server" id="Txtpwd" name="pwd" placeholder="Password"
                        required="">
                    <%-- <input type="password" runat="server" id="Txtpwd" name="pwd" placeholder="Password"
                    class="validate[required]" aria-describedby="basic-addon1" />--%>
                </div>
                <div class="rem-pass-agile">
                    <div class="left-side ">
                        <!-- <input type="checkbox" class="checked">
              <span class="span">Remember me </span> -->
                    </div>
                    <div class="right-side">
                        <a href="Forgot.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 550,height: 280,marginTop : 0 } )"
                            class="for">Forgot password...?</a>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="btnn">
                    <asp:Button ID="BtnSubmit" runat="server" Style="border-radius: 50px;" Text="Sign In" class="btncolor" OnClick="BtnSubmit_Click" />

                    <%-- <asp:Button ID="BtnSubmit" runat="server" Text="Submit" class="submit" OnClick="BtnSubmit_Click" />--%>
                    <%-- <button type="submit" id="BtnSubmit1" runat="server" class="btn" onclick ="">
                            Submit
                        </button>--%>
                    <br>
                    <div class="clear">
                    </div>
                </div>

                <div class="line-mid">
                    <h4 class=" ">or</h4>
                </div>
                <div class="icons">
                    <div class="follow-one ">
                        <a href="NewJoining1.aspx" class="submit" style="color: #fff; text-decoration: none"
                            runat="server" id="aJoining">Sign Up</a>
                    </div>
                    <div class="follow-two ">
                        <a href="https://unifi4u.io/" class="submit" style="color: #fff; font-size: medium; text-decoration: none"
                            runat="server" id="ahome">Go To Website </a>
                    </div>
                </div>
                <!--<div class="left-side"> <a href="#" class="for ">Already have an account..? </a></div> -->
            </div>
            <!---728x90--->
        </div>
    </form>
</body>
</html>
