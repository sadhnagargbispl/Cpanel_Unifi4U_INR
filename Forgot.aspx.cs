using System.Data.SqlClient;
using System.IO;
using System.Net;
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Web.UI;
public partial class Forgot : System.Web.UI.Page
{
    public SqlConnection objSqlConnection;

    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    DataTable Dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           

            if (!Page.IsPostBack)
            {
                // getData();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void Submit_Click(object sender, EventArgs e)
    {
        try
        {
            SqlCommand Comm;
            DataTable Dt;

            SqlDataAdapter Ad;
            string scrname;
            //lblerror.Text = "";
            if (txtIDNo.Text == "")
            {
                scrname = "<SCRIPT language='javascript'>alert('ID No. can not be left blank');" + "</SCRIPT>";
                this.RegisterStartupScript("MyAlert", scrname);
                return;
            }
            if (txtemail.Text == "")
            {
                scrname = "<SCRIPT language='javascript'>alert('Enter Email ID.');" + "</SCRIPT>";
                this.RegisterStartupScript("MyAlert", scrname);
                return;
            }

            if (txtIDNo.Text == "" || txtemail.Text == "")
            {
                scrname = "<SCRIPT language='javascript'>alert('Please Fill Detail');" + "</SCRIPT>";
                this.RegisterStartupScript("MyAlert", scrname);
                return;
            }

            DAL objdal = new DAL();
            string IDNo = txtIDNo.Text.Replace("'", "").Replace(";", "").Replace("=", "").Replace("-", "");
            if (IDNo != "")
            {
                string MemberPass = "";
                string MemberTransPassw = "";
                string str = objdal.Isostart  + " Exec Sp_MemberForgotPassw '" + IDNo + "'" + objdal.IsoEnd;
                Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    string Username = Dt.Rows[0]["Idno"].ToString();
                    string Password = Dt.Rows[0]["Passw"].ToString();
                    string TranPassw = Dt.Rows[0]["EPassw"].ToString();
                    string Email = Dt.Rows[0]["Email"].ToString();
                    string MemfristName = Dt.Rows[0]["MemName"].ToString();
                    string compname = Dt.Rows[0]["CompName"].ToString();
                    string website = Dt.Rows[0]["WebSite"].ToString();
                    Session["SmsId"] = Dt.Rows[0]["smsUsernm"];
                    Session["SmsPass"] = Dt.Rows[0]["SmPass"];
                    Session["ClientId"] = Dt.Rows[0]["smsSenderID"];
                    Session["CompMail"] = Dt.Rows[0]["CompMail"];
                    Session["MailPass"] = Dt.Rows[0]["mailPass"];
                    Session["MailHost"] = Dt.Rows[0]["mailHost"];

                    if (txtemail.Text == Dt.Rows[0]["email"].ToString())
                    {
                        if (txtemail.Text == Dt.Rows[0]["email"].ToString())
                        {
                            MemberPass = Password;
                            MemberTransPassw = TranPassw;
                            MemberPass = MemberPass.Replace("%", "%25").Replace("&", "%26").Replace("#", "%23").Replace("'", "%22").Replace(",", "%2C").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A").Replace("!", "%21").Replace("/", "%2F").Replace("@", "%40");
                            MemberTransPassw = MemberTransPassw.Replace("%", "%25").Replace("&", "%26").Replace("#", "%23").Replace("'", "%22").Replace(",", "%2C").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A").Replace("!", "%21").Replace("/", "%2F").Replace("@", "%40");
                            string sms = "Your Login password is " + MemberPass + " and TXN password is " + MemberTransPassw + " of IDNO " + Username + ".For login go to our site " + website + " .Thank you! Regard: " + compname + "";
                            //objdal.sendsms(sms, TxtMobileNo.Text, Session["ClientId"]);
                            SendToMemberMail(txtIDNo.Text, Email, MemfristName, Password, TranPassw);

                            //scrname = "<SCRIPT language='javascript'>alert('Your Password has been sent on your mobile and E mail Id !');" + "</SCRIPT>";
                            //this.RegisterStartupScript("MyAlert", scrname);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Your Password has been sent on your  E mail Id !');", true);

                            txtIDNo.Text = "";
                            txtemail.Text = "";
                            return;
                        }
                    }
                    else
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Invalid Email Id.');" + "</SCRIPT>";
                        this.RegisterStartupScript("MyAlert", scrname);
                        return;
                    }
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('Invalid Idno.');" + "</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private string SmsAPI(string apiURL, string SMSURL, string action)
    {
        string fUrl = apiURL + "url=" + SMSURL + "&action=" + action;
        string sResponseFromServer = string.Empty;
        try
        {
            WebRequest tRequest;
            Stream dataStream;
            tRequest = WebRequest.Create(fUrl);
            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);
            sResponseFromServer = tReader.ReadToEnd();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
        return sResponseFromServer;
    }
    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Password, string EPassword)
    {
        try
        {
            DataTable dt;
            string sql = "";
            string userEmail = "";

            string StrMsg = "";
            System.Net.Mail.MailAddress SendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress SendTo = new System.Net.Mail.MailAddress(Email);
            System.Net.Mail.MailMessage MyMessage = new System.Net.Mail.MailMessage(SendFrom, SendTo);
            StrMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\">" +
                     "<tr>" +
                     "<td>" +
                     "<span style=\"color: #0099CC; font-weight: bold;\"><h2>Dear " + MemberName + ",</h2></span><br />" +
                     "Your Forgot Login password is <strong>" + Password + "</strong> and Transaction password is <strong>" + EPassword + "</strong> of IDNO <strong>" + IdNo + "</strong>.<br/> For login go to our site : <a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompName"] + "</a><br/>Thank you!<br> Regards : <strong>" + Session["CompName"] + "</strong>" +
                     "<br />" +
                     "<br />" +
                     "</td>" +
                     "</tr>" +
                    "</table>";

            MyMessage.Subject = "Forgot Password";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
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
    public bool SendToMemberMailLife(string IdNo, string Email, string MemberName, string Password, string EPassword)
    {
        try
        {
            DataTable dt;
            string sql = "";
            string userEmail = "";

            string StrMsg = "";
            System.Net.Mail.MailAddress SendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress SendTo = new System.Net.Mail.MailAddress(Email);
            System.Net.Mail.MailMessage MyMessage = new System.Net.Mail.MailMessage(SendFrom, SendTo);
            StrMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%; colour:black;\">" +
                     "<tr>" +
                     "<td>" +
                     "<span style=\" font-weight: bold;\"><h2>Dear " + MemberName + ",</h2></span>" +
                     "Your username and password are given below : <br />" +
                     "<strong>User ID: " + IdNo + "</strong><br />" +
                     "<strong>Password: " + Password + "</strong><br />" +
                     "<strong> Transaction Password: " + EPassword + "</strong><br />" +
                     "You may login to the Member Center at: <a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompWeb"] + "</a><br />" +
                     "<br />" +
                     "<span style=\"color: #0099FF; font-weight: bold;\">Regards,</span><br />" +
                     "<a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompName"] + "</a><br />" +
                     "<br />" +
                     "<br />" +
                     "</td>" +
                     "</tr>" +
                    "</table>";

            MyMessage.Subject = "Forgot Password";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
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
    public bool SendToMemberudaan(string IdNo, string Email, string MemberName, string Password, string EPassword)
    {
        try
        {
            DataTable dt;
            string sql = "";
            string userEmail = "";

            string StrMsg = "";
            System.Net.Mail.MailAddress SendFrom = new  System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress SendTo = new System.Net.Mail.MailAddress(Email);
            System.Net.Mail.MailMessage MyMessage = new System.Net.Mail.MailMessage(SendFrom, SendTo);
            StrMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%; colour:black;\">" +
                     "<tr>" +
                     "<td>" +
                     "<span style=\" font-weight: bold;\"><h2>Dear " + MemberName + ",</h2></span>" +
                     "Your username and password are given below : <br />" +
                     "<strong>User ID: " + IdNo + "</strong><br />" +
                     "<strong>Password: " + Password + "</strong><br />" +
                     "<strong> Transaction Password: " + EPassword + "</strong><br />" +
                     "You may login to the Member Center at: <a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompWeb"] + "</a><br />" +
                     "<br />" +
                     "<span style=\"color: #0099FF; font-weight: bold;\">Regards,</span><br />" +
                     "<a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompName"] + "</a><br />" +
                     "<br />" +
                     "<br />" +
                     "</td>" +
                     "</tr>" +
                    "</table>";

            MyMessage.Subject = "Forgot Password";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
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

}
