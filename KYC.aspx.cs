using ClosedXML.Excel;
using System;
using System.CodeDom;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.Security.Principal;
public partial class KYC : System.Web.UI.Page
{
    double dblBank;
    DataTable tmpTable = new DataTable();
    DAL Obj;
    clsGeneral objGen = new clsGeneral();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    DAL ObjDal = new DAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BtnIdentity.Attributes.Add("onclick", DisableTheButton(Page, BtnIdentity));
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    FillState();
                    FillIdtypeMaster();
                    FillBankMaster();
                    LoadImages();
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            Response.Write("Try later.");
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        try
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
            sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
            sb.Append("this.value = 'Please Wait...';");
            sb.Append("this.disabled = true;");
            sb.Append(pge.Page.GetPostBackEventReference(btn));
            sb.Append(";");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillBankMaster()
    {
        try
        {
            Obj = new DAL();
            string Strquery = "";
            Strquery = ObjDal.Isostart + "SELECT BankCode as Bid, BANKNAME as Bank FROM " + ObjDal.dBName + "..M_BankMaster WHERE ACTIVESTATUS='Y' ORDER BY BANKCode" + ObjDal.IsoEnd;
            tmpTable = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Strquery).Tables[0];
            cmbbank.DataSource = tmpTable;
            cmbbank.DataValueField = "Bid";
            cmbbank.DataTextField = "Bank";
            cmbbank.DataBind();
            cmbbank.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void CmbBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbbank.SelectedItem.Text.ToUpper() == "OTHERS")
        {
            divBank.Visible = true;
            Txtbank.Focus();
            Txtbank.Text = "";
        }
        else
        {
            divBank.Visible = false;
            Txtbank.Text = "";
            Txtbranch.Focus();
        }
    }
    private void FillIdtypeMaster()
    {
        try
        {
            string strQuery = "";
            Obj = new DAL();
            strQuery = ObjDal.Isostart + "SELECT Id, IdType FROM " + ObjDal.dBName + "..M_IdTypeMaster WHERE ACTIVESTATUS='Y'" + ObjDal.IsoEnd;
            tmpTable = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
            DDLAddressProof.DataSource = tmpTable;
            DDLAddressProof.DataValueField = "Id";
            DDLAddressProof.DataTextField = "IdType";
            DDLAddressProof.DataBind();
            DDLAddressProof.SelectedIndex = 0;
            for (int s = 0; s < tmpTable.Rows.Count; s++)
            {
                if (tmpTable.Rows[s]["Id"].ToString() != "0")
                {
                    LblIdproofText.Text += tmpTable.Rows[s]["IdType"].ToString() + ",";
                }
            }

            if (!string.IsNullOrEmpty(LblIdproofText.Text))
            {
                LblIdproofText.Text = LblIdproofText.Text.Remove(LblIdproofText.Text.Length - 1, 1);
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void LoadImages()
    {
        try
        {
            int c = 0;
            string status = "";
            string str = "";
            DataTable dt = new DataTable();
            str = ObjDal.Isostart + "Exec sp_FillKyc " + Convert.ToInt32(Session["Formno"]) + "" + ObjDal.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            if (dt.Rows.Count > 0)
            {
                lblid.Text = dt.Rows[0]["idno"].ToString();
                hdnSessn.Value = Crypto.Encrypt(dt.Rows[0]["idno"].ToString());
                txtaddrs.Text = dt.Rows[0]["Address1"].ToString();
                Txtpincode.Text = dt.Rows[0]["Pincode"].ToString();
                Txtcity.Text = dt.Rows[0]["City"].ToString();
                Txtdistrict.Text = dt.Rows[0]["District"].ToString();
                StateCode.Value = dt.Rows[0]["Statecode"].ToString();
                HDistrictCode.Value = dt.Rows[0]["Districtcode"].ToString();
                HCityCode.Value = dt.Rows[0]["Citycode"].ToString();
                ddlState.SelectedItem.Text = dt.Rows[0]["StateName"].ToString();
                lblverstatus.Text = dt.Rows[0]["idVerf"].ToString();
                DDLAddressProof.SelectedValue = dt.Rows[0]["Id"].ToString();
                DDlVillage.SelectedValue = dt.Rows[0]["areacode"].ToString();
                Lblverdate.Text = dt.Rows[0]["AddrProofDate"] == DBNull.Value ? "" : dt.Rows[0]["AddrProofDate"].ToString();
                LblRemark.Text = dt.Rows[0]["AdressRejectRemark"].ToString();
                LbLrejectRemark.Text = dt.Rows[0]["AddressRejectReason"].ToString();
                status = dt.Rows[0]["Idverf"].ToString();
                TxtIdProofNo.Text = dt.Rows[0]["IdProofNo"].ToString();
                Txtacno.Text = dt.Rows[0]["Acno"].ToString();
                lblverstatus.Text = dt.Rows[0]["BankVerf"].ToString();
                Txtcode.Text = dt.Rows[0]["IFscode"].ToString();
                Txtbranch.Text = dt.Rows[0]["Branchname"].ToString();
                cmbbank.SelectedValue = dt.Rows[0]["BAnkid"].ToString();
                Lblverdate.Text = dt.Rows[0]["BankProofDate"] == DBNull.Value ? "" : dt.Rows[0]["BankProofDate"].ToString();
                txtpan.Text = dt.Rows[0]["Panno"].ToString();
                FillCityPinDetail();

                if (string.IsNullOrEmpty(TxtIdProofNo.Text))
                {
                    TxtIdProofNo.Enabled = true; // Enable if empty
                }
                else
                {
                    TxtIdProofNo.Enabled = false; // Disable if not empty
                }

                if (string.IsNullOrEmpty(txtaddrs.Text))
                {
                    txtaddrs.Enabled = true; // Enable if empty
                }
                else
                {
                    txtaddrs.Enabled = false; // Disable if not empty
                }
                if (string.IsNullOrEmpty(txtpan.Text))
                {
                    txtpan.Enabled = true; // Enable if empty
                }
                else
                {
                    txtpan.Enabled = false; // Disable if not empty
                }
                if (string.IsNullOrEmpty(Txtpincode.Text))
                {
                    Txtpincode.Enabled = true; // Enable if empty
                }
                else
                {
                    Txtpincode.Enabled = false; // Disable if not empty
                }
                if (string.IsNullOrEmpty(ddlState.SelectedItem.Text) || ddlState.SelectedItem.Text.ToString() == "--Choose State Name--")
                {
                    ddlState.Enabled = true; // Enable if empty
                }
                else
                {
                    ddlState.Enabled = false; // Disable if not empty
                }
                if (string.IsNullOrEmpty(Txtdistrict.Text))
                {
                    Txtdistrict.Enabled = true; // Enable if empty
                }
                else
                {
                    Txtdistrict.Enabled = false; // Disable if not empty
                }
                if (string.IsNullOrEmpty(Txtcity.Text))
                {
                    Txtcity.Enabled = true; // Enable if empty
                }
                else
                {
                    Txtcity.Enabled = false; // Disable if not empty
                }
                if (string.IsNullOrEmpty(DDLAddressProof.SelectedItem.Text) || DDLAddressProof.SelectedItem.Text.ToString() == "--Choose Id Proof--")
                {
                    DDLAddressProof.Enabled = true; // Enable if empty
                }
                else
                {
                    DDLAddressProof.Enabled = false; // Disable if not empty
                }
                if (string.IsNullOrEmpty(cmbbank.SelectedItem.Text) || cmbbank.SelectedItem.Text.ToString() == "--Choose Bank Name--")
                {
                    cmbbank.Enabled = true; // Enable if empty
                }
                else
                {
                    cmbbank.Enabled = false; // Disable if not empty
                }
                if (string.IsNullOrEmpty(Txtbranch.Text))
                {
                    Txtbranch.Enabled = true; // Enable if empty
                }
                else
                {
                    Txtbranch.Enabled = false; // Disable if not empty
                }
                if (string.IsNullOrEmpty(Txtcode.Text))
                {
                    Txtcode.Enabled = true; // Enable if empty
                }
                else
                {
                    Txtcode.Enabled = false; // Disable if not empty
                }


                if (dt.Rows[0]["AddrProof"].ToString() == "")
                {
                    ShowIdentity.ImageUrl = "~/images/no_photo.jpg";
                    FrontAddress.HRef = "~/images/no_photo.jpg";
                }
                else
                {
                    ShowIdentity.ImageUrl = dt.Rows[0]["AddrProof"].ToString();
                    FrontAddress.HRef = dt.Rows[0]["AddrProof"].ToString();
                    lblimage.Text = dt.Rows[0]["AddrProof"].ToString();
                    Fuidentity.Attributes.Add("class", "form-control");
                }

                if (dt.Rows[0]["BackAddressProof"].ToString() == "")
                {
                    showBackImage.ImageUrl = "~/images/no_photo.jpg";
                    BackAddress.HRef = "~/images/no_photo.jpg";
                }
                else
                {
                    showBackImage.ImageUrl = dt.Rows[0]["BackAddressProof"].ToString();
                    LblBackImage.Text = dt.Rows[0]["BackAddressProof"].ToString();
                    BackAddress.HRef = dt.Rows[0]["BackAddressProof"].ToString();
                    FileUpload1.Attributes.Add("class", "form-control");
                }
                if (dt.Rows[0]["BankProof"].ToString() == "")
                {
                    bANKiMAGE.ImageUrl = "~/images/no_photo.jpg";
                    BankProof.HRef = "~/images/no_photo.jpg";
                }
                else
                {
                    bANKiMAGE.ImageUrl = dt.Rows[0]["BankProof"].ToString();
                    LblBankImage.Text = dt.Rows[0]["BankProof"].ToString();
                    BankProof.HRef = dt.Rows[0]["BankProof"].ToString();
                    BankKYCFileUpload3.Attributes.Add("class", "form-control");
                }

                if (dt.Rows[0]["PanImg"].ToString() == "")
                {
                    pANiMAGE.ImageUrl = "~/images/no_photo.jpg";
                    PanCard.HRef = "~/images/no_photo.jpg";
                }
                else
                {
                    pANiMAGE.ImageUrl = dt.Rows[0]["PanImg"].ToString();
                    LblPanImage.Text = dt.Rows[0]["PanImg"].ToString();
                    PanCard.HRef = dt.Rows[0]["PanImg"].ToString();
                    PanKYCFileUpload.Attributes.Add("class", "form-control");
                }


                if (!string.IsNullOrEmpty(dt.Rows[0]["AddrProof"].ToString()) && dt.Rows[0]["IsAddrssVerified"].ToString() != "R")
                {
                    Fuidentity.Enabled = false;
                    Fuidentity.Attributes.Add("Class", "form-control");
                }
                else
                {
                    Fuidentity.Enabled = true;
                    c++;
                }

                if (!string.IsNullOrEmpty(dt.Rows[0]["BackAddressProof"].ToString()) && dt.Rows[0]["IsAddrssVerified"].ToString() != "R")
                {
                    FileUpload1.Enabled = false;
                    FileUpload1.Attributes.Add("Class", "form-control");
                }
                else
                {
                    FileUpload1.Enabled = true;
                    c++;
                }




                if (!string.IsNullOrEmpty(LblBankImage.Text) && dt.Rows[0]["IsBankVerified"].ToString() != "R")
                {
                    BankKYCFileUpload3.Enabled = false;
                    BankKYCFileUpload3.Attributes.Add("Class", "form-control");
                }
                else
                {
                    BankKYCFileUpload3.Enabled = true;
                    c++;
                }





                if (!string.IsNullOrEmpty(LblPanImage.Text) && dt.Rows[0]["IsPanVerified"].ToString() != "R")
                {
                    PanKYCFileUpload.Enabled = false;
                    PanKYCFileUpload.Attributes.Add("Class", "form-control");
                }
                else
                {
                    PanKYCFileUpload.Enabled = true;
                    c++;
                }
                if (dt.Rows[0]["IsBankVerified"].ToString() != "R" && !string.IsNullOrEmpty(dt.Rows[0]["BankProof"].ToString()))
                {
                    Txtacno.Enabled = false;
                }
                else
                {
                    Txtacno.Enabled = true;
                    c++;
                }

                if (dt.Rows[0]["IsBankVerified"].ToString() != "R" && Txtacno.Text.Length > 10)
                {
                    Txtacno.Enabled = false;
                }
                else
                {
                    Txtacno.Enabled = true;
                    c++;
                }
            }
            if (dt.Rows[0]["IsAddrssVerified"].ToString() != "R" && c == 0)
            {
                BtnIdentity.Visible = false;
                //Fuidentity.Enabled = false;
                //txtaddrs.Enabled = false;
                //FileUpload1.Enabled = false;
                //DDLAddressProof.Enabled = false;

                //TxtIdProofNo.Enabled = false;
                LbLrejectRemark.Text = "";
            }
            else
            {
                //ddlvillage.Enabled = true;
                BtnIdentity.Visible = true;
                //Fuidentity.Visible = true;
                //txtaddrs.Enabled = true;

                //Txtpincode.Enabled = true;

                //FileUpload1.Enabled = true;
                //DDLAddressProof.Enabled = true;
                //TxtIdProofNo.Enabled = true;
            }

            if (status == "Verification Due")
            {
                VerifyDate.Visible = false;
                Lblverdate.Visible = false;
                LblVerfReason.Visible = false;
                LblVerfRemark.Visible = false;
                LbLrejectRemark.Text = "";
                VerifyDate.Text = "";
                DivVerify.Attributes.Add("style", "color:black");
            }
            else if (status == "Rejected")
            {
                VerifyDate.Visible = true;
                Lblverdate.Visible = true;
                LblVerfReason.Visible = true;
                LblVerfRemark.Visible = true;
                VerifyDate.Text = "Reject Date:";
                DivVerify.Attributes.Add("style", "color:red");
                TxtIdProofNo.Enabled = true; // Enable if empty
                txtaddrs.Enabled = true; // Enable if empty
                txtpan.Enabled = true; // Enable if empty
                Txtpincode.Enabled = true; // Enable if empty
                ddlState.Enabled = true; // Enable if empty
                Txtdistrict.Enabled = true; // Enable if empty
                Txtcity.Enabled = true; // Enable if empty
                DDLAddressProof.Enabled = true; // Enable if empty
                cmbbank.Enabled = true; // Enable if empty
                Txtbranch.Enabled = true; // Enable if empty
                Txtcode.Enabled = true; // Enable if empty

            }
            else
            {
                VerifyDate.Visible = true;
                Lblverdate.Visible = true;
                LblVerfReason.Visible = false;
                LblVerfRemark.Visible = false;
                LbLrejectRemark.Text = "";
                VerifyDate.Text = "Verify Date:";
                DivVerify.Attributes.Add("style", "color:Green");
            }

            LblVerification.Visible = true;

        }



        catch (Exception ex)
        {
            // Handle exception
            Console.WriteLine(ex.Message); // Or use a logging framework
        }
        finally
        {
            // DbConnect.CloseConnection();
        }
    }
    private void FillState()
    {
        try
        {
            var obj = new DAL();
            string query = ObjDal.Isostart + "SELECT StateCode, StateName FROM " + ObjDal.dBName + "..M_STateDivMaster WHERE ActiveStatus = 'Y' AND RowStatus = 'Y' ORDER BY StateCode" + ObjDal.IsoEnd;
            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ddlState.DataSource = dt;
                ddlState.DataValueField = "StateCode";
                ddlState.DataTextField = "StateName";
                ddlState.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    private void FillCityPinDetail()
    {
        DataTable dt = new DataTable();
        string sql = string.Empty;
        if (!string.IsNullOrWhiteSpace(Txtpincode.Text))
        {
            int pincode = Convert.ToInt32(Txtpincode.Text);
            if (pincode != 0)
            {
                sql = ObjDal.Isostart + "select a.Statename, b.DistrictName, c.CityName, d.VillageName, d.Pincode, a.StateCode, b.DistrictCode" +
                      " ,c.CityCode, d.VillageCode from " + ObjDal.dBName + "..M_STateDivMaster as a with (nolock) " +
                      "inner join " + ObjDal.dBName + "..M_DistrictMaster as b with (nolock) on a.StateCode = b.StateCode and a.ActivEstatus = 'Y' and b.ActiveStatus = 'Y' " +
                      "inner join " + ObjDal.dBName + "..M_CityStatemaster as c with (nolock) on b.DistrictCode = c.DistrictCode and c.ActivEstatus = 'Y' " +
                      "inner join " + ObjDal.dBName + "..M_VillageMaster as d with (nolock) on c.CityCode = d.CityCode and d.ActiveStatus = 'Y' " +
                      "where d.Pincode = '" + pincode + "' " +
                      "union all " +
                      "select '' as StateName, '' as DistrictName, '' as CityName, 'Others', '', 0, 0, 0, 381264" + ObjDal.IsoEnd;
            }
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Txtcity.Text = dt.Rows[0]["CityName"].ToString();
                Txtdistrict.Text = dt.Rows[0]["DistrictName"].ToString();
                StateCode.Value = dt.Rows[0]["StateCode"].ToString();
                HDistrictCode.Value = dt.Rows[0]["DistrictCode"].ToString();
                HCityCode.Value = dt.Rows[0]["CityCode"].ToString();
                ddlState.SelectedItem.Text = dt.Rows[0]["StateName"].ToString();
                DDlVillage.DataSource = dt;
                DDlVillage.DataValueField = "VillageCode";
                DDlVillage.DataTextField = "VillageName";
                DDlVillage.DataBind();
                DDlVillage.SelectedIndex = 0;
            }
        }
    }
    protected void Txtpincode_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            string scrname = string.Empty;
            string sql = string.Empty;

            if (!string.IsNullOrWhiteSpace(Txtpincode.Text))
            {
                int pincode = Convert.ToInt32(Txtpincode.Text);

                if (pincode != 0)
                {
                    sql = ObjDal.Isostart + "select a.Statename, b.DistrictName, c.CityName, d.VillageName, d.Pincode, a.StateCode, b.DistrictCode" +
                      " ,c.CityCode, d.VillageCode from " + ObjDal.dBName + "..M_STateDivMaster as a with (nolock) " +
                      "inner join " + ObjDal.dBName + "..M_DistrictMaster as b with (nolock) on a.StateCode = b.StateCode and a.ActivEstatus = 'Y' and b.ActiveStatus = 'Y' " +
                      "inner join " + ObjDal.dBName + "..M_CityStatemaster as c with (nolock) on b.DistrictCode = c.DistrictCode and c.ActivEstatus = 'Y' " +
                      "inner join " + ObjDal.dBName + "..M_VillageMaster as d with (nolock) on c.CityCode = d.CityCode and d.ActiveStatus = 'Y' " +
                      "where d.Pincode = '" + pincode + "' " +
                      "union all " +
                      "select '' as StateName, '' as DistrictName, '' as CityName, 'Others', '', 0, 0, 0, 381264" + ObjDal.IsoEnd;

                    dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        ddlState.SelectedItem.Text = dt.Rows[0]["StateName"].ToString();
                        StateCode.Value = dt.Rows[0]["StateCode"].ToString();
                        Txtdistrict.Text = dt.Rows[0]["DistrictName"].ToString();
                        HDistrictCode.Value = dt.Rows[0]["DistrictCode"].ToString();
                        Txtcity.Text = dt.Rows[0]["CityName"].ToString();
                        HCityCode.Value = dt.Rows[0]["CityCode"].ToString();

                        DDlVillage.DataSource = dt;
                        DDlVillage.DataValueField = "VillageCode";
                        DDlVillage.DataTextField = "VillageName";
                        DDlVillage.DataBind();
                        DDlVillage.SelectedIndex = 0;
                        DDlVillage.Focus();
                    }
                    else
                    {
                        Txtpincode.Focus();
                        ddlState.Items.Clear();
                        StateCode.Value = "0";
                        Txtcity.Text = string.Empty;
                        HCityCode.Value = "0";
                        Txtdistrict.Text = string.Empty;
                        HDistrictCode.Value = "0";
                        DDlVillage.Items.Clear();

                        scrname = "<SCRIPT language='javascript'>alert('Pincode Not exist.');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Permanent Pincode Not exist.');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void BtnIdentity_Click(object sender, EventArgs e)
    {
        try
        {

            DAL obj = new DAL();
            string s1 = "";
            DataTable dt1 = new DataTable();
            string condition = "";
            string adrsProof = "";
            string backAdrsProof;
            string bankProof;
            string panProof;
            string strextension = "";
            string str = "";
            string remark = "";
            string flAddrs = "";

            if (Fuidentity.Enabled)
            {
                if (!Fuidentity.HasFile)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false);
                    return;
                }
            }
            if (txtpan.Text.Trim().Length < 10)
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Invalid Pan no!! ');</SCRIPT>", false);
                return;
            }

            if (Fuidentity.HasFile)
            {
                strextension = System.IO.Path.GetExtension(Fuidentity.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(Fuidentity.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    decimal size = Math.Round((decimal)(Fuidentity.PostedFile.ContentLength) / 1024, 1);
                    if (size > 1024)
                    {
                        string scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                    else
                    {
                        flAddrs = "FA" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(Fuidentity.PostedFile.FileName);
                        Fuidentity.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") + flAddrs);
                        adrsProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + flAddrs;
                    }
                }
                else
                {
                    string scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
            else
            {
                adrsProof = lblimage.Text;
            }

            if (FileUpload1.Enabled)
            {
                if (!FileUpload1.HasFile)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false);
                    return;
                }
            }
            if (FileUpload1.HasFile)
            {
                strextension = System.IO.Path.GetExtension(FileUpload1.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    decimal size = Math.Round((decimal)(FileUpload1.PostedFile.ContentLength) / 1024, 1);

                    if (size > 1024)
                    {
                        string scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                    else
                    {
                        string FlBackAddrs = "BA" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                        FileUpload1.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") + FlBackAddrs);
                        backAdrsProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + FlBackAddrs;
                    }
                }
                else
                {
                    string scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
            else
            {
                backAdrsProof = LblBackImage.Text;
            }

            if (PanKYCFileUpload.Enabled)
            {
                if (!PanKYCFileUpload.HasFile)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false);
                    return;
                }
            }

            if (PanKYCFileUpload.HasFile)
            {
                strextension = System.IO.Path.GetExtension(PanKYCFileUpload.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(PanKYCFileUpload.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    decimal size = Math.Round((decimal)(PanKYCFileUpload.PostedFile.ContentLength) / 1024, 1);

                    if (size > 1024)
                    {
                        string scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                    else
                    {
                        string FlPan = "PAN" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(PanKYCFileUpload.PostedFile.FileName);
                        PanKYCFileUpload.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") + FlPan);
                        panProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + FlPan;
                    }
                }
                else
                {
                    string scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
            else
            {
                panProof = LblPanImage.Text;
            }

            if (BankKYCFileUpload3.Enabled)
            {
                if (!BankKYCFileUpload3.HasFile)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false);
                    return;
                }
            }
            if (BankKYCFileUpload3.HasFile)
            {
                strextension = System.IO.Path.GetExtension(BankKYCFileUpload3.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(BankKYCFileUpload3.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    decimal size = Math.Round((decimal)(BankKYCFileUpload3.PostedFile.ContentLength) / 1024, 1);

                    if (size > 1024)
                    {
                        string scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                    else
                    {
                        string FlBank = "Bank" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(BankKYCFileUpload3.PostedFile.FileName);
                        BankKYCFileUpload3.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") + FlBank);
                        bankProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + FlBank;
                    }
                }
                else
                {
                    string scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
            else
            {
                bankProof = LblBankImage.Text;
            }


            DataTable dt;
            if (cmbbank.SelectedItem.Text.ToUpper() == "OTHERS")
            {
                if (!string.IsNullOrWhiteSpace(Txtbank.Text))
                {
                    string q1 = ObjDal.Isostart + "SELECT * FROM " + ObjDal.dBName + "..M_BankMaster WHERE BankName = '" + Txtbank.Text.Trim() + "' AND ActiveStatus = 'Y' AND RowStatus = 'Y'" + ObjDal.IsoEnd;
                    dt = new DataTable();
                    dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, q1).Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        q1 = "INSERT INTO M_BankMaster (BankCode, BankName, AcNo, IFSCode, Remarks, ActiveStatus, LastModified, UserCode, UserId, IPAdrs, RowStatus) " +
                             "SELECT ISNULL(MAX(BankCode), '1') + 1 AS BankCode, @BankName, '0', '0','', 'Y', 'Add by " + Session["IdNo"].ToString() + " at " + DateTime.Now.ToString() + "', " +
                             "'" + Session["MemName"].ToString() + "', '" + Convert.ToInt32(Session["FormNo"]).ToString() + "', '', 'Y' FROM M_BankMaster";
                        int i = obj.SaveData(q1);
                        if (i > 0)
                        {
                            q1 = "SELECT MAX(BankCode) AS BankCode FROM M_BankMaster WHERE ActiveStatus = 'Y' AND RowStatus = 'Y'";
                            DataTable dt_ = new DataTable();
                            dt_ = obj.GetData(q1);
                            if (dt_.Rows.Count > 0)
                            {
                                dblBank = Convert.ToInt32(dt_.Rows[0]["BankCode"]);
                            }
                        }
                    }
                    else
                    {
                        dblBank = Convert.ToInt32(dt.Rows[0]["BankCode"]);
                    }
                }
            }
            else
            {
                dblBank = Convert.ToInt32(cmbbank.SelectedValue);
            }

            if (!string.IsNullOrWhiteSpace(Txtbank.Text) || !string.IsNullOrWhiteSpace(Txtcode.Text))
            {
                if (Convert.ToInt32(cmbbank.SelectedValue) == 0)
                {
                    string scrname = "<SCRIPT language='javascript'>alert('Choose Bank Name');</SCRIPT>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Txtbranch.Text))
                {
                    string scrname = "<SCRIPT language='javascript'>alert('Enter Branch Name.');</SCRIPT>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Txtcode.Text))
                {
                    string scrname = "<SCRIPT language='javascript'>alert('Enter IFSC Code.');</SCRIPT>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                    return;
                }
            }


            string strSq = ObjDal.Isostart + "Exec sp_FillKyc '" + Session["Formno"] + "'" + ObjDal.IsoEnd;
            dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSq).Tables[0];
            string Remark = "";
            if (dt1.Rows.Count > 0)
            {
                if (ClearInject(dt1.Rows[0]["Address1"].ToString()) != ClearInject(txtaddrs.Text))
                {
                    Remark += "Address Changed From " + ClearInject(dt1.Rows[0]["Address1"].ToString()) + " to " + ClearInject(txtaddrs.Text) + ",";
                }
                if (ClearInject(dt1.Rows[0]["City"].ToString()) != ClearInject(Txtcity.Text))
                {
                    Remark += "City Changed From " + ClearInject(dt1.Rows[0]["City"].ToString()) + " to " + ClearInject(Txtcity.Text) + ",";
                }
                if (ClearInject(dt1.Rows[0]["District"].ToString()) != ClearInject(Txtdistrict.Text))
                {
                    Remark += "District Changed From " + ClearInject(dt1.Rows[0]["District"].ToString()) + " to " + ClearInject(Txtdistrict.Text) + ",";
                }

                if (ClearInject(dt1.Rows[0]["PinCode"].ToString()) != ClearInject(Txtpincode.Text))
                {
                    Remark += "PinCode Changed From " + ClearInject(dt1.Rows[0]["PinCode"].ToString()) + " to " + ClearInject(Txtpincode.Text) + ",";
                }
                if (ClearInject(dt1.Rows[0]["AddrProof"].ToString()) != ClearInject(adrsProof))
                {
                    Remark += "AddressProof Changed From " + ClearInject(dt1.Rows[0]["AddrProof"].ToString()) + " to " + ClearInject(adrsProof) + ",";
                }
                if (ClearInject(dt1.Rows[0]["BackAddressProof"].ToString()) != ClearInject(backAdrsProof))
                {
                    Remark += "BackAddressProof Changed From " + ClearInject(dt1.Rows[0]["BackAddressProof"].ToString()) + " to " + ClearInject(backAdrsProof) + ",";
                }
                if (ClearInject(dt1.Rows[0]["IdProofNo"].ToString()) != ClearInject(TxtIdProofNo.Text.Trim()))
                {
                    Remark += "AddressProofNo Changed From " + ClearInject(dt1.Rows[0]["IdProofNo"].ToString()) + " to " + ClearInject(TxtIdProofNo.Text.Trim()) + ",";
                }
                if (Convert.ToInt32(dt1.Rows[0]["BankId"]) != Convert.ToInt32(cmbbank.SelectedValue))
                {
                    Remark += "Bank Changed From " + Convert.ToInt32(dt1.Rows[0]["BankId"]) + " to " + Convert.ToInt32(cmbbank.SelectedValue) + ",";
                }

                if (ClearInject(dt1.Rows[0]["BranchName"].ToString()) != ClearInject(Txtbranch.Text))
                {
                    Remark += "BranchName Changed From " + ClearInject(dt1.Rows[0]["BranchName"].ToString()) + " to " + ClearInject(Txtbranch.Text) + ",";
                }
                if (ClearInject(dt1.Rows[0]["AcNo"].ToString()) != ClearInject(Txtacno.Text))
                {
                    Remark += "AccountNo Changed From " + ClearInject(dt1.Rows[0]["AcNo"].ToString()) + " to " + ClearInject(Txtacno.Text) + ",";
                }
                if (ClearInject(dt1.Rows[0]["IFSCode"].ToString()) != ClearInject(Txtcode.Text))
                {
                    Remark += "IFSCCode Changed From " + ClearInject(dt1.Rows[0]["IFSCode"].ToString()) + " to " + ClearInject(Txtcode.Text) + ",";
                }
                if (ClearInject(dt1.Rows[0]["BankProof"].ToString()) != ClearInject(bankProof))
                {
                    Remark += "BankProof Changed From " + ClearInject(dt1.Rows[0]["BankProof"].ToString()) + " to " + ClearInject(bankProof) + ",";
                }
                if (ClearInject(dt1.Rows[0]["Panno"].ToString()) != ClearInject(txtpan.Text))
                {
                    Remark += "PANNo Changed From " + ClearInject(dt1.Rows[0]["Panno"].ToString()) + " to " + ClearInject(txtpan.Text) + ",";
                }
                if (ClearInject(dt1.Rows[0]["PanImg"].ToString()) != ClearInject(panProof))
                {
                    Remark += "PanCardImage Changed From " + ClearInject(dt1.Rows[0]["PanImg"].ToString()) + " to " + ClearInject(panProof) + ",";
                }
            }
            if (DDLAddressProof.SelectedValue == "0")
            {
                string scrname = "<SCRIPT language='javascript'>alert('Choose ID Proof Type.');</SCRIPT>";
                RegisterStartupScript("MyAlert", scrname);
                return;
            }

            int AreaCode = 0;
            string q = "";
            if (DDlVillage.SelectedItem.Text.ToUpper() == "OTHERS")
            {
                if (!string.IsNullOrEmpty(TxtVillage.Text))
                {
                    q = ObjDal.Isostart + "Select * from " + ObjDal.dBName + "..M_VillageMaster where VillageName = '" + TxtVillage.Text.Trim() + "' and Activestatus = 'Y' and Pincode = '" + Txtpincode.Text + "'" + ObjDal.IsoEnd;
                    DataSet ds = new DataSet();
                    DataTable Dt = new DataTable();
                    ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, q);
                    Dt = ds.Tables[0];
                    if (Dt.Rows.Count == 0)
                    {
                        q = "insert into M_VillageMaster (VillageName,CityCode,PinCode) Values('" + TxtVillage.Text.ToUpper() + "','" + HCityCode.Value + "','" + Txtpincode.Text + "')";
                        int i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, q);
                        if (i > 0)
                        {
                            q = ObjDal.Isostart + " select Max(VillageCode)as VillageCode from " + ObjDal.dBName + "..M_VillageMaster where ActiveStatus='Y'" + ObjDal.IsoEnd;
                            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, q).Tables[0];
                            if (Dt.Rows.Count > 0)
                            {
                                AreaCode = Convert.ToInt32(Dt.Rows[0]["VillageCode"]);
                            }
                        }
                    }
                    else
                    {
                        AreaCode = Convert.ToInt32(Dt.Rows[0]["VillageCode"]);
                    }
                }
            }
            else
            {
                AreaCode = Convert.ToInt32(DDlVillage.SelectedValue);
            }
            string qry = " Exec Sp_SaveKYCDetails '" + Session["FormNo"] + "','" + Session["MemName"] + "','" + Remark + "','" + txtaddrs.Text.ToUpper() + "','" + Txtcity.Text.ToUpper() + "','" + Txtcity.Text.ToUpper() + "',";
            qry += "'" + Txtdistrict.Text.ToUpper() + "','" + StateCode.Value + "','" + Txtpincode.Text + "','" + AreaCode + "','" + DDLAccountType.SelectedItem.Text + "','" + HCityCode.Value + "','" + HDistrictCode.Value + "',";
            qry += "'" + txtpan.Text.ToUpper() + "','" + Txtacno.Text + "','" + dblBank + "','" + Txtcode.Text.ToUpper() + "','" + Txtbranch.Text.ToUpper() + "','" + DDLAddressProof.SelectedValue + "','" + TxtIdProofNo.Text.Trim().ToUpper() + "',";
            qry += "'" + adrsProof + "','" + backAdrsProof + "','" + panProof + "','" + bankProof + "'";
            string strTrnFun_Query = "BEGIN TRY BEGIN TRANSACTION " + qry + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH";
            int result = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strTrnFun_Query);
            if (result > 0)
            {
                string script = "<script language='javascript'>alert('KYC Upload successfully.');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Image", script, false);

                FillBankMaster();
                LoadImages();
                divBank.Visible = false;
                Txtbank.Text = "";
            }
            else
            {
                string script = "<script language='javascript'>alert('KYC Upload unsuccessfully.');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Image", script, false);

                LoadImages();
            }

        }
        catch (Exception ex)
        {
            string error = "Error: " + ex.Message;
            ScriptManager.RegisterStartupScript(this, GetType(), "Exception", "alert('" + error + "');", true);
        }
    }
    private string ClearInject(string strObj)
    {
        if (strObj == null)
        {
            return string.Empty;
        }

        strObj = strObj.Replace(";", string.Empty)
                       .Replace("'", string.Empty)
                       .Replace("=", string.Empty);

        return strObj;
    }
    protected void txtpan_TextChanged(object sender, EventArgs e)
    {
        if (PanVerify())
        {
            BtnIdentity.Enabled = true;
        }
        else
        {
            BtnIdentity.Enabled = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "Key", "alert('Pan card already registered with another ID.');", true);
            txtpan.Text = string.Empty;
            return;
        }
    }
    private bool PanVerify()
    {
        try
        {
            bool result = false;
            DataTable dt12 = new DataTable();
            DataSet ds12 = new DataSet();
            string str12 = "SELECT COUNT(panno) AS cnt FROM KycVerify AS a, m_membermaster AS b WHERE a.formno = b.formno AND Panno <> '' AND Ispanverified = 'Y' AND panno = @panno";

            string connectionString = constr;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(str12, conn);
                da.SelectCommand.Parameters.AddWithValue("@panno", txtpan.Text);
                da.Fill(ds12);
            }

            dt12 = ds12.Tables[0];
            if (dt12.Rows.Count > 0)
            {
                int count = Convert.ToInt32(dt12.Rows[0]["cnt"]);
                if (count > 1 || count == 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            // Handle exception or log it
            return false;
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlState.SelectedValue == "0")
        {
            BtnIdentity.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('please select State name.!!');", true);
            txtpan.Text = "";
            return;
        }
        else
        {
            BtnIdentity.Enabled = true;
        }
    }
    protected void TxtIdProofNo_TextChanged(object sender, EventArgs e)
    {
        if (AAdharVerify())
        {
            BtnIdentity.Enabled = true;
        }
        else
        {
            BtnIdentity.Enabled = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "Key", "alert('AAdhar Card already registered with another ID.');", true);
            TxtIdProofNo.Text = string.Empty;
            return;
        }
    }
    private bool AAdharVerify()
    {
        try
        {
            bool result = false;
            DataTable dt12 = new DataTable();
            DataSet ds12 = new DataSet();
            string str12 = "Exec v#GetAaDhar @IdProofNo";
            string connectionString = constr;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(str12, conn);
                da.SelectCommand.Parameters.AddWithValue("@IdProofNo", TxtIdProofNo.Text);
                da.Fill(ds12);
            }
            dt12 = ds12.Tables[0];
            if (dt12.Rows.Count > 0)
            {
                int count = Convert.ToInt32(dt12.Rows[0]["cnt"]);
                if (count > 1 || count == 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            // Handle exception or log it
            return false;
        }
    }
}
