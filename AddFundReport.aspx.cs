using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddFundReport : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DAL ObjDal = new DAL();
    string query;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"].ToString() == "OK")
            {
                try
                {
                    if (!Page.IsPostBack)
                    {
                        PaymentDetails();
                    }
                }
                catch (Exception ex)
                {
                    // Handle inner exception
                }
            }
            else
            {
                Response.Redirect("Logout.aspx");
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void PaymentDetails()
    {
        try
        {
          
            query = ObjDal.Isostart  + "exec Sp_GetWalletDetail '" + Session["formno"] + "' " + ObjDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                Session["ReceivedPin"] = Dt;
                RptDirects.DataSource = Dt;
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
            PaymentDetails();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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