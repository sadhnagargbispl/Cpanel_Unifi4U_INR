<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VoucherDetails.aspx.cs" Inherits="VoucherDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%-- <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>--%>

    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    <style>
        .w-100p
        {
            width: 100% !important;
        }
        .rounded-3
        {
            border-radius: 10px !important;
        }
        .profile-bar
        {
            padding: 5px !important;
        }
        .fs-6
        {
            margin-top: 20px !important;
            font-size: 16px !important;
            margin-bottom: 20px !important;
            font-weight: 600 !important;
            color: #152f6b !important;
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="main-content-section">
        <div class="row mb-2">
            <div class="col-lg-12">
                <div class="row profile-bar p-2 align-items-center">
                    <div class="col-lg-4 col-sm-4 col-12 order-lg-2 order-sm-2 order-1">
                        <div class="clearfix rounded-3 mb-1">
                            <img id="Img" src="" runat="server" alt="banner" class="img-responsive w-100p rounded-3" />
                        </div>
                    </div>
                    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                    <div class="col-lg-8 col-sm-8 col-12 order-lg-1 order-sm-1 order-2">
                        <div class="clearfix rounded-3 mb-1">
                            <h1 class="fs-6">
                                <asp:Label ID="LblPackageName" runat="server" Text=""></asp:Label></h1>
                            <p>
                                Discover the ultimate convenience with our all-in-one platform: shop online, manage
                                utilities, book holidays and movies, order food, and find perfect gift vouchers
                                effortlessly. Stay tuned for our upcoming Petro Card to top-up your tank and your
                                life with ease.</p>
                            <p>
                                <b>USER NAME</b> :
                                <asp:Label ID="LblUserName" runat="server" Text=""></asp:Label></p>
                            <p>
                                <b>ID NUMBER</b> :
                                <asp:Label ID="LblUserID" runat="server" Text=""></asp:Label></p>
                            <p>
                                <b>PACKAGE Point</b> :
                                <asp:Label ID="LblPakageAmount" runat="server" Text="0.00"></asp:Label></p>
                            <p>
                                <b>Utility Wallet Balance</b> :
                                <asp:Label ID="LblGiftWalletBala" runat="server" Text="0.00"></asp:Label></p>
                            <p>
                                Amount ($)</p>
                                
                              
                                
                            <div class="form-group d-flex">
                                <asp:TextBox ID="TxtAmount" runat="server" class="form-control" aria-describedby="emailHelp"
                                    placeholder="Enter Amount" AutoPostBack="true" Style="width: 80%; margin-right: 10px;"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:Button ID="BtnProceedToPay" runat="server" Text="Proceed To Pay" class="btn btn-primary border-0 rounded-3 btn-sm fs-5 shadow-0" OnClick="BtnProceedToPay_Click"  />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
 <div class="tabmenu" runat="server" id="DivMDescription_Food" visible="False">
        <div class="row profile-bar p-2 align-items-center">
            <div class="col-lg-12">
                <ul class="nav nav-tabs" id="myTab" role="tablist">
                    <li class="nav-item">
                    
                    <a class="nav-link active" id="home-tab" data-toggle="tab" href="#home"
                        role="tab" aria-controls="home" aria-selected="true">Description</a> </li>
                        
                    <li class="nav-item"><a class="nav-link" id="profile-tab" data-toggle="tab" href="#profile"
                        role="tab" aria-controls="profile" aria-selected="false">How To Use</a> </li>
                        
                    <li class="nav-item"><a class="nav-link" id="contact-tab" data-toggle="tab" href="#contact"
                        role="tab" aria-controls="contact" aria-selected="false">Terms &amp; Conditions</a>
                        
                    </li>
                </ul>
                
                <div class="tab-content" id="myTabContent">
                
          <div class="tab-pane fade show active p-3" id="home" role="tabpanel" aria-labelledby="home-tab">
            <p class="text-Black">
                <asp:Label ID="LblFoodDis" runat="server" Text=""></asp:Label>
            </p>
          </div>
        
        <div class="tab-pane fade p-3" id="profile" role="tabpanel" aria-labelledby="profile-tab">
            <p class="text-Black">
                <asp:Label ID="LblFoodUse" runat="server" Text=""></asp:Label>
            </p>
        </div>
        <div class="tab-pane fade p-3" id="contact" role="tabpanel" aria-labelledby="contact-tab">
            <p class="text-Black">
                <asp:Label ID="LblFoodTerms" runat="server" Text=""></asp:Label>
            </p>
        </div>
    </div>
                </div>
                
            </div>
        </div>
    </div>
</asp:Content>

