<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileManager.aspx.cs" Inherits="YourApp.FileManager.FileManagerPage" %>

<%@ Import Namespace="System.Web" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>File Manager</title>
    <link rel="icon" href="data:image/svg+xml,<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 100 100'><text y='0.9em' font-size='90'>📁</text></svg>">

    <style>
        body {
            font-family: Arial;
            margin: 16px;
        }

        .row {
            margin: 8px 0;
        }

        .muted {
            color: #777;
            font-size: 12px;
        }

        .small {
            width: 160px;
        }

        .msg {
            min-height: 38px; /* matches one line of text */
            line-height: 18px;
            padding: 10px;
            box-sizing: border-box;
            background: #f5f5f5;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
        }

            .grid th, .grid td {
                border: 1px solid #ddd;
                padding: 8px;
            }

            .grid th {
                background: #fafafa;
            }

        .mono {
            font-family: Consolas, monospace;
            font-size: 12px;
            word-break: break-all;
        }

        .icon {
            display: inline-block;
            width: 20px;
            text-align: center;
            font-size: 16px;
            line-height: 1;
        }

        .actions-row {
            display: flex;
            justify-content: space-between;
            align-items: flex-end;
            gap: 20px;
        }

        .actions-left {
            text-align: left;
        }

        .actions-right {
            text-align: right;
        }

        .build-time {
            position: absolute;
            top: 10px;
            right: 16px;
            font-size: 12px;
            color: #666;
            font-family: Consolas, monospace;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label runat="server" ID="lblBuildTime" CssClass="build-time" />


        <h2>File Manager</h2>
        <hr />

        <asp:Panel runat="server" ID="pnlMsg" CssClass="msg" Visible="true">
            <asp:Label runat="server" ID="lblMsg" />
        </asp:Panel>
        <hr />
        <div class="row actions-row">

            <!-- Upload (left) -->
            <div class="actions-left">
                <strong>Upload</strong><br />
                <asp:FileUpload runat="server" ID="fuUpload" />
                <asp:Button runat="server"
                    ID="btnUpload"
                    Text="Upload"
                    OnClick="btnUpload_Click"
                    Style="display: none;" />
            </div>

            <!-- Create folder (right) -->
            <div class="actions-right">
                <strong>Create folder</strong><br />
                <asp:TextBox runat="server"
                    ID="txtNewFolder"
                    CssClass="small"
                    Visible="false" />
                <asp:Button runat="server"
                    ID="btnCreateFolder"
                    Text="Create Folder"
                    OnClick="btnCreateFolder_Click" />
            </div>

        </div>


        <hr />

        <div class="row">
            <strong>Current:</strong>
            <asp:Label runat="server" ID="lblCurrentFolder" />
            &nbsp;
    <asp:Button runat="server" ID="btnUp" Text="Up" OnClick="btnUp_Click" Visible="false" />
        </div>

        <hr />

        <asp:GridView runat="server"
            ID="gvItems"
            CssClass="grid"
            AutoGenerateColumns="false"
            OnRowCommand="gvItems_RowCommand">

            <Columns>

                <%--<asp:BoundField DataField="Type" HeaderText="Type" />--%>


                <asp:TemplateField HeaderText="" ItemStyle-Width="32px" HeaderStyle-Width="32px">
                    <ItemTemplate>
                        <span class="icon">
                            <%# Eval("Type").ToString() == "Folder" ? "📁" : GetFileIcon(Eval("Name").ToString()) %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>



                <%-- NAME (clickable for folders) --%>

                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>

                        <!-- Folder: clickable -->
                        <asp:PlaceHolder runat="server" Visible='<%# Eval("Type").ToString() == "Folder" %>'>
                            <asp:HyperLink runat="server"
                                Text='<%# Eval("Name") %>'
                                NavigateUrl='<%# "FileManager.aspx?p=" + HttpUtility.UrlEncode(Eval("RelativePath").ToString()) %>' />
                        </asp:PlaceHolder>

                        <!-- File: plain text -->
                        <asp:PlaceHolder runat="server" Visible='<%# Eval("Type").ToString() == "File" %>'>
                            <%# Eval("Name") %>
                        </asp:PlaceHolder>

                    </ItemTemplate>
                </asp:TemplateField>

                <%-- URL (clickable for files) --%>

                <asp:TemplateField HeaderText="URL">
                    <ItemTemplate>

                        <asp:PlaceHolder runat="server" Visible='<%# Eval("Type").ToString() == "File" %>'>
                            <div class="mono">
                                <a href="<%# Eval("FullUrl") %>" target="_blank">
                                    <%# Eval("FullUrl") %>
                                </a>
                            </div>
                        </asp:PlaceHolder>

                        <asp:PlaceHolder runat="server" Visible='<%# Eval("Type").ToString() != "File" %>'>
                            <span class="muted">—</span>
                        </asp:PlaceHolder>

                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>

                        <%-- Folder actions --%>
                        <asp:PlaceHolder runat="server" Visible='<%# Eval("Type").ToString() == "Folder" %>'>
                            <asp:LinkButton runat="server"
                                CommandName="DeleteFolder"
                                CommandArgument='<%# Eval("RelativePath") %>'
                                Text="Delete folder"
                                OnClientClick="return confirm('Delete this folder? (Folder must be empty)');" />
                        </asp:PlaceHolder>

                        <%-- File actions --%>
                        <asp:PlaceHolder runat="server" Visible='<%# Eval("Type").ToString() == "File" %>'>

                            <asp:LinkButton runat="server"
                                CommandName="DeleteFile"
                                CommandArgument='<%# Eval("RelativePath") %>'
                                Text="Delete"
                                OnClientClick="return confirm('Delete this file?');" />

                            &nbsp;|&nbsp;

            <asp:PlaceHolder runat="server" Visible='<%# IsRenaming(Eval("RelativePath")) %>'>New name:
                <asp:TextBox runat="server" ID="txtRename" CssClass="small" />
                &nbsp;|&nbsp;
            </asp:PlaceHolder>

                            <asp:LinkButton runat="server"
                                CommandName="RenameToggle"
                                CommandArgument='<%# Eval("RelativePath") %>'
                                Text='<%# RenameButtonText(Eval("RelativePath")) %>' />
                        </asp:PlaceHolder>

                    </ItemTemplate>
                </asp:TemplateField>


            </Columns>
        </asp:GridView>

    </form>

    <script>
        (function () {
            var fu = document.getElementById('<%= fuUpload.ClientID %>');
            var btn = document.getElementById('<%= btnUpload.ClientID %>');

            if (!fu || !btn) return;

            fu.addEventListener('change', function () {
                btn.style.display = fu.files && fu.files.length > 0 ? '' : 'none';
            });
        })();
    </script>

</body>
</html>
