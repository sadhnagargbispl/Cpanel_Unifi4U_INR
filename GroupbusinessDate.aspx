<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GroupbusinessDate.aspx.cs" Inherits="GroupbusinessDate" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<style type="text/css">
        .site_wrapper
        {
            height: 720px;
        }
    </style>
    <link rel="stylesheet" href="plugins/datatables/dataTables.bootstrap.css">--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="plugins/datatables/dataTables.bootstrap.css">
    <%--<div id="content" class="main-content">
       <div class="profile-bar-simple red-border clearfix">
                <h6>Direct Sponsors</h6>
            </div>
      
        <div class="container-fluid">
            <div id="ctl00_ContentPlaceHolder2_upChPwd">
                <div id="ctl00_ContentPlaceHolder2_message">
                </div>
                <div class="row">--%>
    <div class="col-lg-12">
        <div class="">
            <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                <div class="profile-bar-simple red-border clearfix">
                    <h6>Direct Sponsors</h6>
                </div>
                <div class="clearfix gen-profile-box" style="min-height: auto;">
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
                                    <div class="row">
                                        <div class="col-sm-4 ">
                                            <label for="inputdefault">
                                                From :</label>
                                            <asp:TextBox ID="TxtFromDate" CssClass="form-control" TabIndex="2" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtFromDate"
                                                Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtFromDate"
                                                ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-sm-4 ">
                                            <label for="inputdefault">
                                                To :</label>
                                            <asp:TextBox ID="TxtToDate" CssClass="form-control" TabIndex="3" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtToDate"
                                                Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                                ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class=" col-md-4 " style="padding-top: 2%">
                                            <asp:Button ID="BtnSearch" Text="Search" runat="server" class="btn btn-warning btn-flat" OnClick="BtnSearch_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br>
                            <div class="col-md-12">
                                <div id="DivSideA" runat="server">
                                    <asp:Label ID="Label1" runat="server" Text="Total Records" Visible="false"></asp:Label>
                                    <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                    <div class="table-responsive" style="overflow: scroll;">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <table id="Table1" class="table table-bordered table-striped">
                                                    <thead>
                                                        <tr style="background-color: #102f6d;">
                                                            <th style="color: White;">SNo
                                                            </th>
                                                            <th style="color: White;">User ID
                                                            </th>
                                                            <th style="color: White;">Name
                                                            </th>
                                                            <th style="color: White;">Activation Date
                                                            </th>
                                                            <th style="color: White;">Rank
                                                            </th>
                                                            <th style="color: White;">Self New Business<%--<img src="https://cryptologos.cc/logos/tron-trx-logo.png?v=032" width="20" height="20" />--%><%--(<i class="las la-dollar-sign"></i>)--%>
                                                                                
                                                            </th>
                                                            <th style="color: White;">Self Re-Business <%--(<i class="las la-dollar-sign"></i>)--%>
                                                            </th>
                                                            <th style="color: White;">Team New Business <%--(<i class="las la-dollar-sign"></i>)--%>
                                                            </th>
                                                            <th style="color: White;">Team Re-Business <%--(<i class="las la-dollar-sign"></i>)--%>
                                                            </th>
                                                            <th style="color: White;">Total Business<%--(<i class="las la-dollar-sign"></i>)--%>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="Grdtotal" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <%--  <asp:Label ID="lblID" runat="server" Text='<%#Eval("FormNo")%>' Visible="false"></asp:Label>--%>
                                                                        <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("IDno")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("MemberName")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Activation Date")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Rank")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Investment")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("REInvestment")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("TeamInvestment")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("TeamReinvestment")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Total")%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                                <br />
                                                <table id="customers2" class="table table-bordered table-striped">
                                                    <thead>
                                                        <tr style="background-color: #102f6d;">
                                                            <th style="color: White;">
                                                                <%--SNo--%>Leg No.
                                                            </th>
                                                            <th style="color: White;">User ID
                                                            </th>
                                                            <th style="color: White;">Name
                                                            </th>
                                                            <th style="color: White;">Joining Date
                                                            </th>
                                                            <th style="color: White;">Activation Date
                                                            </th>
                                                            <th style="color: White;">Rank
                                                            </th>
                                                            <th style="color: White;">S. New Business<%--(<i class="las la-dollar-sign"></i>)--%>
                                                            </th>
                                                            <th style="color: White;">S. Re-Business<%--(<i class="las la-dollar-sign"></i>)--%>
                                                            </th>
                                                            <th style="color: White;">T. New Business<%--(<i class="las la-dollar-sign"></i>)--%>
                                                            </th>
                                                            <th style="color: White;">T. Re-Business<%--(<i class="las la-dollar-sign"></i>)--%>
                                                            </th>
                                                            <th style="color: White;">Total Business<%--(<i class="las la-dollar-sign"></i>)--%>
                                                            </th>
                                                            <th style="color: White;">Downline
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="DLDirects" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblID" runat="server" Text='<%#Eval("FormNo")%>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("IDno")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("MemFirstName")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Doj")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("UpgradeDate")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Rank")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Investment")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Reinvestment")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("TeamInvestment")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("TeamReinvestment")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Total")%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="LblStatus" runat="server" Text="Downline" Style="color: White;"></asp:Label>
                                                                        <asp:ImageButton ID="edit" runat="server" ImageUrl="images/down.png" OnClick="PerformData"
                                                                            Style="background-color: White;" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="BtnSearch" EventName="Click" />
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
    <%-- </div>
            </div>
        </div>--%>
</asp:Content>
