using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using Microsoft.Ajax.Utilities;
using System.Net;
using System.Security.Policy;

public partial class Index : System.Web.UI.Page
{
    DAL Obj = new DAL();
    DataSet Ds;
    string IsoStart;
    string IsoEnd;
    SqlDataReader dr;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    LoadTeam();
                   
                }
            }
            else
            {
                string key = string.Empty;
                try
                {
                    string KeyE = "6b04d38748f94490a636cf1be3d82841";
                    string IV = "f8adbf3c94b7463d";
                    byte[] KeyB = Encoding.ASCII.GetBytes(KeyE);
                    byte[] IVB = Encoding.ASCII.GetBytes(IV);

                    key = Request.Form["key"];
                    bool Islogin = false;

                    string[] Result = Decrypt(key, KeyB, IVB).Split(',');
                    int Result1 = (int)(DateTime.Now - Convert.ToDateTime(Result[2])).TotalMinutes;

                    if (Result1 > 30)
                    {
                        Response.Redirect("Logout.aspx");
                    }
                }
                catch (Exception)
                {
                    Response.Redirect("Logout.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void LoadTeam()
    {
        try
        {
            DataSet Ds = new DataSet();
            string strquery = string.Empty;
            strquery = IsoStart;
            strquery += " Exec sp_LoadTeamNewUpdateINR '" + Session["FormNo"].ToString() + "' ";
            strquery += IsoEnd;
            Ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, strquery);
            Session["LoadTeam"] = Ds;
            if (Ds.Tables[0].Rows.Count > 0)
            {
                RptIncome.DataSource = Ds.Tables[0];
                RptIncome.DataBind();
            }
            if (Ds.Tables[1].Rows.Count > 0)
            {
                RptDirects.DataSource = Ds.Tables[1];
                RptDirects.DataBind();
            }
            if (Ds.Tables[2].Rows.Count > 0)
            {
                LblMemName.Text = Ds.Tables[2].Rows[0]["Name"].ToString();
                LblMemId.Text = Ds.Tables[2].Rows[0]["IdNo"].ToString();
                Image2.ImageUrl = Ds.Tables[2].Rows[0]["ProfilePic"].ToString();
                lblLink.Text = "http://" + HttpContext.Current.Request.Url.Host + "/Newjoining1.aspx?ref=" + Crypto.Encrypt(Ds.Tables[2].Rows[0]["mid"].ToString() + "/0");
                aRfLink.HRef = lblLink.Text;
            }
            //if (Ds.Tables[4].Rows.Count > 0)
            //{
            //    TotalInv.Text = Ds.Tables[4].Rows[0]["totalinv"].ToString();
            //    lblinvest.Text = Ds.Tables[4].Rows[0]["percentage"].ToString();
            //    lblinv.Text = Ds.Tables[4].Rows[0]["Inv"].ToString();
            //    lblbalance.Text = Ds.Tables[4].Rows[0]["Inv"].ToString();
            //    double progress = 0.00;
            //    if (lblinv.Text == "0.000")
            //    {
            //        divprogress.InnerText = "";
            //    }
            //    else
            //    {
            //        progress = Math.Round((Convert.ToDouble(lblinv.Text) / Convert.ToDouble(lblinvest.Text)) * 100, 2);
            //        divprogress.InnerText = progress + "%";
            //    }
            //    divprogress.Style.Add("color", "white");
            //    divprogress.Style.Add("width", ((Convert.ToDouble(lblinv.Text) / Convert.ToDouble(lblinvest.Text)) * 100).ToString() + "%");
            //    divprogress.Attributes.Add("aria-valuenow", ((Convert.ToDouble(lblinv.Text) / Convert.ToDouble(lblinvest.Text)) * 100).ToString());

            //    TotalInvestment.Text = Ds.Tables[4].Rows[0]["totalinv"].ToString();
            //    lblinvests.Text = Ds.Tables[4].Rows[0]["pers"].ToString();
            //    lblinvs.Text = Ds.Tables[4].Rows[0]["InvROIKE"].ToString();
            //    lblbalances.Text = Ds.Tables[4].Rows[0]["InvROIKE"].ToString();
            //    double progresss = 0.00;
            //    if (lblinvs.Text == "0.000")
            //    {
            //        divprogressbar.InnerText = "";
            //    }
            //    else
            //    {
            //        progresss = Math.Round((Convert.ToDouble(lblinvs.Text) / Convert.ToDouble(lblinvests.Text)) * 100, 2);
            //        divprogressbar.InnerText = progresss + "%";
            //    }
            //    divprogressbar.Style.Add("color", "white");

            //    divprogressbar.Style.Add("width", ((Convert.ToDouble(lblinvs.Text) / Convert.ToDouble(lblinvests.Text)) * 100).ToString() + "%");
            //    divprogressbar.Attributes.Add("aria-valuenow", ((Convert.ToDouble(lblinvs.Text) / Convert.ToDouble(lblinvests.Text)) * 100).ToString());
            //}
            if (Ds.Tables[5].Rows.Count > 0)
            {
                gvBalance.DataSource = Ds.Tables[5];
                gvBalance.DataBind();
            }
            if (Ds.Tables[6].Rows.Count > 0)
            {
                gv.DataSource = Ds.Tables[6];
                gv.DataBind();
            }
            if (Ds.Tables[7].Rows.Count > 0)
            {
                RptNews.DataSource = Ds.Tables[7];
                RptNews.DataBind();
            }
            if (Ds.Tables[8].Rows.Count > 0)
            {
                SpanRank.InnerHtml = Ds.Tables[8].Rows[0]["Rank"].ToString();
            }
            else
            {
                SpanRank.InnerText = "Not Achieved";
            }
            if (Ds.Tables[10].Rows.Count > 0)
            {
                LblOpenLevel.InnerText = Ds.Tables[10].Rows[0]["OpenLevel"].ToString();
            }
            else
            {
                LblOpenLevel.InnerText = "0";
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }

    }
    protected void RptDirects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            RptDirects.PageIndex = e.NewPageIndex;
            LoadTeam();
        }
        catch (Exception ex)
        {
            // Handle or log the exception if necessary
        }
    }
    private void Load_PetroCardSale()
    {
        try
        {
            DataSet Ds = new DataSet();
            string strquery = Obj.Isostart + " Exec Sp_GetPetroCardSaleCpanel " + Obj.IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strquery);
            //GridPetroCardselect.DataSource = Ds.Tables[0];
            //GridPetroCardselect.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void Load_Headings()
    {
        try
        {
            DataSet Ds = new DataSet();
            string strquery = Obj.Isostart + " Exec Sp_GetPopup " + Obj.IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strquery);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        Ds.Tables[0].Rows[i]["divstart"] = Ds.Tables[0].Rows[i]["ImgPath"];
                    }
                    if (i == 1)
                    {
                        Ds.Tables[0].Rows[i]["divstart"] = Ds.Tables[0].Rows[i]["ImgPath"];
                    }
                }
            }
            Session["PopupImg"] = Ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
    private static string Encrypt(string plainText, byte[] Key, byte[] IV)
    {
        byte[] encrypted;
        using (AesManaged aes = new AesManaged())
        {
            ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    encrypted = ms.ToArray();
                }
            }
        }
        return Convert.ToBase64String(encrypted);
    }
    private static string Decrypt(string data, byte[] Key, byte[] IV)
    {
        byte[] cipherText = Convert.FromBase64String(data);
        string plaintext = null;

        using (AesManaged aes = new AesManaged())
        {
            ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        plaintext = reader.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }
}
