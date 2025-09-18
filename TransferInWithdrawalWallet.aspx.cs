using ClosedXML.Excel;
using System;
using System.CodeDom;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Activities.Expressions;
using System.Xml;
using Newtonsoft.Json;
using System.Text;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using Irony;
public partial class TransferInWithdrawalWallet : System.Web.UI.Page
{
    private SqlConnection Conn;
    private SqlCommand Comm;
    private SqlDataAdapter Adp;
    private SqlDataReader Dr;
    private DAL ObjDal = new DAL();
    private string Query;
    private int Todate;
    private int BankID = 0;
    private string BranchName = "";
    private string PayName = "";
    private string IFSCode = "";
    private string Acno = "";
    private string PanNo = "";
    private string MobileNo = "";
    private string ISpan = "";
    private clsGeneral clsgen = new clsGeneral();
    private string scrname = "";
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BtnSubmit.Attributes.Add("onclick", DisableTheButton(Page, BtnSubmit));
            BtnOtp.Attributes.Add("onclick", DisableTheButton(Page, BtnOtp));
            ResendOtp.Attributes.Add("onclick", DisableTheButton(Page, ResendOtp));

            if (Session["Status"] as string != "OK")
            {
                Response.Redirect("Logout.aspx");
            }
            string walletType = ddlWalletType.SelectedValue;
            if (!IsPostBack)
            {
                Session["OtpCount"] = 0;
                Session["OtpTime"] = null;
                Session["Retry"] = null;
                Session["OTP_"] = null;
                Fill_WalletType();
                HdnCheckTrnns.Value = GenerateRandomString();
                if (FillDetailUpdate())
                {
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
    private string FillStopWithdrawal()
    {
        string RStatus = "";
        try
        {
            string query = "";
            query = "Exec Sp_GetStopWithdrawalCheck '" + Session["FormNo"] + "' ";
            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                RStatus = dt.Rows[0]["RStatus"].ToString();
            }
            else
            {
                RStatus = "N";
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
            RStatus = "";
        }
        return RStatus;
    }
    private void Fun_Sp_GetCryptoAPIFor_FundWithdraw_Admin()
    {
        try
        {
            string sql = "";
            DataTable dt_API_Master = new DataTable();
            sql = " Exec Sp_GetCryptoAPIFor_FundWithdraw_Admin";
            dt_API_Master = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            Session["SinglePayout"] = dt_API_Master.Rows[0]["APIURL"].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private bool GetReqStatus()
    {
        bool result = false;
        try
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string remark = "Debited for " + ddlWalletType.SelectedItem.Text + " transfer To Withdrawal Wallet ";
            string strSql = ObjDal.Isostart + " exec SP_GetReqStatusUpdate_1 '" + Session["FormNo"] + "','" + remark + "'" + ObjDal.IsoEnd;
            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(dt.Rows[0]["Cnt"]);
                    if (Convert.ToInt32(dt.Rows[0]["Cnt"]) == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            return result;
        }
    }
    private void Fill_WalletType()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = ObjDal.Isostart + " exec Sp_GetWalletType_1 " + ObjDal.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ddlWalletType.DataSource = dt;
                ddlWalletType.DataTextField = "WalletName";
                ddlWalletType.DataValueField = "AcType";
                ddlWalletType.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private bool GetWithdrawlStatus(out string alertMessage)
    {
        alertMessage = "";
        try
        {
            bool result = false;
            DataTable dt = new DataTable();

            string strSql = ObjDal.Isostart + " Exec sp_GetWithdwarlDate_1 '" + Session["formno"] + "','" + ddlWalletType.SelectedValue + "'" + ObjDal.IsoEnd;
            DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                string da = dt.Rows[0]["da"].ToString().ToUpper();
                alertMessage = dt.Rows[0]["msg"].ToString(); // message from DB

                if (da == "Y")
                {
                    result = true;
                    alertMessage = ""; // don't show message if allowed
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
    }
    protected void ddlWalletType_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtReqAmt.Text = "0";
        TxtTds.Text = "0";
        txtwithdrawls.Text = "0";
        GetFundwithdrawalLimit();
        string message;
        if (!GetReqStatus())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('This fund has been already transferred today.!');location.replace('Index.aspx');", true);
            return;
        }
        if (!GetWithdrawlStatus(out message) && !string.IsNullOrEmpty(message))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + message + "');location.replace('Index.aspx');", true);
            return;
        }
        if (!GetAmountStatus())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('You Are Not Authorised For Withdrawal.Because Of Your Wallet Balance Is Low.!');location.replace('Index.aspx');", true);
            return;
        }
        try
        {
            string selectedValue = ddlWalletType.SelectedValue;
            DataTable dt = new DataTable();
            string strSql = " Exec Sp_GetStackingWithdrawalConditionUpdate '" + Session["formno"] + "','" + selectedValue + "' ";
            DataSet ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["Income"]) > 0)
                {
                    LblWithdrawalConditonIncome.Visible = true;
                    TxtReqAmt.Enabled = true;
                    LblWithdrawalConditonIncome.Text = dt.Rows[0]["Msg"].ToString();
                }
                else
                {
                    TxtReqAmt.Enabled = false;
                    LblWithdrawalConditonIncome.Visible = true;
                    LblWithdrawalConditonIncome.Text = dt.Rows[0]["Msg"].ToString();
                }
            }

            TxtCredit.Text = Amount().ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        try
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
            sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
            sb.Append("this.value = 'Please Wait...';");
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
    private void CheckAmount()
    {
        int temp = 0;
        if (Convert.ToDecimal(TxtReqAmt.Text) < Convert.ToDecimal(Session["FMinwithdrawl"]))
        {
            string scrname = "<script language='javascript'>alert('Minimum request amount is " + Session["FMinwithdrawl"] + ".');</script>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            BtnSubmit.Enabled = true;
            TxtTds.Text = "0";
            txtwithdrawls.Text = "0";

            TxtReqAmt.Text = "0";
            TxtFrxInfra.Text = string.Empty;
        }
        else if (Convert.ToDecimal(TxtReqAmt.Text) > Convert.ToDecimal(Session["MaxWithdrawl"]))
        {
            string scrname = "<script language='javascript'>alert('Maximum request amount is " + Session["MaxWithdrawl"] + "');</script>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            BtnSubmit.Enabled = true;
            TxtTds.Text = "0";
            txtwithdrawls.Text = "0";

            TxtReqAmt.Text = "0";
            TxtFrxInfra.Text = string.Empty;
            return;
        }
        else if (Convert.ToDecimal(TxtReqAmt.Text) > Amount())
        {
            string scrname = "<script language='javascript'>alert('Insufficient Balance.');</script>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            BtnSubmit.Enabled = true;
            TxtTds.Text = "0";
            txtwithdrawls.Text = "0";

            TxtReqAmt.Text = "0";
            TxtFrxInfra.Text = string.Empty;
        }
        else if (Convert.ToDecimal(TxtReqAmt.Text) == 0)
        {
            string scrname = "<script language='javascript'>alert('Minimum request amount is " + Session["FMinwithdrawl"] + ".');</script>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            BtnSubmit.Enabled = true;
            TxtTds.Text = "0";
            txtwithdrawls.Text = "0";

            TxtReqAmt.Text = "0";
            TxtFrxInfra.Text = string.Empty;
        }
        else
        {

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string strSql = ObjDal.Isostart + " Select * from  [dbo].[ufnGetWithdrawalCharge]('" + TxtReqAmt.Text + "','" + Session["formno"] + "','" + ddlWalletType.SelectedValue + "') " + ObjDal.IsoEnd;
            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                TxtTds.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["AdminChargeAmount"]), 2).ToString();
                txtwithdrawls.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["NetAMount"]), 2).ToString();
            }
        }
    }
    private Decimal Amount()
    {
        try
        {
            Decimal RtrVal = 0;
            string Str = ObjDal.Isostart + "Select balance From dbo.ufnGetBalance('" + Session["FormNo"] + "','" + ddlWalletType.SelectedValue + "')" + ObjDal.IsoEnd;
            SqlConnection cnn = new SqlConnection(constr1);
            using (SqlDataReader dr = SqlHelper.ExecuteReader(cnn, CommandType.Text, Str))
            {
                if (dr.Read())
                {
                    RtrVal = Convert.ToDecimal(dr["Balance"]);
                }
            }
            return RtrVal;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private bool GetFundwithdrawalLimit()
    {
        try
        {
            DivTermsAndCondition.Visible = true;
            bool result = false;
            DataTable dt = new DataTable();
            string strSql = ObjDal.Isostart + " exec Sp_GetFundlimitUpdate '" + ddlWalletType.SelectedValue + "' " + ObjDal.IsoEnd;
            DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (ddlWalletType.SelectedValue == "M")
                {
                    DataTable dt_ = new DataTable();
                    string strSql_ = " Exec Sp_getRankCappingForWithdrawal '" + Session["formno"] + "' ";
                    DataSet ds_ = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql_);
                    dt_ = ds_.Tables[0];
                    Session["FMinwithdrawl"] = dt.Rows[0]["MinWithdrawl"];
                    Session["MinFWithdraDeduction"] = dt.Rows[0]["WithdrawlDeduction"];
                    Session["MaxWithdrawl"] = dt_.Rows[0]["Capping"];
                    result = true;
                }
                else
                {
                    Session["FMinwithdrawl"] = dt.Rows[0]["MinWithdrawl"];
                    Session["MinFWithdraDeduction"] = dt.Rows[0]["WithdrawlDeduction"];
                    Session["MaxWithdrawl"] = dt.Rows[0]["MaxWithdrawl"];
                    result = true;
                }
            }
            else
            {
                Session["FMinwithdrawl"] = 0;
                Session["MinFWithdraDeduction"] = 0;
                Session["MaxWithdrawl"] = 0;
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private bool GetAmountStatus()
    {
        try
        {
            bool result = false;
            DataTable dt = new DataTable();
            string strSql = ObjDal.Isostart + "Select balance From dbo.ufnGetBalance('" + Session["FormNo"] + "','" + ddlWalletType.SelectedValue + "')" + ObjDal.IsoEnd;
            DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = ds.Tables[0];

            if (Convert.ToDouble(dt.Rows[0]["balance"]) < Convert.ToDouble(Session["FMinwithdrawl"]))
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
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
            string walletType = ddlWalletType.SelectedValue;
            if (Convert.ToDecimal(txtwithdrawls.Text) == 0)
            {
                string scrName = "<script language='javascript'>alert('Minimum request amount is " + Session["FMinwithdrawl"] + ".');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                ResetFields();
                return;
            }

            if (ddlWalletType.SelectedValue == "Z")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Please Select Wallet Type.!');", true);
                return;
            }

            if (!GetAmountStatus())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('You do not have enough balance for withdrawal.');location.replace('Index.aspx');", true);
                return;
            }
            string str = ObjDal.Isostart + " Exec Sp_CheckTransctionPassword '" + Convert.ToInt32(Session["Formno"]) + "','" + TxtTransPass.Text.Trim() + "'" + ObjDal.IsoEnd;
            DataTable dts = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            if (dts.Rows.Count > 0)
            {
                otp_save_function();
            }
            else
            {
                string scrName = "<SCRIPT language='javascript'>alert('Invalid Wallet Password.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
            }
            //int OTP_ = 0;
            //Random rs = new Random();
            //OTP_ = rs.Next(100001, 999999);
            //if (Session["OTP_"] == null)
            //{
            //    if (SendMail(OTP_.ToString()))
            //    {
            //        Session["OtpTime"] = DateTime.Now.AddMinutes(5);
            //        Session["Retry"] = "1";
            //        Session["OTP_"] = OTP_;
            //        int i = 0;
            //        string query = "";
            //        query = "INSERT INTO AdminLogin (UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID, ForType) ";
            //        query += "VALUES ('" + Session["formno"] + "', '" + Session["MemName"] + "', '" + TxtOtp.Text + "', '0', '" + OTP_ + "', GETDATE(), '" + OTP_ + "', ";
            //        query += "'" + Session["EMail"].ToString().Trim() + "', 'transferWithdrawal')";
            //        i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, query));
            //        if (i > 0)
            //        {
            //            divotp.Visible = true;
            //            BtnSubmit.Visible = false;
            //            BtnOtp.Visible = true;
            //            ResendOtp.Visible = true;
            //            string scrname = "<script language='javascript'>alert('OTP Sent On Mail');</script>";
            //            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            //            return;
            //        }
            //        else
            //        {
            //            string scrname = "<script language='javascript'>alert('Try Later');</script>";
            //            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        string scrname = "<script language='javascript'>alert('OTP Try Later');</script>";
            //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            //        return;
            //    }
            //}
            //else
            //{
            //    TxtReqAmt.Enabled = false;
            //    ddlWalletType.Enabled = false;
            //    divotp.Visible = true;
            //    BtnSubmit.Visible = false;
            //    BtnOtp.Visible = true;
            //    ResendOtp.Visible = false;
            //}
        }
        catch (Exception ex)
        {
            string scrName = "<SCRIPT language='javascript'>alert('Try Later');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrName, false);
        }
    }
    private void ResetFields()
    {
        TxtReqAmt.Text = "0";
        TxtTds.Text = "0";
        txtwithdrawls.Text = "0";

        TxtFrxInfra.Text = "";
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
                     "Your OTP for " + ddlWalletType.SelectedItem.Text + " to Withdrawal Wallet is <span style=\"font-weight: bold;\">" + otp + "</span> (valid for 5 minutes)." +
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
            TxtReqAmt.Enabled = false;
            ddlWalletType.Enabled = false;
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
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
                string query = "SELECT TOP 1 * FROM " + ObjDal.dBName + "..AdminLogin AS a WHERE EmailID = '" + Session["EMail"].ToString().Trim() + "' AND emailotp = '" + transPassw + "' AND ForType = 'transferWithdrawal' ORDER BY AID DESC";
                dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];

                if (dt1.Rows.Count > 0)
                {
                    otp_save_function();
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
                        "'" + Session["EMail"].ToString().Trim() + "', 'transferWithdrawal')";

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
    protected void otp_save_function()
    {
        try
        {
            string strSql = "Insert into TrnFundUniqe (Transid, Rectimestamp) values (" + HdnCheckTrnns.Value + ", GETDATE())";
            int updateEffect = ObjDal.SaveData(strSql);

            if (updateEffect > 0)
            {
                string responseData = string.Empty;
                try
                {
                    string scrname = string.Empty;
                    Label1.Text = string.Empty;

                    if (decimal.Parse(TxtReqAmt.Text) < decimal.Parse(Session["FMinwithdrawl"].ToString()))
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Minimum Request Amount is " + Session["FMinwithdrawl"].ToString() + ".');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
                        return;
                    }

                    decimal result = 0;
                    if (Convert.ToDecimal(TxtReqAmt.Text) > Amount())
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Insufficient Balance.');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
                        return;
                    }
                    string Str_TrnFun = "";
                    string Str_TrnFun_Query = "";
                    string remark1 = "Received from " + ddlWalletType.SelectedItem.Text + "";
                    string remark = "Debited for " + ddlWalletType.SelectedItem.Text + " transfer To Withdrawal Wallet ";
                    string remarks = "" + ddlWalletType.SelectedItem.Text + " To Withdrawal Wallet Transfer Of " + TxtReqAmt.Text + " ";
                    Str_TrnFun = " exec Sp_TrnvoucherinsertWithdrawalwallet '" + Session["Formno"] + "','" + Convert.ToDecimal(TxtReqAmt.Text) + "','" + ddlWalletType.SelectedValue + "',";
                    Str_TrnFun += "'" + remark + "','" + remark1 + "','" + remarks + "'";
                    Str_TrnFun_Query = "BEGIN TRY BEGIN TRANSACTION " + Str_TrnFun + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH";
                    int i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_TrnFun_Query);
                    if (i > 0)
                    {
                        string responseResult = "success";
                        if (responseResult.ToUpper() == "SUCCESS")
                        {
                            TxtCredit.Text = Amount().ToString();
                            TxtReqAmt.Text = "0";
                            TxtTds.Text = "0";
                            txtwithdrawls.Text = "0";

                            TxtFrxInfra.Text = string.Empty;
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Key", "alert('Transfer Withdrawal Wallet Successfully.!');location.replace('TransferInWithdrawalWallet.aspx');", true);
                            return;
                        }
                        else
                        {
                            TxtReqAmt.Text = "0";
                            TxtTds.Text = "0";
                            txtwithdrawls.Text = "0";
                            TxtCredit.Text = Amount().ToString();
                            TxtFrxInfra.Text = string.Empty;
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Key", "alert('Your Request Is Rejected. Please Contact Admin.!');location.replace('TransferInWithdrawalWallet.aspx');", true);
                            return;
                        }
                    }
                    else
                    {
                        TxtReqAmt.Text = "0";
                        TxtTds.Text = "0";
                        txtwithdrawls.Text = "0";

                        TxtCredit.Text = Amount().ToString();
                        TxtFrxInfra.Text = string.Empty;
                        scrname = "<SCRIPT language='javascript'>alert('Try again later.');</SCRIPT>";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Login Error", scrname, false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    string scrname = "<SCRIPT language='javascript'>alert('" + ex.Message + "');</SCRIPT>";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Login Error", scrname, false);
                    return;
                }
            }
            else
            {
                string scrname = "<SCRIPT language='javascript'>alert('Something Went Wrong.');</SCRIPT>";
                ScriptManager.RegisterStartupScript(Page, GetType(), "Upgraded", scrname, false);
                return;
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that might be thrown
            // Log or process the exception as needed
            Console.WriteLine("Error in otp_save_function: " + ex.Message);
        }
    }
    public DataSet convertJsonStringToDataSet(string jsonString)
    {
        try
        {
            XmlDocument xd = new XmlDocument();
            jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlNodeReader(xd));
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public string GenerateRandomString()
    {
        string result = "";
        string currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        Random random = new Random();
        int randomNumber = random.Next(0, 999);
        string formattedDateTime = currentDateTime + randomNumber.ToString("D3");
        result = formattedDateTime;
        return result;
    }
    public static string Base64Encode(string plainText)
    {
        try
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void TxtReqAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlWalletType.SelectedValue == "I")
            //{

            DataTable dt = new DataTable();
            string strSql = "Exec Sp_GetStackingWithdrawalConditionUpdate '" + Session["formno"] + "','" + ddlWalletType.SelectedValue + "'";
            DataSet ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                decimal requestedAmount = Convert.ToDecimal(TxtReqAmt.Text);
                decimal TotalLimit = (Convert.ToDecimal(dt.Rows[0]["WithdrawalLimit"]));
                if (TotalLimit >= requestedAmount)
                {

                    CheckAmount();
                }
                else
                {
                    string scrname = "<script language='javascript'>alert('" + dt.Rows[0]["Msg"].ToString() + "');</script>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                    BtnSubmit.Enabled = true;
                    TxtTds.Text = "0";
                    txtwithdrawls.Text = "0";
                    TxtReqAmt.Text = "0";
                }
            }
        }
        catch (Exception ex)
        {
            string scrname = "<script language='javascript'>alert('Minimum request amount is " + Session["FMinwithdrawl"] + "');</script>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            BtnSubmit.Enabled = true;
            TxtTds.Text = "0";
            txtwithdrawls.Text = "0";

            TxtReqAmt.Text = "0";
        }
    }
    private bool FillDetailUpdate()
    {
        bool result = false;
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable tmpTable = new DataTable();
            string qq = "exec Sp_GetBankWallet " + Session["FormNo"].ToString();
            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qq);
            tmpTable = ds.Tables[0];
            dt = ds.Tables[0];
            if (tmpTable.Rows.Count > 0)
            {
                TxtWalletAddres.Text = tmpTable.Rows[0]["WalletAddress"].ToString();
                Session["transpassw"] = tmpTable.Rows[0]["EPassw"].ToString();
                Session["MemberKitId"] = tmpTable.Rows[0]["KitId"].ToString();
            }
            else
            {
                TxtWalletAddres.Text = "";
                Session["MemberKitId"] = "0";
            }
            result = !string.IsNullOrEmpty(TxtWalletAddres.Text);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }
}
