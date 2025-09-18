using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    DAL objDal = new DAL();
    DataTable dt = new DataTable();
    SqlConnection DbConnect;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (Application["WebStatus"] != null)
            {
                if (Application["WebStatus"].ToString() == "N")
                {
                    Session.Abandon();
                    Response.Redirect("default.aspx", false);
                }
            }
            if (!Page.IsPostBack)
            {
                DataTable Dt = new DataTable();
                string str = "";
                if (Session["Status"] != null && Session["Status"].ToString() == "OK")
                {
                    FillStopWithdrawal();
                    string Strrank = objDal.Isostart + "Exec Sp_GetMasterDetailINR '" + Session["Formno"].ToString() + "' " + objDal.IsoEnd;
                    Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Strrank).Tables[0];
                    if (Dt.Rows.Count > 0)
                    {
                        LblId.Text = Dt.Rows[0]["idno"].ToString();
                        LblName.Text = Dt.Rows[0]["memname"].ToString();
                        LblRegistrationDate.Text = Dt.Rows[0]["DOJ"].ToString();
                        LblStackingActivationDate.Text = Dt.Rows[0]["DOA"].ToString();
                        //LblpetrocardActivationDate.Text = Dt.Rows[0]["Petrocarddate"].ToString();
                    }
                    string lnt = Crypto.Encrypt("uid=" + Session["IDNo"].ToString() + "&pwd=" + Session["MemPassw"].ToString() + "&mobile=" + Session["MobileNo"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void FillStopWithdrawal()
    {
        try
        {
            string query = "";
            query = "Exec Sp_GetStopWithdrawalCheck '" + Session["FormNo"] + "' " ;

            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                Session["RStatus"] = dt.Rows[0]["RStatus"];
            }
            else
            {
                Session["RStatus"] = "N";
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }

    private string Encryptdata(string Data)
    {

        string strmsg = string.Empty;
        try
        {
            byte[] encode = new byte[Data.Length];
            encode = Encoding.UTF8.GetBytes(Data);
            strmsg = Convert.ToBase64String(encode);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return strmsg;
    }


}
