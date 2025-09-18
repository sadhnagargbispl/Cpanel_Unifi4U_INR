using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reply : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    string scrname;
    DAL objDAL = new DAL();
    cls_DataAccess DbConnect;
    string CIdQS;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        objDAL = new DAL();
        if (string.IsNullOrEmpty(Request.QueryString["CId"]) == false)
        {
            CIdQS = Request.QueryString["CId"];

            if (!IsPostBack)
            {
                if (Session["Status"] != null && Session["Status"].ToString() == "OK")
                {
                    if (string.IsNullOrEmpty(Request.QueryString["CId"]) == false)
                    {
                        BindData();
                    }
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'> window.top.location.reload();" + "</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                }
            }
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"].ToString() == "OK")
            {
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void BindData()
    {
        try
        {
            CIdQS = Request.QueryString["CId"];
            string sql = "Select M.IDNo,M.MemName,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,";
            sql += "M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate FROM";
            sql += "(Select b.MemFirstName +' '+ b.MemLastName as MemName,a.*";
            sql += " FROM " + objDAL.dBName + "..M_ComplaintMaster as a," + objDAL.dBName + "..M_MemberMaster as b";
            sql += " WHERE a.IDNo=b.IDNo AND a.CID='" + CIdQS + "') as M LEFT JOIN " + objDAL.dBName + "..M_SolutionMaster as S";
               sql += " ON M.CID=S.CID";
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                LblCType.Text = Dt.Rows[0]["CType"].ToString();
                TxtComplaint.Text = Dt.Rows[0]["Complaint"].ToString();
                TxtPreReply.Text = "";
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    if (TxtPreReply.Text != "")
                    {
                        TxtPreReply.Text = TxtPreReply.Text + Environment.NewLine + "-----------------------------------------" + Environment.NewLine;
                    }
                    if (Dt.Rows[i]["Solution"].ToString().Trim() != "")
                    {
                        TxtPreReply.Text = TxtPreReply.Text + Dt.Rows[i]["SDate"].ToString() + ": " + Environment.NewLine + Dt.Rows[i]["Solution"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void ClearAll()
    {
        try
        {
            LblCType.Text = "";
            TxtComplaint.Text = "";
            TxtPreReply.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception (ex.Message);
        }
    }

}
