using BSI_Logics.Controller;
using BSI_Logics.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace BSI_Practice.WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class StationaryRequestWebService : System.Web.Services.WebService
    {
        private readonly StationaryRequestController controller = new StationaryRequestController();

        [WebMethod]
        public string GetCurrentLoginData(string email)
        {
            try
            {
                var currentLoginData = controller.GetCurrentLoginData(email);
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = true,
                    Message = "OK",
                    Data = currentLoginData
                });                
            }

            catch(Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                });
            }
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
            try
            {
                var response = controller.SaveUpdate(header, details);
                return new JavaScriptSerializer().Serialize(new { Success = response.Success, Message = response.Message });
            }
            catch (Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new { Success = false, Message = $"Error: {ex.Message}" });
            }
        }
    }
}
