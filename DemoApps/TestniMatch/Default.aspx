<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DemoApps.TestniMatch.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test match</title>
    <link href="https://st.tenisliga.com/Images/FavIconBall.png" rel="icon" type="image/x-icon" />
</head>
<body style="font-family: Tahoma, Arial, Helvetica, sans-serif; font-size: 11px;">
    <form id="form1" runat="server">
        <div class="contentPanel">
            Termin:
            <asp:TextBox ID="txtTermin" runat="server" autocomplete="off"></asp:TextBox>
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
            <hr />

            <table>
                <tr>
                    <td valign="top">
                        <asp:GridView ID="gvPlayersSelected" runat="server" OnSelectedIndexChanged="gvPlayersSelected_SelectedIndexChanged" OnRowDataBound="gvPlayersSelected_RowDataBound" OnDataBound="gvPlayersSelected_DataBound">
                            <Columns>
                                <asp:CommandField SelectText="Select" ShowSelectButton="True"></asp:CommandField>
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnRaspored" runat="server" Text="Raspored" OnClick="btnRaspored_Click" Visible="false" />
                        <hr />
                        <asp:Panel runat="server" ID="panSelectedPlayer" GroupingText="Selected player" Visible="false" DefaultButton="btnChangeRedosljed">

                            <asp:Label ID="lblSelectedPlayerId" runat="server">

                            </asp:Label>
                            <br />
                            <asp:Label ID="lblSelectedPlayerName" runat="server"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtRedosljed" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                            <hr />
                            <asp:Button ID="btnChangeRedosljed" runat="server" Text="Redosljed" OnClick="btnChangeRedosljed_Click" />
                            <br />
                            <br />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />


                        </asp:Panel>

                        <asp:Label ID="lblMails" runat="server"></asp:Label>
                        <asp:GridView ID="gvMails" runat="server" ShowHeader="false"></asp:GridView>
                        <button id="btnGetMails" onclick="copyTableToString()" type="button">Copy Emails</button>

                        <hr />
                        <table>
                            <tr>
                                <td>
                                    <button type="button" onclick="copyAndHide()">Report</button>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkOpis" runat="server" Text="Opis" ClientIDMode="Static" onclick="copyAndHide()" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="tableCopyContainer"></div>
                        <br />
                        <button id="btnGetReport" onclick="copyTableToClipboard()" type="button">Copy Report</button>

                    </td>
                    <td>
                        <asp:GridView ID="gvPlayersAvalilable" runat="server" OnSelectedIndexChanged="gvPlayersAvalilable_SelectedIndexChanged">
                            <Columns>
                                <asp:CommandField SelectText="&nbsp;&lt;&lt;&nbsp;" ShowSelectButton="True"></asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>



        </div>
    </form>

    <script>
        function copyTableToString() {

            const table = document.getElementById('gvMails');
            const cells = table.querySelectorAll('td');
            const values = Array.from(cells).map(td => td.innerText.trim());
            const csv = values.join(', ');

            // Fallback method for HTTP or older browsers
            const temp = document.createElement('textarea');
            temp.value = csv;
            document.body.appendChild(temp);
            temp.select();

            try {
                document.execCommand('copy');
                //alert('Copied:\n' + csv);
            } catch (err) {
                alert('Copy failed: ' + err);
            }
            document.body.removeChild(temp);


            const btn = document.getElementById("btnGetMails");
            //if (!original || !btn) return;

            // 🕒 Set button text to current time
            const now = new Date();
            const timeStr = now.toLocaleTimeString([], { hour12: false }); // 24-hour format
            btn.textContent = timeStr;

        }
    </script>
    <script>
        function copyAndHide() {
            const original = document.getElementById("gvPlayersSelected");
            if (!original) return;

            // Clone the table deeply (structure + data)
            const clone = original.cloneNode(true);

            // 🆔 Append "Copy" to the table ID
            clone.id = original.id + "Copy";

            // 🧹 Remove the last row (if it exists)
            //if (clone.rows.length > 1) {
            //    clone.deleteRow(clone.rows.length - 1);
            //}

            // Always hide columns 1, 2, and 3 (0-based)
            const hiddenCols = [0, 1, 2, 5];

            // If chkOpis is NOT checked, also hide column 5
            const chkOpis = document.getElementById("chkOpis");
            if (!chkOpis.checked) hiddenCols.push(4);

            // Hide requested columns
            for (const row of clone.rows) {
                for (const i of hiddenCols.sort((a, b) => b - a)) {
                    if (row.cells[i]) row.cells[i].style.display = "none";
                }
            }

            // Place the copy in the container (replace any previous copy)
            const container = document.getElementById("tableCopyContainer");
            container.innerHTML = "";
            container.appendChild(clone);
        }
    </script>

    <script>

        function copyTableToClipboard() {
            const table = document.getElementById("gvPlayersSelectedCopy");
            if (!table) return;

            let text = "";
            for (const row of table.rows) {
                const visibleCells = [...row.cells]
                    .filter(cell => cell.style.display !== "none")
                    .map(cell => cell.innerText.trim());
                text += visibleCells.join("\t") + "\n";
            }

            // ✅ Modern browsers
            if (navigator.clipboard && window.isSecureContext) {
                navigator.clipboard.writeText(text);
            } else {
                // 🧩 Fallback for HTTP / old browsers (like IE)
                const temp = document.createElement("textarea");
                temp.value = text;
                document.body.appendChild(temp);
                temp.select();
                document.execCommand("copy");
                document.body.removeChild(temp);
            }

            alert("Table copied to clipboard!");
        }

    </script>

</body>
</html>
