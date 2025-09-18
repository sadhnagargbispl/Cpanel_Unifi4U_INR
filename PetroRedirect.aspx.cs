using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PetroRedirect : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["IDNO"] == null)
            {
                Response.Redirect("https://petrocard.unifi4u.com/");
            }
            else
            {
                string formPostText = "";
                formPostText = "<form method=\"POST\" action=\"https://petrocard.unifi4u.com/members/index.php\" name=\"frm2Post\">";
                formPostText += "<input type=\"hidden\" name=\"token\" value=\"bfcfbb3ca3108b1df2eafd14a0aaf1bc\" />";
                formPostText += "<input type=\"hidden\" name=\"mod\" value=\"interLogin\" />";
                formPostText += "<input type=\"hidden\" name=\"userid\" value=\"" + Session["IDNO"].ToString() + "\" />";
                formPostText += "<input type=\"hidden\" name=\"password\" value=\"" + Session["MemPassw"].ToString() + "\" /> ";
                formPostText += "<script type=\"text/javascript\">document.frm2Post.submit();</script>";
                formPostText += "</form>";
                // formPostText += "<input type=\"hidden\" name=\"redirectoff\" value=\"Yes\" /> ";
                Response.Write(formPostText);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }


    }
    private string Base64Encode(string plainText)
    {
        byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    private string Base64Decode(string base64EncodedData)
    {
        byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }


}
