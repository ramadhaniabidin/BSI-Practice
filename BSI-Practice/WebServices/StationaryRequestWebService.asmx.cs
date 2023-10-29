using BSI_Logics.Controller;
using BSI_Logics.Models;
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
    /// Summary description for StationaryRequestWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class StationaryRequestWebService : System.Web.Services.WebService
    {
        private readonly StationaryRequestController controller = new StationaryRequestController();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetCurrentLoginData(string loginToken)
        {
            var returnedOutput = "";
            try
            {
                var currentLoginData = controller.GetCurrentLoginData(loginToken);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    currentLoginData
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
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
        public string GetStationaryItems()
        {
            var returnedOutput = "";
            try
            {
                var StationaryItems = controller.GetAllStationary();
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    StationaryItems
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
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
        public string GetApproverList()
        {
            string returnedOutput = string.Empty;
            try
            {
                var approver_name = controller.GetApproverList();
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    approver_name
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            catch(Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"Error : {ex.Message}"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            return returnedOutput;
        }

        [WebMethod]
        public string GetStockAndUnit(string item_name)
        {
            var returnedOutput = "";
            try
            {
                var StockAndUnit = controller.GetStockAndUnit(item_name);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    StockAndUnit
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
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
        public string SaveUpdate(StationaryRequestHeader header, List<StationaryRequestDetail> details)
        {
            var returnedOutput = "Bitch Ass";
            try
            {
                controller.SaveUpdate(header, details);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            catch (Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"Error : {ex.Message}"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            return returnedOutput;
        }

        [WebMethod]
        public string GetRequestData(string folio_no)
        {
            var returnedOutput = "";
            try
            {
                var header = controller.GetRequestHeader(folio_no);
                var details = controller.GetRequestDetails(folio_no);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    header, 
                    details
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            catch(Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"Error : {ex.Message}"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            return returnedOutput;
        }
    }
}
