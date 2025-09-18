using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UtilityServicesRedirect : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["IDNO"] == null)
            {
                Response.Redirect("https://utility.unifi4u.com/");
            }
            else
            {
                string Token = "darabUnMar5489pidlAytvgjgjghewUF4875brlE8a4i5n61046barghfdhcfghf";
                string memPassw = Session["MemPassw"].ToString();
                string info12 = "UserName=" + Session["IDNO"].ToString() + "&Password=" + memPassw.Replace("%25", "%25").Replace("#", "%23").Replace("&", "%26").Replace("'", "%22").Replace("@", "%40") + "&Action=Login&Token=" + Token;
                string refes = Base64Encode(info12);
                string Url = "https://utility.unifi4u.com/handler/login.ashx?url=" + refes;
                Response.Redirect(Url);
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
