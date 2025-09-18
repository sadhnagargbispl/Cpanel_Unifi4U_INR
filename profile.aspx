<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="profile.aspx.cs" Inherits="profile" %>


<asp:Content ID="content2" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="assets/jquery.min.js">
    </script>

    <script type="text/javascript" src="assets/jquery.validationEngine-en.js"></script>

    <script type="text/javascript" src="assets/jquery.validationEngine.js"></script>

    <link href="assets/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var jq = $.noConflict();
        function pageLoad(sender, args) {
            jq(document).ready(function () {

                jq("#aspnetForm").validationEngine('attach', { promptPosition: "topRight" });
            });

            jq("#<%=BtnSubmit.ClientID %>").click(function () {


                var valid = jq("#aspnetForm").validationEngine('validate');
                var vars = jq("#aspnetForm").serialize();
                if (valid == true) {
                    return true;

                } else {
                    return false;
                }
            });
        }


    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Profile
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <div class="centered">
                        <div class="clr">
                            <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                        </div>
                        <div class="col-md-6">
                            <h4>Your Placement Detail
                            </h4>
                            <div class="form-group">
                                <label>
                                    Refferal ID<span class="red">*</span></label>
                                <asp:TextBox ID="txtReferalId" CssClass="form-control" runat="server" AutoPostBack="True"></asp:TextBox>
                            </div>
                            <div class="form-group" id="DivSponsorName" runat="server" visible="false">
                                <label>
                                    Sponsor Name<span class="red">*</span></label>
                                <asp:TextBox ID="TxtReferalNm" class="form-control" runat="server" Enabled="False"></asp:TextBox>
                            </div>
                            <div class="form-group" id="DivUplinerId" runat="server" visible="false">
                                <label>
                                    Placement ID<span class="red">*</span></label>
                                <asp:TextBox ID="TxtUplinerid" class="form-control" runat="server" AutoPostBack="True"
                                    Enabled="False"></asp:TextBox>
                            </div>
                            <div class="form-group " id="DivUplinerName" runat="server" visible="false">
                                <label>
                                    Placement Name<span class="red">*</span></label>
                                <asp:TextBox ID="TxtUplinerName" class="form-control" runat="server" Enabled="False"></asp:TextBox>
                            </div>
                            <div class="form-group greybt" style="display: none">
                                <label>
                                    Position<span class="red">*</span></label>
                                <asp:TextBox ID="lblPosition" class="form-control" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                            <h4>Personal Detail
                            </h4>
                            <div class="form-group ">
                                <label>
                                    Your Name<span class="red">*</span></label>
                                <asp:HiddenField ID="hdnidno" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtFrstNm" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                    runat="server" ValidationGroup="eInformation"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label>
                                    Date Of Joining<span class="red">*</span></label>
                                <asp:TextBox ID="TxtDoj" CssClass="form-control" runat="server" ValidationGroup="eInformation"
                                    ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label>
                                    Date Of Activation</label>
                                <asp:TextBox ID="TxtDoa" CssClass="form-control" runat="server" ValidationGroup="eInformation"
                                    ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group " style="display: none;">
                                <label>
                                    Father's Name</label>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <asp:DropDownList CssClass="form-control" ID="CmbType" runat="server">
                                            <asp:ListItem Value="S/O" Text="S/O"></asp:ListItem>
                                            <asp:ListItem Value="W/O" Text="W/O"></asp:ListItem>
                                            <asp:ListItem Value="C/O" Text="C/O"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-10" style="padding-left: 0px;">
                                        <asp:TextBox ID="txtFNm" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group  greybt" style="display: none;">
                                <label>
                                    Date of Birth</label>
                                <asp:TextBox ID="TxtDobDate" class="form-control" runat="server"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtDobDate"
                                    Format="dd-MM-yyyy"></ajaxToolkit:CalendarExtender>
                            </div>
                            <div class="form-group " style="display: none;">
                                <label>
                                    Door No. <span class="red">*</span></label>
                                <asp:TextBox ID="txtDoorNo" onkeypress="return isNumberKey(event);" CssClass="form-control validate[required]"
                                    runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group " style="display: none;">
                                <label>
                                    Street Name. <span class="red">*</span></label>
                                <asp:TextBox ID="txtStreetName" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group " style="display: none;">
                                <label>
                                    Area Name <span class="red">*</span></label>
                                <asp:TextBox ID="txtAreaName" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group " style="display: none;">
                                <label>
                                    Post Office <span class="red">*</span></label>
                                <asp:TextBox ID="txtLandmark" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group " style="display: none;">
                                <label>
                                    District <span class="red">*</span></label>
                                <asp:TextBox ID="txtDistrict" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label>
                                    Country <span class="red">*</span></label>
                                <asp:DropDownList ID="ddlCountryName" runat="server" CssClass="form-control" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group " style="display: none;">
                                <label>
                                    State<span class="red">*</span></label>
                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group " style="display: none;">
                                <label>
                                    Pin Code <span class="red">*</span></label>
                                <asp:TextBox ID="txtPinCode" onkeypress="return isNumberKey(event);" CssClass="form-control validate[required,custom[pincode]]"
                                    runat="server" MaxLength="6" ValidationGroup="eInformation"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label>
                                    Mobile No.<span class="red">*</span></label>
                                <div class="row">
                                    <div class="col-md-2">
                                        <%--  <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" ReadOnly="true">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="ddlCountry" CssClass="form-control "
                                            runat="server" ValidationGroup="eInformation" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtMobileNo" onkeypress="return isNumberKey(event);" CssClass="form-control validate[required]"
                                            runat="server" MaxLength="10" ValidationGroup="eInformation"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <label>
                                    Mobile No2.</label>
                                <asp:TextBox ID="txtPhNo" onkeypress="return isNumberKey(event);" CssClass="form-control"
                                    runat="server" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="form-group greybt">
                                <label>
                                    E-Mail ID</label>
                                <asp:TextBox ID="txtEMailId" CssClass="form-control validate[custom[email]]" runat="server"></asp:TextBox>
                            </div>

                            <%-- <h4>
                                            <span>Nominee Detail</span>
                                        </h4>--%>
                            <div class="form-group " style="display: none;">
                                <label>
                                    Nominee Name</label>
                                <asp:TextBox ID="txtNominee" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                    runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group greybt " style="display: none;">
                                <label>
                                    Relation</label>
                                <asp:TextBox ID="txtRelation" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                    runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group " runat="server" id="divSendOtp">
                                <asp:Button ID="BtnSubmit" runat="server" Text="Send Otp" CssClass="btn btn-primary"
                                    ValidationGroup="eInformation" OnClick="BtnSubmit_Click" />
                            </div>
                            <div runat="server" id="divotp" visible="false">
                                <div class="form-group">
                                    <label for="idproof">
                                        Enter Otp*
                                    </label>
                                    <asp:TextBox ID="TxtOtp" CssClass="form-control" onkeypress="return isNumberKey(event);"
                                        runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="None" ControlToValidate="TxtOtp"
                                        runat="server" ErrorMessage="Otp Required" SetFocusOnError="true" ValidationGroup="Save1">                                
                                    </asp:RequiredFieldValidator>
                                    <b>
                                        <asp:Label ID="lblOtp" runat="server" Text="Label" Visible="false"></asp:Label></b>
                                </div>
                                <div class="form-group ">

                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
                                        ShowSummary="False" ValidationGroup="eInformation" />
                                </div>
                            </div>
                            <div class="form-group ">
                                <asp:Button ID="BtnOtp" runat="server" Text="Update" class="btn btn-primary" Visible="False"
                                    ValidationGroup="Save" OnClick="BtnOtp_Click" />
                                <asp:Button ID="ResendOtp" runat="server" Text="Resend Otp" class="btn btn-primary"
                                    Visible="False" OnClick="ResendOtp_Click" />
                                &nbsp;<asp:Button ID="CmdCancel" runat="server" Text="Cancel" CssClass="btn btn-primary"
                                    ValidationGroup="Form-Reset" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

