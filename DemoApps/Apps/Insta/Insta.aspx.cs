using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Drawing;



namespace DemoApps.Apps.Insta
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            if (!IsPostBack)
            {
                txtRename.Focus();
                DeleteFilesOlderThan24Hours("Upload");
            }
        }

        protected async void btnInsta_Click(object sender, EventArgs e)
        {

            try
            {
                var result = await PublishToInstagramAsync(txtUrl.Text);
                lblMessage.Text += result;
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }

        }

        protected void bntSave_Click(object sender, EventArgs e)
        {
            if (hidBinaryString.Value.Split(',').Length > 1)
            {
                string binaryString = hidBinaryString.Value.Split(',')[1];
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(binaryString));

                string fileName = string.Format("{0}.png", DateTime.Now.Ticks);
                ViewState["fileName"] = fileName;

                string filePath = string.Format(@"{0}\Apps\Insta\Upload\{1}", Server.MapPath("~"), fileName);
                ViewState["filePath"] = filePath;


                var bytes = Convert.FromBase64String(binaryString);
                using (var imageFile = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                string filePathInsta = filePath.Replace(".png", "_insta.png");
                string fileNameInsta = fileName.Replace(".png", "_insta.png");
                InstagramStoryConverter.ConvertToStory(filePath, filePathInsta, Color.Black);


                lblBinaryString.Text = Request.Url.AbsoluteUri.Replace(Request.Url.LocalPath, string.Empty);
                lblBinaryString.Text = string.Format(@"{0}/Apps/Insta/Upload/{1}", lblBinaryString.Text, fileNameInsta);

                txtUrl.Text = lblBinaryString.Text;
            }

        }
        protected void btnRename_Click(object sender, EventArgs e)
        {
            string oldFilePath = ViewState["filePath"].ToString();
            string oldFileName = ViewState["fileName"].ToString();
            string newFilePath = oldFilePath.Replace(oldFileName, txtRename.Text + ".png");
            //if (File.Exists(newFilePath))
            //    newFilePath = newFilePath.Replace(txtRename.Text + ".png", txtRename.Text + DateTime.Now.Ticks.ToString() + ".png");
            File.Copy(oldFilePath, newFilePath, true);
            File.Delete(oldFilePath);

            lblBinaryString.Text = lblBinaryString.Text.Replace(oldFileName, txtRename.Text + ".png");
        }


        public static async Task<string> PublishToInstagramAsync(string imageUrl)
        {
            // Important on some .NET Framework environments:
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var baseUrl = "https://features.tenisliga.com/api/ImageToInstagram/PublishURLToInstagram";
            var encodedImageUrl = Uri.EscapeDataString(imageUrl);

            var requestUrl = $"{baseUrl}?imageUrl={encodedImageUrl}";

            using (var client = new HttpClient())
            {
                // If your API returns plain text, accept text/plain; otherwise use application/json
                client.DefaultRequestHeaders.Accept.ParseAdd("text/plain");

                var resp = await client.GetAsync(requestUrl).ConfigureAwait(false);
                var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                // If it fails, you want the body for debugging
                resp.EnsureSuccessStatusCode();

                return body;
            }
        }

        public static void DeleteFilesOlderThan24Hours(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return;

            TimeSpan timeLimit = TimeSpan.FromHours(24);

            string fileName = string.Empty;
            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                fileName = filePath;
                try
                {
                    if (DateTime.Now - File.GetLastWriteTime(filePath) > timeLimit)
                    {
                        //new ActivityDAL().SaveCall(null, "Deleting file: " + filePath);
                        File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    //new ActivityDAL().SaveCall(null, "Error deleting file: " + fileName + Environment.NewLine + ex.Message);

                }
            }
        }
    }
}