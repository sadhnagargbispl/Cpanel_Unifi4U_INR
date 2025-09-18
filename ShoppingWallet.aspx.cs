using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShoppingWallet : System.Web.UI.Page
{
    string query;
    DataTable Dt = new DataTable();
    DAL ObjDal = new DAL();
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] == null)
            {
                Response.Redirect("logout.aspx");

            }
            if (!Page.IsPostBack)
            {
                Session["ShopFund"] = null;
                FillBalance();
                FillDetail();
            }

        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }

    }
    private void FillBalance()
    {
        try
        {
            DataTable Dt1=new DataTable();
            query = ObjDal.Isostart + "Select * From dbo.ufnGetBalance('" + Session["FormNo"] + "','S')" + ObjDal.IsoEnd;
            Dt1 = SqlHelper.ExecuteDataset (constr1  , CommandType.Text, query).Tables[0];
            if (Dt1.Rows.Count > 0)
            {
                MCredit.InnerText = Dt1.Rows[0]["Credit"].ToString();
                MDebit.InnerText = Dt1.Rows[0]["Debit"].ToString();
                MBal.InnerText = Dt1.Rows[0]["Balance"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception (ex.Message);
        }
    }
    private void FillDetail ()
    {
        try
        {
            DataTable Dt = new DataTable();
            query = ObjDal.Isostart + " Select Row_Number() Over(Order by VoucherId Desc) As Rn,* From [V#RptServiceFunds] Where FormNo='" + Session["Formno"] + "' " + ObjDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            {
                Session["ShopFund"] = Dt;
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
            FillDetail();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}