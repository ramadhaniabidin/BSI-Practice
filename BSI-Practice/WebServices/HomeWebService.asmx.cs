using BSI_Logics.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace BSI_Practice.WebServices
{
    /// <summary>
    /// Summary description for HomeWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HomeWebService : System.Web.Services.WebService
    {
        private readonly HomeController controller = new HomeController();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetRequestList(int current_login_role_id)
        {
            var returnedOutput = "";
            try
            {
                var requestList = controller.GetRequestList(current_login_role_id);
                var response = new
                {
                    Success = true,
                    Message = "OK",
                    ListItems = requestList
                };
                returnedOutput = new JavaScriptSerializer().Serialize(response);
            }
            catch(Exception ex)
            {
                var response = new
                {
                    Success = false,
                    Message = $"Error = {ex.Message}"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(response);
            }
            return returnedOutput;
        }
    }
}
