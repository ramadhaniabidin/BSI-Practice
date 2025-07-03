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
        public string GetRequestList()
        {
            try
            {
                var requestList = controller.GetRequestList();
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = true,
                    Message = "OK",
                    ListItems = requestList
                });
            }
            catch(Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = false,
                    Message = $"Error = {ex.Message}"
                });
            }
        }

        [WebMethod]
        public string GetHistoryLogList(int ID)
        {
            try
            {
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = true, Message = "OK",
                    HistoryLog = controller.HistoryLogs(ID)
                });
            }
            catch(Exception ex)
            {
                return HomeController.ThrowException(ex.Message);
            }
        }
    }
}
