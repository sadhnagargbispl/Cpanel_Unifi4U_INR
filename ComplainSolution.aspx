<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ComplainSolution.aspx.cs" Inherits="ComplainSolution" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Ticket Status
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
                        <div class="form-horizontal">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <table id="customers2" class="table datatable">
                                                <thead>
                                                    <tr>
                                                        <th>SNo
                                                        </th>
                                                        <th>Complaint Id
                                                        </th>
                                                        <th>Complaint Date
                                                        </th>
                                                        <th>Complaint
                                                        </th>
                                                        <th>Reply Date
                                                        </th>
                                                        <th>Reply
                                                        </th>
                                                        <th>Status
                                                        </th>
                                                        <th>View
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="RptDirects" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                                </td>
                                                                <td>
                                                                    <%#Eval("CID")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("CDate")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Complaint")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("SDate")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Solution")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Status")%>
                                                                </td>
                                                                <td>
                                                                    <a class="btn btn_default" href='<%# "Reply.aspx?CID=" + (Eval("VCid"))  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 350,marginTop : 0 } )">
                                                                        <asp:Label ID="LBModify" runat="server" Text=" View Detail" ForeColor ="blue" />
                                                                    </a>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <%-- <asp:AsyncPostBackTrigger ControlID="CmdSave" EventName="Click" />--%>
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

    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <%--    <script type="text/javascript" src="assets/datatable.css"></script>--%>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js"></script>

    <%--  <script type="text/javascript" src="js/plugins/datatables/jquery.dataTables.min.js"></script>--%>

    <script type="text/javascript" src="assets/tableExport.js"></script>

    <%--  <script type="text/javascript">
        var jq = $.noConflict();
        function pageLoad(sender, args) {
           

            jq(document).ready(function() {
                jq('#customers2').DataTable();

            });
        }


    </script>
    --%>
</asp:Content>
















