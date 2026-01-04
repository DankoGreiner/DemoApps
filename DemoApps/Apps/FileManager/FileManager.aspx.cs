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
                Response.End();
            }
            if (!IsPostBack)
                BindGrid();
        }


        private void BindGrid()
        {
            var folder = CurrentFolderRel;

            // show current folder in UI (optional label you add in .aspx)
            lblCurrentFolder.Text = Server.HtmlEncode("/" + folder);

            gvItems.DataSource = Svc.ListFolder(folder);
            gvItems.DataBind();

            // set upload target default to current folder
            //txtUploadFolder.Text = folder;

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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //var rel = Svc.SaveUpload(fuUpload.PostedFile, txtUploadFolder.Text);
                var rel = Svc.SaveUpload(fuUpload.PostedFile, string.Empty);
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

    }
}
