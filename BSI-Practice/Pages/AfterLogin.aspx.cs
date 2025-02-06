using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BSI_Practice.Pages
{
    public partial class AfterLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["UserID"].ToString()))
            {
                string script = "alert('User ID is missing. Redirecting to the login page.'); window.location='/Pages/TestLogin.aspx';";
                ClientScript.RegisterStartupScript(this.GetType(), "alertRedirect", script, true);
            }
        }
    }
}