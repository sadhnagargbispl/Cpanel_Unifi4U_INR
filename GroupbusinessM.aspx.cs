using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GroupbusinessM : System.Web.UI.Page
{
    SqlConnection Conn;
    SqlCommand Comm;
    DataSet Ds = new DataSet();
    SqlDataAdapter Ad;
    DAL ObjDal = new DAL();
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["Status"] != null)

        {
            try
            {
           
                if (!Page.IsPostBack)
                {
                    Filltotal(Session["Formno"].ToString(), Session["Formno"].ToString());
                    LoadDownline(Session["FormNo"].ToString());
                }
            }
            catch (Exception ex)
            {
                string path = HttpContext.Current.Request.Url.AbsoluteUri;
                string text = path + " (Page_Load):   " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
                ObjDal.WriteToFile(text + ex.Message);
                Response.Write("Try later.");
            }
        }
        else
        {
            Response.Redirect("logout.aspx");
        }
    }
    private void Filltotal(string Formno, string dwnFormno)
    {
        try
        {
            DataTable Dt=new DataTable();
            string strSql = ObjDal.Isostart + " Exec Sp_MydirectReportDCG '" + Formno + "','" + dwnFormno + "'" + ObjDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];
            {
                Session["DirectDownline"] = Dt;
                Grdtotal.DataSource = Dt;
                Grdtotal.DataBind();
            }
           
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void LoadDownline(string FormNo)
    {
        try
        {
            DataTable Dt1=new DataTable();
            string s = "";
            s = "";
            string strSql = ObjDal.Isostart + " Exec Sp_MydirectReportNew22 '" + FormNo + "'" + ObjDal.IsoEnd;
            Dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];
            {
                Session["DirectDownline"] = Dt1;
                DLDirects.DataSource = Dt1;
                DLDirects.DataBind();
            }
          
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void PerformData(object sender, EventArgs e)
    {
        try
        {
            RepeaterItem Di = (RepeaterItem)((sender as ImageButton).NamingContainer);
            string FormNo = (Di.FindControl("lblID") as Label).Text;
            Filltotal(Session["formno"].ToString(), FormNo);
            LoadDownline(FormNo);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Data"] = Session["Formno"];
            Session["Data"] = Session["dwnFormno"];
            Filltotal(Session["Formno"].ToString(), Session["dwnFormno"].ToString());
            LoadDownline(Session["FormNo"].ToString());
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

}