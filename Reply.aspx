<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reply.aspx.cs" Inherits="Reply" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
 <link rel="stylesheet" type="text/css" href="assets/default-tables.css">
    <link rel="stylesheet" type="text/css" href="assets/datatable.css">
</head>
<body>
    <form id="form1" runat="server">
    <div class="form-group ">
    <h4  >Reply</h4>
</div>
<div class="form-group"><label for="inputdefault"> Complaint type : </label> 
    <asp:label class="form-control"  ID="LblCType"  runat="server"></asp:label>
  </div>
    <div class="form-group"><label for="inputdefault"> Complaint: </label>
    <asp:TextBox class="form-control" ID="TxtComplaint" ReadOnly="true" TextMode="MultiLine"  runat="server"></asp:TextBox>
    </div>
          <div class="form-group"><label for="inputdefault">Previous Reply</label> 
       <asp:TextBox class="form-control" ID="TxtPreReply" ReadOnly="true" TextMode="MultiLine"  runat="server"></asp:TextBox>
   
   
    </div>
    </form>
</body>
</html>