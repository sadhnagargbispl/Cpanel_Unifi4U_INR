using System.Data.SqlClient;
using System.IO;
using System.Net;
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web;
using System.Web.UI;

public partial class Default : System.Web.UI.Page
{
    string uid;
    string Pwd;
    string Memberid;
    string type;
    string scrname;
    SqlConnection conn = new SqlConnection();
    SqlCommand Cmm = new SqlCommand();
    int i;
    SqlDataReader dr;
    
    DAL ObjDal = new DAL();
    DataTable Dt = new DataTable();
    ModuleFunction objModuleFun;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    
    {
        //Session["Status"] = "";
        try
        {
            if (Application["WebStatus"] != null)
            {
                if ((string)Application["WebStatus"] == "N")
                {
                    Session.Abandon();
                    Response.Write("<big><b>" + Application["WebMessage"] + "</b></big>");
                    Response.End();
                    return;
                }
            }
            if (Session["Status"] != null && (string)Session["Status"] == "OK")
            {
                Response.Redirect("Index.aspx", false);
                return;
            }

            string strURL = HttpContext.Current.Request.Url.AbsoluteUri;
            string url = "";

            string Str;

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            if (!Page.IsPostBack)
            {

                if (Request["lgnT"] != null)
                {
                    ModuleFunction objModuleFun = new ModuleFunction();
                    Str = Crypto.Decrypt(Request["lgnT"].Replace(" ", "+"));

                    Str = Str.Replace("uid=", "þ").Replace("&pwd=", "þ").Replace("&mobile=", "þ");
                    string[] qrystr = Str.Split('þ');
                    if ((DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Year.ToString() + (DateTime.Now.Month - 1).ToString() == Request["ID"]) ||
    (DateTime.Now.Day.ToString() + (DateTime.Now.Hour - 1).ToString() + DateTime.Now.Year.ToString() + (DateTime.Now.Month - 1).ToString() == Request["ID"]))

                    {
                        if (Str.Contains("þ"))
                        {
                            int UIdIndx = Str.IndexOf("&pwd");
                            uid = qrystr[1].ToString();
                            Pwd = qrystr[2].ToString();
                            Session["Adminmob"] = qrystr[3].ToString();
                        }
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                        return;
                    }
                }
                else if (Request["uid"] != null)
                {
                    uid = Request["uid"];
                    Pwd = Request["pwd"];
                    type = Request["ref"];
                    uid = uid.Replace("'", "").Replace("=", "").Replace(";", "");
                    Pwd = Pwd.Replace("'", "").Replace("=", "").Replace(";", "");
                }

                if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(Pwd))
                {
                    if (type == "F")
                    {
                        enterFranchisePg();
                    }
                    else
                    {
                        enterHomePgForAdmin();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            conn.Close();
        }
    }
    private void CheckWebStatus()
    {
        try
        {
            WebClient client = new WebClient();
            string baseurl;
            Stream data;

            string s = "";

            try
            {
                baseurl = "https://cpanel.djiomart.com/updateweb_status.aspx";
                data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                s = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                // handle exception
            }

            if (!string.IsNullOrEmpty(s))
            {
                if (s.ToUpper().Split('|')[0] == "N")
                {
                    Response.Write(s.Split('|')[1]);
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
            return StrObj.Trim();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void enterHomePgForAdmin()
    {
        SqlConnection cnn = new SqlConnection();

        try
        {
            cnn = new SqlConnection(constr1);

            if (uid.Length > 0 && Pwd.Length > 0)
            {
                string scrname;
                
                SqlDataReader Dr;

                string strSql = ObjDal.Isostart + " Exec sp_Login_NewINR '" + ClearInject((uid == "" ? ClearInject(Txtuid.Value) : ClearInject(uid))) + "',";
                strSql += "'" + (Pwd == "" ? ClearInject(Txtpwd.Value) : ClearInject(Pwd)) + "'" + ObjDal.IsoEnd;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
                dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    scrname = "<script language='javascript'>alert('Please Enter valid UserName or Password.');</script>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                }
                else
                {
                    Session["Run"] = 0;
                    Session["Status"] = "OK";
                    Session["IDNo"] = dt.Rows[0]["IDNo"];
                    Session["FormNo"] = dt.Rows[0]["Formno"];
                    Session["MemName"] = dt.Rows[0]["MemFirstName"] + " " + dt.Rows[0]["MemLastName"];
                    Session["MobileNo"] = dt.Rows[0]["Mobl"];
                    Session["MemKit"] = dt.Rows[0]["INRKitID"];
                    Session["Package"] = dt.Rows[0]["KitName"];
                    Session["Position"] = dt.Rows[0]["fld3"];
                    Session["Doj"] = ((DateTime)dt.Rows[0]["Doj"]).ToString("dd-MMM-yyyy");
                    Session["DOA"] = ((DateTime)dt.Rows[0]["INRUpgradedate"]).ToString("dd-MMM-yyyy");
                    Session["Address"] = dt.Rows[0]["Address1"];
                    Session["IsFranchise"] = dt.Rows[0]["Fld5"];
                    Session["ActiveStatus"] = dt.Rows[0]["INRActiveStatus"];
                    Session["MemPassw"] = dt.Rows[0]["Passw"];
                    Session["MFormno"] = dt.Rows[0]["MFormNo"];
                    Session["MemUpliner"] = dt.Rows[0]["UplnFormno"];
                    Session["MID"] = dt.Rows[0]["MID"];
                    Session["EMail"] = dt.Rows[0]["Email"];
                    Session["profilepic"] = dt.Rows[0]["profilepic"];
                    Session["Panno"] = dt.Rows[0]["Panno"];
                    Session["ActivationDate"] = dt.Rows[0]["ActivationDate"];
                    Session["MemEPassw"] = dt.Rows[0]["Epassw"];

                    Response.Redirect("index.aspx", false);
                }
                cnn.Close();
            }
            else
            {
                cnn.Close();
            }
        }
        catch (Exception ex)
        {
            if (cnn != null)
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            Response.Write(ex.Message);
        }
    }
    private void enterHomePg()
    {
        SqlConnection cnn = new SqlConnection();
        try
        {
            if (uid.Length > 0 && Pwd.Length > 0)
            {
                string scrname;
                DataTable dt = new DataTable();
                SqlDataReader Dr;
                string strSql = ObjDal.Isostart + " Exec sp_Login1INR '" + ClearInject((uid == "" ? ClearInject(Txtuid.Value) : ClearInject(uid))) + "',";
                strSql += "'" + (Pwd == "" ? ClearInject(Txtpwd.Value) : ClearInject(Pwd)) + "'" + ObjDal.IsoEnd;
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
                dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    scrname = "<script language='javascript'>alert('Please Enter valid UserName or Password.');</script>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                }
                else
                {
                    Session["Run"] = 0;
                    Session["Status"] = "OK";
                    Session["IDNo"] = dt.Rows[0]["IDNo"];
                    Session["FormNo"] = dt.Rows[0]["Formno"];
                    Session["MemName"] = dt.Rows[0]["MemFirstName"] + " " + dt.Rows[0]["MemLastName"];
                    Session["MobileNo"] = dt.Rows[0]["Mobl"];
                    Session["MemKit"] = dt.Rows[0]["INRKitID"];
                    Session["Package"] = dt.Rows[0]["KitName"];
                    Session["Position"] = dt.Rows[0]["fld3"];
                    Session["Doj"] = ((DateTime)dt.Rows[0]["Doj"]).ToString("dd-MMM-yyyy");
                    Session["DOA"] = ((DateTime)dt.Rows[0]["INRUpgradedate"]).ToString("dd-MMM-yyyy");
                    Session["Address"] = dt.Rows[0]["Address1"];
                    Session["IsFranchise"] = dt.Rows[0]["Fld5"];
                    Session["ActiveStatus"] = dt.Rows[0]["INRActiveStatus"];
                    Session["MemPassw"] = dt.Rows[0]["Passw"];
                    Session["MFormno"] = dt.Rows[0]["MFormNo"];
                    Session["MemUpliner"] = dt.Rows[0]["UplnFormno"];
                    Session["MID"] = dt.Rows[0]["MID"];
                    Session["EMail"] = dt.Rows[0]["Email"];
                    Session["profilepic"] = dt.Rows[0]["profilepic"];
                    Session["Panno"] = dt.Rows[0]["Panno"];
                    Session["ActivationDate"] = dt.Rows[0]["ActivationDate"];
                    Session["MemEPassw"] = dt.Rows[0]["Epassw"];
                    Response.Redirect("index.aspx", false);
                }
                cnn.Close();
            }
            else
            {
                cnn.Close();
            }
        }
        catch (Exception ex)
        {
            if (cnn != null)
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            Response.Write(ex.Message);
        }
    }

    private void enterFranchisePg()
    {
        try
        {
            if (uid.Length > 0 && Pwd.Length > 0)
            {
                string Str = " Select * from M_FranchiseMaster where userid='" + uid + "' and passw='" + Pwd + "' ";
                Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, Str).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    Response.Redirect("Default.aspx?Error=Y", false);
                }
                else
                {
                    Session["Franchise"] = "OK";
                    Session["IDNo"] = dr["UserID"];
                    Session["UserID"] = dr["FormNo"];
                    Session["MemName"] = dr["FranchiseName"];
                    Session["Doj"] = ((DateTime)Dt.Rows[0]["Doj"]).ToString("dd-MMM-yyyy");
                    Response.Redirect("Franchise/findex.aspx", false);
                    dr.Close();
                }
            }
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
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
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
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    private bool GetDebitStatus(string Formno)
    {
        try
        {
            bool result = false;
            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string Str = ObjDal.Isostart + "Select Debit From dbo.ufnGetBalance('" + Formno + "','M')" + ObjDal.IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Str);
            dt = Ds.Tables[0];
            if (Convert.ToInt32(dt.Rows[0]["Debit"]) >= 2400)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
        catch (Exception ex)
        {
            // Handle exception
            return false;
        }
    }
    private bool GetPayoutAmountStatus(string Formno)
    {
        try
        {
            bool result = false;
            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string Str = ObjDal.Isostart + " Select TotalIncome from IncomeSummary Where Formno = '" + Formno + "' " + ObjDal.IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Str);
            dt = Ds.Tables[0];
            if (Convert.ToInt32(dt.Rows[0]["TotalIncome"]) >= 2300)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
        catch (Exception ex)
        {
            // Handle exception
            return false;
        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["uid"] != null)
            {
                uid = Request["uid"];
                Pwd = Request["pwd"];
            }
            else
            {
                uid = Txtuid.Value;
                Pwd = Txtpwd.Value;
            }
            type = Request["ref"];
            uid = uid.Replace("'", "").Replace("=", "").Replace(";", "");
            Pwd = Pwd.Replace("'", "").Replace("=", "").Replace(";", "");

            //if (GetDebitStatus(uid.ToUpper().Replace("OS", "")) == false)
            //{
            //    Response.Redirect("http://www.osmtechno.com/");
            //}

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(Pwd))
            {
                if (type == "F")
                {
                    enterFranchisePg();
                }
                else
                {
                    enterHomePg();
                }
            }
            else
            {
                Response.Redirect("logout.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

}
