using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BSI_Practice.Pages
{
    public partial class TestLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void loginAction(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            Session["UserID"] = email;
            Response.Redirect("Pages/AfterLogin.aspx");
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}