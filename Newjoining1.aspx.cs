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
using System.Activities;
using System.ServiceModel.Activities;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
public partial class Newjoining1 : System.Web.UI.Page
{

    private double _dblAvailLeg = 0;
    private cls_DataAccess dbConnect;
    private DAL ObjDAL = new DAL();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dRead;
    public string DsnName, UserName, Passw;
    private string strQuery, strCaptcha;
    private DataTable tmpTable = new DataTable();
    private int minSpnsrNoLen, minScrtchLen;
    private double Upln, dblSpons, dblState, dblBank, dblIdNo;
    private string dblDistrict, dblTehsil, IfSC;
    private string dblPlan;
    private DateTime CurrDt;
    private string scrname;
    private string LastInsertID = "";
    private string Email = "";
    private string InVoiceNo;
    private int SupplierId;
    private string BillNo;
    private string TaxType;
    private string BillDate;
    private int SBillNo;
    private string SoldBy = "WR";
    private string FType;
    private string Password = "";
    private string membername = "";
    private string clsGeneral = "";
    private clsGeneral dbGeneral = new clsGeneral();
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private SqlConnection cnn;
    DataTable Dt = new DataTable();
    string IsoStart;
    string IsoEnd;
    protected void Page_Load(object sender, System.EventArgs e)
    {
        this.CmdSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.CmdSave));
        this.BtnOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnOtp));
        this.ResendOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.ResendOtp));
        try
        {
            if (Application["WebStatus"] == null)
            {
                if (Application["WebStatus"] == "N")
                {
                    Session.Abandon();
                    Response.Redirect("default.aspx", false);
                }
            }
            cnn = new SqlConnection(constr1);
            dbConnect = new cls_DataAccess((string)Application["Connect"]);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            txtUplinerId.Text = (txtUplinerId.Text).Replace("'", "").Replace("=", "").Replace(";", "");
            string sr = "";
            string[] sbstr;
            string Key = "";
            string K = "";
            if (!Page.IsPostBack)
            {
                Session["OtpCount"] = 0;
                Session["OtpTime"] = null;
                Session["OTP_"] = null;
                Session["Retry"] = null;
                HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                //getData();

                Session["OtpCount"] = 0;
                ClrCtrl();

                RbtnLegNo.Items.Add("Group A");
                RbtnLegNo.Items.Add("Group B");
                RbtnLegNo.Items[0].Selected = true;

                if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                {
                    K = Request["s"];
                    K = K.Replace(" ", "+");
                    sr = Crypto.Decrypt(K);

                    sbstr = sr.Split('/');
                    string UplinerFormno = sbstr[1];

                    DataTable dt = new DataTable();
                    string s = ObjDAL.Isostart + " select * from " + ObjDAL.dBName + "..M_MemberMaster where Formno='" + UplinerFormno + "'" + ObjDAL.IsoEnd;
                    dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtUplinerId.Text = dt.Rows[0]["Idno"].ToString();
                    }

                    string LegNo = sbstr[3];

                    txtUplinerId.ReadOnly = true;
                    txtRefralId.Text = Session["Idno"].ToString();

                    if (LegNo == "1")
                    {
                        RbtnLegNo.SelectedIndex = 0;
                    }
                    else
                    {
                        RbtnLegNo.SelectedIndex = 1;
                    }

                    RbtnLegNo.Enabled = false;
                    Session["iLeg"] = LegNo;
                }
            refLink:

                if (Request.QueryString["ref"] != null)
                {

                    string req = Request.QueryString["ref"].Replace(" ", "+");
                    string str = Crypto.Decrypt(req);
                    string[] rfAr = str.Split('/');
                    if (rfAr.Length >= 1)
                    {
                        if (rfAr[0] != "" & rfAr[1] == "0")
                        {
                            txtRefralId.Text = GetIDno(rfAr[0].ToString());

                            //goto refLink;
                        }
                        else if (rfAr[0] != "" & rfAr[1] == "1")
                        {
                            txtRefralId.Text = GetIDno(rfAr[0].ToString());
                            RbtnLegNo.SelectedIndex = 0;
                            RbtnLegNo.Enabled = false;
                            goto refLink;
                        }
                        else if (rfAr[0] != "" & rfAr[1] == "2")
                        {
                            txtRefralId.Text = GetIDno(rfAr[0].ToString());
                            RbtnLegNo.SelectedIndex = 1;
                            RbtnLegNo.Enabled = false;
                            goto refLink;
                        }
                    }

                    //if (Request.QueryString["RefFormNo"].Length > 0)
                    //{
                    //    txtRefralId.Text = Get_IDNo(Convert.ToInt32(Request.QueryString["RefFormNo"]));
                    //    if (txtRefralId.Text.Trim() != "")
                    //    {
                    //        FillReferral(cnn);
                    //    }

                    //    txtRefralId.ReadOnly = true;
                    //}
                    if (txtRefralId.Text.Trim() != "")
                    {
                        FillReferral(cnn);
                    }
                    txtRefralId.ReadOnly = true;
                }

                FillPaymode(cnn);

                dbGeneral.Fill_Date_box(ddlDOBdt, ddlDOBmnth, ddlDOBYr, 1940, DateTime.Now.AddYears(-18).Year);
                dbGeneral.Fill_Date_box(DDlMDay, DDLMMonth, DDLMYear, 1940, DateTime.Now.Year);
                FillBankMaster(cnn);
                // FillStateMaster()
                FillCountryMasterName();
                FillCountryMasterCode();
                FindSession();
                GetConfigDtl(cnn);
                // sendSMS()
                vsblCtrl(false, true);
            }

            try
            {
                Session["Dsessid"] = 0;
            }
            catch { }

            if (Session["IsGetExtreme"].ToString() == "N")
            {
                rwSpnsr.Visible = true;
            }
            else
            {
                rwSpnsr.Visible = false;
            }
            
        }
        catch (Exception ex)
        {

        }
    }
    private string ClearInject(string strObj)
    {
        strObj = strObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return strObj.Trim();
    }
    private string GetIDno(string Mid)
    {
        string Result = "";
        try
        {
            DataTable dt = new DataTable();

            //string strSql = IsoStart + "Select IDNO from " + ObjDAL.dBName + " ..M_MemberMAster Where MID = '" + Mid + "' " + IsoEnd;
            string strSql = IsoStart + "Select IDNO from " + ObjDAL.dBName + "..M_MemberMAster Where MID = '" + Mid + "' " + IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];

            if ((dt.Rows.Count > 0))
                Result = dt.Rows[0]["IDNO"].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return Result;
    }
    private string Get_IDNo(int MyFormNo)
    {
        string IdNo = "";
        try
        {

            DataTable dt = new DataTable();

            string strSql = IsoStart + "select IdNo  from " + ObjDAL.dBName + "..M_MemberMaster WHERE FormNo='" + MyFormNo + "' " + IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                IdNo = dt.Rows[0]["IdNo"].ToString();
            }
            //return IdNo;
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
        return IdNo;
    }
    private string GetIDno(int Mid, ref SqlConnection cnn)
    {
        string Result = "";
        try
        {
            string strSql = IsoStart + "Select IDNO from " + ObjDAL.dBName + "..M_MemberMAster Where MID = '" + Mid + "' " + IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                Result = Dt.Rows[0]["IDNO"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return Result;
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
    private string Get_IDNoUp(string myFormNo)
    {
        try
        {
            string idNo = "";
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string strSql = "SELECT idno FROM M_MemberMaster WHERE formno = '" + myFormNo + "'";

            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                idNo = dt.Rows[0]["idno"].ToString();
            }

            return idNo;
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
            return null; // Ensure a return in case of exception
        }
    }
    private void FillCountryMasterName()
    {
        try
        {
            DataTable dt = new DataTable();
            string strQuery = "Exec Sp_GetCountryINR";
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
            ddlCountryNAme.DataSource = dt;
            ddlCountryNAme.DataValueField = "CId";
            ddlCountryNAme.DataTextField = "CountryName";
            ddlCountryNAme.DataBind();
            try
            {
                DataTable dt_ = new DataTable();
                string strQuery_ = IsoStart + "SELECT StdCode FROM " + ObjDAL.dBName + "..M_CountryMaster WHERE ACTIVESTATUS='Y' AND Cid = '" + ddlCountryNAme.SelectedValue + "' " + IsoEnd;
                dt_ = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery_).Tables[0];

                if (dt_.Rows.Count > 0)
                {
                    ddlMobileNAme.Text = dt_.Rows[0]["StdCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }
    public string GenerateRandomStringJoining(int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < iLength; i++)
        {
            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
        }

        return sResult;
    }
    private void FillPaymode(SqlConnection cnn)
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string strSql = IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_PayModeMaster WHERE ActiveStatus='Y' " + IsoEnd;

            if (Session["DtPayMode"] == null)
            {
                ds = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
                dt = ds.Tables[0];
                Session["DtPayMode"] = dt;
            }
            else
            {
                dt = (DataTable)Session["DtPayMode"];
            }

            if (dt.Rows.Count > 0)
            {
                DdlPaymode.DataSource = dt;
                DdlPaymode.DataValueField = "PID";
                DdlPaymode.DataTextField = "Paymode";
                DdlPaymode.DataBind();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    private void GetConfigDtl(SqlConnection cnn)
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string strSql = IsoStart + "select *  from " + ObjDAL.dBName + "..M_ConfigMaster " + IsoEnd;

            if (Session["DtConfigDetail"] == null)
            {
                ds = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
                dt = ds.Tables[0];
                Session["DtConfigDetail"] = dt;
            }
            else
            {
                dt = (DataTable)Session["DtConfigDetail"];
            }

            if (dt.Rows.Count > 0)
            {
                Session["IsGetExtreme"] = dt.Rows[0]["IsGetExtreme"];
                Session["IsTopUp"] = dt.Rows[0]["IsTopUp"];
                Session["IsSendSMS"] = dt.Rows[0]["IsSendSMS"];
                Session["IdNoPrefix"] = dt.Rows[0]["IdNoPrefix"];
                Session["IsFreeJoin"] = dt.Rows[0]["IsFreeJoin"];
                Session["IsStartJoin"] = dt.Rows[0]["IsStartJoin"];
                Session["JoinStartFrm"] = dt.Rows[0]["JoinStartFrm"];
                Session["IsSubPlan"] = dt.Rows[0]["IsSubPlan"];
            }
            else
            {
                Session["IsGetExtreme"] = "N";
                Session["IsTopUp"] = "N";
                Session["IsSendSMS"] = "N";
                Session["IdNoPrefix"] = "";
                Session["IsFreeJoin"] = "N";
                Session["IsStartJoin"] = "N";
                Session["JoinStartFrm"] = "01-Sep-2011";
                Session["IsSubPlan"] = "N";
            }
        }
        catch
        {
            Session["CompName"] = "";
            Session["CompAdd"] = "";
            Session["CompWeb"] = "";
        }
    }
    private string Get_IDNo(int myFormNo, ref SqlConnection cnn)
    {
        try
        {
            string idNo = "";
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string strSql = IsoStart + "select IdNo  from " + ObjDAL.dBName + "..M_MemberMaster WHERE FormNo='" + myFormNo + "' " + IsoEnd;

            ds = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                idNo = dt.Rows[0]["IdNo"].ToString();
            }

            return idNo;
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
            return null;
        }
    }
    protected void vsblCtrl(bool isVsbl, bool isOnlyDv)
    {
        try
        {
            if (!isOnlyDv)
            {
                txtUplinerId.Enabled = !isVsbl;
                txtRefralId.Enabled = !isVsbl;
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    public string GenerateRandomString(int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < iLength; i++)
        {
            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
        }

        return sResult;
    }
    private void ClrCtrl()
    {
        // txtAddLn2.Text = "";
        txtAddLn1.Text = "";
        txtEMailId.Text = "";
        txtFNm.Text = "";
        txtFrstNm.Text = "";
        txtMobileNo.Text = "";
        txtNominee.Text = "";
        txtPanNo.Text = "";
        txtPhNo.Text = "";
        txtPinCode.Text = "";
        txtRelation.Text = "";
        txtUplinerId.Text = "";
        lblUplnrNm.Text = "";
        ddlDistrict.Text = "";
        ddlTehsil.Text = "";
        TxtBranchName.Text = "";
        TxtAccountNo.Text = "";
        txtIfsCode.Text = "";
        // txtPIN.Text = "";
        // txtScratch.Text = "";
        txtRefralId.Text = "";
        lblRefralNm.Text = "";
        // dv_Main.Visible = false;
        txtUplinerId.Enabled = true;
        txtRefralId.Enabled = true;
        // txtPIN.Enabled = true;
        // txtScratch.Enabled = true;
        // cmdNext.Visible = true;

        RbtnLegNo.Enabled = true;
    }
    private void FillBankMaster(SqlConnection Cnn)
    {
        try
        {
            DataTable dt = new DataTable();

            if (Session["DtBankMaster"] == null)
            {
                DataSet Ds = new DataSet();
                string strSql = IsoStart + "SELECT BankCode as Bid, BANKNAME as Bank FROM " + ObjDAL.dBName + "..M_BankMaster WHERE ACTIVESTATUS='Y' and Rowstatus='Y' ORDER BY BankName" + IsoEnd;
                Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
                dt = Ds.Tables[0];
                Session["DtBankMaster"] = dt;
            }
            else
            {
                dt = (DataTable)Session["DtBankMaster"];
            }

            if (dt.Rows.Count > 0)
            {
                CmbBank.DataSource = dt;
                CmbBank.DataValueField = "Bid";
                CmbBank.DataTextField = "Bank";
                CmbBank.DataBind();
                CmbBank.SelectedIndex = 0;
            }

            TxtBank.Text = CmbBank.SelectedItem.Text;
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    public string Validt_SpnsrDtl(string chkby, SqlConnection Cnn)
    {
        try
        {
            string Validt_SpnsrDtl = "";

            if (Session["IsGetExtreme"].ToString() == "N")
            {
                if (string.IsNullOrEmpty(txtUplinerId.Text))
                {
                    string scrname = "<SCRIPT language='javascript'>alert('Check Placement Id');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    txtUplinerId.Focus();
                    return "";
                }
            }

            txtRefralId.Text = txtRefralId.Text.Replace("'", "").Replace("=", "").Replace(";", "").Trim();
            txtUplinerId.Text = txtUplinerId.Text.Replace("'", "").Replace("=", "").Replace(";", "").Trim();

            if (!string.IsNullOrEmpty(txtRefralId.Text))
            {
                try
                {
                    DataTable dt = new DataTable();
                    DataSet Ds = new DataSet();
                    string strSql = IsoStart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName, ActiveStatus from " + ObjDAL.dBName + "..M_MemberMaster where Idno = '" + txtRefralId.Text + "'" + IsoEnd;
                    Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
                    dt = Ds.Tables[0];

                    if (dt.Rows.Count == 0)
                    {
                        string scrname = "<SCRIPT language='javascript'>alert('Sponsor ID Not Exist.');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                        Validt_SpnsrDtl = "";
                        vsblCtrl(false, true);
                        return "";
                    }
                    else
                    {
                        Session["Kitid"] = 1;
                        Session["Bv"] = 0;
                        Session["JoinStatus"] = "N";
                        Session["RP"] = 0;
                        Validt_SpnsrDtl = "OK";
                    }
                    Session["Refral"] = dt.Rows[0]["FormNo"];
                    lblRefralNm.Text = dt.Rows[0]["MemName"].ToString();
                }
                catch (Exception ex)
                {
                    Response.Write("Please check sponsor ID.");
                }
            }
            else
            {
                string scrname = "<SCRIPT language='javascript'>alert('Check Sponsor ID.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                txtRefralId.Focus();
                return "";
            }

            // Additional code for Upliner logic here...

            return Validt_SpnsrDtl;
        }
        catch (Exception ex)
        {
            return "";
        }
    }
    private void FindSession()
    {
        try
        {
            Session["SessID"] = 1;
            return;
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }

        try
        {
            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = ObjDAL.Isostart + "Select Max(SessId) as SessId from " + ObjDAL.dBName + "..M_SessnMaster  " + ObjDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0]; ;
            dt = Dt;
            if (dt.Rows.Count > 0)
            {
                Session["SessID"] = dt.Rows[0]["SessID"];
            }
            else
            {
                errMsg.Text = "Session Not Exist. Please Enter New Session.";
                return;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private bool checkAvailLeg()
    {
        try
        {
            int iLegNo = 0;
            int iformNo = 0;

            if (RbtnLegNo.SelectedIndex == 0)
            {
                iLegNo = 1;
            }
            else if (RbtnLegNo.SelectedIndex == 1)
            {
                iLegNo = 2;
            }
            else
            {
                string scrname = "<SCRIPT language='javascript'>alert('Choose Position.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return false;
            }

            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = IsoStart + "Select * from " + ObjDAL.dBName + "..M_MemberMaster where IdNo='" + txtUplinerId.Text + "'" + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = Ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                iformNo = Convert.ToInt32(dt.Rows[0]["FormNo"]);
            }
            else
            {
                errMsg.Text = "Check Placeunder Id.";
                string scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return false;
            }

            DataTable dt12 = new DataTable();
            DataSet Ds12 = new DataSet();
            string strSql12 = IsoStart + "SELECT COUNT(*) AS CNT FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE UpLnFormNo=" + iformNo + " And LegNo=" + iLegNo + IsoEnd;
            Ds12 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql12);
            dt12 = Ds12.Tables[0];

            if (dt12.Rows.Count > 0 && Convert.ToInt32(dt12.Rows[0]["CNT"]) > 0)
            {
                errMsg.Text = (iLegNo == 1 ? "LEFT" : "RIGHT") + " Position already used, please select correct Position!";
                string scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return false;
            }
            else
            {
                _dblAvailLeg = iformNo;
                return true;
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
            return false;
        }
    }
    protected void txtUplinerId_TextChanged(object sender, EventArgs e)
    {
        FillSponsor(ref cnn);
    }
    private void FillSponsor(ref SqlConnection Cnn)
    {
        try
        {
            errMsg.Text = "";
            lblErrEpin.Text = "";
            int i = 0;
            txtUplinerId.Text = txtUplinerId.Text.Trim().Replace(";", "").Replace("'", "").Replace("=", "");

            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = IsoStart + " Select FormNo,MemFirstName + ' ' + MemLastName as MemName from " + ObjDAL.dBName +
                            "..M_MemberMaster where IDNo='" + txtUplinerId.Text + "'" + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
            dt = Ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                lblUplnrNm.Text = dt.Rows[0]["MemName"].ToString();
                Session["Uplnr"] = dt.Rows[0]["FormNo"].ToString();
                i += 1;
            }
            else
            {
                errMsg.Text = "Invalid PlaceUnder ID!!";
                lblErrEpin.Text = "Invalid PlaceUnder ID!!";
                string scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            }

            if (i == 1)
            {
                checkAvailLeg();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    private void FillReferralUp()
    {
        try
        {
            string scrname = "";
            txtRefralId.Text = txtRefralId.Text.Trim().Replace(";", "").Replace("'", "").Replace("=", "");

            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = " Select FormNo,MemFirstName + ' ' + MemLastName as MemName,idno,kitname,kitamount  from M_MemberMaster as a, " +
                            " INR_kitmaster as b where a.INRkitid = b.kitid AND idno = '" + txtRefralId.Text + "' and IsBlock='N' ";
            Ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            dt = Ds.Tables[0];

            if (dt.Rows.Count > 0)
            {               
                lblRefralNm.Text = dt.Rows[0]["MemName"].ToString();
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Invalid Referal Code.!!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                txtRefralId.Text = "";
                return;
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    private void FillReferral(SqlConnection Cnn)
    {
        try
        {
            lblErrEpin.Text = "";
            errMsg.Text = "";
            txtRefralId.Text = txtRefralId.Text.Trim().Replace(";", "").Replace("'", "").Replace("=", "");

            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = IsoStart + "Select FormNo,MemFirstName + ' ' + MemLastName as MemName,INRActiveStatus as Activestatus from " +
                            ObjDAL.dBName + "..M_MemberMaster where IDNo='" + txtRefralId.Text + "' and IsBlock='N' " + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
            dt = Ds.Tables[0];

            if (dt.Rows.Count == 0)
            {
                string scrname = "<SCRIPT language='javascript'>alert('No such record/This ID is Flashed./This Id Not Active!!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                txtRefralId.Text = "";
                return;
            }
            //else if (dt.Rows[0]["ActiveStatus"].ToString() == "N")
            //{
            //    string scrname = "<SCRIPT language='javascript'>alert('This ID is not eligible for sponsor.');</SCRIPT>";
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            //    return;
            //}
            else
            {
                lblRefralNm.Text = dt.Rows[0]["MemName"].ToString();
                TxtSponsor.Text = txtRefralId.Text.Trim().Replace(";", "").Replace("'", "").Replace("=", "");
                LblSponsorName.Text = dt.Rows[0]["MemName"].ToString();
                Lblsponsorformno.Text = dt.Rows[0]["FormNo"].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    protected void CmdCancel_Click(object sender, EventArgs e)
    {
        ClrCtrl();
    }
    protected void txtRefralId_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FillReferral(cnn);
        }
        catch (Exception ex)
        {
            // Handle the exception if necessary
        }
    }
    protected void txtMobileNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtMobileNo.Text))
            {
                string moblno = txtMobileNo.Text;
                string check = moblno.Substring(0, 1);

                if (check == "0")
                {
                    txtMobileNo.Text = "";
                    CmdSave.Enabled = true;
                    chkterms.Checked = false;
                    string scrname = "<SCRIPT language='javascript'>alert('Invalid Mobile No.!');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(txtMobileNo.Text))
            {
                DataTable Dt1 = new DataTable();
                DataSet Dsmob = new DataSet();
                string strSql = IsoStart + "select Count(mobl) as mobileno from " + ObjDAL.dBName + "..M_Membermaster where Mobl='" + txtMobileNo.Text.Trim() + "' " + IsoEnd;
                Dsmob = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
                Dt1 = Dsmob.Tables[0];

                if (Convert.ToInt32(Dt1.Rows[0]["mobileno"]) >= 1)
                {
                    txtMobileNo.Text = "";
                    CmdSave.Enabled = true;
                    chkterms.Checked = false;
                    LblMobile.Visible = true;
                    LblMobile.Text = "Already Registered by this Mobile Number.!";
                    //string scrname = "<SCRIPT language='javascript'>alert('Already Registered by this Mobile Number.');</SCRIPT>";
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
                else
                {
                    LblMobile.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
        }
    }
    protected void txtEMailId_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable DtEmail = new DataTable();
            DataSet DsEmail = new DataSet();
            string strSql = IsoStart + "select Count(Email) as Email from " + ObjDAL.dBName + "..M_Membermaster where Email='" + txtEMailId.Text.Trim() + "' " + IsoEnd;
            DsEmail = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            DtEmail = DsEmail.Tables[0];

            if (Convert.ToInt32(DtEmail.Rows[0]["Email"]) >= 1)
            {
                txtEMailId.Text = "";
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                LblEmainID.Visible = true;
                LblEmainID.Text = "Already Registered by this Email ID.!";
                return;
            }
            else
            {
                LblEmainID.Visible = false;
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
        }
    }
    protected void ddlCountryNAme_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCountryMasterCode();
    }
    private void FillCountryMasterCode()
    {
        try
        {
            DataTable dt = new DataTable();
            string strQuery = IsoStart + "SELECT StdCode FROM " + ObjDAL.dBName + "..M_CountryMaster WHERE ACTIVESTATUS='Y' AND Cid = '" + ddlCountryNAme.SelectedValue + "' " + IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];

            if (dt.Rows.Count > 0)
            {
                ddlMobileNAme.Text = dt.Rows[0]["StdCode"].ToString();
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
        }
    }
    protected void CmdSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!chkterms.Checked)
            {
                ShowAlert("Please select Terms and Conditions.!");
                return;
            }
            if (string.IsNullOrEmpty(txtRefralId.Text))
            {
                ShowAlert("Please Enter Sponsor Id.!");
                return;
            }
            if (string.IsNullOrEmpty(Txtusername.Text))
            {
                ShowAlert("Please Enter User Name.!");
                return;
            }
            if (string.IsNullOrEmpty(TxtSponsor.Text))
            {
                ShowAlert("Please Enter Sponsor.!");
                return;
            }
            if (string.IsNullOrEmpty(txtFrstNm.Text))
            {
                ShowAlert("Please Enter Full Name.!");
                return;
            }
            if (ddlCountryNAme.SelectedValue == "0")
            {
                ShowAlert("Please Select Country.!");
                return;
            }
            if (string.IsNullOrEmpty(txtMobileNo.Text))
            {
                ShowAlert("Please Enter Mobile No.!");
                return;
            }
            if (string.IsNullOrEmpty(txtEMailId.Text))
            {
                ShowAlert("Please Enter Email Id.!");
                return;
            }
            // Check if Email is already registered
            string emailQuery = IsoStart + "SELECT COUNT(Email) FROM " + ObjDAL.dBName + "..M_Membermaster WHERE Email = '" + txtEMailId.Text.Trim() + "' " + IsoEnd;
            int emailCount = Convert.ToInt32(SqlHelper.ExecuteScalar(constr1, CommandType.Text, emailQuery));
            if (emailCount >= 1)
            {
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                ShowAlert("Already Registered by this Email ID.");
                return;
            }
            string usernameQuery = IsoStart + "SELECT COUNT(idno) FROM " + ObjDAL.dBName + "..M_Membermaster WHERE idno = '" + Txtusername.Text.Trim() + "' " + IsoEnd;
            int usernameCount = Convert.ToInt32(SqlHelper.ExecuteScalar(constr1, CommandType.Text, usernameQuery));
            if (usernameCount >= 1)
            {
                Txtusername.Text = "";
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                LblUseName.Visible = true;
                LblUseName.Text = "Already Registered by this User ID.!";
                return;
            }
            else
            {
                LblUseName.Visible = false;
            }
            string mobileQuery = IsoStart + "SELECT COUNT(mobl) FROM " + ObjDAL.dBName + "..M_Membermaster WHERE Mobl = '" + txtMobileNo.Text.Trim() + "' " + IsoEnd;
            int mobileCount = Convert.ToInt32(SqlHelper.ExecuteScalar(constr1, CommandType.Text, mobileQuery));
            if (mobileCount >= 1)
            {
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                ShowAlert("Already Registered by this Mobile Number.");
                return;
            }
            int OTP_ = new Random().Next(100001, 999999);

            if (Session["OTP_"] == null)
            {
                if (SendMail(OTP_.ToString()))
                {
                    Session["OtpTime"] = DateTime.Now.AddMinutes(5);
                    Session["Retry"] = "1";
                    Session["OTP_"] = OTP_;
                    string query = "INSERT INTO AdminLogin (UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID, ForType) ";
                    query += "VALUES ('0', '','" + TxtOtp.Text + "', '0','" + OTP_ + "', GETDATE(),'" + OTP_ + "','" + txtEMailId.Text.Trim() + "', 'RegistrationINR')";
                    int i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, query);
                    if (i > 0)
                    {
                        divotp.Visible = true;
                        CmdSave.Visible = false;
                        BtnOtp.Visible = true;
                        ResendOtp.Visible = true;
                        ShowAlert("OTP Sent On Mail");
                        return;
                    }
                    else
                    {
                        ShowAlert("Try Later");
                        return;
                    }
                }
                else
                {
                    ShowAlert("OTP Try Later");
                    return;
                }
            }
            else
            {
                txtRefralId.Enabled = false;
                Txtusername.Enabled = false;
                TxtSponsor.Enabled = false;
                txtFrstNm.ReadOnly = true;
                ddlCountryNAme.Enabled = false;
                TxtWalletaddress.Enabled = false;
                divotp.Visible = true;
                CmdSave.Visible = false;
                BtnOtp.Visible = true;
                ResendOtp.Visible = false;
                chkterms.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            // Proper error handling
            Response.Write("An error occurred. Please try again later.");
        }
        
    }
    private void ShowAlert(string message)
    {
        string script = "<script language='javascript'>alert('" + message + "');</script>";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Alert", script, false);
    }
    public bool SendMail(string otp)
    {
        try
        {
            string strMsg = "";
            string emailAddress = txtEMailId.Text.ToString().Trim();
            System.Net.Mail.MailAddress sendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress sendTo = new System.Net.Mail.MailAddress(emailAddress);
            System.Net.Mail.MailMessage myMessage = new System.Net.Mail.MailMessage(sendFrom, sendTo);

            strMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\"> " +
                     "<tr>" +
                     "<td>" +
                     "Your OTP for Registration is <span style=\"font-weight: bold;\">" + otp + "</span> (valid for 5 minutes)." +
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
            txtRefralId.Enabled = false;
            Txtusername.Enabled = false;
            TxtSponsor.Enabled = false;
            txtFrstNm.ReadOnly = true;
            CmbType.Enabled = true;
            txtFNm.ReadOnly = true;
            txtEMailId.Enabled = false;
            txtMobileNo.Enabled = false;
            txtFNm.Enabled = false;
            txtFrstNm.Enabled = false;
            ddlCountryNAme.Enabled = false;
            TxtWalletaddress.Enabled = false;
            chkterms.Enabled = false;
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
            if (TxtPasswd.Text == "")
            {
                scrname = "<SCRIPT language='javascript'>alert('Please Enter Password.!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            DataTable dt1 = new DataTable();
            Session["OtpCount"] = Convert.ToInt32(Session["OtpCount"]) + 1;

            if (Session["OTP_"] != null && Session["OTP_"].ToString() == TxtOtp.Text.Trim())
            {
                string query = "SELECT TOP 1 * FROM " + ObjDAL.dBName + "..AdminLogin AS a WHERE EmailID = '" + txtEMailId.Text.ToString().Trim() + "' AND emailotp = '" + TxtOtp.Text.Trim() + "' ";
                query += "AND ForType = 'RegistrationINR' ORDER BY AID DESC";

                dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];

                if (dt1.Rows.Count > 0)
                {
                    SaveIntoDB();
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
                string memberName = "";
                string mobileNo = "0";
                string sms = "";
                int result = 0;
                string query = "";

                query = "INSERT INTO AdminLogin (UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID, ForType) " +
                        "VALUES ('0', '" + memberName + "', '" + TxtOtp.Text + "', '" + mobileNo + "', '" + OTP_ + "', GETDATE(), '" + OTP_ + "', " +
                        "'" + txtEMailId.Text.ToString().Trim() + "', 'RegistrationINR')";

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
    public void SaveIntoDB()
    {
        try
        {
            int UpdateData = 0;
            if (UpdateData == 0)
            {
                char IsPanCard;
                string strQry = "";
                string strDOB, strDOM, strDOJ, s;
                int iLeg;
                char cGender, cMarried; // Declare variables
                cGender = 'M';          // Assign value
                cMarried = 'N';        // Assign value

                string hostIp = Context.Request.UserHostAddress; // Retrieve and assign IP address
                string HostIp = Context.Request.UserHostAddress.ToString();
                int DistrictCode, CityCode, VillageCode;
                CmdSave.Enabled = false;
                try
                {
                    if (Validt_SpnsrDtl("", cnn) == "OK")
                    {
                        iLeg = Convert.ToInt32(Session["iLeg"]);
                        if ((RbtnLegNo.SelectedIndex == 0))
                            iLeg = 1;
                        else if ((RbtnLegNo.SelectedIndex == 1))
                            iLeg = 2;
                        else
                        {
                            chkterms.Checked = false;
                            CmdSave.Enabled = true;
                            scrname = "<SCRIPT language='javascript'>alert('Choose Position.');" + "</SCRIPT>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Choose Position.');", true);
                            RbtnLegNo.Enabled = true;
                            return;
                        }
                        //TxtPasswd.Text = GenerateRandomString(6)
                        string s1 = "";
                        string q = "";
                        int i = 0;
                        DataTable Dt;
                        int BankCode = 0;
                        if (CmbBank.SelectedItem.Text.ToUpper() == "OTHERS") // Check if the selected bank is "OTHERS"
                        {
                            if (!string.IsNullOrWhiteSpace(TxtBank.Text)) // Check if TxtBank is not empty
                            {
                                DataTable dt = new DataTable(); // Initialize DataTable
                                DataSet ds = new DataSet(); // Initialize DataSet
                                q = IsoStart + "Select * from " + ObjDAL.dBName + "..M_BankMaster where BankName='" + TxtBank.Text.Trim() + "' and Activestatus='Y' and RowStatus='Y' " + IsoEnd;

                                ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, q); // Execute the SQL query
                                dt = ds.Tables[0]; // Get the first table from the dataset

                                if (dt.Rows.Count == 0) // If no records found
                                {
                                    q = "";
                                    q = "insert into M_BankMaster (BankCode, BankName, AcNo, IFSCode, Remarks, ActiveStatus, LastModified, UserCode, UserId, IPAdrs, RowStatus) " +
                                        "Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode, '" + TxtBank.Text.ToUpper() + "', '0', '0', " +
                                        "'', 'Y', 'Add by " + Session["IdNo"] + " at " + DateTime.Now.ToString() + "', '" + Session["MemName"] + "', " +
                                        "'" + Convert.ToString(Session["FormNo"]) + "', '', 'Y' From M_BankMaster";

                                    i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, q)); // Execute the insert query

                                    if (i > 0) // If the insert was successful
                                    {
                                        string qs = IsoStart + " select Max(BankCode) as BankCode from " + ObjDAL.dBName + "..M_BankMaster where ActiveStatus='Y' and RowStatus='Y'" + IsoEnd;
                                        DataTable dtRead = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qs).Tables[0]; // Get the max BankCode

                                        if (dtRead.Rows.Count > 0)
                                        {
                                            dblBank = Convert.ToInt32(dtRead.Rows[0]["BankCode"]); // Get the BankCode
                                        }
                                    }
                                }
                                else // If a record exists
                                {
                                    dblBank = Convert.ToInt32(dt.Rows[0]["BankCode"]); // Get the existing BankCode
                                }
                            }
                        }
                        else // If the selected bank is not "OTHERS"
                        {
                            dblBank = Convert.ToInt32(CmbBank.SelectedValue); // Get the selected value
                        }

                        int AreaCode = 0;
                        AreaCode = 0;
                        string RegestType = "";
                        if (RbCategory.SelectedValue == "IN") // Check if the selected value is "IN"
                        {
                            RegestType = "IN"; // Assign "IN" to RegestType
                        }
                        else
                        {
                            RegestType = CbSubCategory.SelectedValue; // Assign the selected value of CbSubCategory to RegestType
                        }

                        int PostalAreaCode = 0;
                        strDOB = ddlDOBdt.Text + "-" + ddlDOBmnth.Text + "-" + ddlDOBYr.Text; // Concatenate day, month, and year for date of birth
                        strDOM = DDlMDay.Text + "-" + DDLMMonth.Text + "-" + DDLMYear.Text; // Concatenate day, month, and year for date of marriage
                        strDOJ = dbConnect.Get_ServerDate().ToString("dd-MMM-yyyy"); // Format the server date as "dd-MMM-yyyy"
                        string dblDistrict = ClearInject(ddlDistrict.Text.ToUpper()); // Get and clear injected text for district
                        string dblTehsil = ClearInject(ddlTehsil.Text.ToUpper()); // Get and clear injected text for tehsil

                        if (string.IsNullOrEmpty(dblDistrict))
                        {
                            dblDistrict = "";
                        }

                        dblState = 0;
                        DistrictCode = 0;
                        CityCode = 0;
                        VillageCode = 0;
                        IfSC = ClearInject(txtIfsCode.Text.ToUpper());

                        dblPlan = "0";
                        InVoiceNo = "0";

                        if (Session["SessID"] == null || (int)Session["SessID"] == 0)
                        {
                            FindSession();
                        }

                        string Name = "";
                        string fathername = "";

                        if (RbCategory.SelectedValue == "IN")
                        {
                            Name = ClearInject(txtFrstNm.Text.ToUpper());
                            fathername = ClearInject(txtFNm.Text.ToUpper());
                        }
                        else
                        {
                            fathername = ClearInject(txtFrstNm.Text.ToUpper());
                            Name = ClearInject(TxtCompanyName.Text.ToUpper());
                        }
                        if (!string.IsNullOrWhiteSpace(TxtAccountNo.Text) || !string.IsNullOrWhiteSpace(txtIfsCode.Text.Trim()))
                        {
                            if (string.IsNullOrWhiteSpace(TxtAccountNo.Text))
                            {
                                chkterms.Checked = false;
                                CmdSave.Enabled = true;
                                string script = "alert('Enter Account No.');";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
                                return;
                            }

                            if (CmbBank.SelectedValue == "0") // Assuming SelectedValue is a string
                            {
                                chkterms.Checked = false;
                                CmdSave.Enabled = true;
                                string script = "alert('Choose Bank Name.');";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
                                return;
                            }

                            if (string.IsNullOrWhiteSpace(TxtBranchName.Text))
                            {
                                chkterms.Checked = false;
                                CmdSave.Enabled = true;
                                string script = "alert('Enter Branch Name.');";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
                                return;
                            }

                            if (string.IsNullOrWhiteSpace(DDLAccountType.SelectedValue))
                            {
                                chkterms.Checked = false;
                                CmdSave.Enabled = true;
                                string script = "alert('Enter Account Name.');";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
                                return;
                            }

                            if (string.IsNullOrWhiteSpace(txtIfsCode.Text))
                            {
                                chkterms.Checked = false;
                                CmdSave.Enabled = true;
                                string script = "alert('Enter IFSC Code.');";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
                                return;
                            }
                        }
                        // TxtPasswd.Text = GenerateRandomString(6);
                        var Strquery1 = "Insert into Trnjoining (Transid) values(" + HdnCheckTrnns.Value + ")";
                        int UpdateData1 = 0;
                        UpdateData1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strquery1));
                        if (UpdateData1 > 0)
                        {
                            strQry = "INSERT INTO m_memberMaster(SessId, IdNo, CardNo, FormNo, KitId, UpLnFormNo, RefId, LegNo, RefLegNo, RefFormNo, " +
         "MemFirstName, MemLastName, MemRelation, MemFName, MemDOB, MemGender, MemOccupation, NomineeName, Address1, Address2, Post, " +
         "Tehsil, City, District, StateCode, CountryId, PinCode, PhN1, Fax, Mobl, MarrgDate, Passw, Doj, Relation, PanNo, " +
         "BankID, MICRCode, BranchName, EMail, BV, INRUpGrdSessId, E_MainPassw, EPassw, ActiveStatus, billNo, RP, HostIp, " +
         "PID, Paymode, ChDDNo, ChDDBankID, ChDDBank, ChddDate, ChDDBranch, IsPanCard, AadharNo, AadharNo2, AAdharNo3, Fld5, usercode,INRKitId,INRActiveStatus) " +
         "VALUES (" + Convert.ToInt32(Session["SessID"]) + ", '" + Txtusername.Text.ToString().Replace(";", "").Replace("'", "").Replace("=", "").Replace(" ", "") + "', 0, 0, " + Convert.ToInt32(Session["Kitid"]) + ", " +
         Lblsponsorformno.Text + ", 0, '" + iLeg + "', 0, " + Convert.ToInt32(Session["Refral"]) + ", '" + ClearInject(txtFrstNm.Text.ToUpper()) + "', " +
         "'', '" + CmbType.SelectedValue + "', '" + ClearInject(txtFNm.Text.ToUpper()) + "', '" + strDOB + "', '" + cGender + "', '', " +
         "'" + ClearInject(txtNominee.Text.ToUpper()) + "', '" + ClearInject(txtAddLn1.Text.ToUpper()) + "', '', '', '" + dblTehsil + "', " +
         "'" + dblTehsil + "', '" + dblDistrict + "', " + dblState + ", " + ddlCountryNAme.SelectedValue + ", '" + txtPinCode.Text + "', " +
         "'" + txtPhNo.Text + "', 'CHOOSE ACCOUNT TYPE', '" + txtMobileNo.Text + "', '" + strDOM + "', '" + ClearInject(TxtPasswd.Text) + "', " +
         "GETDATE(), '" + ClearInject(txtRelation.Text.ToUpper()) + "', '" + ClearInject(txtPanNo.Text.ToUpper()) + "', " + dblBank + ", " +
         "'" + (ClearInject(TxtMICR.Text.ToUpper())) + "', '" + (TxtBranchName.Text.ToUpper()) + "', '" + ClearInject(txtEMailId.Text) + "', " +
         Convert.ToInt32(Session["Bv"]) + ", 0, '" + ClearInject(TxtPasswd.Text) + "', '" + ClearInject(TxtPasswd.Text) + "', '" + Session["JoinStatus"] + "', " +
         "'" + InVoiceNo + "', '" + Session["RP"] + "', '" + HostIp + "', " + Convert.ToInt32(DdlPaymode.SelectedValue) + ", " +
         "'" + (DdlPaymode.SelectedItem.Text.ToUpper()) + "', '" + ClearInject(TxtDDNo.Text) + "', '0', '" + ClearInject(TxtIssueBank.Text.ToUpper()) + "', " +
         "'" + (TxtDDDate.Text) + "', '" + ClearInject(TxtIssueBranch.Text) + "', 'N', '" + ClearInject(TxtAAdhar1.Text) + "', " +
         "'" + ClearInject(TxtAadhar2.Text) + "', '" + ClearInject(TxtAadhar3.Text) + "', '" + Session["TransIDJoin"] + "', '" + ddlMobileNAme.Text + "'," + Convert.ToInt32(Session["Kitid"]) + ",'" + Session["JoinStatus"] + "')";

                            int isOk = 0;
                            int retryqry = 0;
                        Savedata:
                            ;
                            isOk = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strQry));
                            LastInsertID = "0";
                            if ((isOk > 0))
                            {
                                string membername = "";
                                string SPONSORID1 = "";
                                string SPONSORnAME = "";
                                string Doj = "";
                                string kitamount = "";
                                string Email = "";
                                string Password = "";
                                DataTable Dtsms = new DataTable();
                                string strSql = string.Empty;

                                // Execute stored procedure to get login details
                                strSql = IsoStart + " EXEC Sp_GetLoginDetailINR '" + Txtusername.Text.ToString().Replace(";", "").Replace("'", "").Replace("=", "").Replace(" ", "") + "'" + IsoEnd;
                                Dtsms = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];

                                if (Dtsms.Rows.Count > 0)
                                {
                                    membername = Dtsms.Rows[0]["MemfirstName"].ToString() + " " + Dtsms.Rows[0]["MemLastName"].ToString();
                                    SPONSORID1 = Dtsms.Rows[0]["SPONSORID"].ToString();
                                    SPONSORnAME = Dtsms.Rows[0]["SPONSORnAME"].ToString();
                                    Doj = Dtsms.Rows[0]["JoiningDate"].ToString();
                                    kitamount = Dtsms.Rows[0]["kitamount"].ToString();
                                    Email = Dtsms.Rows[0]["Email"].ToString();
                                    LastInsertID = Dtsms.Rows[0]["IDNO"].ToString();
                                    Password = Dtsms.Rows[0]["Passw"].ToString();
                                    Session["Kit"] = Dtsms.Rows[0]["IsBill"];

                                    //FUND_LOGIN_CHECK(Dtsms.Rows[0]["IDNO"].ToString(), Dtsms.Rows[0]["Passw"].ToString(), Dtsms.Rows[0]["formno"].ToString());
                                }
                                else
                                {
                                    LastInsertID = "10001";
                                }


                                CmdSave.Enabled = true;
                                SendToMemberMail(LastInsertID, Email, membername, Password);
                                Session["LASTID"] = LastInsertID;
                                Session["Join"] = "YES";
                                Response.Redirect("Welcome.Aspx?IDNo=" + LastInsertID, false);
                            }
                            else
                            {
                                if (retryqry <= 2)
                                {
                                    retryqry += 1;
                                    goto Savedata;
                                }
                                CmdSave.Enabled = true;
                                chkterms.Checked = false;
                                scrname = "<SCRIPT language='javascript'>alert('Try Again Later.');" + "</SCRIPT>";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Try Again Later.');", true);
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    CmdSave.Enabled = true;
                    chkterms.Checked = false;
                    string scrname = "<SCRIPT language='javascript'>alert('" + e.Message + "');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + e.Message + "');", true);

                    string path = HttpContext.Current.Request.Url.AbsoluteUri;
                    string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
                    ObjDAL.WriteToFile(text + e.Message);
                    Response.Write("Try later.");
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('This id already register.!');location.replace('Registration.aspx');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            dbConnect.closeConnection();
        }
    }
    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Password)
    {
        try
        {
            string StrMsg = "";
            System.Net.Mail.MailAddress SendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress SendTo = new System.Net.Mail.MailAddress(Email);
            System.Net.Mail.MailMessage MyMessage = new System.Net.Mail.MailMessage(SendFrom, SendTo);
            StrMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\">" +
                     "<tr>" +
                     "<td>" +
                     "<span style=\"color: #0099CC; font-weight: bold;\"><h2>Dear " + MemberName + ",</h2></span><br />" +
                     " It is with great pleasure that we extend our warmest welcome to you as a new member of the <br />" +
                     " " + Session["CompName"] + " family! Congratulations on embarking upon an exciting journey into the world of " + Session["CompName"] + ". <br />" +
                     "<br />" +
                     "<span style=\"color: #0099FF; font-weight: bold;\">Warm regards,</span><br />" +
                     " Team " + Session["CompName"] + " <br />" +
                     "<br />" +
                     "<span style=\"color: #0099FF; font-weight: bold;\">Registration Information</span><br />" +
                     "<strong>Login ID: " + IdNo + "</strong><br />" +
                     "<strong>Password: " + Password + "</strong><br />" +
                     "You may login to the Member Center at: <a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompWeb"] + "</a><br />" +
                     "<br />" +
                     "<br />" +
                     "</td>" +
                     "</tr>" +
                     "</table>";

            MyMessage.Subject = "Warm Welcome to " + Session["CompName"] + ".!";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());
            smtp.Send(MyMessage);
            return true;
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
            return false;
        }
    }
    protected void btngenerate_Click(object sender, EventArgs e)
    {

    }
    protected void Txtusername_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable DtEmail = new DataTable();
            DataSet DsEmail = new DataSet();
            string strSql = IsoStart + "select Count(idno) as idno from " + ObjDAL.dBName + "..M_Membermaster where idno = '" + Txtusername.Text.Trim() + "' " + IsoEnd;
            DsEmail = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            DtEmail = DsEmail.Tables[0];

            if (Convert.ToInt32(DtEmail.Rows[0]["idno"]) >= 1)
            {
                Txtusername.Text = "";
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                LblUseName.Visible = true;
                LblUseName.Text = "Already Registered by this User ID.!";
                return;
            }
            else
            {
                LblUseName.Visible = false;
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
        }
    }
    protected void TxtWalletaddress_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable DtEmail = new DataTable();
            DataSet DsEmail = new DataSet();
            string strSql = IsoStart + "select Count(walletaddress) as walletaddress from " + ObjDAL.dBName + "..M_Membermaster where walletaddress = '" + TxtWalletaddress.Text.Trim() + "' " + IsoEnd;
            DsEmail = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            DtEmail = DsEmail.Tables[0];

            if (Convert.ToInt32(DtEmail.Rows[0]["walletaddress"]) >= 1)
            {
                txtEMailId.Text = "";
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                LblWalletaddress.Visible = true;
                LblWalletaddress.Text = "Already Registered by this Wallet Address.!";
                return;
            }
            else
            {
                LblWalletaddress.Visible = false;
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
        }
    }
    protected void TxtSponsor_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrEpin.Text = "";
            errMsg.Text = "";
            TxtSponsor.Text = TxtSponsor.Text.Trim().Replace(";", "").Replace("'", "").Replace("=", "");

            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = IsoStart + "Select FormNo,MemFirstName + ' ' + MemLastName as MemName,ActiveStatus from " +
                            ObjDAL.dBName + "..M_MemberMaster where IDNo = '" + TxtSponsor.Text + "' and IsBlock='N' " + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = Ds.Tables[0];

            if (dt.Rows.Count == 0)
            {
                string scrname = "<SCRIPT language='javascript'>alert('No such record/This ID is Flashed./This Id Not Active!!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                TxtSponsor.Text = "";
                return;
            }
            else
            {
                LblSponsorName.Text = dt.Rows[0]["MemName"].ToString();
                Lblsponsorformno.Text = dt.Rows[0]["FormNo"].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
}
