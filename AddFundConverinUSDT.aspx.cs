using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using System.Xml;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;
using System;
public partial class AddFundConverinUSDT : System.Web.UI.Page
{
    private SqlDataReader ds;
    private SqlDataReader ds1;
    private SqlConnection Conn;
    private SqlCommand Comm;
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dRead;
    private cls_DataAccess dbConnect;
    private int TransferId;
    private DataTable tmpTable = new DataTable();
    private DataTable dt1;
    private DataTable dt2;
    private string strQuery = "";
    private SqlDataAdapter Ad;
    private string scrname;
    private clsGeneral objGen = new clsGeneral();
    private string coinRate = "";
    private DAL Obj = new DAL();
    private string IsoStart;
    private string IsoEnd;
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.btnRetry.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnRetry));
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {

                    Fun_Sp_GetCryptoAPIFor_FundWithdraw_Cpanel();
                    hdnPaymentId.Value = "0x5e9aB3A0AEEb0911651BE02F0a979c3F47c54D6F";
                    SpnAddress.InnerText = "0x5e9aB3A0AEEb0911651BE02F0a979c3F47c54D6F";
                    HdnFormno.Value = Session["Formno"].ToString();
                    //GenerateQrCode(Session["Formno"].ToString());
                }
            }
            else
            {
                Response.Redirect("Logout.aspx");
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }

    }
    private void Fun_Sp_GetCryptoAPIFor_FundWithdraw_Cpanel()
    {
        try
        {
            string sql = "";
            DataTable dtAPIMaster = new DataTable();
            sql = " EXEC Sp_GetCryptoAPIFor_FundWithdraw_SRI ";
            dtAPIMaster = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            Session["SRIBALANCE"] = dtAPIMaster.Rows[0]["APIURL"];
            Session["SRISINGLEPAYOUT"] = dtAPIMaster.Rows[1]["APIURL"];
            Session["STATUSCHECK"] = dtAPIMaster.Rows[2]["APIURL"];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
            sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
            sb.Append("this.value = 'Please wait...';");
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
    protected void btnRetry_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(TxtThxhash.Text))
            {
                string scrname = "<SCRIPT language='javascript'>alert('Please Enter TxnHash.!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Enter TxnHash.!');", true);
            }
            else
            {
                TokenHashCheck(TxtThxhash.Text.Trim(), Session["formno"].ToString());
            }
            // Fund_Balance_Check(hdnPaymentId.Value, HdnFormno.Value, OrderId.Value, privatekey.Value);
        }
        catch (Exception ex)
        {
            // Handle exception (if needed)
        }
    }
    public string TokenHashCheck(string txnhash, string Formno_V)
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        try
        {
            string scrname = "";
            string str = "";
            DataSet dsLogin = new DataSet();
            string completeUrl = Session["STATUSCHECK"].ToString() + txnhash.Trim();
            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,Req_From)VALUES('" + sResult.Trim() + "','" + Formno_V.Trim() + "','" + completeUrl.Trim() + "','TRANSACTIONHASHUSDT')";
            int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));
            string responseString = string.Empty;
            string CustomerOTPmessage = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(completeUrl);
                request1.ContentType = "application/json";
                request1.Method = "GET";
                HttpWebResponse httpWebResponse = (HttpWebResponse)request1.GetResponse();
                StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
                responseString = reader.ReadToEnd();

                string sql_res = "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + responseString.Trim() + "',postdata = '" + responseString.Trim() + "' " +
                                 "Where ReqID = '" + sResult.Trim() + "' AND Req_From = 'TRANSACTIONHASHUSDT' ";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));

                string json = responseString;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var Data = jss.Deserialize<object>(responseString);
                dsLogin = ConvertJsonStringToDataSet(responseString);
                str = responseString;

                string auth = dsLogin.Tables[0].Rows[0]["status"].ToString();
                string Hash = dsLogin.Tables[0].Rows[0]["transactionHash"].ToString();
                string Final_Amount = dsLogin.Tables[0].Rows[0]["amount"].ToString();
                string To_WalletAddress = dsLogin.Tables[0].Rows[0]["to"].ToString();
                string From_WalletAddress = dsLogin.Tables[0].Rows[0]["from"].ToString();
                string TokenContractAddress = dsLogin.Tables[0].Rows[0]["token"].ToString();

                string sql1 = "";
                if (auth.ToUpper() == "SUCCESS")
                {
                    sql1 = "insert into ApiReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request," +
                           "Response,ApiStatus,RectimeStamp,ApiType,TxnHash,AMount,PostData,TypeB,FromID,ToID) " +
                           "Values('" + Formno_V + "','" + sResult + "','" + To_WalletAddress + "','" + dsLogin.Tables[0].Rows[0]["token"] + "','" + completeUrl + "','" + responseString + "'," +
                           "'" + auth + "',getdate(),'Re-TranscationHash','" + Hash + "','" + Decimal.Parse(Final_Amount, System.Globalization.NumberStyles.Float) + "'," +
                           "'','TranscationHash','" + To_WalletAddress + "','" + From_WalletAddress + "');";

                    string QueryStr = "Begin Try Begin Transaction " + sql1 + " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH";
                    int x = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, QueryStr);

                    string sql = "";
                    if (TokenContractAddress.ToLower() == "0xfc2df33ab32dca6478a0fabd956f6bc8ce11eae2")
                    {
                        if (From_WalletAddress.ToLower() == hdnPaymentId.Value.ToLower())
                        {
                            scrname = "<SCRIPT language='javascript'>alert('No transaction found. Please try again later.');</SCRIPT>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('No transaction found. Please try again later.');", true);
                            TxtThxhash.Text = "";
                        }
                        else
                        {
                            if (To_WalletAddress.ToLower() == hdnPaymentId.Value.ToLower())
                            {
                                sql = "Exec Sp_GetWalletaddressUpdet '" + Hash.Trim() + "'";
                                DataTable DtS = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                                if (DtS.Rows.Count > 0)
                                {
                                    string Response = DtS.Rows[0]["response"].ToString();
                                    if (Response.ToUpper() == "OK")
                                    {
                                        GetResponseCheckloop(To_WalletAddress, Final_Amount, Hash);
                                    }
                                    else
                                    {
                                        scrname = "<SCRIPT language='javascript'>alert('" + Response + "');</SCRIPT>";
                                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Response + "');", true);
                                        TxtThxhash.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                scrname = "<SCRIPT language='javascript'>alert('No transaction found. Please try again later.');</SCRIPT>";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('No transaction found. Please try again later.');", true);
                                TxtThxhash.Text = "";
                            }
                        }
                    }
                    else
                    {
                        scrname = "<SCRIPT language='javascript'>alert('No transaction found. Please try again later.');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('No transaction found. Please try again later.');", true);
                        TxtThxhash.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                string errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,SelectType)values('" + ex.Message + "',getdate(),'" + completeUrl + "','" + responseString + "','Admin')";
                int x1 = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, errorQry);
                scrname = "<SCRIPT language='javascript'>alert('No transaction found. Please try again later.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('No transaction found. Please try again later.');", true);
                TxtThxhash.Text = "";
            }
            return str;
        }
        catch (Exception ex)
        {
            // Handle any other exceptions if necessary
            return string.Empty;
        }
    }
    private string GetResponseCheckloop(string WalletAddress, string amount, string Txnhash)
    {
        string result = "";
        string message = "";
        string Status = "";
        string txnin = "";
        string txnout = "";
        string scrname = "";
        string sql = "";

        try
        {
            string TransactionStatus = "1";
            string vi = WalletAddress;
            string CallbackUrl = "";
            message = "";

            if (!string.IsNullOrEmpty(TransactionStatus))
            {
                int i = 0;
                if (TransactionStatus.Trim() == "1")
                {
                    string Bal = Fund_Balance_Check(WalletAddress, Session["formno"].ToString(), amount, Txnhash);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions if necessary, for example, log the error message
            result = "Error: " + ex.Message;
        }

        return result;
    }
    public string Fund_Balance_Check(string walletaddress, string Formno_V, string Amount, string Txnhash)
    {
        string URL = "";
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        Random random = new Random();
        int random_number = random.Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        string postData = "";
        string Balance = "";
        string str = "";
        string statusApi = "";
        int Result_RR = 0;
        DataSet dsLogin = new DataSet();
        DataTable dsLoginTo_Address = new DataTable();
        string responseString = string.Empty;
        string To_WalletAddress = "";
        string From_WalletAddress = "";

        try
        {
            decimal value = 0;
            string Code = "";
            DataSet data = new DataSet();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                URL = Session["SRIBALANCE"].ToString();
                WebRequest tRequest = WebRequest.Create(URL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.ContentLength = 0;
               postData = "{\"walletAddress\": \"" + walletaddress.Trim() + "\"}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                string sql_req = "insert Into Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,postdata,Req_From) Values('" + sResult.Trim() + "','" + Formno_V.Trim() + "','" + URL.Trim() + "','" + postData.Trim() + "','BalanceUSDT')";
                int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                WebResponse tResponse = tRequest.GetResponse();
                using (StreamReader tReader = new StreamReader(tResponse.GetResponseStream()))
                {
                    str = tReader.ReadToEnd();
                }

                string sql_res = "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + str.Trim() + "' Where ReqID = '" + sResult.Trim() + "' AND Req_From = 'BalanceUSDT' ";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));

                //string MaxVoucherNo = "2" + DateTime.Now.ToString("MMMddyyyyHHmmssfff");
                //long maxVno = Convert.ToInt64(MaxVoucherNo) + Convert.ToInt64(Formno_V);
                //MaxVoucherNo = maxVno.ToString();
                //string ReqNo = MaxVoucherNo;
                string ReqNo = string.Empty;
                string MaxVoucherNo = "2" + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
                long maxVno = Convert.ToInt64(MaxVoucherNo) + Convert.ToInt64(Session["Formno"].ToString());
                ReqNo = maxVno.ToString();
                try
                {
                    data = ConvertJsonStringToDataSet(str);
                    DataView dv = new DataView(data.Tables[1]);
                    dv.RowFilter = " To = '" + walletaddress.Trim() + "'";
                    dsLoginTo_Address = dv.ToTable();

                    if (Convert.ToDecimal(data.Tables[0].Rows[0]["Balance"]) > 0)
                    {
                        if (dsLoginTo_Address.Rows.Count > 0)
                        {
                            foreach (DataRow row in dsLoginTo_Address.Rows)
                            {
                                To_WalletAddress = row["to"].ToString();
                                From_WalletAddress = row["from"].ToString();
                                double originalValue = Convert.ToDouble(row["value"]);
                                decimal Final_Amount = Convert.ToDecimal(originalValue / Math.Pow(10, 18));
                                string Hash = row["hash"].ToString();
                                string ContractAddress = row["contractAddress"].ToString().Trim();
                                string TokenName = row["tokenSymbol"].ToString();

                                if (TokenName.ToUpper() == "SRI")
                                {
                                    if (walletaddress.ToLower() == To_WalletAddress.ToLower())
                                    {
                                        if (Hash.ToLower() == Txnhash.ToLower())
                                        {
                                            string TxnInsert = "insert into TrnvoucherTxnHash(Formno,From_walletAddress,To_walletAddress,Amount,Txnhash,To_PrivateKey) values('" + Formno_V + "','" + From_WalletAddress + "','" + To_WalletAddress + "','" + Final_Amount + "','" + Hash + "','" + To_WalletAddress + "')";
                                            int xI = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, TxnInsert);

                                            if (xI > 0)
                                            {
                                                string Str_Check = " Exec Sp_CheckTxnHAsh '" + Hash.Trim() + "'";
                                                DataTable dt_Check = SqlHelper.ExecuteDataset(constr, CommandType.Text, Str_Check).Tables[0];

                                                if (dt_Check.Rows.Count != 0)
                                                {
                                                    if (ContractAddress == "0xfc2df33ab32dca6478a0fabd956f6bc8ce11eae2")
                                                    {
                                                        string strs = "insert into ApiReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request,Response,ApiStatus,RectimeStamp,ApiType,TxnHash,AMount,PostData,TypeB,FromID,ToID) Values('" + Formno_V + "','" + walletaddress + "','" + walletaddress + "','" + walletaddress + "','" + URL + "','" + str + "','" + statusApi + "',getdate(),'Re-Transcation','" + Hash + "','" + Final_Amount + "','','QrCode','" + To_WalletAddress + "','" + From_WalletAddress + "')";
                                                        string QueryStr = " Begin Try Begin Transaction " + strs + " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH";
                                                        int x = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, QueryStr);

                                                        if (x > 0)
                                                        {
                                                            Result_RR = 1;
                                                            decimal ApiTotalAmount = CalculateTotalRate(Final_Amount);
                                                            string TokenResoponse = DeductWalletApi(Formno_V, ApiTotalAmount.ToString(), From_WalletAddress, ReqNo, Final_Amount.ToString(), Hash);

                                                            if (TokenResoponse.ToUpper().Trim() == "SUCCESS")
                                                            {
                                                                string sql_resToken = "Update TrnvoucherTxnHash Set Is_Pay = 'A', Update_DATe = getdate() Where To_walletAddress = '" + To_WalletAddress.Trim() + "' AND Is_Pay = 'P'";
                                                                int x_resToken = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_resToken));

                                                                if (x_resToken > 0)
                                                                {
                                                                    // Additional logic can be added here
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string updateStr = "Update TrnvoucherTxnHash Set From_walletAddress = '" + From_WalletAddress + "', To_walletAddress = '" + To_WalletAddress + "' Where Txnhash = '" + Hash + "' And Formno = '" + Formno_V + "'";
                                                        int x = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, updateStr);
                                                    }
                                                }
                                                else
                                                {
                                                    string updateStr = "Update TrnvoucherTxnHash Set From_walletAddress = '" + From_WalletAddress + "', To_walletAddress = '" + To_WalletAddress + "' Where Txnhash = '" + Hash + "' And Formno = '" + Formno_V + "'";
                                                    int x = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, updateStr);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (Result_RR > 0)
                    {
                        string message = "Sell Transaction Successfully submitted, Please Check Your Wallet!";
                        string urlStr = "AddFundConverinUSDT.aspx";
                        string script = "window.onload = function(){ alert('" + message + "'); window.location = '" + urlStr + "'; }";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('No transaction found. Please try again later.');", true);
                    }
                }
                catch (Exception ex)
                {
                    string errorQry = "";
                    string ErrorMsg = ex.Message;

                    string sql_res1 = "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + ErrorMsg.Trim() + "' Where ReqID = '" + sResult.Trim() + "'";
                    int x_res1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res1));

                    errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,formno) values('" + ErrorMsg + "',getdate(),'" + URL + "','" + To_WalletAddress.Trim() + "','" + str + "','" + Formno_V + "')";
                    int x1 = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, errorQry);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('No transaction found. Please try again later.');", true);
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions at the outer level
        }
        return Balance;
    }
    public string DeductWalletApi(string formnoV, string withdrawlAmount, string walletAddress, string reqNo, string SRIAmount, string SRIHash)
    {
        string requestUrl1 = "";
        string getPayoutDetail = "";
        DataTable dtQuery = new DataTable();
        string sResponseFromServer1 = string.Empty;
        string statusApi = "";
        string sResult = "";
        string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        Random random = new Random();
        int randomNumber = random.Next(0, 999);
        string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
        sResult = formattedDatetime;
        int i = 0;
        string hash_ = "";
        string postData = "";
        try
        {
            DataSet dslogin = new DataSet();
            string str = string.Empty;
            decimal value = 0;
            string code = "";
            DataSet ds = new DataSet();
            DataSet data = new DataSet();
            string url = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                url = Session["SRISINGLEPAYOUT"].ToString();

                WebRequest tRequest = WebRequest.Create(url);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.ContentLength = 0;

                postData = "{\"to\":\"" + walletAddress.Trim() + "\",\"amount\":\"" + decimal.Parse(withdrawlAmount, System.Globalization.NumberStyles.Float) + "\",\"txId\":\"" + sResult.Trim() + "\"}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                string sqlReq = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, Req_From, Rate,BHTSAmount, BHTSHash) " +
                                "VALUES ('" + reqNo.Trim() + "', '" + formnoV.Trim() + "', '" + url.Trim() + "', '" + postData.Trim() + "', 'SinglePayoutUSDT', " +
                                "'" + HdnApiRate.Value + "', '" + SRIAmount.Trim() + "', '" + SRIHash.Trim() + "')";
                int xReq = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq));

                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                str = tReader.ReadToEnd();

                string sqlRes = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + str.Trim() + "' WHERE ReqID = '" + reqNo.Trim() + "' AND Req_From = 'SinglePayoutUSDT'";
                int xRes = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes));

                dslogin = ConvertJsonStringToDataSet(str.ToString());
                string resultStatus = "";
                if (!string.IsNullOrEmpty(str))
                {
                    resultStatus = dslogin.Tables[0].Rows[0]["Status"].ToString();
                }
                else
                {
                    resultStatus = "failed";
                }

                if (resultStatus.ToUpper() == "FAILED")
                {
                    try
                    {
                        hash_ = dslogin.Tables[0].Rows[0]["txhash"].ToString();
                    }
                    catch (Exception) { }
                    if (string.IsNullOrEmpty(hash_))
                    {
                        hash_ = "";
                    }
                }
                else if (resultStatus.ToUpper() == "SUCCESS")
                {
                    hash_ = dslogin.Tables[0].Rows[0]["txhash"].ToString();
                }

                string strs = "INSERT INTO ApiReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, Response, ApiStatus, RectimeStamp, ApiType, TxnHash, Amount, PostData, TypeB) " +
                              "VALUES ('" + formnoV + "', '" + walletAddress + "', '" + walletAddress + "', '" + walletAddress + "', '" + url + "', '" + str + "', '" + statusApi + "', " +
                              "GETDATE(), 'Token Payout', '" + hash_ + "', '" + decimal.Parse(withdrawlAmount, System.Globalization.NumberStyles.Float) + "', '" + postData + "', 'QrCode');";
                string query = "BEGIN TRY BEGIN TRANSACTION " + strs + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH";
                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, query));

                if (i > 0 && resultStatus.ToUpper() == "SUCCESS")
                {
                    statusApi = resultStatus;
                }
            }
            catch (Exception ex)
            {
                string errorQry = "";
                string errorMsg = ex.Message;
                errorQry = "INSERT INTO TrnLogData(ErrorText, LogDate, Url, WalletAddress, PostData, formno) " +
                           "VALUES ('" + errorMsg + "', GETDATE(), '" + url + "', '" + walletAddress.Trim() + "', '" + postData + "', '" + formnoV + "');";
                errorQry += "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + errorMsg + "' WHERE ReqID = '" + reqNo.Trim() + "'";

                int xRes = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, errorQry));
                if (xRes > 0)
                {
                    statusApi = "failed";
                }
            }
        }
        catch (Exception)
        {
            statusApi = "failed";
        }

        return statusApi;
    }
    public decimal CalculateTotalRate(decimal amount)
    {
        decimal totalRate = 0m;
        decimal rate = 0m;

        try
        {
            // Build the SQL query
            string sql = "SELECT Gasfees FROM " + Obj.dBName + "..GasFeesCheck WHERE statusapi = 'Y' ";
            DataTable dtApiMaster = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];

            // Check if data is returned
            if (dtApiMaster.Rows.Count > 0)
            {
                rate = Convert.ToDecimal(dtApiMaster.Rows[0]["Gasfees"]);
                HdnApiRate.Value = rate.ToString();  // Assuming HdnApiRate is a hidden field
                totalRate = amount * rate;
            }
            else
            {
                // Handle case when no data is found (optional)
                // For example, log this or set a default value for rate
            }

            return totalRate;
        }
        catch (Exception ex)
        {
            // Log the exception and return 0
            // You can log the error to a file or a database for further analysis
            return totalRate;
        }
    }
    public DataSet ConvertJsonStringToDataSet(string jsonString)
    {
        try
        {
            XmlDocument xd = new XmlDocument();
            jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlNodeReader(xd));
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}