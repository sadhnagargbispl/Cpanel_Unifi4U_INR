using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangeTransPass : System.Web.UI.Page
{
    private cls_DataAccess dbConnect;
    private DAL objDal = new DAL();
    DataTable Dt = new DataTable();
    DataTable tmptbl = new DataTable();
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.BtnUpdate.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnUpdate));
            if (Session["Status"] != null)
            {
                //BtnUpdate.Attributes.Add("OnClick", "return ValidForm();")
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
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
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            System.Threading.Thread.Sleep(2000);
            UpdatePass();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void UpdatePass()
    {
        string strQry;
              try
        {
            if (oldpass.Text == "")
            {
                string scrname = "<SCRIPT language='javascript'>alert('Old Transaction password Can not be left blank');</SCRIPT>";
                ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                oldpass.Focus();
                return;
            }
            else if (pass1.Text == "")
            {
                string scrname = "<SCRIPT language='javascript'>alert('New Transaction Password Can not be left blank');</SCRIPT>";
                ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                pass1.Focus();
                return;
            }
            else if (pass2.Text == "")
            {
                string scrname = "<SCRIPT language='javascript'>alert('Confirm Transaction Password Can not be left blank');</SCRIPT>";
                ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                pass2.Focus();
                return;
            }
            else if (oldpass.Text == "" || pass1.Text == "" || pass2.Text == "")
            {
                string scrname = "<SCRIPT language='javascript'>alert('Field Can not be left blank');</SCRIPT>";
                ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                oldpass.Focus();
                return;
            }
            else
            {
                if (pass1.Text != pass2.Text)
                {
                    string scrname = "<SCRIPT language='javascript'>alert('Transaction Password and Confirm Transaction Password not match');</SCRIPT>";
                    ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                    pass1.Text = "";
                    pass2.Text = "";
                    oldpass.Focus();
                    return;
                }
                else
                {
                    string str = objDal .Isostart +"Select IdNo from " + objDal.dBName + "..M_MemberMaster " +
                                    " Where " +
                                    " FormNo='" + Session["Formno"] + "' and " +
                                    " Epassw ='" + oldpass.Text.Trim() + "' " + objDal.IsoEnd ;
                    tmptbl = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
                    if (tmptbl.Rows.Count == 1)
                    {
                        strQry = "Update M_MemberMaster Set EPassw='" + pass1.Text + "' Where FormNo=" + Session["FormNo"] + ";";
                        strQry += " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
                                    "(0,'" + Session["MemName"] + "','Change Transaction Password','Change Transaction Password','Transaction Password Changed',Getdate(),'" + Session["FormNo"] + "')";
                        int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery (constr ,CommandType.Text, strQry));
                        if (i != 0)
                        {
                            string scrname = "<SCRIPT language='javascript'>alert('Transaction Password Changed Successfully !!');</SCRIPT>";
                            ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                        }
                    }
                    else
                    {
                        string scrname = "<SCRIPT language='javascript'>alert('Check Old Transaction  Password');</SCRIPT>";
                        ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                        oldpass.Text = "";
                        oldpass.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void sendSMS()
    {
        dbConnect.OpenConnection();
        if (Session["MobileNo"] != null && Session["MobileNo"].ToString().Length >= 10)

        {
            WebClient client = new WebClient();
            string baseurl;
            Stream data;
            string sms = "Dear " + Session["MemName"] +
                        " Your Password has been Successfully changed" +
                        ",New Password is : '" + pass1.Text + "'";

            try
            {
                baseurl = "http://www.unicel.in/SendSMS/sendmsg.php?uname=" + Session["SmsId"] +
                                    " &pass=" + Session["SmsPass"] +
                                    "&send=" + Session["ClientId"] +
                                    "&dest=" + Session["MobileNo"] +
                                    "&msg=" + sms + "";
                data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred: " + ex.Message;
                lblErrorMessage.Text = errorMessage;
            }
        }
    }

}