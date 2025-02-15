using BSI_Logics.Common;
using BSI_Logics.Controller;
using BSI_Logics.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace BSI_Practice.WebServices
{
    /// <summary>
    /// Summary description for LoginWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
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
        public string CheckEmailExists(string email)
        {
            try
            {
                bool emailExists = controller.CheckEmailExists(email);
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = true,
                    InfoMessage = "OK",
                    EmailExists = emailExists
                });
            }
            catch(Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = false,
                    InfoMessage = ex.Message
                });
            }
        }

        [WebMethod]
        public string SignUp(string email, string password)
        {
            try
            {
                bool emailExists = controller.CheckEmailExists(email);
                if (emailExists)
                {
                    return new JavaScriptSerializer().Serialize(new
                    {
                        Success = false,
                        InfoMessage = "Email already registered, please use different email"
                    });
                }
                controller.SignUp(email, password);
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = true,
                    InfoMessage = "OK"
                });
            }
            catch(Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = false,
                    InfoMessage = ex.Message
                });
            }
        }

        [WebMethod]
        public string TextAppConfig()
        {
            try
            {
                string path = Utility.TestAppConfig();
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = true,
                    Message = "OK",
                    Path = path
                });
            }
            catch(Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = false,
                    ex.Message
                });
            }
        }

        [WebMethod]
        public string TestConnectionString()
        {
            try
            {
                string connString = Utility.GetSQLConnection();
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = true,
                    Message = "OK",
                    ConnectionString = connString
                });
            }
            catch(Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = false,
                    ex.Message
                });
            }
        }

        [WebMethod]
        public string TestFetchItems()
        {
            try
            {
                var items = Utility.TestFetchItems();
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = true,
                    Message = "OK",
                    FetchedItems = items
                });
            }
            catch(Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = false,
                    ex.Message
                });
            }
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

        [WebMethod]
        public string GenerateLoginToken1(string email)
        {
            var returnedOutput = "";
            try
            {
                var LoginToken = controller.GenerateLoginToken1(email);
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
            catch (Exception ex)
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

        [WebMethod]
        public string HashPassword(string password)
        {
            try
            {
                string hashedPassword = controller.HashPassword(password);
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = true,
                    Message = "OK",
                    HashedPassowd = hashedPassword
                });
            }
            catch(Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = false,
                    ex.Message
                });
            }
        }

        [WebMethod]
        public string VerifyPassword(string inputPassword, string hashedPassword)
        {
            try
            {
                bool verified = controller.VerifyPassword(inputPassword, hashedPassword);
                return new JavaScriptSerializer().Serialize(new 
                { 
                    Success = true,
                    Message = "OK",
                    PasswordVerified = verified
                });

            }
            catch (Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = false,
                    ex.Message
                });
            }
        }
    }
}
