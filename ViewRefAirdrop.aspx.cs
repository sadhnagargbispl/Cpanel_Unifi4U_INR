using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewRefAirdrop : System.Web.UI.Page
{
    DataTable Dt = new DataTable ();
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
    protected void Page_Load(object sender, EventArgs e)
    {
        string scrname;
        objModuleFun = new ModuleFunction();

        if (!Page.IsPostBack)
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!string.IsNullOrEmpty(Request["Sessid"]))
                {
                    BindData();
                }
            }
            else
            {
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close", scrname, false);
            }
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
    public void BindData(string SrchCond = "")
    {
        try
        {
            string cond = "";
            string formno = "";
            DataTable Dt = new DataTable();
            string strSql = objDAL.Isostart + " exec sp_GetDailyInsome '" + Session["formno"] + "','" + Request["Sessid"] + "' " + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];
            Session["PaidairdropData"] = Dt;
            if (Dt.Rows.Count > 0)
            {
                GvData.DataSource = Dt;
                GvData.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected void GvData_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["PaidairdropData"];
            GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
}


