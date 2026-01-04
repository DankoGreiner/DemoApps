using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DemoApps
{
    public partial class Default : System.Web.UI.Page
    {
        string validPass = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.DayOfWeek.ToString().Substring(0, 1).ToLower());
        protected void Page_Load(object sender, EventArgs e)
        {
            //string s = DataAccess.DBConnectionString;
            btnLogin.Text = btnLogin.Text + DateTime.Now.DayOfWeek.ToString().Substring(0, 1).ToLower();
            if (Session["Login"] != null)
            {
                btnLogin.Text = "Logout";
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Session["Login"] != null)
            {
                Session["Login"] = null;
            }
            else
            {
                if (txtPassword.Text == validPass)
                {
                    Session["Login"] = 1;
                }
            }
            Response.Redirect(Request.RawUrl.ToString());
        }
    }
}