<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Forgot.aspx.cs" Inherits="Forgot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::<%=Session["Title"]%>::</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta http-equiv="X-UA-Compatible" content="chrome=1" />
    <meta http-equiv="X-UA-Compatible" content="IE=8;FF=3;OtherUA=4" />
      <link href="css/bootstrap.min.css" rel="stylesheet">
</head>
<body style="font-family: Arial; font-weight: bold; font-size: 15px; text-align: left">
    <form id="form1" runat="server">
        <div>
            <table cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td valign="middle" align="center">
                <table class="rounded_colhead" cellspacing="0" cellpadding="0" width="500" border="0">
                    <tbody>
                        <tr>
                            <td valign="top" align="left" colspan="2">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td style="height: 35px" class="PanelTitle" valign="middle" align="left" width="8%">
                                                <%--<img height="150" src="img/forgot.gif" width="150" />--%>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td style="width: 250px" class="textBP" valign="middle" align="right">
                                                        </td>
                                                        <td style="width: 285px" valign="middle" align="left">
                                                        </td>
                                                        <td valign="top" align="center" rowspan="7">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 250px; height: 30px;" class="textBP" valign="middle" align="center">
                                                            <b>&nbsp;User ID</b>
                                                        </td>
                                                        <td style="width: 285px; height: 30px;" valign="middle" align="left">
                                                            <label>
                                                                <asp:TextBox ID="txtIDNo" runat="server" class="textboxBG" Width="200px" 
                                                                MaxLength="15"></asp:TextBox>
                                                            </label>
                                                            <asp:RequiredFieldValidator ID="RequireIDNo" runat="server" 
                                                                ControlToValidate="txtIDNo" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                        <tr>
                                                        <td style="width: 250px" class="textBP" valign="middle" height="30" width="250px"
                                                            align="center">
                                                            <b>&nbsp;Email Id</b>
                                                        </td>
                                                        <td style="width: 285px" valign="middle" align="left">
                                                            <label>
                                                                <asp:TextBox ID="txtemail" runat="server" class="textboxBG"
                                                                    Width="200px"></asp:TextBox></label>
                                                            <asp:RequiredFieldValidator ID="Requiredemail" runat="server" 
                                                                ControlToValidate="txtemail" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                 <asp:RegularExpressionValidator ID="EmailExpressionValidator" runat="server" ControlToValidate="txtemail"
                                                    ErrorMessage="Enter Valid Email ID!" Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    SetFocusOnError ="true" ValidationGroup="eInformation"></asp:RegularExpressionValidator>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 250px" valign="bottom" align="left">
                                                            &nbsp;</td>
                                                        <td style="padding-right: 73px; width: 285px" valign="middle" align="left">
                                                            <asp:Button ID="Submit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="Submit_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>                                                        
                                                        <td align="center" colspan="2">
                                                         
                                                            &nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
        </div>
    </form>
</body>
</html>
