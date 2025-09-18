using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Xml;
using System.Net;
using System.Text;

public partial class ProccessApi : System.Web.UI.Page
{
    public string _ReqType;
    public GetMsg2 ErrObj = new GetMsg2();
    SqlConnection Conn;
    SqlCommand Comm;
    SqlDataAdapter Adp;
    DataSet Ds;
    SqlDataReader Dr;
    string _NewID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    Random Rnd = new Random();
    bool Bool = true;
    string HostIp = HttpContext.Current.Request.UserHostAddress.ToString();
    string _Company = "";
    string _Logo = "";
    string strQry = "";
    string _MailID = "";
    string _MailPass = "";
    string _MailHost = "";
    string _SMSSender = "APPSMS";
    string _SMSUser = "";
    string _SMSPass = "";
    string _RefFormNo = "";
    string _UpLnFormNo = "";
    string _Token = "GW739IESP1956rerir";
    string _Tokenlogin = "JaiGW739IESPrerirDarbar";
    string membername = "";
    clsGeneral objGen = new clsGeneral();
    string KeyE = "6b04d38748f94490a636cf1be3d82841";
    string IV = "f8adbf3c94b7463d";
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    DAL ObjDal = new DAL();
    string IsoStart;
    string IsoEnd;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.Form.HasKeys() && Request.Form["MyJson"] != null)
            {
                string sRequestData = Request.Form["MyJson"];
                sRequestData = ClearInject(sRequestData);
                var jss = new JavaScriptSerializer();
                var dict = jss.Deserialize<Dictionary<string, string>>(sRequestData);

                try
                {
                    string Sqlsrt = "INSERT INTO ReqType(reqtype) VALUES('" + sRequestData.Replace("//n", "\\n") + "')";
                    int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sqlsrt));
                }
                catch (Exception) { }

                if (dict.ContainsKey("reqtype"))
                {
                    _ReqType = dict["reqtype"];
                }
            }
            else
            {
                Request.InputStream.Position = 0;
                using (var inputStream = new StreamReader(Request.InputStream))
                {
                    string json = inputStream.ReadToEnd();

                    try
                    {
                        string Sqlsrt = "INSERT INTO ReqType(reqtype) VALUES('" + json.Replace("//n", "\\n") + "')";
                        int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sqlsrt));
                    }
                    catch (Exception) { }

                    if (!string.IsNullOrEmpty(json))
                    {
                        var jss = new JavaScriptSerializer();
                        var data = jss.Deserialize<object>(json);

                        var dataDict = data as Dictionary<string, object>;
                        if (dataDict != null && dataDict.ContainsKey("reqtype"))
                        {
                            _ReqType = dataDict["reqtype"].ToString();

                            if (_ReqType == "shopping")
                            {
                                // Response.Write(RepurchaseProductRequest(data));
                            }
                            else
                            {
                                var dict = jss.Deserialize<Dictionary<string, string>>(json);

                                if (dict.ContainsKey("reqtype"))
                                {
                                    _ReqType = dict["reqtype"];

                                    if (!string.IsNullOrEmpty(_ReqType))
                                    {
                                        Process(_ReqType, dict);
                                    }
                                    else
                                    {
                                        Response.Write("{\"response\":\"FAILED\"}");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Response.Write("{\"response\":\"FAILED\"}");
                        }
                    }
                    else
                    {
                        Response.Write("{\"response\":\"FAILED\"}");
                    }
                }
            }
        }
        catch (Exception)
        {
            Response.Write("{\"response\":\"FAILED\"}");
        }

        Response.End();
    }
    public void Process(string _Reqtype,  Dictionary<string, string> dict)
    {
        try
        {
            if (_Reqtype == "livepriceapi")
            {
                string Result_Json = livepriceapi();
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "rateapi")
            {
                string Result_Json = livepriceapiUpdate();
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "sribalanceapi")  
            {
                string _Reqaddress = ClearInject(dict["walletaddress"]);
                //string _Reqtokencontractaddress = ClearInject(dict["tokencontractaddress"]);
                //string Result_Json = Fund_Balance_Check(_Reqaddress, _Reqtokencontractaddress);
                string Result_Json = Fund_Balance_Check(_Reqaddress);
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "usdtbalanceapi")
            {
                string _Reqaddress = ClearInject(dict["walletaddress"]);
                //string _Reqtokencontractaddress = ClearInject(dict["tokencontractaddress"]);
                //string Result_Json = Fund_Balance_Checkusdt(_Reqaddress, _Reqtokencontractaddress);
                string Result_Json = Fund_Balance_Checkusdt(_Reqaddress);
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "srisinglepayoutapi")
            {
                string _Reqaddress = ClearInject(dict["to"]);
                string _ReqAmount = ClearInject(Convert.ToString(Convert.ToDecimal(dict["amount"])));
                string _Reqliverate = ClearInject(Convert.ToString(Convert.ToDecimal(dict["rate"])));
                string _Requsdtamount = ClearInject(Convert.ToString(Convert.ToDecimal(dict["usdtamount"])));
                string _Requsdthash = ClearInject(dict["txHash"]);
                string Result_Json = DeductWalletApi(_Reqaddress, _ReqAmount, _Reqliverate, _Requsdtamount, _Requsdthash);
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "usdtsinglepayoutapi")
            {
                string _Reqaddress = ClearInject(dict["to"]);
                string _ReqAmount = ClearInject(Convert.ToString(Convert.ToDecimal(dict["amount"])));
                string _Reqliverate = ClearInject(Convert.ToString(Convert.ToDecimal(dict["rate"])));
                string _Requsdtamount = ClearInject(Convert.ToString(Convert.ToDecimal(dict["usdtamount"])));
                string _Requsdthash = ClearInject(dict["txHash"]);
                string Result_Json = DeductWalletApiusdt(_Reqaddress, _ReqAmount, _Reqliverate, _Requsdtamount, _Requsdthash);
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else
            {
                ErrObj.Response = "FAILED";
                WriteJson(ErrObj);
            }
        }
        catch (Exception)
        {
            ErrObj.Response = "FAILED";
            WriteJson(ErrObj);
        }
    }
    private string livepriceapi()
    {
        DataTable dt_GetAPI = new DataTable();
        string str_GetAPI = " Exec Sp_GetwalletUSDTAP ";
        dt_GetAPI = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str_GetAPI).Tables[0];
        string _Output = "";
        if (dt_GetAPI.Rows.Count > 0)
        {
            _Output = "{\"response\":\"OK\",\"rate\":\"" + dt_GetAPI.Rows[0]["Gasfees"] + "\",\"msg\":\"OK\"}";
        }
        return _Output;
    }
    private string livepriceapiUpdate()
    {
        DataTable dt_GetAPI = new DataTable();
        string str_GetAPI = " Exec Sp_GetwalletUSDTAPUppdate ";
        dt_GetAPI = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str_GetAPI).Tables[0];
        string _Output = "";
        if (dt_GetAPI.Rows.Count > 0)
        {
            _Output = "{\"response\":\"OK\",\"rate\":\"" + dt_GetAPI.Rows[0]["Gasfees"] + "\",\"msg\":\"OK\"}";
        }
        return _Output;
    }
    public void WriteJson(object _object)
    {
        try
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonData = javaScriptSerializer.Serialize(_object);
            WriteRaw(jsonData);
        }
        catch (Exception)
        {
            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
    }

    public void WriteRaw(string text)
    {
        try
        {
            Response.Write(text);
        }
        catch (Exception)
        {
            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
    }

    public DataSet ConvertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        xd = JsonConvert.DeserializeXmlNode(jsonString);
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));
        return ds;
    }
    //protected string Fund_Balance_Check(string walletaddress, string tokencontractaddress)
    protected string Fund_Balance_Check(string walletaddress)
    {
        string _Output = "";
        string OrderI_V = "";
        string privatekey_V = "";
        string URL = "";
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        string postData = "";
        string str = "";
        string statusApi = "";
        int Result_RR = 0;
        DataTable dsLoginTo_Address = new DataTable();
        string To_WalletAddress = "";
        decimal value = 0;
        DataSet data = new DataSet();

        try
        {
            bool Bool = true;
            if (Bool)
            {
                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                    URL = "http://88.99.244.121:7004/balance";
                    WebRequest tRequest = WebRequest.Create(URL);
                    tRequest.Method = "POST";
                    tRequest.ContentType = "application/json";
                    tRequest.ContentLength = 0;

                    //postData = "{\"walletAddress\": \"" + walletaddress.Trim() + "\",\"tokenContractAddress\":\"" + tokencontractaddress.Trim() + "\"}";
                    postData = "{\"walletAddress\": \"" + walletaddress.Trim() + "\"}";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                    string sql_req = "insert Into Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,postdata,Req_From)";
                    sql_req += " Values('" + sResult.Trim() + "','0','" + URL.Trim() + "','" + postData.Trim() + "','App')";
                    int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));

                    tRequest.ContentLength = byteArray.Length;
                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }

                    WebResponse tResponse = tRequest.GetResponse();
                    using (Stream dataStream = tResponse.GetResponseStream())
                    using (StreamReader tReader = new StreamReader(dataStream))
                    {
                        str = tReader.ReadToEnd();
                    }

                    string sql_res = "Update Tbl_ApiRequest_ResponseQrCode  Set Response = '" + str.Trim() + "' Where ReqID = '" + sResult.Trim() + "' AND Req_From = 'App'";
                    int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));

                    data = ConvertJsonStringToDataSet(str);
                    DataView dv = new DataView(data.Tables[1]);
                    dv.RowFilter = "To = '" + walletaddress.Trim() + "'";
                    dsLoginTo_Address = dv.ToTable();

                    if (dsLoginTo_Address.Rows.Count > 0)
                    {
                        foreach (DataRow row in dsLoginTo_Address.Rows)
                        {
                            To_WalletAddress = row["to"].ToString();
                            string From_WalletAddress = row["from"].ToString();
                            string Hash = row["hash"].ToString();
                            double originalValue = Convert.ToDouble(row["value"].ToString());
                            decimal Final_Amount = (decimal)(originalValue / Math.Pow(10, 18));
                            decimal AcurateTotalAmount = Decimal.Parse(Final_Amount.ToString(), System.Globalization.NumberStyles.Float);

                            string strs = "";
                            strs = "insert into ApiReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request,Response,ApiStatus,RectimeStamp,ApiType,TxnHash,";
                            strs += "AMount,PostData,TypeB,FromID,ToID,Req_From) ";
                            strs += "Values('0','" + sResult + "','" + walletaddress + "','" + walletaddress + "','" + URL + "','" + str + "',";
                            strs += "'" + statusApi + "',getdate(),'RE_TRANSCATION','" + Hash + "','" + Final_Amount + "',";
                            strs += "'','CHECK_BALANCE','" + To_WalletAddress + "','" + From_WalletAddress + "','App')";

                            string QueryStr = " Begin Try   Begin Transaction " + strs + " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
                            int x = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, QueryStr));
                            if (x > 0)
                            {
                                Result_RR = 1;
                            }
                        }
                    }

                    _Output = Result_RR > 0 ? str : str;
                }
                catch (Exception ex)
                {
                    string ErrorMsg = ex.Message;
                    string errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,formno,Req_From) ";
                    errorQry += "values('" + ErrorMsg + "',getdate(),'" + URL + "','" + To_WalletAddress.Trim() + "','" + postData + "','0','App')";
                    int x1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, errorQry));
                    _Output = str;
                }
            }
            else
            {
                _Output = str;
            }
        }
        catch (Exception ex)
        {
            _Output = str;
        }

        return _Output;
    }
    //protected string Fund_Balance_Checkusdt(string walletaddress, string tokencontractaddress)
    protected string Fund_Balance_Checkusdt(string walletaddress)
    {
        string _Output = "";
        string OrderI_V = "";
        string privatekey_V = "";
        string URL = "";
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        string postData = "";
        string str = "";
        string statusApi = "";
        int Result_RR = 0;
        DataTable dsLoginTo_Address = new DataTable();
        string To_WalletAddress = "";
        decimal value = 0;
        DataSet data = new DataSet();

        try
        {
            bool Bool = true;
            if (Bool)
            {
                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                    URL = "http://88.99.244.121:7003/balance";
                    WebRequest tRequest = WebRequest.Create(URL);
                    tRequest.Method = "POST";
                    tRequest.ContentType = "application/json";
                    tRequest.ContentLength = 0;

                    //postData = "{\"walletAddress\": \"" + walletaddress.Trim() + "\",\"tokenContractAddress\":\"" + tokencontractaddress.Trim() + "\"}";
                    postData = "{\"walletAddress\": \"" + walletaddress.Trim() + "\"}";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                    string sql_req = "insert Into Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,postdata,Req_From)";
                    sql_req += " Values('" + sResult.Trim() + "','0','" + URL.Trim() + "','" + postData.Trim() + "','App')";
                    int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));

                    tRequest.ContentLength = byteArray.Length;
                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }

                    WebResponse tResponse = tRequest.GetResponse();
                    using (Stream dataStream = tResponse.GetResponseStream())
                    using (StreamReader tReader = new StreamReader(dataStream))
                    {
                        str = tReader.ReadToEnd();
                    }

                    string sql_res = "Update Tbl_ApiRequest_ResponseQrCode  Set Response = '" + str.Trim() + "' Where ReqID = '" + sResult.Trim() + "' AND Req_From = 'App'";
                    int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));

                    data = ConvertJsonStringToDataSet(str);
                    DataView dv = new DataView(data.Tables[1]);
                    dv.RowFilter = "To = '" + walletaddress.Trim() + "'";
                    dsLoginTo_Address = dv.ToTable();

                    if (dsLoginTo_Address.Rows.Count > 0)
                    {
                        foreach (DataRow row in dsLoginTo_Address.Rows)
                        {
                            To_WalletAddress = row["to"].ToString();
                            string From_WalletAddress = row["from"].ToString();
                            string Hash = row["hash"].ToString();
                            double originalValue = Convert.ToDouble(row["value"].ToString());
                            decimal Final_Amount = (decimal)(originalValue / Math.Pow(10, 18));
                            decimal AcurateTotalAmount = Decimal.Parse(Final_Amount.ToString(), System.Globalization.NumberStyles.Float);

                            string strs = "";
                            strs = "insert into ApiReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request,Response,ApiStatus,RectimeStamp,ApiType,TxnHash,";
                            strs += "AMount,PostData,TypeB,FromID,ToID,Req_From) ";
                            strs += "Values('0','" + sResult + "','" + walletaddress + "','" + walletaddress + "','" + URL + "','" + str + "',";
                            strs += "'" + statusApi + "',getdate(),'RE_TRANSCATION','" + Hash + "','" + Final_Amount + "',";
                            strs += "'','CHECK_BALANCE','" + To_WalletAddress + "','" + From_WalletAddress + "','App')";

                            string QueryStr = " Begin Try   Begin Transaction " + strs + " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
                            int x = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, QueryStr));
                            if (x > 0)
                            {
                                Result_RR = 1;
                            }
                        }
                    }

                    _Output = Result_RR > 0 ? str : str;
                }
                catch (Exception ex)
                {
                    string ErrorMsg = ex.Message;
                    string errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,formno,Req_From) ";
                    errorQry += "values('" + ErrorMsg + "',getdate(),'" + URL + "','" + To_WalletAddress.Trim() + "','" + postData + "','0','App')";
                    int x1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, errorQry));
                    _Output = str;
                }
            }
            else
            {
                _Output = str;
            }
        }
        catch (Exception ex)
        {
            _Output = str;
        }

        return _Output;
    }
    public string DeductWalletApi(string WalletAddress, string withdrawalAmount, string liveRate, string USDTAmount, string USDTHash)
    {
        string _Output = "";
        string requestUrl1 = "";
        string GetPayoutDetail = "";
        DataTable dtQuery = new DataTable();
        string sResponseFromServer1 = string.Empty;
        string StatusApi = "";
        string ReqNo = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        ReqNo = formatted_datetime;
        int i = 0;
        string hash_ = "";
        string postData = "";
        DataSet dsLogin = new DataSet();
        string str = string.Empty;
        decimal value = 0;
        string Code = "";
        DataSet ds = new DataSet();
        DataSet data = new DataSet();
        string URL = "";

        try
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                URL = "http://88.99.244.121:7004/singlePayout";
                WebRequest tRequest = WebRequest.Create(URL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.ContentLength = 0;
                postData = "{\"to\":\"" + WalletAddress.Trim() + "\",\"amount\":\"" + Convert.ToDecimal(withdrawalAmount) + "\",\"txId\":\"" + ReqNo.Trim() + "\"}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                string sql_req = "insert Into Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,postdata,Req_From,Rate,BhtsAmount,BhtsHash)";
                sql_req += " Values('" + ReqNo.Trim() + "','0','" + URL.Trim() + "','" + postData.Trim() + "','App','" + Convert.ToDecimal(liveRate) + "','" + Convert.ToDecimal(USDTAmount) + "','" + USDTHash.Trim() + "')";
                int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));

                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                using (WebResponse tResponse = tRequest.GetResponse())
                using (Stream dataStream = tResponse.GetResponseStream())
                using (StreamReader tReader = new StreamReader(dataStream))
                {
                    str = tReader.ReadToEnd();
                }

                string sql_res = "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + str.Trim() + "' Where ReqID = '" + ReqNo.Trim() + "' AND Req_From = 'App'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));

                dsLogin = ConvertJsonStringToDataSet(str);
                DataSet ds1 = new DataSet();
                string resultStatus = "";
                string strs = "";

                strs += "insert into ApiReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request,Response,ApiStatus,RectimeStamp,ApiType,TxnHash,AMount,PostData,TypeB,Req_From) ";
                strs += "Values('0','" + WalletAddress + "','" + WalletAddress + "','" + WalletAddress + "','" + URL + "','" + str + "','" + StatusApi + "',getdate(),";
                strs += "'Token Payout','" + hash_ + "','" + Convert.ToDecimal(withdrawalAmount) + "','" + postData + "','QrCode','App');";
                string Query = " Begin Try   Begin Transaction " + strs + " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";

                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Query));
                if (i > 0)
                {
                    _Output = str;
                }
            }
            catch (Exception ex)
            {
                string errorQry = "";
                string ErrorMsg = "";

                try
                {
                    ErrorMsg = ex.Message;
                }
                catch
                {
                    ErrorMsg = "";
                }

                errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,formno,Req_From) ";
                errorQry += "values('" + ErrorMsg + "',getdate(),'" + URL + "','" + WalletAddress.Trim() + "','" + postData + "','0','App');";
                errorQry += "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + ErrorMsg + "' Where ReqID = '" + ReqNo.Trim() + "' AND Req_From = 'App'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, errorQry));

                if (x_res > 0)
                {
                    _Output = str;
                }
            }
        }
        catch (Exception ex)
        {
            _Output = str;
        }
        return _Output;
    }
    public string DeductWalletApiusdt(string WalletAddress, string withdrawalAmount, string liveRate, string USDTAmount, string USDTHash)
    {
        string _Output = "";
        string requestUrl1 = "";
        string GetPayoutDetail = "";
        DataTable dtQuery = new DataTable();
        string sResponseFromServer1 = string.Empty;
        string StatusApi = "";
        string ReqNo = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        ReqNo = formatted_datetime;
        int i = 0;
        string hash_ = "";
        string postData = "";
        DataSet dsLogin = new DataSet();
        string str = string.Empty;
        decimal value = 0;
        string Code = "";
        DataSet ds = new DataSet();
        DataSet data = new DataSet();
        string URL = "";

        try
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                URL = "http://88.99.244.121:7003/singlePayout";
                WebRequest tRequest = WebRequest.Create(URL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.ContentLength = 0;
                postData = "{\"to\":\"" + WalletAddress.Trim() + "\",\"amount\":\"" + Convert.ToDecimal(withdrawalAmount) + "\",\"txId\":\"" + ReqNo.Trim() + "\"}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                string sql_req = "insert Into Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,postdata,Req_From,Rate,BhtsAmount,BhtsHash)";
                sql_req += " Values('" + ReqNo.Trim() + "','0','" + URL.Trim() + "','" + postData.Trim() + "','App','" + Convert.ToDecimal(liveRate) + "','" + Convert.ToDecimal(USDTAmount) + "','" + USDTHash.Trim() + "')";
                int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));

                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                using (WebResponse tResponse = tRequest.GetResponse())
                using (Stream dataStream = tResponse.GetResponseStream())
                using (StreamReader tReader = new StreamReader(dataStream))
                {
                    str = tReader.ReadToEnd();
                }

                string sql_res = "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + str.Trim() + "' Where ReqID = '" + ReqNo.Trim() + "' AND Req_From = 'App'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));

                dsLogin = ConvertJsonStringToDataSet(str);
                DataSet ds1 = new DataSet();
                string resultStatus = "";
                string strs = "";

                strs += "insert into ApiReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request,Response,ApiStatus,RectimeStamp,ApiType,TxnHash,AMount,PostData,TypeB,Req_From) ";
                strs += "Values('0','" + WalletAddress + "','" + WalletAddress + "','" + WalletAddress + "','" + URL + "','" + str + "','" + StatusApi + "',getdate(),";
                strs += "'Token Payout','" + hash_ + "','" + Convert.ToDecimal(withdrawalAmount) + "','" + postData + "','QrCode','App');";
                string Query = " Begin Try   Begin Transaction " + strs + " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";

                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Query));
                if (i > 0)
                {
                    _Output = str;
                }
            }
            catch (Exception ex)
            {
                string errorQry = "";
                string ErrorMsg = "";

                try
                {
                    ErrorMsg = ex.Message;
                }
                catch
                {
                    ErrorMsg = "";
                }

                errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,formno,Req_From) ";
                errorQry += "values('" + ErrorMsg + "',getdate(),'" + URL + "','" + WalletAddress.Trim() + "','" + postData + "','0','App');";
                errorQry += "Update Tbl_ApiRequest_ResponseQrCode Set Response = '" + ErrorMsg + "' Where ReqID = '" + ReqNo.Trim() + "' AND Req_From = 'App'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, errorQry));

                if (x_res > 0)
                {
                    _Output = str;
                }
            }
        }
        catch (Exception ex)
        {
            _Output = str;
        }
        return _Output;
    }
    public string JsonEncode(string str)
    {
        str = str.Replace("\\", "\\\\");
        str = str.Replace("\"", "\\\"");
        str = str.Replace("//n", " \\n ");
        str = str.Replace("\\n", "\n");
        str = str.Replace("\n", " \\n ");
        if (!string.IsNullOrEmpty(str))
        {
            str = str.Replace(Environment.NewLine, " \\n ");
        }
        str = str.Replace("\r\n", " \\n ");
        str = str.Replace("\t", "\\t");
        return str;
    }
    private string ClearInject(string str)
    {
        string strReturn = str.Replace("'", "''").Replace("\t", " ");
        strReturn = strReturn.Replace("\\\\", "\\");
        if (!string.IsNullOrEmpty(strReturn))
        {
            strReturn = strReturn.Replace(Environment.NewLine, " \\n ");
        }
        return strReturn;
    }
}
public class GetMsg2
{
    private string _Error;

    public string Response
    {
        get { return _Error; }
        set { _Error = value; }
    }
}
