<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyDirects.aspx.cs" Inherits="MyDirects" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- DataTables -->
    <link rel="stylesheet" href="plugins/datatables/dataTables.bootstrap.css">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Level Wise Report
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <div class="centered">
                        <div class="clr">
                            <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>
                                            Search By</label>
                                        <asp:DropDownList ID="rbtnsearch" runat="server" class="form-control" OnSelectedIndexChanged="rbtnsearch_SelectedIndexChanged">
                                            <asp:ListItem Text="Level Wise" Selected="True" Value="L"></asp:ListItem>
                                            <%-- <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Right" Value="2"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3" id="lbllevel" runat="server">
                                    <div class="form-group">
                                        <label>
                                            Level</label>
                                        <asp:DropDownList ID="DdlLevel" CssClass="form-control" TabIndex="1" runat="server" OnSelectedIndexChanged="DdlLevel_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3" id="divSearch" runat="server">
                                    <div class="form-group">
                                        <label>
                                            Search</label>
                                        <asp:DropDownList ID="DDlSearchby" CssClass="form-control" TabIndex="2" runat="server">
                                            <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>
                                            &nbsp;</label>
                                        <asp:Button ID="BtnSubmit" runat="server" Text="Search" TabIndex="3" class="btn btn-primary" OnClick="BtnSubmit_Click" />
                                         <asp:Button ID="btnexport" runat="server" Text="Export to Excel" TabIndex="3" class="btn btn-primary" OnClick="btnexport_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <div class="col-md-3">
                            </div>
                            <div class="sda-content-3">
                                <table id="table" class="table table-bordered table-striped">
                                    <tbody>
                                        <tr>
                                            <th></th>
                                            <th style="text-align: center;">Direct
                                            </th>
                                            <th style="text-align: center">Indirect
                                            </th>
                                            <th style="text-align: center">Total
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>Joining
                                            </td>
                                            <td id="tdDirectleft" runat="server" style="text-align: center">0
                                            </td>
                                            <td id="tdDirectright" runat="server" style="text-align: center">0
                                            </td>
                                            <td id="TotalDirect" runat="server" style="text-align: center">0
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Active
                                            </td>
                                            <td id="tddirectActive" runat="server" style="text-align: center">0
                                            </td>
                                            <td id="tdindirectActive" runat="server" style="text-align: center">0
                                            </td>
                                            <td id="TotalActive" runat="server" style="text-align: center">0
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Total Business
                                            </td>
                                            <td id="Directunit" runat="server" style="text-align: center">0
                                            </td>
                                            <td id="indirectunit" runat="server" style="text-align: center">0
                                            </td>
                                            <td id="totalunit" runat="server" style="text-align: center">0
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="col-md-3">
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div id="DivSideA" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div style="overflow: scroll;">
                                            <asp:Label ID="Label1" runat="server" Text="Total Records"></asp:Label>
                                            <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                            <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-striped"
                                                AllowPaging="true" PageSize="10" OnPageIndexChanging="RptDirects_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SNo">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- jQuery 2.2.3 -->

    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <%--    <script type="text/javascript" src="assets/datatable.css"></script>--%>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js"></script>

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
