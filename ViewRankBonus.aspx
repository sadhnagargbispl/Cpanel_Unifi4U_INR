<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewRankBonus.aspx.cs" Inherits="ViewRankBonus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="LblNo" runat="server" ForeColor="Black" Font-Size="6px"></asp:Label>
    </div>
    <div style="margin-bottom: 10px; padding-right: 20px; padding-top: 20px">
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="5px"
            GridLines="Both" AllowPaging="true" class="table datatable" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" Font-Size="12px" Width="95%" ShowHeader="true"
            PageSize="50" EmptyDataText="No data to display.">
            <Columns>
                <asp:TemplateField HeaderText="S.No.">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1%>.
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
