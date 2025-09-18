using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using Irony;
using System.Configuration;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;

//partial class CheckLogin : System.Web.UI.Page
public partial class Checklogin : System.Web.UI.Page
{
    private SqlConnection Conn;
    private SqlCommand Comm;
    private DataTable Dt;
    private SqlDataAdapter Ad;
    private string str = "";
    private string _RefFormNo = "";
    private string _UpLnFormNo = "";
    private string _Company = "";
    private clsGeneral objGen = new clsGeneral();
    private DAL objAccess;
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            using (SqlConnection Conn = new SqlConnection(constr.ToString()))
            {
                Conn.Open();
                string sRequestData = HttpContext.Current.Request.Url.ToString();
                string _Company = Session["CompName"].ToString();

                if (Request["token"] == "darabUnMar54lE8a4i5n610kvhjkvhj46barghfdhcfgkhkbhjhf" || Request["CompId"] == "1017")
                {
                    try
                    {
                        string sqlstr = "INSERT INTO ReqType(reqtype) VALUES(@RequestData)";
                        SqlCommand cmd = new SqlCommand(sqlstr, Conn);
                        cmd.Parameters.AddWithValue("@RequestData", sRequestData.Replace("//n", "\n"));

                        int i = cmd.ExecuteNonQuery();

                        if (i > 0)
                        {
                            if (Request["action"] == null)
                            {
                                CheckInfo();
                            }
                            else if (Request["action"].ToUpper() == "LOGIN")
                            {
                                CheckInfoDetailData();
                            }
                            else if (Request["action"].ToUpper() == "ADDBV")
                            {
                                AddBV();
                            }
                            else if (Request["action"].ToUpper() == "DEBITBV")
                            {
                                DrBV();
                            }
                            else if (Request["action"].ToUpper() == "GETBALANCE")
                            {
                                string Uname = Request["Username"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                                string Pwd = Request["Password"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                                string WallettType = Request["Wallertype"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");

                                if (WallettType != "R")
                                {
                                    Response.Write("{\"response\":\"FAILED\",\"msg\":\"Invalid wallet type!\"}");
                                    return;
                                }
                                int FormNo = GetFormNo(Uname, Pwd.Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@"));
                                if (FormNo > 0)
                                {
                                    double AvailBal = Convert.ToDouble(GetBalance(FormNo.ToString(), WallettType.ToUpper()));
                                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"OK\",\"walletBalance\":\"" + AvailBal + "\",\"msg\":\"success\",\"wallettype\":\"" + WallettType + "\"}");
                                }
                                else
                                {
                                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"walletBalance\":\"0\",\"walletbalanceinr\":\"0\",\"msg\":\"Invalid Login details\",\"wallettype\":\"" + WallettType + "\"}");
                                }
                            }
                            else if (Request["action"].ToUpper() == "DEDUCTWALLETAMOUNT")
                            {
                                string WallettType = Request["Wallertype"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                                DeductWalletAmount(WallettType);
                            }
                            else if (Request["action"].ToUpper() == "REFUNDWALLETAMOUNT")
                            {
                                string WallettType = Request["Wallertype"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                                RefundWalletAmount(WallettType);
                            }
                            else if (Request["action"].ToUpper() == "ADDWALLET")
                            {
                                AddWalletAmount();
                            }
                            else if (Request["action"].ToUpper() == "CONFIRMVOUCHER")
                            {
                                string Uname = Request["Username"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                                string Pwd = Request["Password"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                                int FormNo = GetFormNo(Uname, Pwd.Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@"));
                                string VoucherRequest = Request["voucherno"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                                string VoucherResponse = Request["response"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                                string WallettType = Request["Wallertype"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");

                                if (FormNo > 0)
                                {
                                    ConfirmVoucher(VoucherRequest, VoucherResponse, WallettType);
                                }
                                else
                                {
                                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
                                }
                            }
                            else
                            {
                                Response.Write("{\"response\":\"FAILED\"}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception if necessary
                    }
                }
                else
                {
                    Response.Write("{\"response\":\"FAILED\"}");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"response\":\"FAILED\"}");
        }
    }
    private string Application(string v)
    {
        throw new NotImplementedException();
    }
    private string Replace(string sRequestData, string v1, string v2)
    {
        throw new NotImplementedException();
    }
    private string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    private void CheckInfoDetailData()
    {
        string _Output = "";
        try
        {
            string ewallet = "";
            string rwallet = "";
            string rewardwallet = "";
            string PointWallet = "";
            string ismovie = "N";
            string col = "";
            string strqry = "";
            _Output = "{\"data\":[";

            string SqlStr = "Exec ProcCheckInfoNewUpdateINR '" +
                            Request["Username"].Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@") + "',";
            SqlStr += "'" + Request["Password"].Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@") + "'";

            DataTable Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, SqlStr).Tables[0];
            string Res = "";

            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows[0]["Status"].ToString().ToUpper() == "BLOCK")
                {
                    _Output = _Output + "],\"loginid\":\"\",\"name\":\"Invalid credential\",\"email\":\"\",\"city\":\"\",\"isactive\":\"\"," +
                          "\"mobileno\":\"0\",\"DOJ\":\"\",\"pbalance\":\"0\",\"status\":\"Block\",\"response\":\"Failed\"}";
                }
                else
                {
                    if (Dt.Rows[0]["activestatus"].ToString() == "Y")
                    {
                        foreach (DataRow Dr in Dt.Rows)
                        {
                            string OrderDate = "";
                            Res += "{" +
                                   "\"sno\":\"" + Dr["sno"] + "\",\"OrderNo\":\"" + Dr["OrderNo"] + "\",\"orderdate\":\"" + Dr["orderdate"] + "\"," +
                                   "\"kitname\":\"" + Dr["KitName"] + "\",\"kitamount\":\"" + Dr["KitAmount"] + "\",\"kitid\":\"" + Dr["KitId"] + "\"," +
                                   "\"shoppoint\":\"" + Dr["shoppoint"] + "\",\"coupon\":\"" + Dr["Coupon"] + "\",\"promoid\":\"" + Dr["promoid"] + "\",\"isholiday\":\"" + Dr["isholiday"] + "\"" +
                                   "},";
                        }

                        if (Res.Length > 0)
                        {
                            Res = Res.Remove(Res.Length - 1, 1);
                        }
                    }
                    _Output = _Output + Res + "],\"loginid\":\"" + Dt.Rows[0]["idno"] + "\",\"name\":\"" + Dt.Rows[0]["MemName"] + "\"," +
                              "\"email\":\"" + Dt.Rows[0]["Email"] + "\",\"city\":\"" + Dt.Rows[0]["City"] + "\",\"isactive\":\"" + Dt.Rows[0]["activestatus"] + "\"," +
                              "\"mobileno\":\"" + Dt.Rows[0]["mobl"] + "\",\"DOJ\":\"" + Dt.Rows[0]["JoinDate"] + "\"," +
                              "\"abalance\":\"" + Dt.Rows[0]["PBalance"] + "\",\"status\":\"" + Dt.Rows[0]["Status1"] + "\",\"response\":\"OK\"}";
                }

            }
            else
            {
                _Output = _Output + "],\"loginid\":\"\",\"name\":\"Invalid credential\",\"email\":\"\",\"city\":\"\",\"isactive\":\"\"," +
                          "\"mobileno\":\"0\",\"DOJ\":\"\",\"pbalance\":\"0\",\"status\":\"\",\"response\":\"Failed\"}";
            }

            Response.Write(_Output.Replace("}{", "},{"));
        }
        catch (Exception ex)
        {
            _Output = _Output + "],\"loginid\":\"\",\"name\":\"Invalid\",\"email\":\"\",\"city\":\"\",\"isactive\":\"\"," +
                      "\"mobileno\":\"0\",\"DOJ\":\"\",\"mwinr\":\"0\",\"rpinr\":\"0\",\"mwbalance\":\"0\",\"rpbalance\":\"0\",\"status\":\"\",\"response\":\"Failed\"}";
            Response.Write(_Output.Replace("}{", "},{"));
        }
    }
    private void AddWalletAmount() // 29Aug16,NJ
    {
        string VoucherNo = "";
        string str1 = "";
        string AType = "";
        string FType = "";
        string NType = "";
        string Amount = Replace(Replace(Request["Amount"], "'", ""), "=", "");

        string TxnData = Request["reqno"];
        string status = Replace(Replace(Request["status"], "'", ""), "=", "");
        try
        {
            var LoginSuccess = 0;
            int FormNo = 0;
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            string info = Base64Decode(Request["reqno"]);
            string[] separators = new[] { "/", "=", "&", ":" };
            string[] sa = info.Split(separators, StringSplitOptions.None);
            string str = "Select formno,amount,Acttype,fromidno,Narration FROM OnlinetransactionW WHERE OrderId='" + TxnData + "' ";
            //Comm = new SqlCommand(str, Conn);
            //SqlDataReader Dr = Comm.ExecuteReader();
            DataSet ds = new DataSet();
            DataTable Dt = new DataTable();
            Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];
            //if (Dr.Read)
            if (Dt.Rows.Count > 0)
            {
                LoginSuccess = 1;
                FormNo = Convert.ToInt32(Dt.Rows[0]["FormNo"]);
                Amount = Dt.Rows[0]["Amount"].ToString();
                AType = Dt.Rows[0]["Acttype"].ToString();
                FType = Dt.Rows[0]["fromidno"].ToString();
                NType = Dt.Rows[0]["Narration"].ToString();
            }
            //Dr.Close();
            // FormNo = GetFormNo(Uname, Pwd)
            if (FormNo > 0)
                LoginSuccess = 1;
            else
            {
                Response.Write("{\"loginid\": \"NO\",\"response\":\"FAILED\",\"addamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Invalid User\"}");
                return;
            }
            //string[] TData = TxnData.Split(";");
            string[] separators1 = new[] { ";" };
            string[] TData = TxnData.Split(separators1, StringSplitOptions.None);
            string TxnID, Remarks;
            double Bv;

            LoginSuccess = 2;

            if (LoginSuccess == 2)
            {
                //if (Conn.State == ConnectionState.Closed)
                //    Conn.Open();
                if (status == "Success" & AType == "Wallet")
                {
                    str = "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo   ,Actype,VType ,sessid,WSessID) " + " Select ISNULL(MAX(VoucherNo)+1,1001),Cast(Convert(varchar,Getdate(),106) as DateTime),'0','" + FormNo + "','" + Amount + "'," + " 'Fund Wallet Credited by Online against ReqNo." + sa[0].ToString() + "'," + " '" + sa[0].ToString() + "','S','C', convert(varchar,cast(cast(getdate() as varchar) as datetime),112)," + " '" + Session["CurrentSessn"] + "' FROM TrnVoucher;";
                    str = str + "Update WalletReq SET  IsApprove = 'A',ApproveDate=GEtdate(),ApproveBy='0',ApproveRemark='By Online " + NType + "' where FormNo='" + FormNo + "' And ReqNo='" + sa[0].ToString() + "';";
                    str = str + "Update OnlinetransactionW SET  Status='Success' where FormNo='" + FormNo + "' And OrderId='" + TxnData + "'";
                    DataSet ds1 = new DataSet();
                    DataTable Dt1 = new DataTable();
                    Dt1 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];

                    //Comm = new SqlCommand(str, Conn);
                    //Comm.ExecuteNonQuery();

                    // Dim s As String = "select Max(VoucherNo)as VoucherNo from TrnVoucher "
                    // Comm = New SqlCommand(s, Conn)
                    // Dr = Comm.ExecuteReader()

                    // If Dr.Read Then
                    // VoucherNo = Dr("VoucherNo")
                    // End If
                    // Dr.Close()
                    Response.Write("{\"response\":\"OK\",\"addamount\":\"" + Amount + "\",\"msg\": \"success\"}");
                }
                else if (status == "Failed" & AType == "Wallet")
                {
                    str = "Update WalletReq SET  IsApprove = 'R',ApproveDate=GEtdate(),ApproveBy='0',ApproveRemark='By Online' where FormNo='" + FormNo + "' And ReqNo='" + sa[0].ToString() + "';";
                    str = str + "Update OnlinetransactionW SET  Status='Failed' where FormNo='" + FormNo + "' And OrderId='" + TxnData + "'";
                    DataSet ds2 = new DataSet();
                    DataTable Dt2 = new DataTable();
                    Dt2 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];

                    //Comm = new SqlCommand(str, Conn);
                    //Comm.ExecuteNonQuery();
                    Response.Write("{\"response\":\"FAILED\",\"addamount\":\"0\",\"msg\": \"failed\"}");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"loginid\": \"NO\",\"response\":\"FAILED\",\"addamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\"}");
        }
    }
    // ' Start By Rakesh Soni
    private string GetWalletName(string WalletType)
    {
        string RtrVal = "";
        try
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string str = "Select * from VoucherType Where AcType = '" + WalletType + "'";
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str);
            dt = ds.Tables[0];
            if ((dt.Rows.Count > 0))
                RtrVal = dt.Rows[0]["WalletName"].ToString();
            else
                RtrVal = "";

        }

        catch (Exception ex)
        {
        }
        return RtrVal;
    }
    private double GetBalance(string FormNo, string WalletType)
    {
        double RtrVal = 0;
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string str = "Select balance From dbo.ufnGetBalance('" + FormNo + "','" + WalletType + "')";
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str);
            dt = ds.Tables[0];
            if ((dt.Rows.Count > 0))
            {
                RtrVal = Convert.ToDouble(dt.Rows[0]["Balance"]);
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        return RtrVal;
    }
    private void DeductWalletAmount(string WalletType)
    {
        string VoucherNo = "";

        string Uname = Request["Username"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
        string Pwd = Request["Password"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
        string TxnData = Request["TxnData"].Trim().Replace("'", "").Replace("=", "");

        try
        {
            int LoginSuccess = 0, FormNo = 0;
            using (SqlConnection Conn = new SqlConnection(constr.ToString()))
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();
                string isblock = "N";
                string str1 = "Select * FROM M_MemberMaster WHERE IDNo = '" + Uname + "'";
                using (SqlCommand Comm = new SqlCommand(str1, Conn))
                {
                    using (SqlDataReader Dr = Comm.ExecuteReader())
                    {
                        if (Dr.Read())
                        {
                            isblock = Dr["IsBlock"].ToString();
                        }
                    }
                }
                if (isblock.ToString() == "Y")
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"deductamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Invalid User\",\"wallettype\": \"" + WalletType + "\"}");
                    return;
                }
                FormNo = GetFormNo(Uname, Pwd.Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@"));
                if (WalletType != "R")
                {
                    Response.Write("{\"response\":\"FAILED\",\"msg\": \"Invalid wallet type!\"}");
                    return;
                }
                string WalletName = GetWalletName(WalletType);
                if (FormNo > 0)
                {
                    LoginSuccess = 1;
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"deductamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Invalid User\",\"wallettype\": \"" + WalletType + "\"}");
                    return;
                }
                string[] TData = TxnData.Split(';');
                string TxnID, Remarks;
                double Bv = 0, finalBv = 0;
                if (TData.Length == 3)
                {
                    TxnID = TData[0].Trim();
                    Bv = Math.Round(Convert.ToDouble(TData[1]), 2);
                    finalBv = Convert.ToDouble(TData[1]);
                    Remarks = TData[2].Trim();
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"deductamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Insufficient Data\",\"wallettype\": \"" + WalletType + "\"}");
                    return;
                }

                double amount = 0;
                double AvailBal = Convert.ToDouble(GetBalance(FormNo.ToString(), WalletType));
                if (AvailBal < finalBv && LoginSuccess == 1)
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"deductamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Available Balance is " + AvailBal + "\",\"wallettype\": \"" + WalletType + "\"}");
                    return;
                }
                else
                {
                    LoginSuccess = 2;
                }

                if (LoginSuccess == 2)
                {
                    if (finalBv > 0)
                    {
                        string sessn = "", Coinrate = "", RPrate = "";
                        string q = @"SELECT SUM(sessid) AS sessid, SUM(coinrate) AS coinrate, SUM(RpRate) AS RPrate
                                 FROM (
                                     SELECT ISNULL(MAX(Sessid), CONVERT(VARCHAR, GETDATE(), 112)) AS Sessid, 0 AS CoinRate, 0 AS Rprate 
                                     FROM D_Sessnmaster 
                                     UNION ALL 
                                     SELECT 0 AS Sessid, CoinRate, Rprate 
                                     FROM M_CoinMaster 
                                     WHERE activeStatus = 'Y'
                                 ) AS a";
                        DataTable DtQ = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, q).Tables[0];
                        if (DtQ.Rows.Count > 0)
                        {
                            sessn = DtQ.Rows[0]["Sessid"].ToString();
                            Coinrate = DtQ.Rows[0]["CoinRate"].ToString();
                            RPrate = DtQ.Rows[0]["RpRate"].ToString();
                        }

                        string sql = "SELECT * FROM TrnVoucher WHERE refno = '" + TxnID + "'";
                        DataTable dt = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, sql).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"deductamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Already Deducted Amount\",\"wallettype\": \"" + WalletType + "\"}");
                        }
                        else
                        {
                            string str = @"INSERT INTO TrnVoucher (VoucherNo, VoucherDate, DrTo, CrTo, Amount, Narration, RefNo, Actype, VType, sessid, WSessID,TAmount)
                                       SELECT ISNULL(MAX(VoucherNo) + 1, 1001), CAST(CONVERT(VARCHAR, GETDATE(), 106) AS DATETIME), '" + FormNo + @"', '0', '" + Bv + @"', 
                                       '" + WalletName + "  Used by " + Remarks + " against order no." + TxnID + @"', '" + TxnID + "', '" + WalletType + @"', 'D', 
                                       CONVERT(VARCHAR, CAST(CAST(GETDATE() AS VARCHAR) AS DATETIME), 112), '1','" + finalBv + @"'
                                       FROM TrnVoucher";

                            int i = SqlHelper.ExecuteNonQuery(Conn, CommandType.Text, str);
                            if (i > 0)
                            {
                                string s = "SELECT TOP 1 VoucherNo AS VoucherNo, Amount FROM TrnVoucher WHERE Drto = '" + FormNo + "' AND Actype = '" + WalletType + "' AND RefNo = '" + TxnID + "' ORDER BY Voucherid DESC";
                                DataTable dt_ = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, s).Tables[0];
                                if (dt_.Rows.Count > 0)
                                {
                                    VoucherNo = dt_.Rows[0]["VoucherNo"].ToString();
                                    amount = Convert.ToDouble(dt_.Rows[0]["Amount"]);
                                }

                                string OutPut_ = "{\"loginid\": \"" + Uname + "\",\"response\":\"OK\",\"deductamount\":\"" + finalBv + "\",\"deductamountusdt\":\"" + amount + "\",\"voucherno\":\"" + VoucherNo + "\",\"msg\": \"success\",\"wallettype\": \"" + WalletType + "\"}";
                                Response.Write(OutPut_);
                            }
                            else
                            {
                                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"deductamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\",\"wallettype\": \"" + WalletType + "\"}");
                            }
                        }
                    }
                    else
                    {
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"deductamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\",\"wallettype\": \"" + WalletType + "\"}");
                    }
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"deductamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Login failed\",\"wallettype\": \"" + WalletType + "\"}");
                }


            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"deductamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\",\"wallettype\": \"" + WalletType + "\"}");
        }
    }
    private void RefundWalletAmount(string WalletType)
    {
        string VoucherNo = "";
        string Uname = Request["Username"].Replace("'", "").Replace("=", "").Replace(";", "").Trim();
        string Pwd = Request["Password"].Replace("'", "").Replace("=", "").Replace(";", "").Trim();
        string TxnData = Request["TxnData"].Replace("'", "").Replace("=", "").Trim();

        try
        {
            int LoginSuccess = 0, FormNo = 0;
            using (SqlConnection Conn = new SqlConnection(constr.ToString()))
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                FormNo = GetFormNo_1(Uname);
                string isblock = "N";
                string str1 = "Select * FROM M_MemberMaster WHERE IDNo = '" + Uname + "'";
                using (SqlCommand Comm = new SqlCommand(str1, Conn))
                {
                    using (SqlDataReader Dr = Comm.ExecuteReader())
                    {
                        if (Dr.Read())
                        {
                            isblock = Dr["IsBlock"].ToString();
                        }
                    }
                }
                if (isblock.ToString() == "Y")
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Invalid User\",\"wallettype\": \"" + WalletType + "\"}");
                    return;
                }

                if (WalletType != "R")
                {
                    Response.Write("{\"response\":\"FAILED\",\"msg\": \"Invalid wallet type!\"}");
                    return;
                }
                string WalletName = GetWalletName(WalletType);

                if (FormNo > 0)
                {
                    LoginSuccess = 1;
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Invalid User\",\"wallettype\": \"" + WalletType + "\"}");
                    return;
                }
                string[] TData = TxnData.Split(';');
                string TxnID, Remarks = "";
                double Bv = 0;
                double amount = 0, finalamount = 0;

                if (TData.Length == 3)
                {
                    TxnID = TData[0].Trim();
                    finalamount = Convert.ToDouble(TData[1]);
                    Bv = Math.Round(Convert.ToDouble(TData[1]), 8);
                    Remarks = TData[2].Trim();
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Amount Not Refunded\",\"wallettype\": \"" + WalletType + "\"}");
                    return;
                }

                string str123 = "Select Isnull(SUM(Debit),0) As Debit ,  Isnull(SUM(credit),0) As Credit,isnull(sum(CoinRate),0) as CoinRate, isnull(sum(Rprate),0) as RPrate ,"
                    + " Case when '" + WalletType + "'='S'  then isnull(" + Bv.ToString() + ",0) "
                    + " when '" + WalletType + "'='M' then isnull(" + Bv.ToString() + ",0)"
                    + " else isnull(sum(" + Bv.ToString() + "),0) end as amount  from ( "
                    + " Select Amount As Debit, 0 As credit,0 as coinrate,0 as RPrate  from  TrnVoucher "
                    + "  Where Drto =  '" + FormNo + "' And RefNo = '" + TxnID + "' And VType = 'D' And AcType = '" + WalletType + "'"
                    + " union All "
                    + " Select 0 As Debit, Amount  As credit,0 as coinrate,0 as RPrate  from  TrnVoucher "
                    + " Where Crto =  '" + FormNo + "' And RefNo = '" + TxnID + "' And VType = 'C' And AcType = '" + WalletType + "'"
                    + " Union all select 0 as Debit,0 as Credit,isnull(CoinRate,0) ,isnull(Rprate,0)  from M_CoinMaster where activeStatus='Y')As a";

                DataTable dt123 = new DataTable();
                using (DataSet ds123 = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, str123))
                {
                    dt123 = ds123.Tables[0];
                }

                if (dt123.Rows.Count > 0)
                {
                    amount = Convert.ToDouble(dt123.Rows[0]["Amount"]);
                }

                if (Convert.ToDouble(dt123.Rows[0]["Debit"]) > 0)
                {
                    if (Convert.ToDouble(dt123.Rows[0]["Debit"]) < (Convert.ToDouble(dt123.Rows[0]["credit"]) + Convert.ToDouble(dt123.Rows[0]["Amount"])))
                    {
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"refundamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Amount greater than Debit Amount.\",\"wallettype\": \"" + WalletType + "\"}");
                        return;
                    }
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"refundamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Order Number Not Exists.\",\"wallettype\": \"" + WalletType + "\"}");
                    return;
                }

                LoginSuccess = 2;

                if (LoginSuccess == 2)
                {
                    if (Conn.State == ConnectionState.Closed)
                        Conn.Open();

                    if (Bv > 0)
                    {
                        string sessn = "";
                        string q = "select Isnull(Max(Sessid),Convert(Varchar,Getdate(),112)) as Sessid from D_Sessnmaster ";
                        using (SqlCommand Comm = new SqlCommand(q, Conn))
                        {
                            using (SqlDataReader Dr = Comm.ExecuteReader())
                            {
                                if (Dr.Read())
                                {
                                    sessn = Dr["SessId"].ToString();
                                }
                            }
                        }

                        string str = "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,Actype,VType,sessid,WSessID) "
                            + "Select ISNULL(MAX(VoucherNo)+1,1001),Cast(Convert(varchar,Getdate(),106) as DateTime),'0','" + FormNo + "','" + amount.ToString() + "','" + WalletName + "  Credited " + Remarks + " against transaction no." + TxnID + "',"
                            + "'" + TxnID + "','" + WalletType + "','C', convert(varchar,cast(cast(getdate() as varchar) as datetime),112),'1' FROM TrnVoucher";

                        using (SqlCommand Comm = new SqlCommand(str, Conn))
                        {
                            Comm.ExecuteNonQuery();
                        }

                        string s = "select Max(VoucherNo) as VoucherNo from TrnVoucher ";
                        string voucherNo = "";
                        using (SqlCommand Comm = new SqlCommand(s, Conn))
                        {
                            using (SqlDataReader Dr = Comm.ExecuteReader())
                            {
                                if (Dr.Read())
                                {
                                    voucherNo = Dr["VoucherNo"].ToString();
                                }
                            }
                        }

                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"OK\",\"refundamount\":\"" + (finalamount).ToString() + "\",\"refundamountusdt\":\"" + amount.ToString() + "\",\"voucherno\":\"" + voucherNo + "\",\"msg\": \"success\",\"wallettype\": \"" + WalletType + "\"}");
                    }
                    else
                    {
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"refundamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\",\"wallettype\": \"" + WalletType + "\"}");
                    }
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"refundamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Login failed\",\"wallettype\": \"" + WalletType + "\"}");
                }


            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"refundamountusdt\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\",\"wallettype\": \"" + WalletType + "\"}");
        }
    }

    private void ConfirmVoucher(string VoucherRequest, string VoucherResponse, string WalletType)
    {
        try
        {
            using (SqlConnection Conn = new SqlConnection(constr.ToString()))
            {
                Conn.Open();
                double AvailBal = 0;
                int i = 0;
                double BalanceINR = 0;

                if (VoucherResponse.ToUpper() == "OK")
                {
                    int FormNo = GetFormNo(Request["UserName"], Request["Password"]);
                    using (SqlCommand Comm = new SqlCommand("UPDATE TrnVoucher SET Narration = Narration + '" + "" + "-" + VoucherResponse + "' WHERE VoucherNo = '" + VoucherRequest + "' AND Drto = '" + FormNo + "' AND Actype = '" + WalletType + "'", Conn))
                    {
                        i = Comm.ExecuteNonQuery();
                    }

                    if (FormNo > 0)
                    {
                        AvailBal = Convert.ToDouble(GetBalance(FormNo.ToString(), WalletType));
                    }

                    if (i > 0)
                    {
                        Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"OK\",\"ewallet\":\"" + AvailBal + "\",\"wallettype\":\"" + WalletType + "\"}");
                    }
                    else
                    {
                        Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\",\"wallettype\":\"" + WalletType + "\"}");
                    }
                }
                else if (VoucherResponse.ToUpper() == "FAILED")
                {
                    int FormNo = GetFormNo(Request["UserName"], Request["Password"]);
                    using (SqlCommand Comm = new SqlCommand("INSERT INTO TempTrnVoucher (VoucherId, VoucherNo, VoucherDate, DrTo, Crto, Amount, Narration, Refno, Actype, RecTimeStamp, VType, Sessid, WSessid) " +
                                                          "SELECT VoucherId, VoucherNo, VoucherDate, DrTo, Crto, Amount, Narration + '" + "" + "-" + VoucherResponse + "', RefNo, Actype, RecTimeStamp, VType, Sessid, WSessid " +
                                                          "FROM TrnVoucher WHERE VoucherNo = '" + VoucherRequest + "'", Conn))
                    {
                        Comm.ExecuteNonQuery();
                    }

                    using (SqlCommand Comm = new SqlCommand("DELETE FROM TrnVoucher WHERE VoucherNo = '" + VoucherRequest + "' AND Drto = '" + FormNo + "' AND Actype = '" + WalletType + "'", Conn))
                    {
                        i = Comm.ExecuteNonQuery();
                    }

                    if (FormNo > 0)
                    {
                        AvailBal = Convert.ToDouble(GetBalance(FormNo.ToString(), WalletType));
                    }

                    Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\",\"wallettype\":\"" + WalletType + "\"}");
                }
                Conn.Close();
            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"response\":\"FAILED\",\"ewallet\":\"0\"}");
        }
    }
    private void checkvoucher(string VoucherRequest, string VoucherResponse)
    {
        try
        {
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            Conn.Open();
            double AvailBal = 0;
            int i = 0;
            if (VoucherResponse.ToUpper() == "OK")
            {
                int FormNo = GetFormNo(Request["UserName"], Request["Password"]);
                Comm = new SqlCommand("Update TrnVoucher Set Narration=Narration +'" + "" + "-" + VoucherResponse + "' where VoucherNo='" + VoucherRequest + "' and Drto='" + FormNo + "' and Actype='" + Session["RWalletType"] + "'", Conn);
                i = Comm.ExecuteNonQuery();

                if (FormNo > 0)
                    AvailBal = Convert.ToDouble(GetSWalletBalance(Convert.ToString(FormNo)));
                if (i > 0)
                    Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"OK\",\"ewallet\":\"" + AvailBal + "\",\"wallettype\":\"" + Session["RWalletType"] + "\"}");
                else
                    Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
            }
            else if (VoucherResponse.ToUpper() == "FAILED")
            {
                int FormNo = GetFormNo(Request["UserName"], Request["Password"]);
                Comm = new SqlCommand("Insert into TempTrnVoucher (VoucherId,VoucherNo,VoucherDate,DrTo,Crto,Amount, Narration,Refno,Actype,RecTimeStamp,VType,Sessid,WSessid)select VoucherId,VoucherNo," + " VoucherDate,DrTo,Crto,Amount,Narration +'" + "" + "-" + VoucherResponse + "',RefNo,Actype,RecTimeStamp,VType,Sessid,WSessid from TrnVoucher where VoucherNo='" + VoucherRequest + "'", Conn);
                Comm.ExecuteNonQuery();
                Comm = new SqlCommand("Delete from TrnVoucher where VoucherNo='" + VoucherRequest + "' and Drto='" + FormNo + "' and Actype='" + Session["RWalletType"] + "'", Conn);
                Comm.ExecuteNonQuery();

                if (FormNo > 0)
                    AvailBal = Convert.ToDouble(GetSWalletBalance(Convert.ToString(FormNo)));
                Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
            }
            Conn.Close();
        }
        catch (Exception ex)
        {
            Response.Write("{\"response\":\"FAILED\";\"ewallet\":\"0\"}");
        }
    }
    private void CheckInfo()
    {
        try
        {
            using (SqlConnection Conn = new SqlConnection(constr.ToString()))
            {
                Conn.Open();
                string rwallet = "";
                string ismovie = "N";
                using (SqlCommand Comm = new SqlCommand(
                    "Exec ProcCheckInfoINR '" +
                    Request["Username"] + "','" +
                    Request["Password"]
                        .Replace("%25", "%")
                        .Replace("%23", "#")
                        .Replace("%26", "&")
                        .Replace("%22", "'")
                        .Replace("%40", "@") + "'", Conn))
                {
                    using (SqlDataReader Dr = Comm.ExecuteReader())
                    {
                        if (Dr.Read())
                        {
                            double BalanceINR = 0;
                            rwallet = Convert.ToString(GetBalance(Dr["FormNo"].ToString(), "A"));
                            ismovie = Dr["ismovie"].ToString();

                            if (Dr["Status"].ToString().ToUpper() == "BLOCK")
                            {
                                Response.Write("{\"formno\": \"\",\"loginid\": \"Invalid\",\"name\": \"Invalid Credentials\",\"doj\": \"\",\"email\": \"\",\"mobileno\": \"\",\"city\": \"\",\"isactive\": \"\",\"kitid\":\"\",\"kitname\":\"\",\"kitstatus\":\"\",\"status\": \"Block\",\"awallet\": \"0\",\"ismovie\":\"\",\"activedate\":\"\",\"kitamount\":\"0\",\"isholiday\":\"\",\"shoppoint\":\"\",\"promoid\":\"\",\"coupon\":\"\",\"promovalue\":\"0\"}");
                            }
                            else
                            {
                                Response.Write("{\"formno\": \"" + Dr["FormNo"] + "\",\"loginid\": \"" + Dr["IDNo"] + "\",\"name\": \"" + Dr["MemName"] +
                                "\",\"doj\": \"" + Dr["JoinDate"] + "\",\"email\": \"" + Dr["Email"] + "\",\"mobileno\": \"" + Dr["Mobl"] +
                                "\",\"city\": \"" + Dr["City"] + "\",\"isactive\": \"" + Dr["ActiveStatus"] + "\",\"kitid\":\"" + Dr["KitId"] +
                                "\",\"kitname\":\"" + Dr["KitName"] + "\",\"kitstatus\":\"" + Dr["KitStatus"] + "\",\"status\": \"" + Dr["STATUS1"] +
                                "\",\"awallet\": \"" + rwallet + "\",\"ismovie\":\"" + ismovie +
                                "\",\"activedate\":\"" + Dr["DOA"] + "\",\"kitamount\":\"" + Dr["KitAmount"] + "\",\"isholiday\":\"" + Dr["isholiday"] +
                                "\",\"shoppoint\":\"" + Dr["shoppoint"] + "\",\"promoid\":\"" + Dr["promoid"] + "\",\"coupon\":\"" + Dr["Coupon"] +
                                "\",\"promovalue\":\"0\"}");
                            }
                        }
                        else
                        {
                            Response.Write("{\"formno\": \"0\",\"loginid\": \"" + Request["Username"] + "\",\"name\": \"Invalid Credentials\",\"doj\": \"\",\"email\": \"\",\"mobileno\": \"0\",\"city\": \"\",\"isactive\": \"\",\"kitid\":\"0\",\"kitname\":\"\",\"kitstatus\":\"\",\"status\": \"FAIL\",\"rwallet\":\"0\",\"ewallet\":\"0\",\"ismovie\":\"" + ismovie + "\",\"activedate\":\"\",\"kitamount\":\"0\",\"isholiday\":\"\"}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"formno\": \"0\",\"loginid\": \"Invalid\",\"name\": \"Invalid Credentials\",\"doj\": \"\",\"email\": \"\",\"mobileno\": \"0\",\"city\": \"\",\"isactive\": \"\",\"kitid\":\"0\",\"kitname\":\"\",\"kitstatus\":\"\",\"status\": \"FAIL\",\"rwallet\":\"0\",\"ewallet\":\"0\"}");
        }
    }
    private void AddBV() //Through recharge API; 16Apr16, NJ
    {
        try
        {
            objAccess = new DAL();
            string Uname = Request["Username"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
            string Pwd = Request["Password"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
            string TxnData = Request["TxnData"].Trim().Replace("'", "").Replace("=", "");
            int LoginSuccess = 0;
            int FormNo = 0;
            using (SqlConnection Conn = new SqlConnection(constr.ToString()))
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();
                string isblock = "N";
                string str = "Select * FROM M_MemberMaster WHERE IDNo = '" + Uname + "'";
                using (SqlCommand Comm = new SqlCommand(str, Conn))
                {
                    using (SqlDataReader Dr = Comm.ExecuteReader())
                    {
                        if (Dr.Read())
                        {
                            LoginSuccess = 1;
                            FormNo = Convert.ToInt32(Dr["FormNo"]);
                            isblock = Dr["IsBlock"].ToString();
                        }
                    }
                }
                if (isblock.ToString() == "Y")
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Login failed.\"}");
                }
                string[] TData = TxnData.Split(';');
                string TxnID, Remarks = "";
                double Bv = 0;

                if (TData.Length == 3)
                {
                    TxnID = TData[0].Trim();
                    Bv = Convert.ToDouble(TData[1]);
                    Remarks = TData[2].Trim();
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Insufficient Data.\"}");
                    return;
                }

                if (LoginSuccess == 1)
                {
                    string strCheck = "Exec Sp_TrnAddBVINR '" + TxnID + "','" + Bv + "','CR'";
                    DataTable dtCheck = objAccess.GetData(strCheck);

                    if (Convert.ToInt32(dtCheck.Rows[0]["ID"]) > 0)
                    {
                        if (Conn.State == ConnectionState.Closed)
                            Conn.Open();

                        str = "INSERT INTO RepurchIncomeINR (SessID, FormNo, BillNo, BillDate, RepurchIncome, Imported, BillType, SoldBy, Msessid, Remarks, DSessID) " +
                              "VALUES (1, '" + FormNo + "', '" + TxnID + "', CAST(CONVERT(varchar, GetDate(), 106) AS DateTime), '" + Bv + "', 'N', 'R', 'WR', 1, '" + Remarks + TxnID + "', CONVERT(Varchar, GetDate(), 112))";

                        using (SqlCommand insertComm = new SqlCommand(str, Conn))
                        {
                            insertComm.ExecuteNonQuery();
                        }

                        Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"success\"}");
                    }
                    else
                    {
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Order No. Already Exists\"}");
                    }
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Login failed.\"}");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"loginid\": \"" + Request["Username"] + "\",\"msg\": \"Failed.\"}");
        }
    }
    private void DrBV() //Through recharge API; 16Apr16, NJ
    {
        try
        {
            objAccess = new DAL();
            string Uname = Request["Username"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
            string Pwd = Request["Password"].Trim().Replace("'", "").Replace("=", "").Replace(";", "");
            string TxnData = Request["TxnData"].Trim().Replace("'", "").Replace("=", "");

            int LoginSuccess = 0;
            int FormNo = 0;
            using (SqlConnection Conn = new SqlConnection(constr.ToString()))
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                //string str = "Select * FROM M_MemberMaster WHERE IDNo='" + Uname + "'";
                //using (SqlCommand Comm = new SqlCommand(str, Conn))
                //{
                //    using (SqlDataReader Dr = Comm.ExecuteReader())
                //    {
                //        if (Dr.Read())
                //        {
                //            LoginSuccess = 1;
                //            FormNo = Convert.ToInt32(Dr["FormNo"]);
                //        }
                //    }
                //}
                string isblock = "N";
                string str = "Select * FROM M_MemberMaster WHERE IDNo = '" + Uname + "'";
                using (SqlCommand Comm = new SqlCommand(str, Conn))
                {
                    using (SqlDataReader Dr = Comm.ExecuteReader())
                    {
                        if (Dr.Read())
                        {
                            LoginSuccess = 1;
                            FormNo = Convert.ToInt32(Dr["FormNo"]);
                            isblock = Dr["IsBlock"].ToString();
                        }
                    }
                }
                if (isblock.ToString() == "Y")
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Login failed.\"}");
                }
                string[] TData = TxnData.Split(';');
                string TxnID, Remarks = "";
                double Bv = 0;

                if (TData.Length == 3)
                {
                    TxnID = TData[0].Trim();
                    Bv = Convert.ToDouble(TData[1]);
                    Remarks = TData[2].Trim();
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Insufficient Data.\"}");
                    return;
                }

                if (LoginSuccess == 1)
                {
                    if (Conn.State == ConnectionState.Closed)
                        Conn.Open();

                    // Calculate Bv - (Bv * 2)
                    double repurchIncome = Bv - (Bv * 2);

                    string str_Rid1 = "Select sum(Repurchincome) As Amount FROM RepurchIncomeINR WHERE FormNo = '" + FormNo + "' AND BillNo = '" + TxnID + "'";
                    DataTable Dt1 = new DataTable();
                    Dt1 = SqlHelper.ExecuteDataset(Conn, CommandType.Text, str_Rid1).Tables[0];
                    decimal amount = Convert.ToDecimal(Dt1.Rows[0]["Amount"]);
                    decimal userAmount = Convert.ToDecimal(Bv); // Assuming TxnAmount is the amount the user provides

                    if (amount == 0)
                    {
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Same bill no already refunded Amount.\"}");
                        return;
                    }
                    else if (amount >= userAmount)
                    {
                        str = "INSERT INTO RepurchIncomeINR (SessID, FormNo, BillNo, BillDate, RepurchIncome, Imported, BillType, SoldBy, Msessid, Remarks, DSessID) " +
                        "VALUES (1, '" + FormNo + "', '" + TxnID + "', CAST(CONVERT(varchar, GETDATE(), 106) AS DateTime), '" + repurchIncome + "', 'N', 'R', 'WR', 1, '" + Remarks + TxnID + "', CONVERT(Varchar, GETDATE(), 112))";

                        using (SqlCommand insertComm = new SqlCommand(str, Conn))
                        {
                            int y = insertComm.ExecuteNonQuery();
                            if (y > 0)
                            {
                                Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"success\"}");
                            }
                            else
                            {
                                Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Failed\"}");
                            }
                        }
                    }
                    else
                    {
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Amount mismatch, please check the amount.\"}");
                        return;
                    }
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Login failed.\"}");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"loginid\": \"" + Request["Username"] + "\",\"msg\": \"Failed.\"}");
        }
    }
    private int GetFormNo(string Uname, string Pwd)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string str = "Select * FROM M_MemberMaster WHERE IDNo = '" + Uname + "' AND Passw = '" + Pwd + "'";
        ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str);
        dt = ds.Tables[0];
        int FormNo = 0;
        string isblock = "N";
        if (dt.Rows.Count > 0)
        {
            FormNo = Convert.ToInt32(dt.Rows[0]["FormNo"]);
            isblock = dt.Rows[0]["IsBlock"].ToString();
        }
        if (isblock.ToString() == "Y")
        {
            FormNo = 0;
        }
        return FormNo;
    }
    private int GetFormNo_1(string Uname)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string str = "Select * FROM M_MemberMaster WHERE IDNo='" + Uname + "'";
        ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str);
        dt = ds.Tables[0];
        int FormNo = 0;
        string isblock = "N";
        if (dt.Rows.Count > 0)
        {
            FormNo = Convert.ToInt32(dt.Rows[0]["FormNo"]);
            isblock = dt.Rows[0]["IsBlock"].ToString();
        }
        if (isblock.ToString() == "Y")
        {
            FormNo = 0;
        }
        return FormNo;
    }
    private void checkEvoucher(string VoucherRequest, string VoucherResponse)
    {
        try
        {
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            Conn.Open();
            double AvailBal = 0;
            int i = 0;
            if (VoucherResponse.ToUpper() == "OK")
            {
                int FormNo = GetFormNo(Request["UserName"], Request["Password"]);
                //Comm = new SqlCommand("Update TrnVoucher Set Narration=Narration +'" + "" + "-" + VoucherResponse + "' where VoucherNo='" + VoucherRequest + "' and Drto='" + FormNo + "' and Actype='" + Session["MWalletType"] + "'", Conn);
                //i = Comm.ExecuteNonQuery();
                string str = "";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                str = "Update TrnVoucher Set Narration=Narration +'\" + \"\" + \"-\" + VoucherResponse + \"' where VoucherNo='\" + VoucherRequest + \"' and Drto='\" + FormNo + \"' and Actype='\" + Session[\"MWalletType\"] + \"'";
                ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str);
                dt = ds.Tables[0];
                if (FormNo > 0)
                    AvailBal = Convert.ToDouble(GetMWalletBalance(Convert.ToString((FormNo))));
                //if (i > 0)
                if (dt.Rows.Count > 0)
                {
                    Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"OK\",\"ewallet\":\"" + AvailBal + "\",\"wallettype\":\"" + Session["MWalletType"] + "\"}");
                }
                else
                    Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
            }
            else if (VoucherResponse.ToUpper() == "FAILED")
            {
                int FormNo = GetFormNo(Request["UserName"], Request["Password"]);
                //Comm = new SqlCommand("Insert into TempTrnVoucher (VoucherId,VoucherNo,VoucherDate,DrTo,Crto,Amount, Narration,Refno,Actype,RecTimeStamp,VType,Sessid,WSessid)select VoucherId,VoucherNo," + " VoucherDate,DrTo,Crto,Amount,Narration +'" + "" + "-" + VoucherResponse + "',RefNo,Actype,RecTimeStamp,VType,Sessid,WSessid from TrnVoucher where VoucherNo='" + VoucherRequest + "'", Conn);
                //Comm.ExecuteNonQuery();
                //Comm = new SqlCommand("Delete from TrnVoucher where VoucherNo='" + VoucherRequest + "' and Drto='" + FormNo + "' and Actype='" + Session("MWalletType") + "'", Conn);
                //i = Comm.ExecuteNonQuery();
                string str1 = "";
                string str2 = "";
                DataSet ds3 = new DataSet();
                DataTable dt3 = new DataTable();
                DataSet ds4 = new DataSet();
                DataTable dt4 = new DataTable();

                str1 = "Insert into TempTrnVoucher (VoucherId,VoucherNo,VoucherDate,DrTo,Crto,Amount, Narration,Refno,Actype,RecTimeStamp,VType,Sessid,WSessid)select VoucherId,VoucherNo,\" + \" VoucherDate,DrTo,Crto,Amount,Narration +'\" + \"\" + \"-\" + VoucherResponse + \"',RefNo,Actype,RecTimeStamp,VType,Sessid,WSessid from TrnVoucher where VoucherNo='\" + VoucherRequest + \"'";
                ds3 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str1);
                dt3 = ds3.Tables[0];

                str2 = "Delete from TrnVoucher where VoucherNo='\" + VoucherRequest + \"' and Drto='\" + FormNo + \"' and Actype='\" + Session[\"MWalletType\"] + \"'";
                ds4 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str2);
                dt4 = ds4.Tables[0];

                if (FormNo > 0)
                    AvailBal = Convert.ToDouble(GetMWalletBalance(Convert.ToString(FormNo)));

                Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
            }
            Conn.Close();
        }
        catch (Exception ex)
        {
            Response.Write("{\"response\":\"FAILED\";\"ewallet\":\"0\"}");
        }
    }
    private void DeductWallet() // 29Aug16,NJ
    {
        string VoucherNo = "";
        string Uname = Replace(Replace(Replace(Request["Username"], "'", ""), "=", ""), ";", "");
        string Pwd = Replace(Replace(Replace(Request["Password"], "'", ""), "=", ""), ";", "");
        string TxnData = Replace(Replace(Request["TxnData"], "'", ""), "=", "");
        try
        {
            var LoginSuccess = 0;
            int FormNo = 0;
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            // Dim str As String = "Select * FROM M_MemberMaster WHERE IDNo='" & Uname & "' AND Passw='" & Pwd & "'"
            // Comm = New SqlCommand(str, Conn)
            // Dim Dr As SqlDataReader = Comm.ExecuteReader()
            // If Dr.Read Then
            // LoginSuccess = 1
            // FormNo = Dr("FormNo")
            // End If
            // Dr.Close()
            FormNo = GetFormNo(Uname, Pwd);
            if (FormNo > 0)
                LoginSuccess = 1;
            else
            {
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Invalid User\",\"wallettype\": \"" + Session["RWalletType"] + "\"}");
                return;
            }
            //string[] TData = TxnData.Split(";");
            string[] separators1 = new[] { ";" };
            string[] TData = TxnData.Split(separators1, StringSplitOptions.None);
            string TxnID, Remarks;
            double Bv;
            if (TData.Length == 3)
            {
                TxnID = Convert.ToString(TData[0]);
                Bv = Convert.ToDouble(TData[1]);
                Remarks = Convert.ToString(TData[2]);
            }
            else
            {
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Insufficient Data\",\"wallettype\": \"" + Session["RWalletType"] + "\"}");
                return;
            }
            double AvailBal = Convert.ToDouble(GetSWalletBalance(Convert.ToString(FormNo)));
            if (AvailBal < Bv & LoginSuccess == 1)
            {
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Available Balance is " + AvailBal + "\",\"wallettype\": \"" + Session["RWalletType"] + "\"}");
                return;
            }
            else
                LoginSuccess = 2;
            if (LoginSuccess == 2)
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();
                if (Bv > 0)
                {
                    string sessn = "";
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    string q = "select Isnull(Max(Sessid),Convert(Varchar,Getdate(),112)) as Sessid from D_Sessnmaster ";
                    ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, q);
                    dt = ds.Tables[0];

                    //Comm = new SqlCommand(q, Conn);
                    //SqlDataReader Dr = Comm.ExecuteReader();
                    // Dim FormNo As Integer = "0"
                    //if (Dr.Read)
                    if (dt.Rows.Count > 0)
                    {
                        sessn = dt.Rows[0]["SessId"].ToString();
                    }
                    //Dr.Close();



                    DataSet ds2 = new DataSet();
                    DataTable dt2 = new DataTable();
                    DataSet ds3 = new DataSet();
                    DataTable dt3 = new DataTable();
                    str = "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo   ,Actype,VType ,sessid,WSessID) " + " Select ISNULL(MAX(VoucherNo)+1,1001),Cast(Convert(varchar,Getdate(),106) as DateTime),'" + FormNo + "','0','" + Bv + "','R Wallet Used by " + Remarks + " against order no." + TxnID + "'," + " '" + TxnID + "','" + Session["RWalletType"] + "','D', convert(varchar,cast(cast(getdate() as varchar) as datetime),112),'" + Session["CurrentSessn"] + "' FROM TrnVoucher";
                    ds2 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str);
                    dt2 = ds2.Tables[0];
                    //Comm = new SqlCommand(str, Conn);
                    //Comm.ExecuteNonQuery();

                    string s = "select Max(VoucherNo)as VoucherNo from TrnVoucher ";
                    //Comm = new SqlCommand(s, Conn);
                    //Dr = Comm.ExecuteReader();

                    //if (Dr.Read)
                    ds3 = SqlHelper.ExecuteDataset(constr, CommandType.Text, s);
                    dt3 = ds3.Tables[0];
                    if (dt3.Rows.Count > 0)
                    {
                        VoucherNo = dt3.Rows[0]["VoucherNo"].ToString();
                    }
                    //Dr.Close();
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"OK\",\"deductamount\":\"" + Bv + "\",\"voucherno\":\"" + VoucherNo + "\",\"msg\": \"success\",\"wallettype\": \"" + Session["RWalletType"] + "\"}");
                }
                else
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\",\"wallettype\": \"" + Session["RWalletType"] + "\"}");
            }
            else
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Login failed\",\"wallettype\": \"" + Session["RWalletType"] + "\"}");
        }
        catch (Exception ex)
        {
            Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\",\"wallettype\": \"" + Session["RWalletType"] + "\"}");
        }
    }
    private double GetSWalletBalance(string FormNo)
    {
        double RtrVal = 0;
        try
        {
            string str1 = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //SqlDataReader Dr;
            //Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            //Conn.Open();
            //Comm = new SqlCommand("Select balance From dbo.ufnGetBalance('" + FormNo + "','" + Session("RWalletType") + "')", Conn);
            str1 = "Select balance From dbo.ufnGetBalance('" + FormNo + "','" + Session["RWalletType"] + "'";
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str1);
            dt = ds.Tables[0];
            //Dr = Comm.ExecuteReader;
            //if (Dr.Read == true)
            if (dt.Rows.Count > 0)
            {
                RtrVal = Convert.ToInt32(dt.Rows[0]["Balance"]);
            }
            //Dr.Close();
            //Comm.Cancel();
            //Conn.Close();
            return RtrVal;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        return RtrVal;
    }
    private double GetMWalletBalance(string FormNo)
    {
        double RtrVal = 0;
        try
        {

            //SqlDataReader Dr;
            //Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            //Conn.Open();
            //Comm = new SqlCommand("Select balance From dbo.ufnGetBalance('" + FormNo + "','" + Session("MWalletType") + "')", Conn);
            //Dr = Comm.ExecuteReader;
            //if (Dr.Read == true)

            string str1 = "Select balance From dbo.ufnGetBalance('" + FormNo + "','" + Session["MWalletType"] + "')";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str1);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                RtrVal = Convert.ToInt32(dt.Rows[0]["Balance"]);
            }
            //Dr.Close();
            //Comm.Cancel();
            //Conn.Close();
            return RtrVal;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        return RtrVal;
    }
    private double GetRewardWalletBalance(string FormNo)
    {
        double RtrVal = 0;
        try
        {

            //SqlDataReader Dr;
            //Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            //Conn.Open();
            //Comm = new SqlCommand("Select balance From dbo.ufnGetBalance('" + FormNo + "','S')", Conn);
            //Dr = Comm.ExecuteReader;
            //if (Dr.Read == true)
            string str1 = "Select balance From dbo.ufnGetBalance('" + FormNo + "','S')";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str1);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                RtrVal = Convert.ToInt32(Dt.Rows[0]["Balance"]);
            }
            //Dr.Close();
            //Comm.Cancel();
            //Conn.Close();
            return RtrVal;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        return RtrVal;
    }
    private double GetPointWalletBalance(string FormNo)
    {
        double RtrVal = 0;
        try
        {

            //SqlDataReader Dr;
            //Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            //Conn.Open();
            //Comm = new SqlCommand("Select balance From dbo.ufnGetBalance('" + FormNo + "','P')", Conn);
            //Dr = Comm.ExecuteReader;
            //if (Dr.Read == true)
            string str1 = "Select balance From dbo.ufnGetBalance('" + FormNo + "','P')";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str1);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                RtrVal = Convert.ToInt32(Dt.Rows[0]["Balance"]);
            }
            //Dr.Close();
            //Comm.Cancel();
            //Conn.Close();
            return RtrVal;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        return RtrVal;
    }
    private void DeductMainWallet() // 29Aug16,NJ
    {
        string VoucherNo = "";
        string Uname = Replace(Replace(Replace(Request["Username"], "'", ""), "=", ""), ";", "");
        string Pwd = Replace(Replace(Replace(Request["Password"], "'", ""), "=", ""), ";", "");
        string TxnData = Replace(Replace(Request["TxnData"], "'", ""), "=", "");
        try
        {
            var LoginSuccess = 0;
            int FormNo = 0;
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            // Dim str As String = "Select * FROM M_MemberMaster WHERE IDNo='" & Uname & "' AND Passw='" & Pwd & "'"
            // Comm = New SqlCommand(str, Conn)
            // Dim Dr As SqlDataReader = Comm.ExecuteReader()
            // If Dr.Read Then
            // LoginSuccess = 1
            // FormNo = Dr("FormNo")
            // End If
            // Dr.Close()
            FormNo = GetFormNo(Uname, Pwd);
            if (FormNo > 0)
                LoginSuccess = 1;
            else
            {
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Invalid User\"}");
                return;
            }
            //string[] TData = TxnData.Split(";");
            string[] separators1 = new[] { ";" };
            string[] TData = TxnData.Split(separators1, StringSplitOptions.None);
            string TxnID, Remarks;
            double Bv;
            int i = 0;
            if (TData.Length == 3)
            {
                TxnID = Convert.ToString(TData[0]);
                Bv = Convert.ToDouble(TData[1]);
                Remarks = Convert.ToString(TData[2]);
            }
            else
            {
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Insufficient Data\",\"wallettype\": \"" + Session["MWalletType"] + "\"}");
                return;
            }
            double AvailBal = Convert.ToDouble(GetMWalletBalance(Convert.ToString(FormNo)));
            if (AvailBal < Bv & LoginSuccess == 1)
            {
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Available Balance is " + AvailBal + "\",\"wallettype\": \"" + Session["MWalletType"] + "\"}");
                return;
            }
            else
                LoginSuccess = 2;
            if (LoginSuccess == 2)
            {
                //if (Conn.State == ConnectionState.Closed)
                //    Conn.Open();
                if (Bv > 0)
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    string sessn = "";
                    string q = "select  Isnull(Max(Sessid),Convert(Varchar,Getdate(),112)) as Sessid from D_Sessnmaster ";
                    ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, q);
                    dt = ds.Tables[0];
                    //Comm = new SqlCommand(q, Conn);
                    //SqlDataReader Dr = Comm.ExecuteReader();
                    //// Dim FormNo As Integer = "0"
                    //if (Dr.Read)
                    if (dt.Rows.Count > 0)
                    {
                        sessn = dt.Rows[0]["SessId"].ToString();
                    }
                    //Dr.Close();



                    str = "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo   ,Actype,VType ,sessid,WSessID) " + " Select ISNULL(MAX(VoucherNo)+1,1001),Cast(Convert(varchar,Getdate(),106) as DateTime),'" + FormNo + "','0','" + Bv + "','E Wallet Used by " + Remarks + " against order no." + TxnID + "'," + " '" + TxnID + "','" + Session["MWalletType"] + "','D', convert(varchar,cast(cast(getdate() as varchar) as datetime),112),'" + Session["CurrentSessn"] + "' FROM TrnVoucher";
                    DataSet ds1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    ds1 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str);
                    dt1 = ds1.Tables[0];



                    //Comm = new SqlCommand(str, Conn);
                    //i = Comm.ExecuteNonQuery();
                    //if (i > 0)
                    if (dt.Rows.Count > 0)
                    {
                        DataSet ds2 = new DataSet();
                        DataTable dt2 = new DataTable();
                        string s = "select Max(VoucherNo)as VoucherNo from TrnVoucher ";
                        ds2 = SqlHelper.ExecuteDataset(constr, CommandType.Text, s);
                        dt2 = ds2.Tables[0];
                        //Comm = new SqlCommand(s, Conn);
                        //Dr = Comm.ExecuteReader();
                        //if (Dr.Read)
                        if (dt2.Rows.Count > 0)
                        {
                            VoucherNo = dt2.Rows[0]["VoucherNo"].ToString();
                        }
                        //Dr.Close();
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"OK\",\"deductamount\":\"" + Bv + "\",\"voucherno\":\"" + VoucherNo + "\",\"msg\": \"success\",\"wallettype\": \"" + Session["MWalletType"] + "\"}");
                    }
                    else
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\",\"wallettype\": \"" + Session["MWalletType"] + "\"}");
                }
                else
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\",\"wallettype\": \"" + Session["MWalletType"] + "\"}");
            }
            else
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Login failed\",\"wallettype\": \"" + Session["MWalletType"] + "\"}");
        }
        catch (Exception ex)
        {
            Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\",\"wallettype\": \"" + Session["MWalletType"] + "\"}");
        }
    }
    private string SaveIntoDB(string Referral, string Name, string Mobile, string EmailID, string NomineeName, string Relation)
    {
        string _Output = "";
        try
        {
            string strQry = "";
            DataTable Dt = new DataTable();
            string dblDistrict, dblTehsil, IfSC, dblPlan, JoinStatus, Category;
            int SessID, dblState, dblBank, InVoiceNo, KitID;
            double Bv, Rp;
            char cGender, cMarried;
            cGender = 'M';
            cMarried = 'N';
            string HostIp = Context.Request.UserHostAddress.ToString();
            string _Response = "";

            string LastId = "";
            try
            {
                strQry = " Exec sp_Register '" + Referral + "','" + Name + "','" + Mobile + "','" + EmailID + "','" + NomineeName + "','" + Relation + "'";
                Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, strQry).Tables[0];
                if ((Dt.Rows[0]["Status"].ToString().ToUpper() == "FAILED"))
                    _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"" + Dt.Rows[0]["Result"].ToString() + "\"}";
                else if ((Dt.Rows[0]["Status"].ToString().ToUpper() == "SUCCESS"))
                {
                    _Output = "{\"response\":\"OK\",\"msg\":\"" + Dt.Rows[0]["Result"].ToString() + "\",";
                    _Output += "\"idno\":\"" + Dt.Rows[0]["Idno"].ToString() + "\", ";
                    _Output += "\"Formno\":\"" + Dt.Rows[0]["Formno"].ToString() + "\", ";
                    _Output += "\"name\":\"" + Dt.Rows[0]["MemfirstName"].ToString() + "\", ";
                    _Output += "\"mobile\":\"" + Dt.Rows[0]["Mobl"].ToString() + "\", ";
                    _Output += "\"email\":\"" + Dt.Rows[0]["email"].ToString() + "\", ";
                    _Output += "\"Passw\":\"" + Dt.Rows[0]["Passw"].ToString() + "\" ";
                    _Output += "}";

                    sendSMS(Convert.ToString(Dt.Rows[0]["Idno"]), Convert.ToString(Dt.Rows[0]["MemfirstName"]), Convert.ToString(Dt.Rows[0]["Passw"]), Convert.ToString(Dt.Rows[0]["Mobl"]));
                    SendToMemberMail(Convert.ToString(Dt.Rows[0]["Idno"]), Convert.ToString(Dt.Rows[0]["email"]), Convert.ToString(Dt.Rows[0]["MemfirstName"]), Convert.ToString(Dt.Rows[0]["Passw"]));
                }
                else
                    _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"Please contact to admin.\"}";
            }
            catch (Exception e)
            {
                _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"" + e.Message + "\"}";
            }
        }

        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"" + ex.Message + "\"}";
        }
        Response.Write(_Output);
        return _Output;
    }
    private string IdActivaton(string IDno, string Count, string OrderNo)
    {
        string _Output = "";
        try
        {
            string strQry = "";
            DataTable Dt = new DataTable();
            string HostIp = Context.Request.UserHostAddress.ToString();
            string _Response = "";
            string LastId = "";
            try
            {
                strQry = " Exec Sp_AppActivateMember '" + IDno + "','" + Count + "','" + OrderNo + "'";
                Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, strQry).Tables[0];
                if ((Dt.Rows[0]["Result"].ToString().ToUpper() == "FAILED"))
                    _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"" + Dt.Rows[0]["Msg"].ToString() + "\"}";
                else if ((Dt.Rows[0]["Result"].ToString().ToUpper() == "SUCCESS"))
                {
                    _Output = "{\"response\":\"OK\",\"msg\":\"" + Dt.Rows[0]["Msg"].ToString() + "\"";
                    _Output += "}";
                }
                else
                    _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"Please contact to admin.\"}";
            }
            catch (Exception e)
            {
                _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"" + e.Message + "\"}";
            }
        }

        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"" + ex.Message + "\"}";
        }
        Response.Write(_Output);
        return _Output;
    }
    private void AddWalletAmount_New() // 29Aug16,NJ
    {
        string VoucherNo = "";
        string Uname = Replace(Replace(Replace(Request["Username"], "'", ""), "=", ""), ";", "");
        string Pwd = Replace(Replace(Replace(Request["Password"], "'", ""), "=", ""), ";", "");
        string TxnData = Replace(Replace(Request["TxnData"], "'", ""), "=", "");
        try
        {
            var LoginSuccess = 0;
            int FormNo = 0;
            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            // Dim str As String = "Select * FROM M_MemberMaster WHERE IDNo='" & Uname & "' AND Passw='" & Pwd & "'"
            // Comm = New SqlCommand(str, Conn)
            // Dim Dr As SqlDataReader = Comm.ExecuteReader()
            // If Dr.Read Then
            // LoginSuccess = 1
            // FormNo = Dr("FormNo")
            // End If
            // Dr.Close()
            FormNo = GetFormNo(Uname, Pwd);
            if (FormNo > 0)
                LoginSuccess = 1;
            else
            {
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"addamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Invalid User\"}");
                return;
            }

            //string[] TData = TxnData.Split(";");
            string[] separators1 = new[] { ";" };
            string[] TData = TxnData.Split(separators1, StringSplitOptions.None);
            string TxnID, Remarks;
            double Bv;
            if (TData.Length == 3)
            {
                TxnID = Convert.ToString(TData[0]);
                Bv = Convert.ToDouble(TData[1]);
                Remarks = Convert.ToString(TData[2]);
            }
            else
            {
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"addamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Insufficient Data\"}");
                return;
            }
            // Dim AvailBal As Double = Val(GetBalance(FormNo, WalletType))
            // If AvailBal < Bv And LoginSuccess = 1 Then
            // Response.Write("{""loginid"": """ & Uname & """,""response"":""FAILED"",""deductamount"":""0"",""voucherno"":""0"",""msg"": ""Available Balance is " & AvailBal & """,""wallettype"": """ & WalletType & """}")
            // Exit Sub
            // Else
            // LoginSuccess = 2
            // End If
            // If LoginSuccess = 2 Then
            //if (Conn.State == ConnectionState.Closed)
            //    Conn.Open();
            if (Bv > 0)
            {
                // Dim sessn As String = ""
                // Dim q As String = "select Isnull(Max(Sessid),Convert(Varchar,Getdate(),112)) as Sessid from D_Sessnmaster "
                // Comm = New SqlCommand(q, Conn)
                SqlDataReader Dr;
                // = Comm.ExecuteReader()
                // Dim FormNo As Integer = "0"
                // If Dr.Read Then
                // sessn = Dr("SessId")
                // End If
                // Dr.Close()
                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();

                str = "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo   ,Actype,VType ,sessid,WSessID) " + " Select ISNULL(MAX(VoucherNo)+1,1001),Cast(Convert(varchar,Getdate(),106) as DateTime),'0','" + FormNo + "','" + Bv + "'," + " 'Shoppe Wallet Credited by " + Remarks + " against " + TxnID + "'," + " '" + TxnID + "','R','C', convert(varchar,cast(cast(getdate() as varchar) as datetime),112)," + " '" + Session["CurrentSessn"] + "' FROM TrnVoucher";
                ds1 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str);
                dt1 = ds1.Tables[0];

                //Comm = new SqlCommand(str, Conn);
                //Comm.ExecuteNonQuery();
                DataSet ds2 = new DataSet();
                DataTable dt2 = new DataTable();
                string s = "select Max(VoucherNo)as VoucherNo from TrnVoucher ";
                ds2 = SqlHelper.ExecuteDataset(constr, CommandType.Text, s);
                dt2 = ds2.Tables[0];

                //Comm = new SqlCommand(s, Conn);
                //Dr = Comm.ExecuteReader();

                //if (Dr.Read)
                if (dt2.Rows.Count > 0)
                {
                    VoucherNo = dt2.Rows[0]["VoucherNo"].ToString();
                }
                //Dr.Close();
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"OK\",\"addamount\":\"" + Bv + "\",\"voucherno\":\"" + VoucherNo + "\",\"msg\": \"success\"}");
            }
            else
                Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"addamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Login failed\"}");
        }
        catch (Exception ex)
        {
            Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"addamount\":\"0\",\"voucherno\":\"0\",\"msg\": \"Failed\"}");
        }
    }
    public string FillState()
    {
        string _output = "";
        try
        {
            SqlDataAdapter adp;
            DataTable Dt = new DataTable();


            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            Conn.Open();
            _output = "{\"states\": [";
            adp = new SqlDataAdapter("Select * FROM M_stateDivMaster WHERE ActiveStatus='Y' order by StateName  ", Conn);
            adp.Fill(Dt);
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow Dr in Dt.Rows)
                    _output = _output + "{\"statecode\":\"" + Dr["StateCode"] + "\",\"statename\":\"" + Dr["StateName"] + "\"},";
                _output = _output.Remove(_output.Length - 1, 1);

                _output = _output + "],\"response\":\"OK\"}";
                Comm.Cancel();
            }
            else
                _output = "{\"response\":\"FAILED\",\"msg\":\"No State Found \"}";

            Comm.Cancel();
        }
        catch (Exception ex)
        {
            _output = "{\"response\":\"FAILED\",\"msg:\"" + ex.Message + "\"}";
        }
        Response.Write(_output);
        return _output;
    }
    public string FillBank()
    {
        string _output = "";
        try
        {
            SqlDataAdapter adp;
            DataTable Dt = new DataTable();


            Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            Conn.Open();
            _output = "{\"bank\": [";
            adp = new SqlDataAdapter("SELECT BankCode as Bid,BANKNAME as Bank FROM M_BankMaster WHERE ACTIVESTATUS='Y' ORDER BY BANKname  ", Conn);
            adp.Fill(Dt);
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow Dr in Dt.Rows)
                    _output = _output + "{\"bankcode\":\"" + Dr["Bid"] + "\",\"bankname\":\"" + Dr["Bank"] + "\"},";
                _output = _output.Remove(_output.Length - 1, 1);

                _output = _output + "],\"response\":\"OK\"}";
                Comm.Cancel();
            }
            else
                _output = "{\"response\":\"FAILED\",\"msg\":\"No Bank Found \"}";

            Comm.Cancel();
        }
        catch (Exception ex)
        {
            _output = "{\"response\":\"FAILED\",\"msg:\"" + ex.Message + "\"}";
        }
        Response.Write(_output);
        return _output;
    }
    public string Validate_(string Referral, string Sponsor, string Side)
    {
        //SqlDataReader Dread;
        //Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
        //Conn.Open();
        // Checking If Entered Referral Id Exists Or Not
        //Comm = new SqlCommand("Select FormNo,MemFirstName + ' ' + MemLastName as MemName  from M_MemberMaster where  Idno='" + Referral + "' AND (Formno in (Select FormnoDwn FROM M_MemTreeRelation) OR MID=1) ", Conn); // ActiveStatus='Y' Condition Removed on 28Feb17;linking to M_MemTreeRelation on 28Feb17
        //Dread = Comm.ExecuteReader;
        //if (Dread.Read == false)
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        string str = "Select FormNo,MemFirstName + ' ' + MemLastName as MemName  from M_MemberMaster where  Idno='" + Referral + "' AND (Formno in (Select FormnoDwn FROM M_MemTreeRelation) OR MID=1) ";
        ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str);
        dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {
            return "Invalid Referral ID.";
        }
        if (dt.Rows.Count > 0)
        {
            _RefFormNo = dt.Rows[0]["FormNo"].ToString();
        }
        //Dread.Close();
        //Comm.Cancel();

        string _IsGetExtreme = "N";

        // Checking If Entered Referral Id Exists Or Not
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();

        //Comm = new SqlCommand("select * from M_ConfigMaster ", Conn);
        //Dread = Comm.ExecuteReader;
        //if (Dread.Read == true)
        string str1 = "select * from M_ConfigMaster";
        ds1 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str1);
        dt1 = ds1.Tables[0];
        if (dt1.Rows.Count > 0)
        {

            _IsGetExtreme = dt1.Rows[0]["IsGetExtreme"].ToString();
        }
        //Dread.Close();
        //Comm.Cancel();

        if (_IsGetExtreme == "N")
        {
            DataSet ds2 = new DataSet();
            DataTable dt2 = new DataTable();
            // Checking If Entered Sponsor Id Exists Or Not
            //Comm = new SqlCommand("Select FormNo,MemFirstName + ' ' + MemLastName as MemName  from M_MemberMaster where Idno='" + Sponsor + "'", Conn);
            //Dread = Comm.ExecuteReader;
            //if (Dread.Read == false)
            string str2 = "Select FormNo,MemFirstName + ' ' + MemLastName as MemName  from M_MemberMaster where Idno='" + Sponsor + "'";
            ds2 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str2);
            dt2 = ds2.Tables[0];
            if (dt2.Rows.Count == 0)
            {
                return "Invalid Sponsor ID.";
            }
            if (dt2.Rows.Count > 0)
            {
                _UpLnFormNo = dt2.Rows[0]["FormNo"].ToString();
            }
            //Dread.Close();
            //Comm.Cancel();

            // Checking If Entered Side Valid Or Not
            DataSet ds3 = new DataSet();
            DataTable dt3 = new DataTable();

            //Comm = new SqlCommand("SELECT COUNT(*) AS CNT From M_MemberMaster WHERE UpLnFormNo in (Select FormNo From M_MemberMaster Where IDNo='" + Sponsor + "') And Legno = " + Side + "", Conn);
            //Dread = Comm.ExecuteReader;
            //if (Dread.Read == false)
            string str3 = "SELECT COUNT(*) AS CNT From M_MemberMaster WHERE UpLnFormNo in (Select FormNo From M_MemberMaster Where IDNo='" + Sponsor + "') And Legno = " + Side + "";
            ds3 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str3);
            dt3 = ds3.Tables[0];
            if (dt3.Rows.Count == 0)
            {
                return "Selected Side Not Available.";
            }
            else if (Convert.ToInt32(dt2.Rows[0]["CNT"]) >= 1)
            {
                return "Selected Side Not Available.";
            }
            //Dread.Close();
            //Comm.Cancel();


            // Checking If Entered Sponsor ID Exists In Referral Downline Or Not
            if (_RefFormNo != _UpLnFormNo)
            {
                DataSet ds4 = new DataSet();
                DataTable dt4 = new DataTable();
                //Comm = new SqlCommand("Select * from M_MemTreeRelation where FormNo=" + _RefFormNo + " And FormNoDwn=" + _UpLnFormNo + "", Conn);
                //Dread = Comm.ExecuteReader;
                //if (Dread.Read == false)
                string str4 = "Select * from M_MemTreeRelation where FormNo=" + _RefFormNo + " And FormNoDwn=" + _UpLnFormNo + "";
                ds4 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str4);
                dt4 = ds4.Tables[0];
                if (dt4.Rows.Count == 0)
                {
                    return "Sponsor does not exist in referral downline.";
                }
                //Dread.Close();
                //Comm.Cancel();
            }
        }

        return "OK";
    }
    private void sendSMS(string MSG, string Mobl, string Idno)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        //SqlCommand cmd;
        //SqlDataReader dread;
        //cmd = new SqlCommand("Select *,DATEADD(day, 15, Doj) as Maxdate from m_MemberMaster where IDNo = '" + Idno + "'", Conn);
        //dread = cmd.ExecuteReader;
        //if (dread.Read == true)
        string str = "Select *,DATEADD(day, 15, Doj) as Maxdate from m_MemberMaster where IDNo = '" + Idno + "'";
        ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, str);
        dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            Session["SMSIDNo"] = dt.Rows[0]["IDNo"];
            Session["SMSIDPass"] = dt.Rows[0]["Passw"];
            Session["Name"] = dt.Rows[0]["MemFirstName"];
            Session["MaxDate"] = dt.Rows[0]["MaxDate"];
        }
        //dread.Close();

        if ((Mobl).Length >= 10)
        {
            WebClient client = new WebClient();
            string baseurl;
            Stream data;
            // Dim sms As String = "Welcome " & Session("Name") & ", Thank you for Registration. Your login id is " & Session("SMSIDNo") & " and password is " & Session("SMSIDPass") & ". Best of luck, Regards: " & Session("CompName") & ""
            string Sms = "Congratulations " + Session["Name"] + "! Welcome to " + Session["CompName"] + ". Your login details are ID: " + Session["SMSIDNo"] + "/Login Pwd:" + Session["SMSIDPass"] + "/Trans Pwd:" + Session["SMSIDPass"] + "Visit" + Session["CompWeb"] + "for more details.";
            try
            {
                // baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & txtMobileNo.Text & "&msg=" & sms & ""
                // baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & txtMobileNo.Text & "&msg=" & Sms & ""
                baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" + Session["SmsId"] + "&pwd=" + Session["SmsPass"] + "&Message=" + Sms + "&Contacts=" + Mobl + "&SenderId=" + Session["ClientId"] + "";
                data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s;
                s = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                //MsgBox(ex.Message);
            }
            string sms1 = "Dear " + Session["Name"] + " (" + Session["SMSIDNo"] + "), Kindly submit scan copy of your Joining form, KYC, PAN copy and cancelled cheque on kyc@dreamtouchindia.com till " + Session["MaxDate"] + "";
            try
            {
                // baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & txtMobileNo.Text & "&msg=" & sms1 & ""
                baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" + Session["SmsId"] + "&pwd=" + Session["SmsPass"] + "&Message=" + sms1 + "&Contacts=" + Mobl + "&SenderId=" + Session["ClientId"] + "";

                data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s;
                s = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                //MsgBox(ex.Message);
            }
        }

        //dread.Close();
    }
    private void sendSMS(string Idno, string Name, string Passw, string mobile)
    {
        try
        {
            if ((mobile).Length >= 10)
            {
                WebClient client = new WebClient();
                string baseurl;
                Stream data;
                // Dim Sms As String = " Welcome to " & Session("CompName") & ". Your login details are ID: " & Session("SMSIDNo") & "/Login Pwd:" & MemberPass & "/Trans Pwd:" & MemberTransPassw & " Visit " & Session("CompWeb1") & " for more details."
                // Welcome To *, Thank You For Registration.Your ID Is * and Password is *. Visit .* Best of luck.
                // 'Dim sms As String = "Welcome To " & Session("CompName") & ", Thank You For Registration.Your ID Is " & Idno & " and Password is " & Passw & ". Visit " & Session("CompWeb1") & "  Best of luck."

                // 'Dim sms As String = "Welcome To Triple Seven Enterprise, We are glad to have you in our Family. Your User ID is " & Idno & " and Password and Transaction is " & Passw & ". Visit www.tripleseven.life / www.tripleseven.world Godspeed : Triple Seven"
                string sms = "";
                sms = "Welcome To Digtal Expo, We are glad to have you in our Family. Your User ID is " + Idno + " and Password and Transaction is " + Passw + ". Visit www.tripleseven.life / www.tripleseven.world Godspeed : Triple Seven";
                try
                {
                    baseurl = "http://www.apiconnecto.com/API/SMSHttp.aspx?UserId=" + Session["SmsId"] + "&pwd=" + Session["SmsPass"] + "&Message=" + sms + "&Contacts=" + mobile + "&SenderId=" + Session["ClientId"];
                    // baseurl = "http://103.233.79.246//submitsms.jsp?user=" & Session("SmsId") & "&key=5d0f10e5faXX&mobile=" & txtMobileNo.Text.Trim & "&message=" & sms & "&senderid=" & Session("ClientId") & "&accusage=1"
                    data = client.OpenRead(baseurl);
                    StreamReader reader = new StreamReader(data);
                    string s;
                    s = reader.ReadToEnd();
                    data.Close();
                    reader.Close();
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Password)
    {
        try
        {
            DataTable dt;
            string sql = "";
            string userEmail = "";


            string StrMsg = "";
            System.Net.Mail.MailAddress SendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress SendTo = new System.Net.Mail.MailAddress(Email);
            System.Net.Mail.MailMessage MyMessage = new System.Net.Mail.MailMessage(SendFrom, SendTo);
            StrMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\"> " + "<tr>" + "<td>" + "Welcome To Triple Seven Enterprise, We are glad to have you in our Family.<br />" + "Your User ID is " + IdNo + " and Password and Transaction is  " + Password + ". <br />" + "Visit https://tripleseven.life / https://tripleseven.world <br /> Godspeed : Triple Seven" + "</td>" + "</tr>" + "</table>";

            MyMessage.Subject = "Welcome and Congratulations!";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());
            smtp.Send(MyMessage);
            return true;
        }

        catch (Exception ex)
        {
            // Response.Write(ex.Message)
            Response.Write("Try later.");
        }
        return true;
    }
}
