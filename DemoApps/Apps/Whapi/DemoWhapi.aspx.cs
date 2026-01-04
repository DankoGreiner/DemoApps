using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace DemoApps.Apps.Whapi
{
    public partial class DemoWhapi : System.Web.UI.Page
    {

        string token = "19FGAKHGqrajH1F1H5x7jyiVPRZrmIec";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTestWhapi_Click(object sender, EventArgs e)
        {
            try
            {
                lblTestMessage.Text = string.Empty;
                string result = SendWhapiMessage();
                lblTestMessage.Text = result + " " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                lblTestMessage.Text = string.Empty;

                lblTestMessage.Text = DateTime.Now.ToString() + "<br>";
                lblTestMessage.Text = ex.Message;
                if (ex.InnerException != null)
                {
                    lblTestMessage.Text += "<br>";
                    lblTestMessage.Text += ex.InnerException.ToString();
                }
            }
        }

        private string SendWhapiMessage()
        {
            string to = "38598220294";
            string body = "Hello from sync Web Forms" + " " + DateTime.Now.ToString();

            string result = WhapiSendTextSync(to, body);

            return result;
        }

        public string WhapiSendTextSync(string to, string body)
        {
            // Important for .NET Framework in many environments:
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // ✅ Correct endpoint (do NOT append /messages/text again elsewhere)
            var url = "https://gate.whapi.cloud/messages/text";

            // Build JSON without Newtonsoft
            var payloadObj = new { to = to, body = body };
            var json = new JavaScriptSerializer().Serialize(payloadObj);
            var data = Encoding.UTF8.GetBytes(json);

            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json";
            req.Accept = "application/json";
            req.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
            req.ContentLength = data.Length;

            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
            }

            try
            {
                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var reader = new StreamReader(resp.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                // Read WHAPI error response body (very useful for debugging)
                var resp = ex.Response as HttpWebResponse;
                if (resp != null)
                {
                    using (resp)
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        return $"HTTP {(int)resp.StatusCode} {resp.StatusDescription}\n{reader.ReadToEnd()}";
                    }
                }

                return ex.ToString();
            }
        }

        protected void btnCreateGropu_Click(object sender, EventArgs e)
        {
            try
            {
                lblTestMessage.Text = string.Empty;
                string result = CreateWhapiGroup();
                lblTestMessage.Text = result + " " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                lblTestMessage.Text = string.Empty;

                lblTestMessage.Text = DateTime.Now.ToString() + "<br>";
                lblTestMessage.Text = ex.Message;
                if (ex.InnerException != null)
                {
                    lblTestMessage.Text += "<br>";
                    lblTestMessage.Text += ex.InnerException.ToString();
                }
            }
        }

        public string CreateWhapiGroup()
        {
            string returnResponse = "No Response";
            // REQUIRED on many .NET Framework servers
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string url = "https://gate.whapi.cloud/groups";

            var payload = new
            {
                subject = "Parovi 3A. Liga",
                participants = new[] { "38598220294", "385923370036" }
            };

            string json = new JavaScriptSerializer().Serialize(payload);
            byte[] data = Encoding.UTF8.GetBytes(json);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseBody = reader.ReadToEnd();
                    returnResponse = responseBody;

                    // ✅ LOG EVERYTHING
                    Console.WriteLine("=== WHAPI CREATE GROUP RESPONSE ===");
                    Console.WriteLine(responseBody);
                    Console.WriteLine("=================================");

                    // WebForms alternative:
                    System.Diagnostics.Debug.WriteLine(responseBody);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var resp = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        returnResponse = error;

                        Console.WriteLine("=== WHAPI ERROR ===");
                        Console.WriteLine($"HTTP {(int)resp.StatusCode}");
                        Console.WriteLine(error);
                    }
                }
                else
                {
                    Console.WriteLine(ex.ToString());
                    returnResponse = ex.ToString();
                }
            }
            return returnResponse;
        }
    }
}