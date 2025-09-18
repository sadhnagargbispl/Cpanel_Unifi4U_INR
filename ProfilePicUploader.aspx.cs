using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProfilePicUploader : System.Web.UI.Page
{
    DAL Obj = new DAL();
    DataSet Ds;
    string IsoStart;
    string IsoEnd;
    SqlDataReader dr;
    string scrname;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Status"].ToString() == "OK")
        {
            if (!Page.IsPostBack)
            {
                LoadImages();
            }
            if (FUIdentity.HasFile)
            {
                UploadFile();
            }
        }
        else
        {
            Response.Redirect("logout.aspx");
        }

    }
    private void LoadImages()
    {
        cls_DataAccess DbConnect = new cls_DataAccess(constr.ToString());
        DbConnect.OpenConnection();

        using (SqlCommand cmd = new SqlCommand("SELECT IDNo, MemFirstName AS MemName, ProfilePic FROM M_MemberMaster WHERE Formno = @Formno", DbConnect.cnnObject))
        {
            cmd.Parameters.AddWithValue("@Formno", Session["Formno"] != null ? Session["Formno"].ToString() : string.Empty);

            using (SqlDataReader dRead = cmd.ExecuteReader())
            {
                if (dRead.Read())
                {
                    lblNm.Text = dRead["MemName"].ToString() + "[" + dRead["IDNo"].ToString() + "]";

                    if (dRead["ProfilePic"] == DBNull.Value || string.IsNullOrEmpty(dRead["ProfilePic"].ToString()))
                    {
                        Image4.ImageUrl = "~/images/no_photo.jpg";
                    }
                    else
                    {
                        Image4.ImageUrl = dRead["ProfilePic"].ToString();
                    }
                }
            }
        }

        DbConnect.cnnObject.Close();
    }
    protected void UploadFile()
    {
        try
        {
            string FlNm = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            if (FUIdentity.PostedFile != null && FUIdentity.PostedFile.FileName != "")
            {
                string strExtension = System.IO.Path.GetExtension(FUIdentity.FileName).ToUpper();
                if (strExtension == ".JPG" || strExtension == ".GIF" || strExtension == ".PNG" || strExtension == ".JPEG")
                {
                    string FileName = Server.MapPath("images/UploadImage/" + FlNm + strExtension);
                    // Resize Image Before Uploading to Database
                    System.Drawing.Image imageToBeResized = System.Drawing.Image.FromStream(FUIdentity.PostedFile.InputStream);
                    int imageHeight = imageToBeResized.Height;
                    int imageWidth = imageToBeResized.Width;
                    int maxHeight = 100;
                    int maxWidth = 100;

                    imageHeight = (imageHeight * maxWidth) / imageWidth;
                    imageWidth = maxWidth;
                    if (imageHeight > maxHeight)
                    {
                        imageWidth = (imageWidth * maxHeight) / imageHeight;
                        imageHeight = maxHeight;
                    }

                    using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(imageToBeResized, imageWidth, imageHeight))
                    {
                        if (strExtension == ".JPG" || strExtension == ".JPEG")
                            bitmap.Save(FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        if (strExtension == ".PNG")
                            bitmap.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
                        if (strExtension == ".GIF")
                            bitmap.Save(FileName, System.Drawing.Imaging.ImageFormat.Gif);
                    }

                    FileName = "http://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + FlNm + strExtension;

                    // Create SQL Connection
                    using (SqlConnection con = new SqlConnection(Application["Connect"].ToString()))
                    {
                        con.Open();
                        string sql = "UPDATE m_MemberMaster SET ProfilePic = @Image WHERE Formno = @Formno";

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@Image", FileName);
                            cmd.Parameters.AddWithValue("@Formno", Session["Formno"].ToString());

                            int result = cmd.ExecuteNonQuery();
                            if (result != 0)
                            {
                                scrname = "<SCRIPT language='javascript'>alert('Profile Pic Upload Successfully.!!');</SCRIPT>";
                            }
                            else
                            {
                                scrname = "<SCRIPT language='javascript'>alert('Profile Pic Upload Unsuccessfully.!!');</SCRIPT>";
                            }
                        }
                    }

                    LoadImages();
                    scrname = "<SCRIPT language='javascript'> window.top.location.reload();</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                }
            }
            else
            {
                // DistImage.ImageUrl = "ImgHandler.ashx?id=" + Session["Formno"] + "&type=Photo";
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

}