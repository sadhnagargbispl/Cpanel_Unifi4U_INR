//using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class AddFundPayment : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DAL ObjDal = new DAL();
    string IsoStart;
    string IsoEnd;
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
                    GenerateQrCode(Session["Formno"].ToString());
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
            string sql = string.Empty;
            DataTable dt_API_MAster = new DataTable();
            sql = IsoStart + " Exec Sp_GetCryptoAPIFor_FundWithdraw_Cpanel " + IsoEnd;
            dt_API_MAster = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];

            Session["CreateQrCode"] = dt_API_MAster.Rows[0]["APIURL"];
            Session["BalanceCheckQrCode"] = dt_API_MAster.Rows[1]["APIURL"];
            Session["TokenCheckQrCode"] = dt_API_MAster.Rows[2]["APIURL"];
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
    public string GenerateQrCode(string Formno)
    {
        string completeUrl = string.Empty;
        try
        {
            string sql = "select orderid from ApiReqResponse where orderid='" + Session["Orderid"] + "' " +
                         "AND ApiStatus='success'  AND ApiType='Token Payout' ";

            DataTable dt = ObjDal.GetData(sql);

            if (dt.Rows.Count == 0)
            {
                DataSet dsLogin = new DataSet();
                DataTable dt_QrCode = new DataTable();
                string Str_QrcodeGet = "Exec Sp_GetFormnoqrCode '" + Formno + "'";
                dt_QrCode = SqlHelper.ExecuteDataset(constr, CommandType.Text, Str_QrcodeGet).Tables[0];
                string responseString = string.Empty;

                if (dt_QrCode.Rows.Count == 0)
                {
                    completeUrl = Session["CreateQrCode"].ToString();
                    string CustomerOTPmessage = string.Empty;
                    string amount = string.Empty;

                    System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;
                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(completeUrl);
                    request1.ContentType = "application/json";
                    request1.Method = "GET";

                    using (HttpWebResponse httpWebResponse = (HttpWebResponse)request1.GetResponse())
                    using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        responseString = reader.ReadToEnd();
                    }

                    string Str_RR = "Insert Into M_FormnoqrCode(Formno,QrCode,ActiveStatus) Values('" + Formno + "','" + responseString + "','Y')";
                    int RR = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_RR);
                }
                else
                {
                    responseString = dt_QrCode.Rows[0]["QrCode"].ToString();
                }

                string json = responseString;
                var jss = new JavaScriptSerializer();
                var Data = jss.Deserialize<object>(responseString);
                dsLogin = ConvertJsonStringToDataSet(responseString);

                SpnAddress.InnerText = dsLogin.Tables[0].Rows[0]["address"].ToString();
                privatekey.Value = dsLogin.Tables[0].Rows[0]["key"].ToString();
                ImgQrCode.Src = dsLogin.Tables[0].Rows[0]["qrCodeDataURL"].ToString();
                fromidno.Value = Formno.ToString();
                HdnFormno.Value = Formno.ToString();
                OrderId.Value = dsLogin.Tables[0].Rows[0]["key"].ToString();
                Session["Orderid"] = OrderId.Value;

                string strs = "insert into ApiReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request," +
                              "Response,ApiStatus,RectimeStamp,ApiType,TxnHash,AMount,PostData) " +
                              "Values('" + fromidno.Value + "','" + OrderId.Value + "','" + SpnAddress.InnerText + "'," +
                              "'" + privatekey.Value + "','" + completeUrl + "','" + responseString + "','Pending',getdate(),'QrCode Generate','','0','');";

                string Query = " Begin Try   Begin Transaction " + strs + " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
                DAL objdal = new DAL();
                int i = objdal.SaveData(Query);

                if (i > 0)
                {
                    hdnPaymentId.Value = SpnAddress.InnerText;
                }
                else
                {
                    string message = "Try Again.!";
                    string url = "AddFundPayment.aspx";
                    string script = "window.onload = function(){{ alert('{message}'); window.location = '{url}'; }}";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                }
            }
            else
            {
                string message = "Try Again.!";
                string url = "AddFundPayment.aspx";
                string script = "window.onload = function(){{ alert('" + message + "'); window.location = '" + url + "'; }}";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
            }
        }
        catch (Exception ex)
        {
            string errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress) " +
                              "values('" + ex.Message + "',getdate(),'" + completeUrl + "','" + SpnAddress.InnerText + "')";
            int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);
        }

        return completeUrl; // Adjust return as needed
    }
    protected void btnRetry_Click(object sender, EventArgs e)
    {
        try
        {
            Fund_Balance_Check(hdnPaymentId.Value, HdnFormno.Value, OrderId.Value, privatekey.Value);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public string Fund_Balance_Check(string walletaddress, string Formno_V, string OrderI_V, string privatekey_V)
    {
        string URL = string.Empty;
        string sResult = string.Empty;
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        string postData = string.Empty;
        string Balance = string.Empty;
        string str = string.Empty;
        string statusApi = string.Empty;
        int Result_RR = 0;
        DataSet dsLogin = new DataSet();
        DataTable dsLoginTo_Address = new DataTable();
        string responseString = string.Empty;
        string To_WalletAddress = string.Empty;

        try
        {
            decimal value = 0;
            string Code = string.Empty;
            DataSet data = new DataSet();

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                URL = Session["BalanceCheckQrCode"].ToString();
                WebRequest tRequest = WebRequest.Create(URL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.ContentLength = 0;
                postData = "{\"walletAddress\": \"" + walletaddress.Trim().ToLower() + "\"}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata,Req_From) ";
                sql_req += "VALUES ('" + sResult.Trim() + "', '" + Formno_V.Trim() + "', '" + URL.Trim() + "', '" + postData.Trim() + "','BalanceCheck')";
                int x_Req = ObjDal.SaveData(sql_req);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStream = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStream))
                        {
                            str = tReader.ReadToEnd();
                        }
                    }
                }
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + str.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "' AND Req_From = 'BalanceCheck' ";
                int x_res = ObjDal.SaveData(sql_res);
                string json = str;
                DataTable dt_Check = new DataTable();
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //URL = "https://api.bscscan.com/api?module=account&action=tokentx&contractaddress=0x55d398326f99059ff775485246999027b3197955&address="
                //             + walletaddress.Trim() + "&apikey=FARZRYJ3FMED2HBW63XW7EJZDSAZAC9Y6P";
                //string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata,Req_From) ";
                //sql_req += "VALUES ('" + sResult.Trim() + "', '" + Formno_V.Trim() + "', '" + URL.Trim() + "', '" + postData.Trim() + "','BalanceCheck')";
                //int xReq = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));
                //WebRequest tRequest = WebRequest.Create(URL);
                //tRequest.Method = "GET";
                //WebResponse tResponse = tRequest.GetResponse();
                ////System.Threading.Thread.Sleep(1 * 60 * 1000); // 3 minutes = 180,000 ms
                //Stream dataStream = tResponse.GetResponseStream();
                //StreamReader tReader = new StreamReader(dataStream);
                //str = tReader.ReadToEnd();
                //string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + str.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "' AND Req_From = 'BalanceCheck' ";
                //int xRes = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                //Stopwatch watch = Stopwatch.StartNew();
                //string json = str;
                //DataTable dt_Check = new DataTable();
                try
                {
                    data = ConvertJsonStringToDataSet(str);
                    DataView dv = new DataView(data.Tables[1]);
                    dv.RowFilter = "To = '" + walletaddress.Trim().ToLower() + "'";
                    dsLoginTo_Address = dv.ToTable();
                    if (Convert.ToDecimal(data.Tables[0].Rows[0]["Balance"]) > 0 && dsLoginTo_Address.Rows.Count > 0)
                    {
                        foreach (DataRow row in dsLoginTo_Address.Rows)
                        {
                            To_WalletAddress = row["to"].ToString();
                            string From_WalletAddress = row["from"].ToString();
                            double originalValue = Convert.ToDouble(row["value"].ToString());
                            decimal Final_Amount = (decimal)(originalValue / Math.Pow(10, 18));
                            decimal AcurateTotalAmount = Decimal.Parse(Final_Amount.ToString(), System.Globalization.NumberStyles.Float);
                            string Hash = row["hash"].ToString();
                            string ContractAddress = row["contractAddress"].ToString().Trim();
                            string TokenName = row["tokenname"].ToString();
                            if (walletaddress.Trim().ToLower() == To_WalletAddress.Trim().ToLower() && AcurateTotalAmount > 0)
                            {

                                if (TokenName.ToString() == "Binance-Peg BSC-USD")
                                {
                                    int xI = 0;
                                    try
                                    {
                                        string TxnInsert = "insert into TrnvoucherTxnHash(Formno,From_walletAddress,To_walletAddress,Amount,Txnhash,To_PrivateKey)";
                                        TxnInsert += "values('" + Formno_V + "','" + From_WalletAddress + "','" + To_WalletAddress + "',";
                                        TxnInsert += "'" + Decimal.Parse(Final_Amount.ToString(), System.Globalization.NumberStyles.Float) + "','" + Hash + "','" + privatekey_V + "')";
                                        xI = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, TxnInsert);
                                    }
                                    catch (SqlException ex)
                                    {
                                        if (ex.Number == 2627) // SQL error code for primary key violation
                                        {
                                            xI = 0;
                                        }
                                    }
                                    if (xI > 0)
                                    {
                                        string Str_Check = "Exec Sp_CheckTxnHAsh '" + Hash.Trim() + "'";
                                        dt_Check = SqlHelper.ExecuteDataset(constr, CommandType.Text, Str_Check).Tables[0];
                                        if (dt_Check.Rows.Count > 0 && ContractAddress.ToUpper() == "0X55D398326F99059FF775485246999027B3197955")
                                        {
                                            string strs = "";
                                            strs = "insert into ApiReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request,Response,ApiStatus,RectimeStamp,ApiType,TxnHash,AMount,PostData,TypeB,FromID,ToID)";
                                            strs += "Values('" + Formno_V + "','" + OrderI_V + "','" + walletaddress + "','" + privatekey_V + "',";
                                            strs += "'" + URL + "','" + str + "','" + statusApi + "',getdate(),'Re-Transcation','" + Hash + "',";
                                            strs += "'" + Decimal.Parse(Final_Amount.ToString(), System.Globalization.NumberStyles.Float) + "','','QrCode','" + To_WalletAddress + "','" + From_WalletAddress + "');";
                                            strs += " Exec sp_FundAddUpdate_Fund '" + Formno_V + "','" + Decimal.Parse(Final_Amount.ToString(), System.Globalization.NumberStyles.Float) + "',";
                                            strs += "'" + OrderI_V + "','" + walletaddress + "','" + Hash + "','" + From_WalletAddress + "','" + walletaddress + "','" + sResult.Trim() + "';";

                                            string QueryStr = "";
                                            QueryStr = "Begin Try Begin Transaction " + strs + " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH";
                                            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, QueryStr);
                                            if (x > 0)
                                            {
                                                Result_RR = 1;
                                            }
                                        }
                                        else
                                        {
                                            string strs = "";
                                            strs = "Update TrnvoucherTxnHash Set From_walletAddress = '" + From_WalletAddress + "',To_walletAddress = '" + To_WalletAddress + "'";
                                            strs += " Where Txnhash = '" + Hash + "' And Formno = '" + Formno_V + "'";

                                            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, strs);
                                        }
                                    }
                                }

                            }
                        }
                    }
                    if (Result_RR > 0)
                    {
                        string sql_resToken = string.Empty;
                        string ApiTotalAmount = Decimal.Parse(data.Tables[0].Rows[0]["Balance"].ToString(), System.Globalization.NumberStyles.Float).ToString();
                        string TokenResoponse = Fund_Token_Send(To_WalletAddress, privatekey_V, ApiTotalAmount, HdnFormno.Value);
                        if (TokenResoponse.ToUpper().Trim() == "SUCCESSFUL")
                        {
                            sql_resToken = "UPDATE TrnvoucherTxnHash SET Is_Pay = 'A', Update_DATe = GETDATE() WHERE To_walletAddress = '" + To_WalletAddress.Trim() + "' AND Is_Pay = 'P'";
                            int x_resToken = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_resToken));
                        }
                        string message = "Payment Successfully Added in Wallet.!";
                        string urlStr = "AllWalletReport.aspx";
                        string script = "window.onload = function(){{ alert('" + message + "'); window.location = '" + urlStr + "'; }}";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", script, true);
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('No transaction found. Please try again later');", true);
                    }
                }
                catch (Exception ex)
                {
                    string errorQry = string.Empty;
                    string ErrorMsg = ex.Message;

                    errorQry = "INSERT INTO TrnLogData(ErrorText, LogDate, Url, WalletAddress, PostData, formno) ";
                    errorQry += "VALUES('" + ErrorMsg + "', GETDATE(), '" + URL + "', '" + To_WalletAddress.Trim() + "', '" + str + "', '" + Formno_V + "')";
                    SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('No transaction found. Please try again later');", true);
                }
                //watch.Stop();
                //Console.WriteLine("⏱ All transactions processed in: " + watch.Elapsed.TotalSeconds + " seconds");
            }
            catch (Exception ex)
            {
                // Optionally handle top-level exceptions here
            }
        }
        catch (Exception ex)
        {
            // Handle outer exceptions
        }

        return Balance;
    }
    public string Fund_Token_Send(string senderAddress, string senderPrivateKey, string balance, string formno_V)
    {
        string sResult = string.Empty;
        string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int randomNumber = new Random().Next(0, 999);
        string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
        sResult = formattedDatetime;
        string URL = string.Empty;
        string postData = string.Empty;
        string responseStr = string.Empty;
        decimal value = 0;
        string code = string.Empty;
        DataSet data = new DataSet();
        string statusApi = string.Empty;
        string returnStatus = string.Empty;

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            URL = Session["TokenCheckQrCode"].ToString();

            WebRequest tRequest = WebRequest.Create(URL);
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";
            tRequest.ContentLength = 0;
            postData = "{\"senderAddress\":\"" + senderAddress.Trim() + "\",\"senderPrivateKey\":\"" + senderPrivateKey.Trim() + "\",\"recipientAddress\":\"\",\"tokenContractAddress\":\"0x55d398326f99059ff775485246999027b3197955\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            string sqlReq = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata,Req_From) VALUES ('" + sResult.Trim() + "', '" + formno_V.Trim() + "', '" + URL.Trim() + "', '" + postData.Trim() + "','TokenSend')";
            int xReq = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq));
            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse tResponse = tRequest.GetResponse())
            {
                using (Stream dataStream = tResponse.GetResponseStream())
                {
                    using (StreamReader tReader = new StreamReader(dataStream))
                    {
                        responseStr = tReader.ReadToEnd();
                    }
                }
            }
            string sqlRes = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + responseStr.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "' AND Req_From = 'TokenSend'";
            int xRes = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes));
            data = ConvertJsonStringToDataSet(responseStr);
            statusApi = data.Tables[0].Rows[0]["transactionstatus"].ToString();
            string hash_ = string.Empty;

            if (statusApi.ToUpper().Trim() == "SUCCESSFUL")
            {
                try
                {
                    hash_ = data.Tables[0].Rows[0]["transactionHash"].ToString();
                }
                catch
                {
                    hash_ = string.Empty; // Ensure hash_ is empty if there's an exception
                }
            }
            string insertQuery = "INSERT INTO ApiReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, Response, ApiStatus, RectimeStamp, ApiType, TxnHash, Amount, PostData, TypeB) ";
            insertQuery += "VALUES('" + formno_V + "', '" + senderPrivateKey + "', '" + senderAddress + "', '" + senderPrivateKey + "', '" + URL + "', '" + responseStr + "', '" + statusApi + "', GETDATE(), 'Token Payout', '" + hash_ + "', ";
            insertQuery += "" + decimal.Parse(balance, System.Globalization.NumberStyles.Float) + ", '" + postData + "', 'QrCode');";

            string transactionQuery = "BEGIN TRY BEGIN TRANSACTION " + insertQuery + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH";
            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, transactionQuery);

            if (x > 0)
            {
                returnStatus = statusApi;
            }
        }
        catch (Exception ex)
        {
            string errorMsg = ex.Message;

            string errorQuery = "INSERT INTO TrnLogData(ErrorText, LogDate, Url, WalletAddress, PostData, Formno) ";
            errorQuery += "VALUES('" + errorMsg + "', GETDATE(), '" + URL + "', '" + senderAddress.Trim() + "', '" + postData + "', '" + formno_V + "')";

            int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQuery);

            if (x1 > 0)
            {
                returnStatus = "failed";
            }
        }

        return returnStatus;
    }
    public string GenerateRandomString(ref int iLength)
    {
        try
        {
            string sResult = "";
            string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int random_number = new Random().Next(0, 999);
            string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
            sResult = formatted_datetime;
            return sResult;
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
            // Empty: No code to execute
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
            // Empty: No code to execute
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
