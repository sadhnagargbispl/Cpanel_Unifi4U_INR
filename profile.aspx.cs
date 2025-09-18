using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class profile : System.Web.UI.Page
{
    double _dblAvailLeg = 0;
    private clsGeneral dbGeneral = new clsGeneral();
    private cls_DataAccess dbConnect;

    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dRead;

    private string strQuery, strCaptcha;
    DataTable tmpTable = new DataTable();
    // AccClass.MyAccClass.NewClass QryCls = new AccClass.MyAccClass.NewClass();
    int minSpnsrNoLen, minScrtchLen;

    double Upln, dblSpons, dblTehsil, dblDistrict, dblIdNo;
    DateTime CurrDt;
    string[] montharray = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    int LastInsertID = 0;
    string scrname;
    DAL Obj = new DAL();
    string IsoStart;
    string IsoEnd;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.BtnSubmit.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnSubmit));
            this.BtnOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnOtp));
            this.ResendOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.ResendOtp));
            txtReferalId.ReadOnly = true;
            if (Session["Status"] == null)
            {
                Response.Redirect("Logout.aspx");
            }

            if (!Page.IsPostBack)
            {
                Session["OtpCount"] = 0;
                Session["OtpTime"] = null;
                Session["Retry"] = null;
                Session["OTP_"] = null;
                FillCountryName();
                FillDetail();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }

    }
    private void FillCountryName()
    {
        try
        {
            DataTable dt = new DataTable();
            strQuery = IsoStart + " SELECT CId, CountryName FROM " + Obj.dBName + "..M_CountryMaster WHERE ACTIVESTATUS = 'Y' AND cid = 1 ORDER BY CountryName" + IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];

            ddlCountryName.DataSource = dt;
            ddlCountryName.DataValueField = "CId";
            ddlCountryName.DataTextField = "CountryName";
            ddlCountryName.DataBind();
            FillCountryMaster(Convert.ToInt32(ddlCountryName.SelectedValue));
        }
        catch (Exception ex)
        {
            // Handle the exception or log it as needed
        }
    }
    private void FillCountryMaster(int CID)
    {
        try
        {
            DataTable dt = new DataTable();
            strQuery = IsoStart + " SELECT CId, StdCode FROM " + Obj.dBName + "..M_CountryMaster WHERE ACTIVESTATUS = 'Y' and CId = '" + ddlCountryName.SelectedValue + "' ORDER BY StdCode" + IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];

            if (dt.Rows.Count > 0)
            {
                ddlCountry.Text = dt.Rows[0]["StdCode"].ToString();
            }
        }
        catch (Exception ex)
        {
            // Handle the exception or log it as needed
        }
    }
    private void FillDetail()
    {
        try
        {
            string idverified = "";
            string sql = IsoStart + "exec sp_MemDtlINR ' and mMst.Formno=''" + Session["Formno"] + "'''  " + IsoEnd;
            DataTable dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, sql).Tables[0];

            if (dt.Rows.Count > 0)
            {
                txtReferalId.Text = dt.Rows[0]["RefIDNo"] == DBNull.Value ? "" : dt.Rows[0]["RefIDNo"].ToString();
                TxtReferalNm.Text = dt.Rows[0]["RefName"].ToString();
                TxtUplinerid.Text = dt.Rows[0]["UpLnIDNo"] == DBNull.Value ? "" : dt.Rows[0]["UpLnIDNo"].ToString();
                TxtUplinerName.Text = dt.Rows[0]["UpLnName"].ToString();
                hdnidno.Value = dt.Rows[0]["IDno"].ToString();
                txtFrstNm.Text = dt.Rows[0]["MemName"].ToString();
                lblPosition.Text = dt.Rows[0]["LegNo"].ToString() == "1" ? "Left" : "Right";
                txtFNm.Text = dt.Rows[0]["MemFname"].ToString();
                TxtDobDate.Text = Convert.ToDateTime(dt.Rows[0]["MemDob"]).ToString("dd-MMM-yyyy");
                TxtDoj.Text = Convert.ToDateTime(dt.Rows[0]["Doj"]).ToString("dd-MMM-yyyy");
                ddlCountryName.SelectedValue = dt.Rows[0]["CountryId"].ToString();
                FillCountryMaster(Convert.ToInt32(ddlCountryName.SelectedValue));

                ddlCountry.Text = dt.Rows[0]["UserCode"].ToString();
                txtPhNo.Text = dt.Rows[0]["PhN1"].ToString();
                txtMobileNo.Text = dt.Rows[0]["Mobl"].ToString();
                txtEMailId.Text = dt.Rows[0]["EMail"].ToString();
                txtNominee.Text = dt.Rows[0]["NomineeName"].ToString();
                txtRelation.Text = dt.Rows[0]["Relation"].ToString();

                if (dt.Rows[0]["INRActiveStatus"].ToString() == "N")
                {
                    txtFrstNm.Enabled = false;
                    ddlCountryName.Enabled = true;
                    txtMobileNo.Enabled = true;
                    txtEMailId.Enabled = true;

                    ddlCountry.Text = dt.Rows[0]["UserCode"].ToString();

                    if (txtMobileNo.Text == "0")
                    {
                        txtMobileNo.Enabled = true;
                        ddlCountry.Enabled = true;
                    }
                    else
                    {
                        txtMobileNo.Enabled = false;
                        ddlCountry.Enabled = false;
                    }

                    
                }
                else
                {
                    if (dt.Rows[0]["INRActiveStatus"].ToString() == "Y")
                    {
                        TxtDoa.Text = Convert.ToDateTime(dt.Rows[0]["INRUpgradeDate"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        TxtDoa.Text = "";
                    }

                    if (txtMobileNo.Text == "0")
                    {
                        txtMobileNo.Enabled = true;
                        ddlCountry.Enabled = true;
                    }
                    else
                    {
                        txtMobileNo.Enabled = false;
                        ddlCountry.Enabled = false;

                        if (string.IsNullOrEmpty(dt.Rows[0]["UserCode"].ToString()))
                        {
                            ddlCountry.Text = "+91";
                        }
                        else
                        {
                            ddlCountry.Text = dt.Rows[0]["UserCode"].ToString();
                        }
                    }
                }
                

                if (!string.IsNullOrEmpty(txtReferalId.Text))
                {
                    BtnSubmit.Visible = false;
                    txtReferalId.Enabled = false;
                }
                else
                {
                    BtnSubmit.Visible = false;
                    txtReferalId.Enabled = false;
                }

                if (!string.IsNullOrEmpty(txtFrstNm.Text))
                {
                    BtnSubmit.Visible = false;
                    txtFrstNm.Enabled = false;
                }
                else
                {
                    BtnSubmit.Visible = true;
                    txtFrstNm.Enabled = true;
                }
                if (!string.IsNullOrEmpty(TxtDoj.Text))
                {
                    BtnSubmit.Visible = false;
                    TxtDoj.Enabled = false;
                }
                else
                {
                    BtnSubmit.Visible = true;
                    TxtDoj.Enabled = true;
                }

                if (!string.IsNullOrEmpty(TxtDoa.Text))
                {
                    BtnSubmit.Visible = false;
                    TxtDoa.Enabled = false;
                }
                else
                {
                    BtnSubmit.Visible = true;
                    TxtDoa.Enabled = true;
                }

                if (ddlCountryName.SelectedValue != "0") // Assuming SelectedValue is a string
                {
                    BtnSubmit.Visible = false;
                    ddlCountryName.Enabled = false;
                }
                else
                {
                    BtnSubmit.Visible = true;
                    ddlCountryName.Enabled = true;
                }

                
                if (!string.IsNullOrEmpty(txtEMailId.Text))
                {
                    BtnSubmit.Visible = false;
                    txtEMailId.Enabled = false;
                }
                else
                {
                    BtnSubmit.Visible = true;
                    txtEMailId.Enabled = true;
                }

                if (txtMobileNo.Text != "0")
                {
                    BtnSubmit.Visible = false;
                    txtMobileNo.Enabled = false;

                }
                else
                {
                    BtnSubmit.Visible = true;
                    txtMobileNo.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void ddlCountryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillCountryMaster(Convert.ToInt32(ddlCountryName.SelectedValue));
        }
        catch (Exception ex)
        {
            // Handle the exception or log it as needed
        }
    }
    private string ClearInject(string strObj)
    {
        try
        {
            strObj = strObj.Replace(";", "").Replace("'", "").Replace("=", "");
            return strObj.Trim();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            return string.Empty; // Return empty string in case of an exception
        }
    }
    private string DisableTheButton(System.Web.UI.Control pge, System.Web.UI.Control btn)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
            sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
            sb.Append("this.value = 'Please wait...';");
            sb.Append("this.disabled = true;");
            sb.Append(pge.Page.GetPostBackEventReference(btn));
            sb.Append(";");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public bool SendMail(string otp)
    {
        try
        {
            string strMsg = "";
            string emailAddress = Session["EMail"].ToString().Trim();
            System.Net.Mail.MailAddress sendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress sendTo = new System.Net.Mail.MailAddress(emailAddress);
            System.Net.Mail.MailMessage myMessage = new System.Net.Mail.MailMessage(sendFrom, sendTo);

            strMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\"> " +
                     "<tr>" +
                     "<td>" +
                     "Your OTP for Profile is <span style=\"font-weight: bold;\">" + otp + "</span> (valid for 5 minutes)." +
                     "<br />" +
                     "</td>" +
                     "</tr>" +
                     "</table>";

            myMessage.Subject = "Thanks For Connecting!!!";
            myMessage.Body = strMsg;
            myMessage.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());
            smtp.Send(myMessage);
            txtReferalId.Enabled = false;
            
            txtFrstNm.ReadOnly = true;
            CmbType.Enabled = true;
            txtFNm.ReadOnly = true;
            txtEMailId.Enabled = false;
            txtMobileNo.Enabled = false;
            TxtDoa.Enabled = false;
            TxtDoj.Enabled = false;
            txtFNm.Enabled = false;
            txtFrstNm.Enabled = false;
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
           

                int OTP_ = 0;
                Random rs = new Random();
                OTP_ = rs.Next(100001, 999999);

                if (Session["OTP_"] == null)
                {
                    if (SendMail(OTP_.ToString()))
                    {
                        Session["OtpTime"] = DateTime.Now.AddMinutes(5);
                        Session["Retry"] = "1";
                        Session["OTP_"] = OTP_;
                        int i = 0;
                        string query = "";
                        query = "INSERT INTO AdminLogin (UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID, ForType) ";
                        query += "VALUES ('" + Session["formno"] + "', '" + Session["MemName"] + "', '" + TxtOtp.Text + "', '0', '" + OTP_ + "', GETDATE(), '" + OTP_ + "', ";
                        query += "'" + Session["EMail"].ToString().Trim() + "', 'ProfileINR')";
                        i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, query));
                        if (i > 0)
                        {
                            divotp.Visible = true;
                            BtnSubmit.Visible = false;
                            BtnOtp.Visible = true;
                            ResendOtp.Visible = true;
                            string scrname = "<script language='javascript'>alert('OTP Sent On Mail');</script>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                            return;
                        }
                        else
                        {
                            string scrname = "<script language='javascript'>alert('Try Later');</script>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                            return;
                        }
                    }
                    else
                    {
                        string scrname = "<script language='javascript'>alert('OTP Try Later');</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                        return;
                    }
                }
                else
                {
                    txtReferalId.Enabled = false;
                    txtFrstNm.ReadOnly = true;
                    CmbType.Enabled = true;
                    txtFNm.ReadOnly = true;
                    txtEMailId.Enabled = false;
                    txtMobileNo.Enabled = false;
                    TxtDoa.Enabled = false;
                    TxtDoj.Enabled = false;
                    txtFNm.Enabled = false;
                    txtFrstNm.Enabled = false;
                    divotp.Visible = true;
                    BtnSubmit.Visible = false;
                    BtnOtp.Visible = true;
                    ResendOtp.Visible = false;
                }
            
        }
        catch (Exception ex)
        {
        }
        //try
        //{
        //    UpdateDb();
        //}
        //catch (Exception ex)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true); ;

        //}
    }
    protected void BtnOtp_Click(object sender, EventArgs e)
    {
        try
        {
            string scrname = "";
            string email = "";
            DataTable dt = new DataTable();
            string transPassw = TxtOtp.Text.Trim();
            DataTable dt1 = new DataTable();
            Session["OtpCount"] = Convert.ToInt32(Session["OtpCount"]) + 1;

            if (Session["OTP_"] != null && Session["OTP_"].ToString() == transPassw)
            {
                string query = "SELECT TOP 1 * FROM " + Obj.dBName + "..AdminLogin AS a WHERE EmailID = '" + Session["EMail"].ToString().Trim() + "' AND emailotp = '" + transPassw + "' AND ForType = 'ProfileINR' ORDER BY AID DESC";

                dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];

                if (dt1.Rows.Count > 0)
                {
                    UpdateDb();
                }
            }
            else
            {
                TxtOtp.Text = "";

                if (Convert.ToInt32(Session["OtpCount"]) >= 3)
                {
                    Session["OtpCount"] = 0;
                    scrname = "<script language='javascript'>alert('You have tried 3 times with invalid OTP.\\nPlease generate OTP again.');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", scrname, true);
                    ResendOtp.Visible = true;
                    BtnOtp.Visible = true;
                    divotp.Visible = false;
                }
                else
                {
                    scrname = "<script language='javascript'>alert('Invalid OTP.');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", scrname, true);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }
    protected void ResendOtp_Click(object sender, EventArgs e)
    {
        try
        {
            Session["OTP_"] = "";
            int OTP_ = 0;
            Random rs = new Random();
            OTP_ = rs.Next(100001, 999999);

            if (SendMail(OTP_.ToString()))
            {
                string emailId = Session["Email"].ToString();
                string memberName = "";
                string mobileNo = "0";
                string sms = "";
                int result = 0;
                string query = "";

                query = "INSERT INTO AdminLogin (UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID, ForType) " +
                        "VALUES ('0', '" + memberName + "', '" + TxtOtp.Text + "', '" + mobileNo + "', '" + OTP_ + "', GETDATE(), '" + OTP_ + "', " +
                        "'" + Session["EMail"].ToString().Trim() + "', 'ProfileINR')";

                result = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, query));

                if (result > 0)
                {
                    Session["OTP_"] = OTP_;
                    divotp.Visible = true;
                    BtnOtp.Visible = true;
                    ResendOtp.Visible = true;
                    string scrname = "<script language='javascript'>alert('OTP Sent On Mail');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
                else
                {
                    string scrname = "<script language='javascript'>alert('Try Later');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
            }
            else
            {
                string scrname = "<script language='javascript'>alert('OTP Try Later');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void UpdateDb()
    {
        try
        {
            if (hdnidno.Value.ToString().ToUpper() != Session["IDNo"].ToString().ToUpper())
            {
                string scrname = "<SCRIPT language='javascript'>alert('Profile cannot be changed, Please try later.!!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }

            string strQry = "";
            DateTime strDOB;
            string Remark = "";
            string MembName = "";
            string Password = "";
            string TransactionPassword = "";

            try
            {
                string str = "";
                DataTable dt1 = new DataTable();

                try
                {
                    strDOB = DateTime.Parse(TxtDobDate.Text);
                }
                catch (Exception)
                {
                    strDOB = DateTime.Now;
                }

                txtPhNo.Text = string.IsNullOrEmpty(txtPhNo.Text) ? "0" : txtPhNo.Text;
                str = "select * from M_MemberMaster where Formno='" + Session["Formno"] + "'";
                dt1 = Obj.GetData(str);

                if (dt1.Rows.Count > 0)
                {
                    MembName = dt1.Rows[0]["MemFirstName"].ToString() + " " + dt1.Rows[0]["MemLastName"].ToString();
                    Password = dt1.Rows[0]["Passw"].ToString();
                    TransactionPassword = dt1.Rows[0]["EPassw"].ToString();

                    if (ClearInject(dt1.Rows[0]["MemFirstName"].ToString()) != ClearInject(txtFrstNm.Text))
                    {
                        Remark += " Member Name,";
                    }

                    if (dt1.Rows[0]["MemDob"].ToString() != strDOB.ToString())
                    {
                        Remark += " Dob,";
                    }

                    if (Convert.ToInt32(dt1.Rows[0]["CountryId"]) != Convert.ToInt32(ddlCountryName.SelectedValue))
                    {
                        Remark += " Country,";
                    }

                    if (ClearInject(dt1.Rows[0]["PhN1"].ToString()) != ClearInject(txtPhNo.Text))
                    {
                        Remark += " PhoneNo,";
                    }

                    if (ClearInject(dt1.Rows[0]["usercode"].ToString()) != ClearInject(ddlCountry.Text))
                    {
                        Remark += " UserCode,";
                    }

                    if (ClearInject(dt1.Rows[0]["Mobl"].ToString()) != ClearInject(txtMobileNo.Text))
                    {
                        Remark += " MobileNo,";
                    }

                    if (ClearInject(dt1.Rows[0]["Email"].ToString()) != ClearInject(txtEMailId.Text))
                    {
                        Remark += " Email,";
                    }

                    if (ClearInject(dt1.Rows[0]["NomineeName"].ToString()) != ClearInject(txtNominee.Text))
                    {
                        Remark += " NomineeName,";
                    }

                    if (ClearInject(dt1.Rows[0]["Relation"].ToString()) != ClearInject(txtRelation.Text))
                    {
                        Remark += " Relation,";
                    }

                    

                    Remark += " Changed";
                }

                strQry = "EXEC Sp_UpdateMemberProfileUpdate '" + Session["FormNo"] + "', " +
         "'" + ClearInject(txtFrstNm.Text.ToUpper()) + "', '" +
         ClearInject(txtFNm.Text.ToUpper()) + "', " +
         "'" + strDOB.ToString("dd-MMM-yyyy") + "', '" +
         ClearInject(txtPhNo.Text) + "', " +
         "'" + ClearInject(txtMobileNo.Text) + "', '" +
         ClearInject(txtEMailId.Text) + "', " +
         "'" + ClearInject(txtNominee.Text.ToUpper()) + "', '" +
         ClearInject(txtRelation.Text.ToUpper()) + "', " +
         "'', '" +
         ddlCountryName.SelectedValue + "', '" +
         ddlCountry.Text + "';";
                string Qry = "INSERT INTO TempMemberMaster (MId, SessId, TransNo, IdNo, FormNo, " +
              "KitId, UpLnFormNo, RefId, LegNo, RefLegNo, RefFormNo, CardNo, Prefix, MemFirstName, MemLastName, MemRelation, MemFName, MemDOB, " +
              "MemGender, MarrgDate, MemOccupation, Address1, Address2, Post, Tehsil, City, CityCode, District, DistrictCode, StateCode, CountryId, " +
              "CountryName, PinCode, STDCode, PhN1, PhN2, Fax, Mobl, ComMode, Passw, Doj, NomineeName, NomineeDOB, NomineeAge, Relation, PanNo, BankId, " +
              "AcNo, IFSCode, ChDDNo, ChDDBankId, EMail, BV, Imported, UpGrdSessId, IsPanCard, E_MainPassw, EPassw, PlanId, Remarks, Fld1, Fld2, Fld3, " +
              "Fld4, Fld5, ActiveStatus, RecTimeStamp, LastModified, UserCode, UserId, IsMarried, memPic, KitPlanId, IsTopUp, DSessId, UpgradeDate, " +
              "BankName, BillNo, UpgrdDSessId, FSessId, RP, SP, CancelTopUp, MICRCode, BranchName, HostIp, ChDDDate, ChDDBank, ChDDBranch, Fld6, PID, " +
              "Paymode, ProfilePic, IsBlock, BlockRemark, BlockDate, DeliveryAddress, DeliveryCenter, PV, AadharNo, AadharNo2, AadharNo3, MFormno, " +
              "IsCompany, RegType, RegNo, PostalPin, PostalStateCode, PostalCityCode, PostalAreaCode, AreaCode, PostalDistrictCode, ChangeType, " +
              "TRecTimeStamp, CType, rowguid, WalletAddress,INRKitid,INRActivestatus,INRUpGrdSessId,INRUpgradeDate,INRUpgrdDSessId) " +
              "SELECT MId, SessId, TransNo, IdNo, FormNo, KitId, UpLnFormNo, RefId, LegNo, RefLegNo, RefFormNo, CardNo, Prefix, MemFirstName, " +
              "MemLastName, MemRelation, MemFName, MemDOB, MemGender, MarrgDate, MemOccupation, Address1, Address2, Post, Tehsil, City, CityCode, " +
              "District, DistrictCode, StateCode, CountryId, CountryName, PinCode, STDCode, PhN1, PhN2, Fax, Mobl, ComMode, Passw, Doj, NomineeName, " +
              "NomineeDOB, NomineeAge, Relation, PanNo, BankId, AcNo, IFSCode, ChDDNo, ChDDBankId, EMail, BV, Imported, UpGrdSessId, IsPanCard, " +
              "E_MainPassw, EPassw, PlanId, Remarks, Fld1, Fld2, Fld3, Fld4, Fld5, ActiveStatus, RecTimeStamp, LastModified, UserCode, UserId, " +
              "IsMarried, memPic, KitPlanId, IsTopUp, DSessId, UpgradeDate, BankName, BillNo, UpgrdDSessId, FSessId, RP, SP, CancelTopUp, MICRCode, " +
              "BranchName, HostIp, ChDDDate, ChDDBank, ChDDBranch, Fld6, PID, Paymode, ProfilePic, IsBlock, BlockRemark, BlockDate, DeliveryAddress, " +
              "DeliveryCenter, PV, AadharNo, AadharNo2, AadharNo3, MFormno, IsCompany, RegType, RegNo, PostalPin, PostalStateCode, PostalCityCode, " +
              "PostalAreaCode, AreaCode, PostalDistrictCode, 'Update Profile - " + Context.Request.UserHostAddress.ToString() + "', GETDATE(), 'U', null, " +
              "WalletAddress,INRKitid,INRActivestatus,INRUpGrdSessId,INRUpgradeDate,INRUpgrdDSessId " +
              "FROM M_MemberMaster WHERE FormNo='" + Convert.ToInt32(Session["FormNo"]) + "'";
                Qry += " INSERT INTO UserHistory (UserId, UserName, PageName, Activity, ModifiedFlds, RecTimeStamp, MemberId) VALUES " +
        "(0, '" + Session["MemName"] + "', 'Profile', 'Profile Update', '" + Remark + "', GETDATE(), '" + Session["FormNo"] + "')";
                Qry += strQry;
                int i = Obj.SaveData(Qry);
                string message = (i != 0) ? "Profile Successfully Updated.!" : "Try Again Later.!";
                string url = "Profile.aspx";
                string script = "window.onload = function(){ alert('" + message + "'); window.location = '" + url + "'; }";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);

                BtnSubmit.Visible = true;
                //BtnOtp.Visible = false;
                //divotp.Visible = false;
            }
            catch (Exception e)
            {
                string scrname = "<SCRIPT language='javascript'>alert('" + e.Message + "');</SCRIPT>";
                ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname, true);
                dbGeneral.myMsgBx(e.Message);
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
}
