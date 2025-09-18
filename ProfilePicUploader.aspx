<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProfilePicUploader.aspx.cs" Inherits="ProfilePicUploader" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" type="text/css" id="theme" href="css/theme-default.css" />
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data" class="form-horizontal">
       <%-- <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </ajaxToolkit:ToolkitScriptManager>--%>
        <div class="page-content-wrap">
            <div class="row">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="col-md-8">
                            <center>
                                Dear
                            <asp:Label ID="lblNm" runat="server"></asp:Label>,
                            <br />
                                Update Your Profile Pic.
                            <br />
                                <asp:Image ID="Image4" runat="server" />
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                    </div>
                                    <div class="col-md-4">
                                        <asp:FileUpload ID="FUIdentity" multiple class="file" data-preview-file-type="any"
                                            runat="server" />
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                            </center>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <%-- <script type="text/javascript" src="js/plugins/jquery/jquery-ui.min.js"></script>
        <script type="text/javascript" src="js/plugins/bootstrap/bootstrap.min.js"></script>                
        <!-- END PLUGINS -->--%>
    <!-- THIS PAGE PLUGINS -->

    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <script type="text/javascript" src="assets/fileinput/fileinput.min.js"></script>

    <script type="text/javascript" src="assets/filetree/jqueryFileTree.js"></script>

    <%--  <script type='text/javascript' src='js/plugins/icheck/icheck.min.js'></script>
        <script type="text/javascript" src="js/plugins/mcustomscrollbar/jquery.mCustomScrollbar.min.js"></script>
        
        <script type="text/javascript" src="js/plugins/dropzone/dropzone.min.js"></script>
        <script type="text/javascript" src="js/plugins/fileinput/fileinput.min.js"></script>        
        <script type="text/javascript" src="js/plugins/filetree/jqueryFileTree.js"></script>
        <!-- END PAGE PLUGINS -->
        
        <!-- START TEMPLATE -->
        <script type="text/javascript" src="js/settings.js"></script>
        
        <script type="text/javascript" src="js/plugins.js"></script>        
        <script type="text/javascript" src="js/actions.js"></script>--%>
    <!-- END TEMPLATE -->

    <script>
        var jq = $.noConflict;
        jq(function () {
            jq("#file-simple").fileinput({
                showUpload: false,
                showCaption: false,
                browseClass: "btn btn-danger",
                fileType: "any"
            });
            jq("#filetree").fileTree({
                root: '/',
                script: 'assets/filetree/jqueryFileTree.php',
                expandSpeed: 100,
                collapseSpeed: 100,
                multiFolder: false
            }, function (file) {
                alert(file);
            }, function (dir) {
                setTimeout(function () {
                    page_content_onresize();
                }, 200);
            });
        });
    </script>

    <!-- END SCRIPTS -->
</body>
</html>
