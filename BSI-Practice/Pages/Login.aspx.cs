using BSI_Logics.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BSI_Practice.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly LoginController login = new LoginController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(User.Identity.IsAuthenticated) Response.Redirect("/Pages/Home.aspx");
            if (!IsPostBack)
            {
                txtError.Visible = false;
                btnSubmit.Enabled = !string.IsNullOrEmpty(txtEmail.Value.Trim()) && !string.IsNullOrEmpty(txtPassword.Value.Trim());
            }
        }

        protected void Btn_Login(object sender, EventArgs e)
        {
            var email = txtEmail.Value.ToString().Trim();
            var pass = txtPassword.Value.ToString().Trim();
            if(ValidateUser(email, pass))
            {
                FormsAuthentication.SetAuthCookie(email, false);
                Response.Redirect("/Pages/Home.aspx");
            }
            else
            {
                txtError.Visible = true;
            }
        }

        protected bool ValidateUser(string email, string password)
        {
            if (!login.CheckEmailExists(email)) return false;
            var user = login.GetUser(email);
            if (!login.VerifyPassword(password, user.Password)) return false;
            Session["UserID"] = email;
            Session["RoleID"] = user.RoleID;
            return true;
        }
    }
}