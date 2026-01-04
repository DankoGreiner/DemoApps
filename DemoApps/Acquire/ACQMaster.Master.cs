using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DemoApps.Acquire
{
    public partial class ACQMaster : System.Web.UI.MasterPage
    {

        #region Properties
        public string menuTree = string.Empty;
        //private List<MenuItemSEM> menuItemList = new List<MenuItemSEM>();


        #endregion

        #region ClientScripts Reg
        public void AddJS(string javaScriptFile)
        {
            AddJS(javaScriptFile, false);
        }


        /// <summary>
        /// Dodaje referencu na javascript file u stranicu
        /// </summary>
        /// <param name="javaScriptFile"></param>
        /// <param name="addToHead">dodaje u head tag</param>
        public void AddJS(string javaScriptFile, bool addToHead)
        {
            AddJS(javaScriptFile, addToHead, false);
        }

        public void AddJS(string javaScriptFile, bool addToHead, bool versioning)
        {
            if (versioning)
                javaScriptFile += "?v=" + DateTime.Now.ToString("yyyyMMddHHmmss");

            HtmlGenericControl scr = new HtmlGenericControl();
            scr.TagName = "script";
            scr.Attributes.Add("type", "text/javascript");
            scr.Attributes.Add("src", Request.ApplicationPath + javaScriptFile);

            if (addToHead && cphHead != null)
                cphHead.Controls.Add(scr);
            else if (!addToHead && cphBbodyEnd != null)
                cphBbodyEnd.Controls.Add(scr);
        }

        /// <summary>
        /// Dodaje referencu na css file u header stranice
        /// </summary>
        /// <param name="cssFile">i.e. "Css/nanoscroller.css"</param>
        /// <param name="addToTop"></param>
        public void AddCss(string cssFile)
        {
            AddCss(cssFile, false);
        }

        public void AddCss(string cssFile, bool versioning)
        {
            if (versioning)
                cssFile += "?v=" + DateTime.Now.ToString("yyyyMMddHHmmss");

            HtmlGenericControl scr = new HtmlGenericControl();
            scr.TagName = "link";
            scr.Attributes.Add("type", "text/css");
            scr.Attributes.Add("href", Request.ApplicationPath + cssFile);
            scr.Attributes.Add("rel", "stylesheet");
            cphHead.Controls.Add(scr);
        }

        protected override void OnPreRender(EventArgs e)
        {
            AddJS("../js/jquery.min.js");
            AddJS("../js/bootstrap.min.js");
            AddJS("../js/chosen.jquery.min.js");
            AddJS("../js/jquery-ui.js");
            AddJS("../js/datepicker-hr.js");

            AddJS("../js/jquery-ui-1.10.3.min.js");
            AddJS("../js/plugins/daterangepicker/daterangepicker.js");
            AddJS("../js/plugins/chart.js");

            AddJS("../js/Director/app.js");
            AddJS("../js/Director/dashboard.js");
            AddJS("../js/plugins/chart.js");

            AddCss("css/bootstrap.min.css");
            AddCss("css/blog-home.css");
            AddCss("css/chosen.min.css");
            AddCss("css/jquery-ui.css");

            AddCss("css/bootstrap-chosen.css");

            AddCss("css/DemoApp.css", true);

            AddCss("css/font-awesome.min.css");
            AddCss("css/ionicons.min.css");
            AddCss("css/morris/morris.css");
            AddCss("css/jvectormap/jquery-jvectormap-1.2.2.css");
            AddCss("css/datepicker/datepicker3.css");
            AddCss("css/daterangepicker/daterangepicker-bs3.css");
            AddCss("css/iCheck/all.css");
            AddCss("css/style.css");

            base.OnPreRender(e);
        }

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //lblError.Text = string.Empty;
                //divError.Visible = false;
                if (!IsPostBack)
                {
                    if (Request.QueryString["Logout"] != null)
                        Logout();

                    //litMenu.Text = GetMenu(string.Empty);
                }
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblError.Text = ex.Message;
            }
        }

        #endregion

        #region Methods

        private void Logout()
        {
            //Response.Cookies[SessionKeys.UserId].Expires = DateTime.Now.AddDays( -1 );

            //Session.Clear();
            //if ( Response.Cookies["email"] != null )
            //{
            //	Response.Cookies["email"].Expires = DateTime.Now.AddDays( -1 );
            //}
            //if ( Response.Cookies[SessionKeys.UserId] != null )
            //{
            //	Response.Cookies[SessionKeys.UserId].Expires = DateTime.Now.AddDays( -1 );
            //}
        }

        public void HandleException(Exception ex)
        {
            //lblError.Text = ex.Message;
            //divError.Visible = true;
        }


        public string GetMenu(string domain)
        {

            string selectStr = string.Format("SELECT [MenuItemId], [MenuItemParentId], [Link], [LinkTarget], [Active], [MenuItemOrder], ISNULL({0},MenuItemName)  AS MenuItemName, mi.Literal FROM MenuItem mi LEFT OUTER JOIN Translate t ON mi.Literal = t.Literal WHERE Active = 1 ORDER BY MenuItemOrder", "hr");
            DataTable mnuDT = DataAccess.GetDataTable(DataAccess.DBConnectionString, selectStr);

        
            //GetAllMenuItems(null, domain);
            menuTree = menuTree.Trim().TrimEnd("</div>".ToCharArray());
            return menuTree;
        }

        //private void GetAllMenuItems(MenuItemSEM mi, string domain)
        //{
        //    List<MenuItemSEM> children;
        //    //if ( menuItemList == null )
        //    //	menuItemList = new List<MenuItemSEM>();


        //    if (mi != null)
        //    {
        //        menuTree += Environment.NewLine;

        //        menuTree += Environment.NewLine;
        //        if (string.IsNullOrEmpty(mi.Link))//Parent, no link
        //            menuTree += string.Format("<li><a href=#>{0}</a>", mi.MenuItemName);
        //        else if (mi.Link.IndexOf("(") == -1)//No Javascript, normal link
        //            menuTree += string.Format("<li><a href={3}/{0} target={2}>{1}</a>", mi.Link, mi.MenuItemName, mi.LinkTarget, domain);
        //        else//Javascript
        //            menuTree += string.Format("<li><a href=# onclick={0}>{1}</a>", mi.Link, mi.MenuItemName);

        //        children = menuItemList.FindAll(m => m.MenuItemParentId == mi.MenuItemId);


        //        children = menuItemList.FindAll(m => m.MenuItemParentId == mi.MenuItemId);
        //    }
        //    else
        //    {
        //        //menuTree = "<ul>";
        //        children = menuItemList.FindAll(m => m.MenuItemParentId == null);
        //    }

        //    if (children.Count > 0)
        //    {
        //        menuTree += Environment.NewLine;
        //        if (string.IsNullOrEmpty(menuTree.Trim()))
        //            menuTree += "\t<ul  class=\"sidebar-menu\">";
        //        else
        //        {

        //            string removeParent = string.Format("<a href=#>{1}</a>", mi.Link, mi.MenuItemName);
        //            menuTree = menuTree.Replace(removeParent, string.Empty);

        //            menuTree += "<div class=\"dropdown\">";
        //            menuTree += Environment.NewLine;
        //            menuTree += "<a href=\"javascript:void()\" class=\"disabled dropdown-toggle\" id=\"AdministrationMenu\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">";
        //            menuTree += mi.MenuItemName;
        //            menuTree += Environment.NewLine;
        //            menuTree += "</a>";
        //            menuTree += Environment.NewLine;
        //            menuTree += "\t<ul class=\"dropdown-menu pull-right\" aria-labelledby=\"AdministrationMenu\">";
        //        }

        //        menuTree += Environment.NewLine;
        //        foreach (MenuItemSEM mic in children)
        //        {
        //            GetAllMenuItems(mic, domain);
        //        }
        //        menuTree += Environment.NewLine;
        //        menuTree += "\t</ul>";
        //        menuTree += Environment.NewLine;
        //        menuTree += "\t</div>";//Zadnji treba maknut
        //        menuTree += Environment.NewLine;
        //    }

        //    if (mi != null)
        //        menuTree += "</li>";

        //}

        //private void GetAllMenuItems(MenuItemSEM mi)
        //{
        //    List<MenuItemSEM> children;

        //    if (mi != null)
        //    {
        //        menuTree += Environment.NewLine;
        //        //menuTree += string.Format("<li><a href={0}>{1}</a>", mi.Link, mi.MenuItemName);
        //        string notActive = string.Empty;
        //        if (!mi.Active)
        //            notActive = "NOT ACTIVE";
        //        menuTree += string.Format("<li><span onclick=EditMenuItem({0}) style='cursor:pointer'>{1} ({2}) {3}</span>", mi.MenuItemId, mi.MenuItemName, mi.MenuItemOrder, notActive);
        //        children = menuItemList.FindAll(m => m.MenuItemParentId == mi.MenuItemId);
        //    }
        //    else
        //    {
        //        //menuTree = "<ul>";
        //        children = menuItemList.FindAll(m => m.MenuItemParentId == null);
        //    }

        //    if (children.Count > 0)
        //    {
        //        menuTree += Environment.NewLine;
        //        menuTree += "\t<ul>";
        //        menuTree += Environment.NewLine;
        //        foreach (MenuItemSEM mic in children)
        //        {
        //            GetAllMenuItems(mic);
        //        }
        //        menuTree += Environment.NewLine;
        //        menuTree += "\t</ul>";
        //        menuTree += Environment.NewLine;
        //    }

        //    if (mi != null)
        //        menuTree += "</li>";

        //}

        #endregion



    }
}