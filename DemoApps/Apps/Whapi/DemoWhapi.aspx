<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemoWhapi.aspx.cs" Inherits="DemoApps.Apps.Whapi.DemoWhapi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnTestWhapi" runat="server" Text="Test" OnClick="btnTestWhapi_Click" />
            <br />
            <br />
            <asp:Label ID="lblTestMessage" runat="server"></asp:Label>

            <hr />

            <asp:Button ID="btnCreateGropu" runat="server" Text="CreateGroup" OnClick="btnCreateGropu_Click" />

        </div>
    </form>
</body>
</html>
