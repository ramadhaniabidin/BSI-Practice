using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BSI_Practice.Pages
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // this is just a page load
            if (Request.QueryString["logout"] == "true")
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                Response.Redirect("/Pages/Login.aspx");
            }
        }
    }
}