using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts : System.Web.UI.Page
{
    string query;
    DataTable dt = new DataTable();
    DAL Objdal = new DAL();
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"].ToString() != "OK")
            {
                Response.Redirect("logout.aspx");
            }
            if (!Page.IsPostBack)
            {
                Session["DtFillDetail"] = null;
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
            DataTable dt = new DataTable();
            query= Objdal.Isostart + "Select * From dbo.ufnGetBalance('" + Session["FormNo"] + "','M')" + Objdal.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            if(dt.Rows.Count > 0)
            {
                MCredit.InnerText = dt.Rows[0]["Credit"].ToString();
                MDebit.InnerText = dt.Rows[0]["Debit"].ToString();
                MBal.InnerText = dt.Rows[0]["Balance"].ToString();

            }
        }
        catch (Exception ex)
        {
            throw new Exception (ex.Message);
        }
    }
    private void FillDetail()
    {
        try
        {
            DataTable dt= new DataTable();
            query = Objdal.Isostart  + "Select Row_Number() Over(Order by VoucherId Desc) As Rn,* From [V#RptMainFund] Where FormNo='" + Session["Formno"] + "'" + Objdal.IsoEnd ;
            dt= SqlHelper.ExecuteDataset(constr1,CommandType.Text, query).Tables[0];
            {
                Session["DtFillDetail"] = dt;
                Session["MainFund"] = dt;
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
            FillDetail();
            RptDirects.PageIndex = e.NewPageIndex;
            RptDirects.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
  

}
