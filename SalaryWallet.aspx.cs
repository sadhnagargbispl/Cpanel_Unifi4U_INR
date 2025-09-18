using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class SalaryWallet : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DAL ObjDal = new DAL();
    string query;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] == null)
            {
                Response.Redirect("logout.aspx");
            }
            if(!Page.IsPostBack )
            {
                Session["Salarywallet"] = null;
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
            query = ObjDal.Isostart + "Select * From dbo.ufnGetBalance('" + Session["FormNo"] + "','W')" + ObjDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            if(Dt.Rows.Count>0)
            {
                MCredit.InnerText = Dt.Rows[0]["Credit"].ToString();
                MDebit.InnerText = Dt.Rows[0]["Debit"].ToString();
                MBal.InnerText = Dt.Rows[0]["Balance"].ToString();
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
            DataTable Dt=new DataTable ();
            query = ObjDal.Isostart + "Select Row_Number() Over(Order by VoucherId Desc) As Rn,* From [V#RptSalarywallet] Where FormNo='" + Session["Formno"] + "'" + ObjDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset (constr1 ,CommandType.Text, query).Tables[0];
            {
                Session["Salarywallet"] = Dt;
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
