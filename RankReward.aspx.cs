using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class RankReward : System.Web.UI.Page
{
    SqlConnection Conn;
    SqlCommand Comm;
    DataSet Ds = new DataSet();
    SqlDataAdapter Ad;
    DAL obj = new DAL();
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && (string)Session["Status"] == "OK")
            {
                AchieveReward();
            }
            else {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void AchieveReward()
    {
        try
        {
            string str = "Exec Sp_GetRankRewardReportINR '" + Session["formno"] + "'";
            DataTable dt = obj.GetData(str);
            GrdRewards.DataSource = dt;
            GrdRewards.DataBind();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
}
