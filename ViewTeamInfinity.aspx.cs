using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewTeamInfinity : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    ModuleFunction objModuleFun;
    string ReqNo;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["Status"].ToString() != "OK")
        {
            Response.Redirect("Logout.aspx");
        }
    }
    protected void RptDirects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            objDAL = new DAL();
            string scrname;
            objModuleFun = new ModuleFunction();

            if (!Page.IsPostBack)
            {
                if (Session["Status"]!= null)
                {
                    if (!string.IsNullOrEmpty(Request["Sessid"]))
                    {
                        BindData();
                    }
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'> window.top.location.reload();" + "</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void BindData(string SrchCond = "")
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string cond = "";
            string formno = "";
            string strSql = objDAL.Isostart + " exec sp_GetLevelIncome '" + Session["formno"] + "','" + Request["Sessid"] + "' " + objDAL.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];
            Session["GDatalevel"] = dt;
            if (dt.Rows.Count > 0)
            {
                GvData.DataSource = dt;
                GvData.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GDatalevel"];
            GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
}
