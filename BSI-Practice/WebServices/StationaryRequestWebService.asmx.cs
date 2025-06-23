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
        public string GetTaskAndAssignmentID(int header_id)
        {
            string returnedOutput = "";
            try
            {
                var tasks = controller.GetWorkflowTasks();
                var task = tasks.FirstOrDefault(t => t.name.ToString().Contains($"{header_id}"));
                string task_id = Convert.ToString(task["id"]);
                string assignment_id = Convert.ToString(task["taskAssignments"][0]["id"]);

                var result = new
                {
                    Success = true,
                    Message = "OK",
                    task_id,
                    assignment_id
                };
                returnedOutput = new JavaScriptSerializer().Serialize(result);
            }
            catch(Exception ex)
            {
                var result = new
                {
                    Success = false,
                    Message = $"{ex.Message}"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(result);
            }
            return returnedOutput;
        }

        [WebMethod]
        public string ApprovalAction(string action_name, string task_id, string assignment_id)
        {
            string returnedOutput = "";
            try
            {
                string token = controller.GetWorkflowToken();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var payload = new { outcome = action_name };
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var stringContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                string url = $"https://us.nintex.io/workflows/v2/tasks/{task_id}/assignments/{assignment_id}";

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
                request.Content = stringContent;

                var response = client.SendAsync(request).Result;
                var responseBody = response.Content.ReadAsStringAsync();

                var result = new
                {
                    Success = true,
                    Message = "OK",
                    Body = responseBody
                };
                returnedOutput = new JavaScriptSerializer().Serialize(result);
            }
            catch(Exception ex)
            {
                var result = new
                {
                    Success = false,
                    Message = $"{ex.Message}"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(result);
            }
            return returnedOutput;
        }

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
                controller.SaveUpdate(header, details);
                return new JavaScriptSerializer().Serialize(new { Success = true, Message = "OK" });
            }
            catch (Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new { Success = false, Message = $"Error: {ex.Message}" });
            }
        }

        [WebMethod]
        public string InsertWorkflowHistory(int header_id)
        {
            var returnedOutput = "";
            try
            {
                controller.InsertWorkflowHistoryLog(header_id);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            catch(Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"Error = {ex.Message}"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            return returnedOutput;
        }

        [WebMethod]
        public string InsertApprovalLog(WorkflowHistoryModel model)
        {
            string returnedOutput = "";
            try
            {
                controller.InsertApprovalLog(model);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK"
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            catch(Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"Error = {ex.Message}"
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
                StationaryRequestHeader header = controller.GetHeaderData(folio_no);
                List<StationaryRequestDetail> details = controller.GetDetailData(folio_no);
                //CombineModel combineModel = new CombineModel();
                //combineModel.Header = controller.GetHeaderData(folio_no);
                //combineModel.Details = controller.GetDetailData(folio_no);
                Console.WriteLine("Bla bla");
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    //_header = combineModel.Header,
                    _header = header,
                    //_details = details
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
        public string GetHeaderData(string folio_no)
        {
            var returnedOutput = "";
            try
            {
                var header = controller.GetRequestHeader(folio_no);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    Data = header
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
        public string GetDetailsData(string folio_no)
        {
            var returnedOutput = "";
            try
            {
                var details = controller.GetRequestDetails(folio_no);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    Data = details
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
        public string GetCurrentStatusID(string folio_no)
        {
            var returnedOutput = "";
            try
            {
                int status_id = controller.GetCurrentStatusID(folio_no);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    status_id
                };
                returnedOutput =  new JavaScriptSerializer().Serialize(responseBody);
            }
            catch(Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"Error = {ex.Message}",
                };
                returnedOutput = new JavaScriptSerializer().Serialize(responseBody);
            }
            return returnedOutput;
        }

        [WebMethod]
        public string GetRequestDataByJoin(string folio_no)
        {
            string returnedOutput = "";
            try
            {
                var data = controller.GetDataRequestDataByJoin(folio_no);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    data
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
        public string GetWorkflowHistories(string folio_no)
        {
            string returnedOutput = "";
            try
            {
                var workflowData = controller.GetWorkflowHistories(folio_no);
                var responseBody = new
                {
                    Success = true,
                    Message = "OK",
                    data = workflowData
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
