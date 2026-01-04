<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileManager.aspx.cs" Inherits="YourApp.FileManager.FileManagerPage" %>

<%@ Import Namespace="System.Web" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>File Manager</title>
    <style>
        body {
            font-family: Arial;
            margin: 16px;
        }

        .row {
            margin: 8px 0;
        }

        .msg {
            padding: 8px;
            margin: 8px 0;
            border: 1px solid #ddd;
        }

        table {
            border-collapse: collapse;
            width: 100%;
            margin-top: 12px;
        }

        th, td {
            border: 1px solid #ddd;
            padding: 8px;
            vertical-align: top;
        }

        th {
            background: #f6f6f6;
            text-align: left;
        }

        .muted {
            color: #777;
        }

        input[type=text] {
            width: 320px;
        }

        .small {
            width: 180px;
        }

        a {
            word-break: break-all;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <h2>File Manager (~/FileManager/Files)</h2>

        <asp:Panel runat="server" ID="pnlMsg" Visible="false" CssClass="msg">
            <asp:Label runat="server" ID="lblMsg" />
        </asp:Panel>

        <div class="row">
            <strong>Upload</strong><br />
            Target folder (relative to /FileManager/Files):
       
            <%--<asp:TextBox runat="server" ID="txtUploadFolder" CssClass="small" placeholder="e.g. invoices/2025" />--%>
            <br />
            <asp:FileUpload runat="server" ID="fuUpload" />
            <asp:Button runat="server" ID="btnUpload" Text="Upload" OnClick="btnUpload_Click" />
            <span class="muted">Uploads to /FileManager/Files/{folder}</span>
        </div>

        <div class="row">
       
            <asp:Button runat="server" ID="btnCreateFolder" Text="Create Folder" OnClick="btnCreateFolder_Click" />
            <asp:TextBox runat="server" ID="txtNewFolder" Visible="false" />

        </div>

        <hr />
        <div class="row">
            <strong>Current:</strong>
            <asp:Label runat="server" ID="lblCurrentFolder" />
            &nbsp;&nbsp;
    <asp:Button runat="server" ID="btnUp" Text="Up" OnClick="btnUp_Click" Visible="false" />
        </div>

        <hr />

        <asp:GridView runat="server"
            ID="gvItems"
            AutoGenerateColumns="false"
            OnRowCommand="gvItems_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Type" DataField="Type" />
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:PlaceHolder runat="server" Visible='<%# Eval("Type").ToString() == "Folder" %>'>
                            <a href='FileManager.aspx?p=<%# HttpUtility.UrlEncode(Eval("RelativePath").ToString()) %>'>
                                <%# Eval("Name") %>
                            </a>
                        </asp:PlaceHolder>

                        <asp:PlaceHolder runat="server" Visible='<%# Eval("Type").ToString() == "File" %>'>
                            <%# Eval("Name") %>
                        </asp:PlaceHolder>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="RelativePath" DataField="RelativePath" />

                <asp:TemplateField HeaderText="Full URL (files)">
                    <ItemTemplate>
                        <asp:PlaceHolder runat="server" Visible='<%# Eval("Type").ToString() == "File" %>'>
                            <a href='<%# Eval("FullUrl") %>' target="_blank">
                                <%# Eval("FullUrl") %>
                            </a>
                        </asp:PlaceHolder>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actions (files)">
                    <ItemTemplate>

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
</body>
</html>
