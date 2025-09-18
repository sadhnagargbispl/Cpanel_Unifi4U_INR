using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WalletTransfer : System.Web.UI.Page
{
    string scrName;
    DAL Obj = new DAL();
    DAL ObjDAL = new DAL();
    System.Web.UI.HtmlControls.HtmlGenericControl CurrDiv = new System.Web.UI.HtmlControls.HtmlGenericControl();
    string IsoStart;
    string IsoEnd;
    string constr = System.Configuration.ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = System.Configuration.ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            this.cmdSave1.Attributes.Add("onclick", DisableTheButton(this.Page, this.cmdSave1));

            if (!Page.IsPostBack)
            {
                HdnCheckTrnns.Value = GenerateRandomString(6);
                FillWallettype();
                if (!GetReqStatus())
                {
                    // Your request is already pending; please contact the admin!
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "Key",
                        "alert('Request already Sent today, please try again after sometime.!!');location.replace('index.aspx');",
                        true
                    );
                    return;
                }

                Session["CkyPinTransfer1"] = null;
                //GetBalance();
                // GetName();
            }
        }
        else
        {
            Response.Redirect("logout.aspx");
        }

    }
    private void FillWallettype()
    {
        try
        {
            DataTable dt = new DataTable();
            string query = IsoStart + "Exec Sp_GetWalletType " + IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ddlVoucherType.DataSource = dt;
                ddlVoucherType.DataTextField = "WalletName";
                ddlVoucherType.DataValueField = "Actype";
                ddlVoucherType.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    private bool GetReqStatus()
    {
        try
        {
            bool result = false;

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string strSql = "EXEC SP_GetproductStatus " + HdnCheckTrnns.Value;

            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            dt = ds.Tables[0];

            if (Convert.ToInt32(dt.Rows[0]["Cnt"]) == 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
        catch (Exception ex)
        {
            // Log exception or handle as needed
            return false;
        }
    }
    private string DisableTheButton(Control page, Control btn)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(page.Page.GetPostBackEventReference(btn));
        sb.Append(";");

        return sb.ToString();
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
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDouble(txtAmount.Text) < Convert.ToDouble(1))
            {
                string scrname = "<script language='javascript'>alert('Minimum request amount is 1.');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }

            if (Convert.ToDecimal(txtAmount.Text) <= 0)
            {
                scrName = "<SCRIPT language='javascript'>alert('Invalid USDT!!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                return;
            }

            CheckAmount();
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }
    protected void txtMemberId_TextChanged(object sender, EventArgs e)
    {
        GetName();
    }
    private string GetName()
    {
        try
        {
            // Uncomment this block if validation with Session is required
            /*
            if (txtMemberId.Text.Trim().ToUpper() != Session["Idno"].ToString().Trim().ToUpper())
            {
                // Handle mismatched IDs if needed
            }
            */

            string strquery = "";

            strquery = ObjDAL.Isostart +
                       " SELECT * FROM " +
                       Obj.dBName +
                       "..M_membermaster WHERE idno='" +
                       Session["Idno"].ToString().Trim() + "' " +
                       ObjDAL.IsoEnd;

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(
                ConfigurationManager.ConnectionStrings["constr1"].ConnectionString,
                CommandType.Text,
                strquery
            );

            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtMemberName.Text = ds.Tables[0].Rows[0]["Memfirstname"].ToString();
                HdnFormno.Value = ds.Tables[0].Rows[0]["Formno"].ToString();
                return "OK";
            }
            else
            {
                scrName = "<SCRIPT language='javascript'>alert('Invalid ID Does Not Exist');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                TxtMemberName.Text = "";
                HdnFormno.Value = "0";
                return "";
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            return "";
        }
    }
    protected void CheckAmount()
    {
        DataTable dt;
        string str = IsoStart + "Select * From dbo.ufnGetBalance('" + Convert.ToDecimal(Session["Formno"]) + "','" + ddlVoucherType.SelectedValue + "')" + IsoEnd;
        dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
        if (dt.Rows.Count > 0)
        {
            Session["MainBalance"] = Convert.ToDecimal(dt.Rows[0]["Balance"]);
            LblAmount.Text = Convert.ToDecimal(dt.Rows[0]["Balance"]).ToString();
            if (Convert.ToDecimal(Session["MainBalance"]) < Convert.ToDecimal(txtAmount.Text))
            {
                scrName = "<SCRIPT language='javascript'>alert('Insufficient Balance!!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);

                LblAmount.Visible = false;
                cmdSave1.Enabled = false;
                lbladmincharge.Text = "";
            }
            else
            {
                LblAmount.Visible = false;
                cmdSave1.Enabled = true;
            }
        }
    }
    protected void cmdSave1_Click(object sender, EventArgs e)
    {
        try
        {
            string scrname = "";
            if (ddlVoucherType.SelectedValue == "Z")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Please Select Wallet Type.!');", true);
                txtAmount.Text = "0";
                return;
            }
            if (LblAvailableBal.Text == "0")
            {
                scrname = "<script language='javascript'>alert('You do not have enough balance for Wallet.');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            if (txtAmount.Text == "0")
            {
                scrname = "<script language='javascript'>alert('Sorry! You cannot fund request with a zero Amount.');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtPassword.Text.Trim()))
            {
                scrname = "<script language='javascript'>alert('Please Enter Transaction Password.');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }

            if (Convert.ToDecimal(Session["MainBalance"]) < Convert.ToDecimal(txtAmount.Text))
            {
                scrname = "<script language='javascript'>alert('Insufficient Balance!!');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrname, false);
                cmdSave1.Enabled = false;
                lbladmincharge.Text = "";
                txtAmount.Text = "";
                return;
            }

            if (Convert.ToDecimal(txtAmount.Text) < 0)
            {
                scrname = "<script language='javascript'>alert('Sorry! You cannot fund request with a negative value.');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }

            if (GetName() == "OK")
            {
                string str = Obj.Isostart + " Exec Sp_CheckTransctionPassword '" + Convert.ToInt32(Session["Formno"]) + "','" + TxtPassword.Text.Trim() + "'" + Obj.IsoEnd;
                DataTable dts = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, str).Tables[0];
                if (dts.Rows.Count > 0)
                {
                    AmountTransfer();
                }
                else
                {
                    string scrName = "<SCRIPT language='javascript'>alert('Invalid Wallet Password ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                }
            }
            else
            {
                scrname = "<script language='javascript'>alert('Please Enter a valid Wallet Password.');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
    protected void Clear()
    {
        txtMemberId.Text = string.Empty;
        TxtMemberName.Text = string.Empty;
        txtAmount.Text = string.Empty;
        // TxtRemark.Text = string.Empty;
        LblAmount.Text = string.Empty;
        LblAmount.Visible = false;
        LblError.Visible = false;
    }
    protected void AmountTransfer()
    {
        try
        {
            string query;
            string voucherNo = string.Empty;
            string voucherNo2 = string.Empty;
            string voucherNo3 = string.Empty;
            CheckAmount();
            if (Convert.ToDecimal(txtAmount.Text) <= 0)
            {
                scrName = "<SCRIPT language='javascript'>alert('Invalid Amount!!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                return;
            }
            int updateEffect = 0;
            string str = "Insert into Trnfundtransferbyproduct (Transid) values(" + HdnCheckTrnns.Value + ")";
            updateEffect = ObjDAL.SaveData(str);
            if (updateEffect > 0)
            {
                if (Convert.ToDecimal(Session["MainBalance"]) >= Convert.ToDecimal(txtAmount.Text))
                {
                    lblfinal.Text = Convert.ToDecimal(txtAmount.Text).ToString();
                    string remark1 = "Received from " + ddlVoucherType.SelectedItem.Text + " of " + Session["IDNo"];
                    string remark = "Debited for transfer To AirDrop Wallet of " + Session["IDNo"];
                    string remarks = "" + ddlVoucherType.SelectedItem.Text + " To AirDrop Wallet Transfer Of " + txtAmount.Text + " from " + Session["IDNo"];
                    query = " exec Sp_WalletTransfer '" + Session["Formno"] + "','" + lblfinal.Text + "','" + Convert.ToDecimal(lblfinal.Text) + "','" + remark + "','/" + Session["idno"].ToString().Trim() + "'," +
                            "'/" + Session["idno"].ToString().Trim() + "','" + remark1 + "','" + remarks + "',0,'" + Session["MemName"] + "','" + Session["Formno"] + "','" + ddlVoucherType.SelectedValue + "','A'";

                    CheckAmount();

                    if (Convert.ToDecimal(Session["MainBalance"]) >= Convert.ToDecimal(txtAmount.Text))
                    {
                        if (ObjDAL.SaveData(query) > 0)
                        {
                            Clear();

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Amount Transfer Successfully!!');location.replace('WalletTransfer.aspx');", true);
                            LblError.Text = "Amount Transfer Successfully!!";
                            Session["CkyPinTransfer1"] = null;
                            GetBalance();
                            txtMemberId.Text = string.Empty;
                            TxtMemberName.Text = string.Empty;
                            txtAmount.Text = string.Empty;
                            // TxtRemark.Text = string.Empty;
                            LblAmount.Text = string.Empty;
                            cmdSave1.Visible = true;
                        }
                    }
                }
                else
                {
                    Clear();
                    scrName = "<SCRIPT language='javascript'>alert('Insufficient Balance!!');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                    LblError.Text = "Insufficient Balance!!";
                    txtMemberId.Text = string.Empty;
                    TxtMemberName.Text = string.Empty;
                    txtAmount.Text = string.Empty;
                    // TxtRemark.Text = string.Empty;
                    LblAmount.Text = string.Empty;
                    cmdSave1.Visible = true;
                }
            }
            else
            {
                scrName = "<SCRIPT language='javascript'>alert('Try Later.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Try Later.!');location.replace('AllWalletReport.aspx');", true);
        }
    }
    protected void GetBalance()
    {
        try
        {
            DataTable dt;
            string str = "Select * From dbo.ufnGetBalance('" + Convert.ToDecimal(Session["Formno"]) + "','" + ddlVoucherType.SelectedValue + "')";

            dt = ObjDAL.GetData(str);

            if (dt.Rows.Count > 0)
            {
                LblAvailableBal.Text = Convert.ToDecimal(dt.Rows[0]["Balance"]).ToString();
                Session["MainBalance"] = Convert.ToDecimal(dt.Rows[0]["Balance"]).ToString();
            }
            else
            {
                Session["MainBalance"] = 0;
                LblAvailableBal.Text = "0";
            }
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }
    protected void ddlVoucherType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetBalance();
            //if (!GetAmountStatus())
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('You Are Not Authorised For Wallet. Because Your Wallet Balance Is Low.!');location.replace('Index.aspx');", true);
            //    return;
            //}
            LblAvailableBal.Text = Amount().ToString();
        }
        catch (Exception ex)
        {
        }
    }
    private double Amount()
    {
        try
        {
            double rtrVal = 0;
            string str = IsoStart + "Select balance From dbo.ufnGetBalance('" + Session["FormNo"].ToString() + "','" + ddlVoucherType.SelectedValue + "')" + IsoEnd;

            using (SqlConnection cnn = new SqlConnection())
            {
                SqlDataReader dr = SqlHelper.ExecuteReader(constr1, CommandType.Text, str, cnn);
                if (dr.Read())
                {
                    rtrVal = Convert.ToDouble(dr["Balance"]);
                }
                dr.Close();
            }

            return rtrVal;
        }
        catch (Exception ex)
        {
        }

        return 0; // Return a default value in case of an exception
    }
    private bool GetAmountStatus()
    {
        try
        {
            bool result = false;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string str = "";
            str = IsoStart + "Select balance From dbo.ufnGetBalance('" + Session["FormNo"].ToString() + "','" + ddlVoucherType.SelectedValue + "')" + IsoEnd;
            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str);
            dt = ds.Tables[0];

            if (Convert.ToDecimal(dt.Rows[0]["balance"]) < Convert.ToDecimal(Session["MainBalance"]))
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
            // Handle the exception if needed
        }
        return false;  // In case of an exception, return false as default
    }
}