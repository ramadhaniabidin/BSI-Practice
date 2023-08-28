using BSI_Logics.Controller;
using BSI_Logics.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace BSI_Practice.WebServices
{
    /// <summary>
    /// Summary description for LoginWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LoginWebService : System.Web.Services.WebService
    {
        private readonly LoginController controller = new LoginController();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetRoleId(string email)
        {
            var returnedOutput = "";
            int role_id ;
            try
            {
                role_id = controller.GetRoleId(email);
                if(role_id >= 0)
                {
                    var responseBody = new
                    {
                        Success = true,
                        Message = "OK",
                        role_id
                    };
                    returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
                }

                else
                {
                    var responseBody = new
                    {
                        Success = false,
                        Message = "Error, periksa kembali email yang Anda masukkan",
                    };
                    returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
                }
            }
            catch(Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            return returnedOutput;
        }

        [WebMethod]
        public string GenerateLoginToken(string email)
        {
            var returnedOutput = "";
            try
            {
                var LoginToken = controller.GenerateLoginToken(email);
                if (!string.IsNullOrWhiteSpace(LoginToken))
                {
                    var responseBody = new
                    {
                        Success = true,
                        Message = "OK",
                        LoginToken
                    };
                    returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
                }

                else
                {
                    var responseBody = new
                    {
                        Success = false,
                        Message = "Error, mohon periksa kembali email Anda",
                        LoginToken
                    };
                    returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
                }
            }
            catch(Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            return returnedOutput;
        }
    }
}
