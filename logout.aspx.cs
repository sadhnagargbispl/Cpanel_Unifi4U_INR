using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.ExpiresAbsolute = DateAndTime.Now;
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
        Response.Cache.SetAllowResponseInBrowserHistory(false);
        Response.Cache.SetNoStore();

        string nextpage = Session["Logout"] as string;
        Session.Abandon();

        //Session.Clear();
        //Response.Cookies.Remove("");
        //Response.Cookies.Clear();
        //Session.RemoveAll();
        //System.Web.Security.FormsAuthentication.SignOut();

        //Response.Write("<script language=javascript>");
        //Response.Write("{");
        //Response.Write(" var Backlen=history.length;");
        //Response.Write(" history.go(-Backlen);");
        //Response.Write(" window.location.href='" + nextpage + "'; ");
        //Response.Write("}");
        //Response.Write("</script>");

        //Session["Status"] = "";
        //Session["FormNo"] = "";
        //Session["Idno"] = "";
        //Session["MemName"] = "";
        //Session["RefIncome"] = "";
        //Session["KitId"] = "";
        //Session["Uid"] = "";
        //Session["CkyPinTransfer"] = "";

        //Page.ClientScript.RegisterStartupScript(this.GetType(), "cle", "windows.history.clear", true);

        if (nextpage == null || nextpage == "default.aspx")
        {
            nextpage = "default.aspx?mod=logout";
        }

        Response.Redirect(nextpage, false);
        //Response.Redirect("https://www.RegalTrade tech.com/members/index.php?mod=logout", false);
        //Response.Redirect("Default.aspx", false);
    }

}