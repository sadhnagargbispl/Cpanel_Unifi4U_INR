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
using System.Xml;

public partial class ChangePass : System.Web.UI.Page
{
    private cls_DataAccess dbConnect;
    private clsGeneral dbGeneral = new clsGeneral();
    private string scrname;
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dRead;
    private DAL objDal = new DAL();
    DataTable tmptbl = new DataTable();
    DataTable Dt = new DataTable();
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
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
    private void UpdatePass()
    {
        try
        {
            string strQry;
            try
            {
                if (oldpass.Text == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", "<script language='javascript'>alert('Old password Can not be left blank');</script>");
                    oldpass.Focus();
                    return;
                }
                else if (pass1.Text == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", "<script language='javascript'>alert('New Password Can not be left blank');</script>");
                    pass1.Focus();
                    return;
                }
                else if (pass2.Text == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", "<script language='javascript'>alert('Confirm Password Can not be left blank');</script>");
                    pass2.Focus();
                    return;
                }
                else if (oldpass.Text == "" || pass1.Text == "" || pass2.Text == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", "<script language='javascript'>alert('Field Can not be left blank');</script>");
                    oldpass.Focus();
                    return;
                }
                else
                {
                    if (pass1.Text != pass2.Text)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", "<script language='javascript'>alert('Password and Confirm password not match');</script>");
                        pass1.Text = "";
                        pass2.Text = "";
                        oldpass.Focus();
                        return;
                    }
                    else
                    {
                        string str = objDal.Isostart +"Select IdNo from " + objDal.dBName + ".. M_MemberMaster " +
                                     " Where " +
                                     " FormNo='" + Session["Formno"] + "' and " +
                                     " passw ='" + oldpass.Text.Trim() + "' " + objDal.IsoEnd ;
                        tmptbl = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
                        if (tmptbl.Rows.Count == 1)
                        {
                            strQry = "Update M_MemberMaster Set Passw='" + pass1.Text + "',E_MainPassw='" + pass1.Text + "' Where FormNo=" + Session["FormNo"] + ";";
                            int i = objDal.SaveData(strQry);
                            if (i != 0)
                            {
                                Response.Write("<script language='javascript'>window.alert('Password Changed Successfully, Login Again!!');window.location='logout.aspx';</script>");
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", "<script language='javascript'>alert('Check Old Password');</script>");
                            oldpass.Text = "";
                            oldpass.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string path = HttpContext.Current.Request.Url.AbsoluteUri;
                string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
                objDal.WriteToFile(text + ex.Message);
                Response.Write("Try later.");
                ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", "<script language='javascript'>alert('" + ex.Message + "');</script>");
                return;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void sendSMS()
    {
        try
        {
            dbConnect.OpenConnection();
            if (Session["MobileNo"] != null && Session["MobileNo"].ToString().Length >= 10)

            {
                WebClient client = new WebClient();
                string baseurl;
                Stream data;
                string sms = "Dear " + Session["MemName"].ToString() +
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
                    // Assuming you have a label control named lblErrorMessage on your ASPX page
                    lblErrorMessage.Text = errorMessage;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}