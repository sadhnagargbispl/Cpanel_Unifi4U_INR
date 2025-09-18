using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Numerics;


public partial class NewTree : System.Web.UI.Page
{
    private string strQuery;
    private int minDeptLevel;
    private SqlConnection conn = new SqlConnection();
    private SqlCommand Comm = new SqlCommand();
    private SqlDataAdapter Adp1;
    private DataSet dsGetQry = new DataSet();
    private string strDrawKit;
    private DAL obj = new DAL();
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string scrname = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            conn.Open();
            if (!Page.IsPostBack)
            {
                if (Session["Status"] != null && (string)Session["Status"] == "OK")
                    // Call getKits()
                    ValidateTree();
                else
                    Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            //string path = HttpContext.Current.Request.Url.AbsoluteUri;
            //string text = path + ":  " + Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);
            //obj.WriteToFile(text + ex.Message);
            //Response.Write("Try later.");
            //throw new Exception(ex.Message);
            Response.Redirect("Logout.aspx");
        }
    }

    //private void get_FormNo(string IDNo)
    private int get_FormNo(int IDNo)
    {
        // conn = New SqlConnection(Application("Connect"))
        // conn.Open()
        int FormNo = 0;
        try
        {
          
            //SqlDataReader dr;
            //Comm = new SqlCommand("Select FormNo From M_MemberMaster Where IDNo='" + IDNo + "'", conn);
            //dr = Comm.ExecuteReader;
            //if (dr.Read == true)
            //    FormNo = dr("FormNo");
            //dr.Close();
            //Comm.Cancel();
            //// conn.Close()
            ////return FormNo;
            DataTable dt = new DataTable();
            string strSql = "Select FormNo From M_MemberMaster Where IDNo='" + Convert.ToString (IDNo) + "'";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            dt = ds.Tables[0];
            if (dt.Rows.Count> 0)
            {
                FormNo = Convert.ToInt32(dt.Rows[0]["FormNo"].ToString ());  
            }
        }
        catch (Exception ex)
        {
            //string path = HttpContext.Current.Request.Url.AbsoluteUri;
            //string text = path + ":  " + Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);
            //obj.WriteToFile(text + ex.Message);
            //Response.Write("Try later.");
        }
        return FormNo;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string scrname = "";
            int DownFormNo = get_FormNo(Convert.ToInt32 (DownLineFormNo.Value));
            if (DownFormNo != 0)
                Response.Redirect("NewTree.aspx?DownLineFormNo=" + DownFormNo);
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Invalid distributor id');" + "</SCRIPT>";
                this.RegisterStartupScript("MyAlert", scrname);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cmdBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Index.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnStepAbove_Click(object sender, EventArgs e)
    {
        if (Session["Upliner"].ToString() == null & Session["Upliner"].ToString() != "0" & Session["MemUpliner"] != Session["Upliner"])
        {
            string uplnformno = Session["Upliner"].ToString();

            Response.Redirect("NewTree.aspx?DownLineFormNo=" + uplnformno);
        }
        else if (Session["MemUpliner"] == Session["Upliner"])
        {
            Response.Redirect("NewTree.aspx?DownLineFormNo=" + Session["Formno"]);
            Response.Write("Sorry!! You can't see your upliner tree.");
            Response.End();
        }
    }
    private void ValidateTree()
    {
        try
        {
            string strSelectedFormNo="";
            minDeptLevel = 5;
            if ((System.Convert.ToString(Session["FormNO"]) == "" | string.IsNullOrEmpty(Request["DownLineFormNo"]).ToString() == ""))
                strSelectedFormNo = Session["FORMNO"].ToString();
            else if ((Session["Formno"].ToString() == string.IsNullOrEmpty(Request["DownlineFormno"]).ToString()))
                strSelectedFormNo = Session["FORMNO"].ToString();
            else if ((Session["MemUpliner"] != Session["Upliner"]) | (Session["Formno"].ToString() != string.IsNullOrEmpty(Request["DownlineFormno"]).ToString()))
            {
                if (CheckDownLineMemberTree() == false)
                {
                    //Response.Write("Please Check DownLine Member ID");
                    //Response.End();
                    scrname = "<SCRIPT language='javascript'>alert('Please Check DownLine Member ID');location.replace('index.aspx');</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
                else
                    //strSelectedFormNo = Request["DownLineFormNo"];
                    if (Request["DownlineFormno"] == null)
                {
                    strSelectedFormNo = Session["FORMNO"].ToString();
                }
                else
                {
                    strSelectedFormNo = Request["DownLineFormNo"];
                }
                   
            }
            else if ((Session["Formno"] == Session["Upliner"]) | (Session["MemUpliner"] == Session["Upliner"]))
            {
                // BtnStepAbove.Enabled = False
                //Response.Write("Sorry!! You can't see your upliner tree.");
                //Response.End();
                scrname = "<SCRIPT language='javascript'>alert('Sorry!! You can't see your upliner tree.');location.replace('index.aspx');</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            else
            {
                if (CheckDownLineMemberTree() == false)
                {
                    Response.Write("Please Check DownLine Member ID");
                    Response.End();
                }
                strSelectedFormNo = Request["DownLineFormNo"];
            }

            strQuery = getQuery(strSelectedFormNo, minDeptLevel);
            GenerateTree(strQuery);
        }
        catch (Exception ex)
        {
            //string path = HttpContext.Current.Request.Url.AbsoluteUri;
            //string text = path + ":  " + Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);
            //Obj.WriteToFile(text + ex.Message);
            //Response.Write("Try later.");
        }
    }
    private bool CheckDownLineMemberTree()
    {
        bool chk = false;
        try
        {
            //CheckDownLineMemberTree = false;
            //strQuery = " Select FormnoDwn FROM M_MemTreeRelation WHERE  FormNoDwn=" + Request["DownLineFormNo"] + " AND  FormNo=" + Session["FORMNO"];
            if (Request["DownLineFormNo"] == null)
            {
                strQuery = " Select FormnoDwn FROM M_MemTreeRelation WHERE FormNo=" + Session["FORMNO"];

            }
            else
            {
                strQuery = " Select FormnoDwn FROM M_MemTreeRelation WHERE  FormNoDwn=" + Request["DownLineFormNo"] + " AND  FormNo=" + Session["FORMNO"];
            }

           
            DataTable dt;
            dt = new DataTable();
            dt = obj.GetData(strQuery);

            if (dt.Rows.Count <= 0)
                //CheckDownLineMemberTree = false;
                chk = false;
            else
                //CheckDownLineMemberTree = true;
                chk = true;
        }
        // ds1.Dispose()
        catch (Exception ex)
        {
            //string path = HttpContext.Current.Request.Url.AbsoluteUri;
            //string text = path + ":  " + Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);
            //obj.WriteToFile(text + ex.Message);
            //Response.Write("Try later.");
        }
        return chk;
    }
    private void GenerateTree(string strQuery)
    {
        try
        {


            // conn = New SqlConnection(Application("Connect"))
            // conn.Open()
            Comm = new SqlCommand(strQuery, conn);
            Comm.CommandTimeout = 100000000;
            Adp1 = new SqlDataAdapter(Comm);
            Adp1.Fill(dsGetQry);
            double ParentId;
            double FormNo;
            string MemberName;
            string LegNo;
            string Todayleftunit = "";
            string Todayrightunit = "";

            string Doj = "";
            string Category = "";
            double LeftBV, RightBV;
            double LeftJoining, RightJoining;
            string UpLiner, Sponsor;
            int level;
            string NodeName="", IdNo;
            string myRunTimeString = "";
            string ExpandYesNo;
            string strImageFile;
            string strUrlPath = "";
            string UpDt;
            int ActiveDirect, ActiveIndirect, TodayLeftactive, TodayRightActive, TodayLeftJoin, TodayRightJoin;
            double LeftCarryBv, RightCarryBv;
            string tooltipstrig;
            string Block = "";
            string BlockStatus = "";
            string status = "";
            string Target_ = "_self";
            //myRunTimeString = myRunTimeString + "<Script Language=Javascript>" + Constants.vbNewLine;
            myRunTimeString = myRunTimeString + "<Script Language=Javascript>" + Environment.NewLine;
            tooltipstrig = ToolTipTable();

            // --- Define Parent Setting ----------------
            ParentId = -1;

            if (Request["DownLineFormNo"] != "")
                FormNo = Convert.ToDouble(Request["DownLineFormNo"]);
            else
                FormNo = Convert.ToDouble(Session["FormNo"]);
            strImageFile = "img/base.jpg";
            int i = 0;
            // myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" & strImageFile & "');" + vbNewLine
            int LoopValue;
            string FolderFile = "img/Deactivate.jpg";

            foreach (DataRow dr in dsGetQry.Tables[0].Rows)
            {
                strImageFile = dr["JoinColor"].ToString();
                if (i == 0)
                {
                    if (Request["DownLineFormNo"] != "" & i == 0)
                        Session["Upliner"] = dr["uplinerformno"].ToString();
                    else
                        Session["Upliner"] = null;
                    myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" + strImageFile + "');" + Environment.NewLine;
                    i = i + 1;
                }
                // If Session("Formno") = FormNo Then


                // End If
                ParentId = Convert.ToDouble(dr["UPLNFORMNO"].ToString());
                FormNo = Convert.ToDouble(dr["FormNoDwn"].ToString());
                LegNo = dr["legno"].ToString();
                UpLiner = dr["UpLiner"].ToString();
                Sponsor = dr["Sponsor"].ToString();
                Doj = dr["doj"].ToString();
                Category = dr["Category"].ToString();
                LeftBV = Convert.ToDouble(dr["LeftBV"].ToString());
                RightBV = Convert.ToDouble(dr["rightBV"].ToString());
                LeftJoining = Convert.ToDouble(dr["Leftjoining"].ToString());
                RightJoining = Convert.ToDouble(dr["rightjoining"].ToString());
                ActiveDirect = Convert.ToInt32(dr["LeftActive"].ToString());
                ActiveIndirect = Convert.ToInt32(dr["RightActive"].ToString());
                //IdNo = dr["Formno"].ToString();
                IdNo = dr["Formno"].ToString()  + "<br/>(" + dr["memName"] + ")";
                LeftCarryBv = Convert.ToDouble(dr["LeftCarryForwardBv"].ToString());
                RightCarryBv = Convert.ToDouble(dr["RightCarryForwardBv"].ToString());
                Block = dr["IsBlock"].ToString();
                BlockStatus = dr["BlockedStatus"].ToString();
                level = Convert.ToInt32(dr["level"].ToString());
                status = dr["idStatus"].ToString();
                strUrlPath = "NewJoining.aspx?DownLineFormNo=" + FormNo;
                Todayleftunit = "0";
                Todayrightunit = "0";
                TodayLeftactive = 0;
                TodayRightActive = 0;
                TodayLeftJoin = 0;
                TodayRightJoin = 0;
                // strUrlPath = "newtree.aspx?DownLineFormNo=" & FormNo
                UpDt = dr["UpDt"].ToString();
                MemberName = dr["Formno"].ToString();
                // NodeName = dr.Item("memName").ToString
                LoopValue = Convert.ToInt32 (dr["mlevel"]);

                if (LoopValue < 6 & LoopValue > 0)
                    ExpandYesNo = "true";
                else
                    ExpandYesNo = "false";
                if (ParentId == -1)
                    ExpandYesNo = "true";

                if (UpDt == "01 Jan 00")
                    UpDt = "";

                if (FormNo <= 0)
                {
                    // Target_ = "_blank"
                    // If dr("ActiveStatus") = "N" Then
                    // strImageFile = "img/deact.jpg"
                    // ElseIf dr("Kitid") = 1 Then
                    // strImageFile = "img/act.jpg"
                    // Else
                    // strImageFile = "img/deact.jpg"
                    // End If
                    MemberName = "Direct";
                    Target_ = "";
                    strUrlPath = "";
                    // strUrlPath = "" ''"newjoining.aspx?UpLnFormNo=" & ParentId & "&legno=" & LegNo
                    if (LegNo == "1")
                        MemberName = "Left";
                    else
                        MemberName = "Right";
                }
                else
                {
                    if (dr["ActiveStatus"].ToString() == "N")
                        strImageFile = "img/deact.jpg";
                    else if (Convert.ToInt32 (dr["Kitid"]) == 1)
                        strImageFile = "img/Red.jpg";
                    else if (Convert.ToInt32(dr["Kitid"]) == 2)
                        strImageFile = "img/Blue.jpg";
                    else if (Convert.ToInt32(dr["Kitid"]) == 3)
                        strImageFile = "img/Green.jpg";
                    else if (Convert.ToInt32(dr["Kitid"]) == 4)
                        strImageFile = "img/Yellow.jpg";
                    else if (Convert.ToInt32(dr["Kitid"]) == 5)
                        strImageFile = "img/Orange.jpg";
                    else if (Convert.ToInt32(dr["Kitid"]) == 6)
                        strImageFile = "img/purpel.jpg";
                    else
                        strImageFile = "img/empty.png";
                    Target_ = "";
                    strUrlPath = "newtree.aspx?DownLineFormNo=" + FormNo;
                }
                ExpandYesNo = "true";
                strImageFile = dr["JoinColor"].ToString();
                myRunTimeString = myRunTimeString + " mytree.add(" + FormNo + "," + ParentId + "," + "'" + Category + "'" + "," + "'" + Doj + "','" + MemberName + "','" + NodeName + "','" + UpLiner + "'" + ",'" + Sponsor + "'," + LeftBV + "," + RightBV + "," + Todayleftunit + "," + Todayrightunit + "," + ActiveDirect + "," + ActiveIndirect + "," + "'" + strUrlPath + "'" + ",'" + MemberName + "'," + "'" + Target_ + "'" + "," + "'" + strImageFile + "'" + "," + "'" + strImageFile + "'" + "," + ExpandYesNo + ",'" + LeftJoining + "','" + RightJoining + "','" + level + "' ,'" + UpDt + "','" + IdNo + "','" + LeftCarryBv + "','" + RightCarryBv + "','" + Block + "','" + BlockStatus + "','" + TodayLeftactive + "','" + TodayRightActive + "','" + TodayLeftJoin + "','" + TodayRightJoin + "','" + status + "');" + Environment.NewLine;
            }


            myRunTimeString = myRunTimeString + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + " document.write(mytree);" + Environment.NewLine;
            myRunTimeString = myRunTimeString + Environment.NewLine + "</script> " + "<br /> <br /> <br /> <br /> ";

            RegisterClientScriptBlock("clientScript", myRunTimeString);
        }
        catch (Exception ex)
        {
            //string path = HttpContext.Current.Request.Url.AbsoluteUri;
            //string text = path + ":  " + Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);
            //obj.WriteToFile(text + ex.Message);
            //Response.Write("Try later.");
        }
    }
    private string ToolTipTable()
    {
        string strToolTip;
        strToolTip = "onMouseOver=\"ddrivetip('<table width=100% border=0 cellpadding=5 cellspacing=1 bgcolor=#CCCCCC class=containtd>  <tr>     <td width=50% bgcolor=#999999><font color=#FFFFFF><strong>Member ID</strong></font></td>  </tr>  <tr>     <td>430</td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Name</strong></font></td>  </tr>  <tr>     <td>Mr-MAHESH BHARDWAJ </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Date of Joining</strong></font></td>  </tr>  <tr>     <td>2008-08-07 16:14:54 </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Total Status</strong></font></td>  </tr>  <tr>     <td>LEFT:123 , RIGHT:2198 </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Product</strong></font></td>  </tr>  <tr>     <td>CODE NO. 01-S.L. &nbsp;</td>  </tr></table>')\" onMouseOut=\"hideddrivetip()\"";
        return strToolTip;
    }
    private string getQuery(string strSelectedFormNo, int minDeptLevel)
    {
        // ---- check if user pass downline member than .. make according to downline member other wise show his tree
        try
        {
            //getQuery = "exec ShowBinaryTree " + strSelectedFormNo + "," + minDeptLevel;
            
        }
        catch (Exception ex)
        {
            //string path = HttpContext.Current.Request.Url.AbsoluteUri;
            //string text = path + ":  " + Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);
            //obj.WriteToFile(text + ex.Message);
            //Response.Write("Try later.");
        }
        return "exec ShowBinaryTree " + strSelectedFormNo + "," + minDeptLevel;
    }


}
