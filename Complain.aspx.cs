using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Complain : System.Web.UI.Page
{
    private string scrname;
    private DAL ObjDal = new DAL();
    DataTable Dt = new DataTable();
    private string constr1 = WebConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private DataTable dt;
    private string str = "";
    private string query;
    private string constr = WebConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BtnSubMit.Attributes.Add("onclick", DisableTheButton(this.Page, BtnSubMit));

            if (Session["Status"] == null)
            {
                Response.Redirect("logout.aspx");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomString(6);
                    Bind_ComplaintType();
                    Filldetail();
                    Session["DtFillDetail"] = null;
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    private void Bind_ComplaintType()
    {
        try
        {
            if (Session["CompType"] == null)
            {
                string Sql = ObjDal.Isostart + "Select CType,CTypeID FROM " + ObjDal.dBName + "..M_ComplaintTypeMaster WHERE RowStatus='Y' AND ActiveStatus='Y'" + ObjDal.IsoEnd;
                Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql).Tables[0];
                Session["CompType"] = Dt;
            }
            else
            {
                Dt = (DataTable)Session["CompType"];
            }

            CmbCmplntType.DataSource = Dt;
            CmbCmplntType.DataTextField = "CType";
            CmbCmplntType.DataValueField = "CTypeID";
            CmbCmplntType.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void TxtDirectSeller_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string scrname = "";
            if (!string.IsNullOrEmpty(TxtDirectSeller.Text))
            {
                if (Session["MemName"] != null)
                {
                    TxtName.Text = (string)Session["MemName"];
                    TxtMobl.Text = (string)Session["MobileNo"];
                    TxtEmail.Text = (string)Session["EMail"];
                    TxtName.Enabled = false;
                    TxtEmail.Enabled = false;
                    TxtMobl.Enabled = false;
                }
                else
                {
                    TxtName.Text = "";
                    TxtMobl.Text = "";
                    TxtEmail.Text = "";
                    TxtName.Enabled = true;
                    TxtEmail.Enabled = true;
                    TxtMobl.Enabled = true;
                    scrname = "<SCRIPT language='javascript'>alert('Invalid Member Id. ');" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                    return;
                }
            }
            else
            {
                TxtName.Text = "";
                TxtMobl.Text = "";
                TxtEmail.Text = "";
                TxtName.Enabled = true;
                TxtEmail.Enabled = true;
                TxtMobl.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void Filldetail()
    {
        try
        {
            TxtDirectSeller.Text = (string)Session["IDNo"];
            TxtName.Text = (string)Session["MemName"];
            TxtEmail.Text = (string)Session["EMail"];
            TxtMobl.Text = Session["MobileNo"].ToString();


            if (string.IsNullOrEmpty(TxtEmail.Text))
                TxtEmail.Enabled = true;
            else
                TxtEmail.Enabled = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public bool SendMail()
    {
        try
        {
            DataTable Dt = new DataTable();
            string sql = "select top 1 ToUserEmail, CId,GroupName from " + ObjDal.dBName + "..M_ComplaintTypeMaster as a," +
                "" + ObjDal.dBName + "..M_ComplaintMaster as b," + ObjDal.dBName + "..M_UserMaster as c," + ObjDal.dBName + "..M_UserGroupMaster as d " +
                         "where a.CtypeId=b.CtypeId and c.UserId=a.UserId and c.GroupId=d.GroupId and d.ActiveStatus='Y' " +
                         "and d.RowStatus='Y' and  c.ActiveStatus='Y' and c.RowStatus='Y' and a.RowStatus='Y' and a.ActiveStatus='Y' " +
                         "and a.CtypeId='" + Convert.ToInt32(CmbCmplntType.SelectedValue) + "' order by CId Desc";
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            string userEmail = "";
            if (Dt.Rows.Count > 0)
            {
                userEmail = Dt.Rows[0]["ToUserEmail"].ToString();
                LblCompalin.Text = Dt.Rows[0]["CId"].ToString();
                Lblgroup.Text = Dt.Rows[0]["GroupName"].ToString();
            }

            if (!string.IsNullOrEmpty(userEmail))
            {
                string StrMsg = "";
                MailAddress SendFrom = new MailAddress(Session["CompMail"].ToString());
                MailAddress SendTo = new MailAddress(userEmail);
                MailMessage MyMessage = new MailMessage(SendFrom, SendTo);
                StrMsg = "<table style=\"margin:0; padding:10px; font-size:14px; font-family:verdana,arial,helvetica; line-height:23px; text-align:justify;width:100%\">" +
                            "<tr>" +
                            "<td>" +
                            "<span style=\" font-weight: bold;\">Dear Sir/Madam,</span><br />" +
                            "<strong>Name: </strong>" + TxtName.Text.Trim() + "<br />" +
                            "<br />" +
                            "I am sending a complaint, please consider it. <br /><br />" +
                            "<span style=\" font-weight: bold;\">My Complaint</span><br />" +
                            "<strong>Complaint Type: </strong>" + CmbCmplntType.SelectedItem.Text.Trim() + "<br />" +
                            "<strong>Subject: </strong>" + TxtSubject.Text.Trim() + "<br />" +
                            "<strong>Description: </strong>" + TxtDesc.Text.Trim() + "<br /><br />" +
                            "You may check it at Admin Panel: <a href=\"" + Session["AdminWeb"].ToString() + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["AdminWeb"].ToString() + "</a><br />" +
                            "<br />" +
                            "<span style=\"color: #0099FF; font-weight: bold;\">Sincerely,</span><br />" +
                            "<a href=\"" + Session["CompWeb"].ToString() + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompName"].ToString() + "</a><br />" +
                            "<br />" +
                            "<br />" +
                            "</td>" +
                            "</tr>" +
                        "</table>";

                MyMessage.Subject = "Member Complaint";
                MyMessage.Body = StrMsg;
                MyMessage.IsBodyHtml = true;

                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return false;
    }
    protected void BtnSubMit_Click(object sender, EventArgs e)
    {
        try
        {
            string scrname;
            string StrSql1 = "Insert into TrncomplainUniqe (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
            int updateeffect = ObjDal.SaveData(StrSql1);
            if (updateeffect > 0)
            {
                if (string.IsNullOrEmpty(TxtName.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Please Enter Name. ');" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                    TxtDesc.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(TxtMobl.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Please Enter MobileNo. ');" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                    TxtDesc.Focus();
                    return;
                }
                else if (Convert.ToInt32(CmbCmplntType.SelectedValue) == 0)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Please Select Nature of Grievance. ');" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                    CmbCmplntType.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(TxtSubject.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Please Enter Subject. ');" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                    TxtSubject.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(TxtDesc.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Please Enter Description. ');" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                    TxtDesc.Focus();
                    return;
                }

                string Sql = "INSERT INTO M_ComplaintMaster(IDNO,CTypeID,CType,Complaint,Subject,MemberName,Mobileno,Email) VALUES " +
                             " ('" + TxtDirectSeller.Text.Trim() + "','" + Convert.ToInt32(CmbCmplntType.SelectedValue) + "','" + CmbCmplntType.SelectedItem.Text.Trim() + "',N'" + TxtDesc.Text.Trim() + "'," +
                             " N'" + CmbCmplntType.SelectedItem.Text.Trim() + "','" + TxtName.Text.Trim() + "','" + TxtMobl.Text.Trim() + "','" + TxtEmail.Text.Trim() + "')";
                int UpdtEffect = ObjDal.SaveData(Sql);

                if (UpdtEffect == 0)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Complaint not sent. ');" + "</SCRIPT>";
                }
                else
                {

                    DataTable Dt = new DataTable();
                    string sql = "Exec Sp_GetComplainID '" + Convert.ToInt32(CmbCmplntType.SelectedValue) + "'";
                    Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                    if (Dt.Rows.Count > 0)
                    {
                        LblCompalin.Text = Dt.Rows[0]["CId"].ToString();
                    }
                    spanError.InnerText = "Your complaint has been successfully submitted on " + DateTime.Now.ToString("dd MMMM yyyy, hh:mm dddd") + ". Your Complaint No. is " + LblCompalin.Text + ". " +
                         "Our customer service representative will get in touch with you shortly. Keep patience.";
                    DivError.Visible = true;
                    spanError.Visible = true;
                    TxtDesc.Text = "";
                    CmbCmplntType.SelectedValue = "2";
                    //Divse.Visible = false;

                }
            }
            else
            {
                string script = "<script type='text/javascript'>window.location.href='Complain.aspx';</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Redirect", script, false);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void sendSMS()
    {
        WebClient client = new WebClient();
        string baseurl;
        Stream data;
        DateTime datet = DateTime.Now;
        string Sms = "";
        Sms = "Hi " + TxtName.Text + ",  Your complaint has been received and complaint no. is" + LblCompalin.Text + ".Regards " + Session["CompName"];
        try
        {
            baseurl = "http://www.apiconnecto.com/API/SMSHttp.aspx?UserId=" + Session["SmsId"] + "&pwd=" + Session["SmsPass"] + "&Message=" + Sms + "&Contacts=" + TxtMobl.Text + "&SenderId=" + Session["ClientId"];
            data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string s;
            s = reader.ReadToEnd();
            data.Close();
            reader.Close();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public bool SendToMemberMail()
    {
        try
        {
            if (!string.IsNullOrEmpty(TxtEmail.Text))
            {
                string StrMsg = "";
                MailAddress SendFrom = new MailAddress(Session["CompMail"].ToString());
                MailAddress SendTo = new MailAddress(TxtEmail.Text.Trim());
                MailMessage MyMessage = new MailMessage(SendFrom, SendTo);

                StrMsg = "<table style=\"margin:0; padding:10px; font-size:14px; font-family:verdana,arial,helvetica; line-height:23px; text-align:justify;width:100%\"> " +
                        " <tr>" +
                        " <td>" +
                        " <span style=\" font-weight: bold;\"> Dear " + TxtName.Text.Trim() + ",</span><br />" +
                        " We have successfully registered your Query. Your Query details are as under for your reference: - <br />" +
                        " <br /> " +
                          " <strong>&nbsp;&nbsp;&nbsp;&nbsp; Direct Seller ID: </strong>" + TxtDirectSeller.Text.Trim() + "<br />" +
                         " <strong>&nbsp;&nbsp;&nbsp;&nbsp; Name: </strong>" + TxtName.Text.Trim() + "<br />" +
                          " <strong>&nbsp;&nbsp;&nbsp;&nbsp; Complaint No: </strong>" + LblCompalin.Text.Trim() + "<br />" +
                         " <strong>&nbsp;&nbsp;&nbsp;&nbsp; Complaint Type: </strong>" + CmbCmplntType.SelectedItem.Text.Trim() + "<br />" +
                        " <strong>&nbsp;&nbsp;&nbsp;&nbsp;  Subject: </strong>" + TxtSubject.Text.Trim() + "<br />" +
                        " <strong>&nbsp;&nbsp;&nbsp;&nbsp;  Description: </strong>" + TxtDesc.Text.Trim() + "<br />" +
                        " <strong>&nbsp;&nbsp;&nbsp;&nbsp;  Status: </strong> Open <br />" +
                          " <strong>&nbsp;&nbsp;&nbsp;&nbsp; Mobile No: </strong>" + TxtMobl.Text.Trim() + "<br />" +
                          " <strong>&nbsp;&nbsp;&nbsp;&nbsp; Email Id: </strong>" + TxtEmail.Text.Trim() + "<br />" +
                         " <br/> " +
                        " Our customer service representative will get in touch with you shortly. Keep patience. " +
                        " <br /> Warm regards,<br />" +
                        " <span style=\"color: #0099FF; font-weight: bold;\"></span>" +
                        " <a href=\"" + Session["CompWeb"].ToString() + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompName"].ToString() + "</a><br />" +
                        " </td>" +
                        " </tr>" +
                        "<tr> " +
                        "<td>Note: This is an automated mail. Kindly DO NOT REPLY or FORWARD your queries to this email address.<br/>" +
                             " For any other queries, Kindly write to " + Session["CompWeb"].ToString() + " or call to our customer care executive at (+91)-0000000000.<br/>" +
                             "  <span style=\"text-align:center\"><b>DISCLAIMER </b></span><br/> " +
                             " This email (including any attachments) is intended for the sole use of the intended recipient/s and may contain material that is " +
                             " CONFIDENTIAL AND PRIVATE COMPANY  INFORMATION. Any review or reliance by others or copying or distribution or forwarding of any or all " +
                             " of the contents in this message is STRICTLY PROHIBITED. If you are not the intended recipient, please contact the sender by email and delete all copies; your cooperation in this regard is appreciated.</td> " +
                        "</tr>" +
                       " </table>";

                MyMessage.Subject = " Complaint Confirmation";
                MyMessage.Body = StrMsg;
                MyMessage.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(Session["MailHost"].ToString());
                smtp.Credentials = new NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());
                smtp.Send(MyMessage);
                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return false;
    }

    public string GenerateRandomString(int iLength)
    {
        try
        {
            string sResult = "";
            string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int random_number = new Random().Next(0, 999);
            string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
            sResult = formatted_datetime;
            return sResult;
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
            StringBuilder sb = new StringBuilder();
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

}
