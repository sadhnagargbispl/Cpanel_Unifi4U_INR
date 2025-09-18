<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DirectIncome.aspx.cs" Inherits="DirectIncome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <link rel="stylesheet" href="plugins/datatables/dataTables.bootstrap.css">

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <div id="content" class="main-content">
        <div class="content-wrapper">
            <div class="sub-header-container">
                <header class="header navbar navbar-expand-sm">
                    <a href="javascript:void(0);" class="sidebarCollapse"
                        data-placement="bottom" tabindex="-1"><i class="las la-bars"></i></a>
                    <ul class="navbar-nav flex-row">
                        <li>
                            <div class="page-header">
                                <nav class="breadcrumb-one" aria-label="breadcrumb">
                                    <ol class="breadcrumb">
                                        <li class="breadcrumb-item active" aria-current="page">
                                            <span>Dierect Bonus details
                                            </span>
                                        </li>
                                    </ol>
                                </nav>
                            </div>
                        </li>
                    </ul>
                </header>
            </div>
            <div class="container-fluid">
                <div id="ctl00_ContentPlaceHolder2_upChPwd">

                    <div id="ctl00_ContentPlaceHolder2_message">
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card" style="padding: 10px;">
                                <div class="card-header">
                                    <div id="ctl00_ContentPlaceHolder2_DGVReferral" class="clearfix gen-profile-box">
                                        <div class="clearfix gen-profile-box">
                                            <div class="profile-bar clearfix">
                                                <div class="clearfix">
                                                    <br>
                                                    <div class="centered">
                                                        <div class="clr">
                                                            <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                                                        </div>
                                                        <div class="clr">
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="col-md-3">
                                                                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-primary " Visible="false" />
                                                            </div>
                                                        </div>
                                                        <div class="table-responsive">
                                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="TRUE" CssClass="table table-bordered"
                                                                        HeaderStyle-BackColor="" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" AllowPaging="true"
                                                                        PageSize="10" RowStyle-HorizontalAlign="Right" OnPageIndexChanging="RptDirects_PageIndexChanging">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="S.No.">
                                                                                <ItemTemplate>
                                                                                    <%#Container.DataItemIndex + 1%>.
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
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
            </div>
</asp:Content>

