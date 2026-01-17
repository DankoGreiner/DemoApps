<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DemoApps.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" href="data:image/svg+xml,<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 100 100'><text y='0.9em' font-size='90'>📓</text></svg>">
    <title>Demo apps</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <center>

                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                        </td>
                    </tr>
                </table>

            </center>

            <br />
            <br />
            Choose you app
            <hr />
            <dl>
                <dt>
                    <a href="Acquire/Default.aspx">Acquire</a>
                </dt>
                <dd>Description for Acquire</dd>

                <dt>
                    <a href="Obveze/Default.aspx">Obveze</a>
                </dt>
                <dd>Description for Obveze</dd>

                <dt>
                    <a href="Apps/FileManager/FileManager.aspx">FileManager</a>
                </dt>
                <dd>Description for FileManager</dd>

            </dl>
        </div>
    </form>
</body>
</html>
