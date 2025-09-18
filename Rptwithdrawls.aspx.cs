using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Rptwithdrawls : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DAL ObjDal = new DAL();
    string query;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
 
            if (Session["Status"] ==null)
            {
                Response.Redirect("logout.aspx"); 
            }
            if(!Page.IsPostBack)
            {
                LevelDetail();
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }

    }
    private void LevelDetail()
    {
        try
        {
            query = ObjDal.Isostart + "exec Sp_GetFundDetailINR '" + Session["Formno"] + "'" + ObjDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            if (Dt.Rows .Count >0)
            {
                Session["WithdrawData"] = Dt;
                RptDirects.DataSource = Dt;
                RptDirects.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception (ex.Message);
        }
    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            // Code for Page_LoadComplete
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    protected void RptDirects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            RptDirects.PageIndex = e.NewPageIndex;
            LevelDetail();
        }
        catch (Exception ex)
        {
           throw new Exception (ex.Message);
        }
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {
            // Code for Page_Unload
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

}