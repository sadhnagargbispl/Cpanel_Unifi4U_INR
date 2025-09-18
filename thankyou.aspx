<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="thankyou.aspx.cs" Inherits="thankyou" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
        <div class="row mb-2 profile-bar p-3 align-items-center">
            <div class="col-lg-8">
                <div class="thankyou">
                    <h4>
                        Thank You for Your Purchase!
                    </h4>
                    <p class="mt-2">
                        Thank you for your recent purchase of <b>
                            <asp:Label ID="LblPackageName" runat="server" Text=""></asp:Label></b>
                    </p>
                    <p class="mt-2">
                        Order Number:
                        <asp:Label ID="LblOrderNumber" runat="server" Text=""></asp:Label>
                    </p>
                    <p class="mt-2">
                        Order Date:
                        <asp:Label ID="LblOrderDate" runat="server" Text=""></asp:Label>
                    </p>
                </div>
                <div class="form-group d-flex">
                    <asp:Button ID="BtnProceedToPay" runat="server" Text="Redeem Now" class="btn btn-primary border-0 rounded-3 btn-sm fs-5 shadow-0" OnClick="BtnProceedToPay_Click"  />
                </div>
            </div>
            <div class="col-lg-4">
                <img src="mm_voucher/thankyou.png" alt="thank you" class="img-responsive img-fluid border-0">
            </div>
        </div>
    </div>
</asp:Content>

