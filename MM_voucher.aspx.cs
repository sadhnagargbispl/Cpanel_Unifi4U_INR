using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class MM_voucher : System.Web.UI.Page
{
    DAL ObjDAL = new DAL();
    string IsoStart;
    string IsoEnd;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            IsoStart = ObjDAL.Isostart;
            IsoEnd = ObjDAL.IsoEnd;

            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    DataTable tmpTable = new DataTable();

                    //string strQuery = IsoStart + "Exec Sp_GetRegistration '" + Session["formno"] + "'" + IsoEnd;
                    //tmpTable = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
                    //if (tmpTable.Rows.Count > 0)
                    //{
                    //    if (Convert.ToInt32(tmpTable.Rows[0]["Cnt"]) > 0)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('You Are Not Access This Page.Please Contact To Admin.!');location.replace('Home.aspx');", true);
                    //        return;
                    //    }
                    //}

                    //if (Session["ActiveStatus"] != null && Session["ActiveStatus"].ToString() == "Y")
                    //{
                    FUN_SP_GETLIST();
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('This Id is DeActive Please Activate First.!');location.replace('Index.aspx');", true);
                    //    return;
                    //}
                }
            }
            else
            {
                Response.Redirect("default.aspx", false);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FUN_SP_GETLIST()
    {
        try
        {
            DataSet Ds = new DataSet();
            string Sql = IsoStart + "Exec Sp_GetAllPageckeDetail" + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql);

            if (Ds.Tables[0].Rows.Count > 0)
            {
                RepUtility.DataSource = Ds.Tables[0];
                RepUtility.DataBind();
            }

            //if (Ds.Tables[1].Rows.Count > 0)
            //{
            //    RptShoppingVoucher.DataSource = Ds.Tables[1];
            //    RptShoppingVoucher.DataBind();
            //}
            //if (Ds.Tables[2].Rows.Count > 0)
            //{
            //    RepGV.DataSource = Ds.Tables[2];
            //    RepGV.DataBind();
            //}
            //if (Ds.Tables[3].Rows.Count > 0)
            //{
            //    RepMovie.DataSource = Ds.Tables[3];
            //    RepMovie.DataBind();
            //}
            //if (Ds.Tables[4].Rows.Count > 0)
            //{
            //    RepHoliDay.DataSource = Ds.Tables[4];
            //    RepHoliDay.DataBind();
            //}
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
