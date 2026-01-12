using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YourApp.FileManager
{
    public partial class FileManagerPage : Page
    {
        private string CurrentFolder
        {
            get
            {
                return ViewState["CurrentFolder"]?.ToString() ?? Server.MapPath("~/Apps/FileManager/Files");
            }
            set
            {
                ViewState["CurrentFolder"] = value;
            }
        }

        private string RenamingPath
        {
            get { return (ViewState["RenamingPath"] ?? "").ToString(); }
            set { ViewState["RenamingPath"] = value; }
        }

        public bool IsRenaming(object relativePathObj)
        {
            var rel = (relativePathObj ?? "").ToString();
            return !string.IsNullOrEmpty(RenamingPath)
                && string.Equals(RenamingPath, rel, StringComparison.OrdinalIgnoreCase);
        }

        public string RenameButtonText(object relativePathObj)
        {
            return IsRenaming(relativePathObj) ? "Save" : "Rename";
        }

        private FileManagerService Svc => new FileManagerService(Server, Request);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login"] == null)
            {
                Response.Redirect("~/");

                //Response.End();
            }
            if (!IsPostBack)
            {
                BindGrid();
                SetBuildTime();

            }
        }

        private void BindGrid()
        {
            var folder = CurrentFolderRel;

            // show current folder in UI (optional label you add in .aspx)
            lblCurrentFolder.Text = Server.HtmlEncode("/" + folder);

            gvItems.DataSource = Svc.ListFolder(folder);
            gvItems.DataBind();

            // show/hide Up button
            btnUp.Visible = !string.IsNullOrEmpty(folder);
            btnUp.CommandArgument = ParentFolderRel(folder);
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            var parent = ParentFolderRel(CurrentFolderRel);
            Response.Redirect("FileManager.aspx?p=" + Server.UrlEncode(parent));
        }

        private void ShowMsg(string text)
        {
            pnlMsg.Visible = true;
            lblMsg.Text = Server.HtmlEncode(text);
        }

        // ✅ NEW: upload into current folder
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                var folder = CurrentFolderRel; // "" or "Sub/Folder"
                var rel = Svc.SaveUpload(fuUpload.PostedFile, folder);

                ShowMsg("Uploaded: " + rel);
                BindGrid();
            }
            catch (Exception ex)
            {
                ShowMsg("Upload failed: " + ex.Message);
            }
        }

        protected void btnCreateFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewFolder.Visible == false)
                {
                    txtNewFolder.Visible = true;
                    txtNewFolder.Focus();
                }
                else
                {
                    var baseFolder = CurrentFolderRel;
                    var newRel = (baseFolder + "/" + (txtNewFolder.Text ?? "")).Trim('/');

                    Svc.CreateFolder(newRel);

                    txtNewFolder.Text = string.Empty;
                    txtNewFolder.Visible = false;
                    ShowMsg("Folder created.");
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                ShowMsg("Create folder failed: " + ex.Message);
            }
        }

        private string CurrentFolderRel
        {
            get
            {
                var q = (Request.QueryString["p"] ?? "").Trim();
                return q.Replace('\\', '/').TrimStart('/');
            }
        }

        private string ParentFolderRel(string rel)
        {
            rel = (rel ?? "").Replace('\\', '/').Trim('/');
            if (string.IsNullOrEmpty(rel)) return "";
            var idx = rel.LastIndexOf('/');
            return idx <= 0 ? "" : rel.Substring(0, idx);
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var relPath = (e.CommandArgument ?? "").ToString();

                if (e.CommandName == "DeleteFolder")
                {
                    Svc.DeleteFolder(relPath);
                    ShowMsg("Folder deleted: " + relPath);
                    BindGrid();
                    return;
                }

                if (e.CommandName == "DeleteFile")
                {
                    Svc.DeleteFile(relPath);
                    RenamingPath = ""; // close rename mode if any
                    ShowMsg("Deleted: " + relPath);
                    BindGrid();
                    return;
                }

                if (e.CommandName == "RenameToggle")
                {
                    // First click: enter rename mode for this row
                    if (!string.Equals(RenamingPath, relPath, StringComparison.OrdinalIgnoreCase))
                    {
                        RenamingPath = relPath;
                        BindGrid();
                        return;
                    }

                    // Second click: save rename
                    var btn = (Control)e.CommandSource;
                    var row = (GridViewRow)btn.NamingContainer;
                    var txt = (TextBox)row.FindControl("txtRename");

                    Svc.RenameFile(relPath, txt.Text);

                    RenamingPath = "";
                    ShowMsg("Renamed.");
                    BindGrid();
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowMsg("Action failed: " + ex.Message);
            }
        }

        private void SetBuildTime()
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var path = asm.Location;

            if (System.IO.File.Exists(path))
            {
                var dt = System.IO.File.GetLastWriteTime(path);
                lblBuildTime.Text = "Build: " + dt.ToString("yyyy-MM-dd HH:mm");
            }
        }


        protected string GetFileIcon(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return "📄";

            var dot = fileName.LastIndexOf('.');
            var ext = dot >= 0 ? fileName.Substring(dot + 1).ToLowerInvariant() : "";

            switch (ext)
            {
                case "pdf": return "📕";
                case "doc":
                case "docx": return "🟦"; // Word-like
                case "xls":
                case "xlsx":
                case "csv": return "🟩"; // Excel-like
                case "ppt":
                case "pptx": return "🟧"; // PowerPoint-like

                case "zip":
                case "rar":
                case "7z": return "🗜️";

                case "jpg":
                case "jpeg":
                case "png":
                case "gif":
                case "webp": return "🖼️";

                case "mp3":
                case "wav":
                case "flac": return "🎵";

                case "mp4":
                case "mov":
                case "avi":
                case "mkv": return "🎞️";

                case "txt":
                case "md":
                case "log": return "📝";

                case "html":
                case "htm":
                case "css":
                case "js":
                case "json":
                case "xml": return "💻";

                default: return "📄";
            }
        }

    }
}
