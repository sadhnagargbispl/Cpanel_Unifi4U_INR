using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
public partial class thankyou : System.Web.UI.Page
{
    DAL ObjDAL = new DAL();
    string IsoStart;
    string IsoEnd;
    string constr = System.Configuration.ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = System.Configuration.ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            // BtnProceedToPay.Attributes.Add("onclick", DisableTheButton(this.Page, BtnProceedToPay));
            if (!string.IsNullOrEmpty(Request["billno"]))
            {
                FillKit(Request["billno"]);
            }
        }
        else
        {
            Response.Redirect("default.aspx", false);
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please Wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }
    private void FillKit(string BillNo)
    {
        try
        {
            DataSet ds = new DataSet();
            string sql = IsoStart + "Exec Sp_GetBillNoWiseDetail '" + BillNo + "' " + IsoEnd;
            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                LblOrderNumber.Text = ds.Tables[0].Rows[0]["OrderNumber"].ToString();
                LblPackageName.Text = ds.Tables[0].Rows[0]["Kitname"].ToString();
                LblOrderDate.Text = ds.Tables[0].Rows[0]["OrderDate"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnProceedToPay_Click(object sender, EventArgs e)
    {
        string kitid = Request["kitid"];

        if (kitid != null)
        {
            if (kitid == "1" || kitid == "2" || kitid == "3" || kitid == "4" || kitid == "5" || kitid == "6" || kitid == "8" || kitid == "9" || kitid == "10"
                || kitid == "11" || kitid == "12" || kitid == "13" || kitid == "14")
            {
                Response.Redirect("UtilityServicesRedirect.aspx");
            }
            // Uncomment and add conditions for other redirects as needed
            // else if (kitid == "5" || kitid == "6" || kitid == "7" || kitid == "8")
            // {
            //     Response.Redirect("FoodRedirect.aspx");
            // }
            // else if (kitid == "9" || kitid == "10" || kitid == "11" || kitid == "12")
            // {
            //     Response.Redirect("gvRedirect.aspx");
            // }
            // else if (kitid == "13" || kitid == "14" || kitid == "15")
            // {
            //     Response.Redirect("MovieRedirect.aspx");
            // }
            // else if (kitid == "16" || kitid == "17")
            // {
            //     Response.Redirect("HolidayRedirect.aspx");
            // }
        }
    }


}
