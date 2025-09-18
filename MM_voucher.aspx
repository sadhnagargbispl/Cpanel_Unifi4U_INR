<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MM_voucher.aspx.cs" Inherits="MM_voucher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
           <%--<section class="content-header head-new">
          <div class="row">
            <div class="col-md-2 col-xs-6">
              <h4 >Package Purchase</h4>
            </div>
          </div>
        </section>--%>
    <style>
        #productbox
        {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            text-align: center;
            padding: 15px;
            border: 1px solid #ddd;
            border-radius: 10px;
            background-color: #f7f7f7;
        }
        #productbox img
        {
            object-fit: fill;
            height: 300px;
        }
        .img-about-us
        {
            max-width: 100%;
            height: auto;
            display: block;
            margin: 0 auto;
        }
        .title-about
        {
            margin-top: 15px;
            font-size: 16px;
            font-weight: bold;
            color: #333; 
        }
        .nav-tabs > li > a
        {
            margin-right: 2px;
            line-height: 1.42857143;
            border: 1px solid #0f70a76b !important;
            border-radius: 4px 4px 0 0;
        }
        .nav-tabs
        {
            border-bottom: 1px solid #0f70a7;
            background: #ffffff !important;
            font-size: 16px;
            text-transform: uppercase;
        }
        .profile-bar.clearfix.rounded-3.mb-1.probox
        {
            border: solid 1px #0f70a7;
            padding: 10px;
            margin-bottom: 15px;
        }

        .btn.btn-primary, .btn.btn-primary.dropdown-toggle{
            font-size : 1rem !important;
        }
.fs-5
  {
      font-size: 13px !important;
      padding: 7px !important;
      border-radius: 5px !important;
  }
  .btn-primary
  {
      background: #152f6b !important;
      box-shadow: none !important;
      transition: 0.3s ease-in-out !important;
  }
  .btn-primary:hover
  {
      border: 1px solid #152f6b !important;
      background: #fff !important;
      box-shadow: none !important;
      color: #152f6b !important;
      transition: 0.3s ease-in-out !important;
  }
    </style>
    <section class="content">
         <div class="row">
                            <div class="col-lg-12 text-center">
                              <%--  <div class="title-section">
                                    <h1 class="fs-6"> Utility Portal Voucher</h1>
                                </div>
                                <br>--%>



                                <div class="row justify-content-center">

                                    <div class="col-lg-12 col-sm-4 col-12">


<%--                                    <ul class="nav nav-tabs">
                                        <li class="active"><a data-toggle="tab" href="#home">Utility Portal Voucher</a></li>
                                       <li><a data-toggle="tab" href="#menu1">  Health & Wellness Care </a></li>
                                        <li><a data-toggle="tab" href="#menu2">  Shopping </a></li>
                                     
                                      </ul>--%>
                                    
                                      <div class="tab-content" style="margin-top: 10px; padding: 10px;">

                                        <%--<div id="home" class="tab-pane fade in active">--%>



                                               <div class="row">
                                            <asp:Repeater ID="RepUtility" runat="server">
                                                    <ItemTemplate>
     <div class="col-lg-4 col-sm-4 col-12">
        
         <div class="profile-bar clearfix rounded-3 mb-1 probox">
             <img src='<%#Eval("kitimg")%>' alt="voucher image" class="img-fluid img-responsive rounded-3">
             <div class="d-flex justify-content-between mt-2 align-items-center p-2 pt-0 pb-0">
                 <div class="description">
                     <p class="mb-1" align="left" style="width: 200px; margin-top: 5px;">
                         <%#Eval("kitname")%></p>
                     <p class="mb-0 text-success " align="left">
                         <b>Point :
                             <%#Eval("kitamount")%>
                         </b>
                     </p>
                 </div>
                 
                     <a name="" id="" class="btn btn-primary border-0 rounded-3 btn-sm fs-5 shadow-0" style="text-align : right !important;"
                         <%--href='<%#String.Format("VoucherDetails.aspx?KitID={0}", Eval("kitid").ToString()) %>'--%>
                        href='<%#String.Format("VoucherDetails.aspx?KitID={0}", HttpUtility.UrlEncode(Crypto.Encrypt(Eval("kitid").ToString()))) %>'
                         role="button"> Buy Now</a>
             </div>
         </div>
     </div>
 </ItemTemplate>
                                            </asp:Repeater>
                                               </div>                            
                                      </div>


                                    </div>

                                   
                                </div>
                            </div>
                        </div>
        </section>
</asp:Content>

