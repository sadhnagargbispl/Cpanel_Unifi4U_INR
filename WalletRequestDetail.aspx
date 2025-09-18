<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WalletRequestDetail.aspx.cs" Inherits="WalletRequestDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Add Fund Detail
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #000;">
                    <div class="clearfix">
                        <div class="clearfix">
                            <br>
                            <div class="centered">
                                <asp:Label ID="Label1" runat="server" CssClass="error"></asp:Label>
                            </div>
                            <div class="clr">
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <div class="table mb-0" cellspacing="0" cellpadding="4" rules="all" border="1" id="ctl00_ContentPlaceHolder2_DGVReferral"
                                            style="width: 100%; border-collapse: collapse;">
                                            <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-responsive-md"
                                                HeaderStyle-BackColor="" HeaderStyle-ForeColor="White" AllowPaging="true"
                                                PageSize="10" OnPageIndexChanging="RptDirects_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ReqNo">
                                                        <ItemTemplate>
                                                            <%#Eval("ReqNo")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Request Date">
                                                        <ItemTemplate>
                                                            <%#Eval("ReqDate") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment Mode">
                                                        <ItemTemplate>
                                                            <%#Eval("PayMode") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cheque/ TransactionNo">
                                                        <ItemTemplate>
                                                            <%# Eval("ChqNo") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cheque/ Transaction Date">
                                                        <ItemTemplate>
                                                            <%#Eval("ChequeDate") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  <%--  <asp:TemplateField HeaderText="Bank Name">
                                                        <ItemTemplate>
                                                            <%# Eval("BankName") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Branch Name">
                                                        <ItemTemplate>
                                                            <%# Eval("BranchName") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <ItemTemplate>
                                                            <%# Eval("Amount") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remark">
                                                        <ItemTemplate>
                                                            <%# Eval("Remarks") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <%# Eval("Status") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Image">
                                                        <ItemTemplate>
                                                            <a href='<%# "Img.aspx?type=Payment&ID=" + Eval("Reqno") %>'
                                                                onclick="return hs.htmlExpand(this, { objectType: 'iframe', width: 585, height: 380, marginTop: 0 })">
                                                                <asp:Image ID="Image1" runat="server"
                                                                    ImageUrl='<%# Eval("ScannedFile") %>'
                                                                    Height="100px"
                                                                    Width="100px"
                                                                    Visible='<%# Convert.ToBoolean(Eval("ScannedFileStatus")) %>' />
                                                            </a>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
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

    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <script type="text/javascript" src="assets/datatable.css"></script>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js"></script>

    <script type="text/javascript" src="js/plugins/datatables/jquery.dataTables.min.js"></script>

    <script type="text/javascript" src="assets/tableExport.js"></script>

    <script type="text/javascript">
        var jq = $.noConflict();
        function pageLoad(sender, args) {


            jq(document).ready(function () {
                jq('#customers2').DataTable();

            });
        }


    </script>

    <script src="assets/jquery.inbox.js"></script>

    <script type="text/javascript" src="popupassets/jquery.fancybox.pack.js"></script>

    <script type="text/javascript" src="popupassets/jquery.fancybox.pack1.js"></script>

    <link rel="stylesheet" href="popupassets/jquery.fancybox.css" type="text/css" media="screen" />

    <script type="text/javascript">
        jq(document).ready(function () {
            jq(".fbox").fancybox({
                openEffect: 'elastic',
                closeEffect: 'elastic'
            });
        });
    </script>
</asp:Content>

