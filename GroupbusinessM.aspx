<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GroupbusinessM.aspx.cs" Inherits="GroupbusinessM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="stars"></div>
<div class="stars2"></div>
<div class="stars3"></div>
    <div id="content" class="main-content">
        <div class="sub-header-container">
            <header class="header navbar navbar-expand-sm"> <a href="javascript:void(0);" class="sidebarCollapse"
              data-placement="bottom" tabindex="-1"> <i class="las la-bars"></i> </a>
            <ul class="navbar-nav flex-row">
              <li>
                <div class="page-header">
                  <nav class="breadcrumb-one" aria-label="breadcrumb">
                    <ol class="breadcrumb">
                      <li class="breadcrumb-item active" aria-current="page"><span> 
             <%-- <h3 class="box-title">--%> Direct Sponsors<%--</h3>--%>
              </span></li>
                    </ol>
                  </nav>
                </div>
              </li>
            </ul>
          </header>
        </div>
        <div class="container-fluid">
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>

            <div id="ctl00_ContentPlaceHolder2_upChPwd">
                <div id="ctl00_ContentPlaceHolder2_message">
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="card">
                            <div id="ctl00_ContentPlaceHolder1_divgenexbusiness">
                                <div class="clearfix gen-profile-box" style="min-height: auto;">
                                    <div class="profile-bar clearfix" style="background: #000;">
                                        <div class="clearfix">
                                            <br>
                                            <div class="centered">
                                                <div class="clr">
                                                    <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                                                </div>
                                                <div class="clr">
                                                </div>
                                                <table class="table-responsive" style="display: none;">
                                                    <tr>
                                                        <td width="30%">
                                                            <div id="divSearch" runat="server">
                                                                Search
                                                                <asp:DropDownList ID="DDlmonth" Width="300px" CssClass="form-control" TabIndex="2"
                                                                    runat="server">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td style="width: 25%">
                                                            <asp:Button ID="BtnSearch" runat="server" Text="Search" class="btn btn-primary" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <br>
                                            <div class="col-md-12">
                                                <div id="DivSideA" runat="server">
                                                    <asp:Label ID="Label1" runat="server" Text="Total Records" Visible="false"></asp:Label>
                                                    <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                                    <div class="table-responsive">
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <table id="Table1" class="table datatable">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="color: White;">
                                                                                SNo
                                                                            </th>
                                                                            <th style="color: White;">
                                                                                User ID
                                                                            </th>
                                                                            <th style="color: White;">
                                                                                Name
                                                                            </th>
                                                                            <th style="color: White;">
                                                                                Activation Date
                                                                            </th>
                                                                            <th style="color: White;">
                                                                                Rank
                                                                            </th>
                                                                           <th style="color: White;">
                                                                                Self PV <%--(<i class="fa fa-inr" aria-hidden="true"></i>)--%>
                                                                            </th>
                                                                            <th style="color: White;">
                                                                                Team PV <%--(<i class="fa fa-inr" aria-hidden="true"></i>)--%>
                                                                            </th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <asp:Repeater ID="Grdtotal" runat="server">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td>
                                                                                       
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
                                                                                        <%#Eval("Liquidity")%>
                                                                                    </td>
                                                                                    <td>
                                                                                        <%#Eval("team")%>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </tbody>
                                                                </table>
                                                                <br />
                                                                <table id="customers2" class="table datatable">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="color: White;">
                                                                                SNo
                                                                            </th>
                                                                            <th style="color: White;">
                                                                                User ID
                                                                            </th>
                                                                            <th style="color: White;">
                                                                                Name
                                                                            </th>
                                                                            <th style="color: White;">
                                                                                Activation Date
                                                                            </th>
                                                                            <th style="color: White;">
                                                                                Rank
                                                                            </th>
                                                                           
                                                                            <th style="color: White;">
                                                                                Self PV <%--(<i class="fa fa-inr" aria-hidden="true"></i>)--%>

                                                                            </th>
                                                                            <th style="color: White;">
                                                                                Team PV <%--(<i class="fa fa-inr" aria-hidden="true"></i>)--%>
                                                                            </th>
                                                                            <th style="color: White;">
                                                                                Downline
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
                                                                                        <%#Eval("UpgradeDate")%>
                                                                                    </td>
                                                                                
                                                                                    <td>
                                                                                        <%#Eval("Rank")%>
                                                                                    </td>
                                                                                 
                                                                                    <td>
                                                                                        <%#Eval("Amount")%>
                                                                                    </td>
                                                                                    <td>
                                                                                        <%#Eval("team")%>
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
                </div>
            </div>
        </div>
    
</asp:Content>

