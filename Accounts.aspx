<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Accounts.aspx.cs" Inherits="Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
                <div class="stars"></div>
<div class="stars2"></div>
<div class="stars3"></div>
     <link rel="stylesheet" href="adminassets/main.css" type="text/css">
    <div id="content" class="main-content">
        <div class="sub-header-container">
            <header class="header navbar navbar-expand-sm"> <a href="javascript:void(0);" class="sidebarCollapse"
              data-placement="bottom" tabindex="-1"> <i class="las la-bars"></i> </a>
            <ul class="navbar-nav flex-row">
              <li>
                <div class="page-header">
                  <nav class="breadcrumb-one" aria-label="breadcrumb">
                    <ol class="breadcrumb">
                      <li class="breadcrumb-item active" aria-current="page"><span>Working Wallet Details</span></li>
                    </ol>
                  </nav>
                </div>
              </li>
            </ul>
          </header>
        </div>
        <div class="container-fluid">
            <div class="row w3-res-tb">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box box-primary">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness">
                                            <div class="clearfix gen-profile-box" style="min-height: auto;">
                                                <div class="profile-bar clearfix" style="background: #000;">
                                                    <table id="ctl00_cphData_tbUserDetails" style="color: #ccc;" class="rounded_colhead"
                                                        cellspacing="0" cellpadding="0" width="430" border="0">
                                                        <tr>
                                                            <td style="width: 7px; height: 25px" class="textB" valign="middle" align="right"
                                                                rowspan="1">
                                                            </td>
                                                            <td class="textB" valign="middle" align="left" colspan="3" height="35">
                                                                <%-- <h5>
                                        Main Account Status</h5>--%>
                                                            </td>
                                                            <td style="height: 25px; width: 50px; font-size: 12px; font-family: Verdana">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 7px" class="textB" valign="middle" align="right" rowspan="9">
                                                            </td>
                                                            <td class="PanelHeaderBackground" valign="middle" align="right" colspan="3" height="1">
                                                            </td>
                                                            <td height="1">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 6px" valign="middle" align="right" colspan="3">
                                                            </td>
                                                            <td style="height: 6px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 108px; height: 25px" class="textBP" valign="middle" align="right">
                                                                Deposit
                                                            </td>
                                                            <td style="width: 8px; height: 25px" class="colon" valign="middle" align="center">
                                                                :
                                                            </td>
                                                            <td id="MCredit" runat="server" style="height: 25px" class="textBP" valign="middle"
                                                                align="left">
                                                                0.00
                                                            </td>
                                                            <td style="height: 25px">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="PanelHeaderBackground" valign="middle" align="right" colspan="3" height="1">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 108px; height: 25px" class="textBP" valign="middle" align="right">
                                                                Used
                                                            </td>
                                                            <td style="width: 8px; height: 25px" class="colon" valign="middle" align="center">
                                                                :
                                                            </td>
                                                            <td id="MDebit" runat="server" style="height: 25px" class="textBP" valign="middle"
                                                                align="left">
                                                                0.00
                                                            </td>
                                                            <td style="height: 25px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="PanelHeaderBackground" valign="middle" align="right" colspan="3" height="1">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 108px; height: 25px" class="textBP" align="right">
                                                                Balance
                                                            </td>
                                                            <td style="width: 8px; height: 25px" class="colon" align="center">
                                                                :
                                                            </td>
                                                            <td id="MBal" runat="server" style="height: 25px" class="textBP" valign="middle"
                                                                align="left">
                                                                0.00
                                                            </td>
                                                            <td style="height: 25px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 7px; height: 15px" class="text" valign="top" align="right">
                                                            </td>
                                                            <td style="width: 108px; height: 15px" class="text" valign="top" align="right">
                                                            </td>
                                                            <td style="width: 8px; height: 15px" class="text" valign="top" align="center">
                                                            </td>
                                                            <td style="height: 15px" class="text" valign="top" align="left">
                                                            </td>
                                                            <td style="height: 15px" class="text" valign="middle" align="left">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="col-md-12">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                                                                HeaderStyle-BackColor="#c6c8ca" HeaderStyle-ForeColor="Black" AllowPaging="true"
                                                                PageSize="10" OnPageIndexChanging="RptDirects_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="SNo">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Rn") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <%#Eval("Voucherdate") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Voucher No.">
                                                                        <ItemTemplate>
                                                                            <%#Eval("VoucherNo")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText=" Remark">
                                                                        <ItemTemplate>
                                                                            <%#Eval("Narration") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText=" Deposit">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Deposit") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText=" Used">
                                                                        <ItemTemplate>
                                                                            <%#Eval("used") %>
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


                jq(document).ready(function() {
                    jq('#customers2').DataTable();

                });
            }


        </script>
</asp:Content>

