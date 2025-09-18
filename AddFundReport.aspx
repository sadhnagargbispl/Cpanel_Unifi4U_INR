<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddFundReport.aspx.cs" Inherits="AddFundReport" %>

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
                                <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                            </div>
                            <div class="clr">
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <div class="table mb-0" cellspacing="0" cellpadding="4" rules="all" border="1" id="ctl00_ContentPlaceHolder2_DGVReferral"
                                            style="width: 100%; border-collapse: collapse;">
                                            <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="True" RowStyle-Height="25px"
                                                GridLines="Both" AllowPaging="true" CssClass="table table-bordered" AllowSorting="False"
                                                ShowHeader="true" PageSize="10" EmptyDataText="No data to display."
                                                HeaderStyle-BackColor="#c6c8ca" OnPageIndexChanging="RptDirects_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SNo.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="pagination-ys" />
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

</asp:Content>

