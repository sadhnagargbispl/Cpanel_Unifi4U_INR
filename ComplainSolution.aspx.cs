using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ComplainSolution : System.Web.UI.Page
{
    private DAL ObjDal = new DAL();
    private DataTable dt;
    private string str = "";
    private string query;
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null &&  Session["Status"].ToString() == "OK")
            {
                // UserStatus.InnerText = "Welcome " + Session["MemName"] + "(" + Session["Formno"] + ")" + Session["Company"] + "";
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
            if (Session["Status"].ToString() != "OK")
            {
                Response.Redirect("logout.aspx");
            }

            if (!Page.IsPostBack)
            {
                Session["DtFillDetail"] = null;
                FillDetail();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    private void FillDetail()
    {
        try
        {
            DataSet ds = new DataSet();
            //if (Session["DtFillDetail"] == null)
            //{
            //    query = ObjDal.Isostart  + "Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,";
            //    query += "M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate FROM";
            //    query += "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,";
            //      query += " a.* FROM " + ObjDal.dBName + "..M_ComplaintMaster as a," + ObjDal.dBName + "..M_MemberMaster as b WHERE a.IDNo=b.IDNo ";
            //    query += "AND a.IDNo='" + Session["IDNo"] + "') as M LEFT JOIN " + ObjDal.dBName + "..M_SolutionMaster as S ";
            //       query += "  ON M.CID=S.CID WHERE 1=1  ORDER BY M.RecTimeStamp DESC" + ObjDal.IsoEnd;
            //    ds = SqlHelper.ExecuteDataset(constr1,CommandType.Text, query);
            //    Session["DtFillDetail"] = ds;
            //}
            //else
            //{
            //    ds = (DataSet)Session["DtFillDetail"];
            //}
            string str = 
    " Select M.IDNo, M.CID, Cast(M.CID as varchar) as VCId, M.MemName, " +
    "ISNULL(REPLACE(CONVERT(varchar, M.RecTimeStamp, 106), ' ', '-'), '') as CDate, " +
    "M.CType, M.Complaint, ISNULL(S.Solution, '') as Solution, " +
    "ISNULL(REPLACE(CONVERT(varchar, S.RecTimeStamp, 106), ' ', '-'), '') as SDate, " +
    "M.Status FROM " +
    "(Select b.MemFirstName + ' ' + b.MemLastName as MemName, a.*, " +
    "CASE WHEN a.ComplaintStatus = 'O' THEN 'Open' ELSE 'Close' END as Status " +
    "FROM " + ObjDal.dBName + "..M_ComplaintMaster as a, " +
    ObjDal.dBName + "..M_MemberMaster as b WHERE a.IDNo = b.IDNo AND a.IDNo = '" +
    Session["IDNo"] + "') as M LEFT JOIN " + ObjDal.dBName + "..M_SolutionMaster as S " +
    "ON M.CID = S.CID WHERE 1 = 1 ORDER BY M.RecTimeStamp DESC" ;
            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str);
            dt = new DataTable();
            dt = ds.Tables[0];
            Session["DirectData1"] = dt;
            RptDirects.DataSource = dt;
            RptDirects.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void RptDirects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //try
        //{
        //    FillDetail();
        //    RptDirects.PageIndex = e.NewPageIndex;
        //    RptDirects.DataBind();
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}
    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            // Your code logic here
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
            // Your code logic here
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

}
