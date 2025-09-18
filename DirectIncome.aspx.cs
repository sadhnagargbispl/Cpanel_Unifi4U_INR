using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DirectIncome : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    DAL Objdal = new DAL();
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
  
            if (Session["Status"].ToString() == "OK")
            {
                //UserStatus.InnerText = "Welcome " + Session["MemName"] + "(" + Session["Formno"] + ")" + Session["Company"] + "";
            }
            else
            {
                Response.Redirect("logout.aspx");
            }

            if (!Page.IsPostBack)
            {
                Fill_Grid();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            Objdal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }

    private void Fill_Grid()
    {
        try
        {
            string str = "";
            DataSet Ds = new DataSet();
             str = Objdal.Isostart + " exec sp_GetDirectIncomeNew '" + Session["FormNo"] + "' " + Objdal.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            Session["DirectIncome"] = dt;
            if (dt.Rows.Count > 0)
            {
                RptDirects.DataSource = dt;
                RptDirects.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void RptDirects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            RptDirects.PageIndex = e.NewPageIndex;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DirectIncome"] = null;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
}

