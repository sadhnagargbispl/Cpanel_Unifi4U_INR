<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rptwithdrawls.aspx.cs" Inherits="Rptwithdrawls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="plugins/datatables/dataTables.bootstrap.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Withdrawal Detail
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <div class="centered">
                        <div class="clr">
                            <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                        </div>
                        <div class="clr">
                        </div>
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table datatable" HeaderStyle-CssClass="bg-primary"
                                        AllowSorting="true" ShowHeader="true" PageSize="20" EmptyDataText="No data to display." OnPageIndexChanging="RptDirects_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNo.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <%--    <script type="text/javascript" src="assets/datatable.css"></script>--%>

    <script type="text/javascript" src="assets/jquery.dataTables.min.js"></script>

    <%--  <script type="text/javascript" src="js/plugins/datatables/jquery.dataTables.min.js"></script>--%>

    <script type="text/javascript" src="assets/tableExport.js"></script>

    <script type="text/javascript">
        var jq = $.noConflict();
        function pageLoad(sender, args) {


            jq(document).ready(function () {
                jq('#customers2').DataTable();

            });
        }


    </script>

</asp:Content>
