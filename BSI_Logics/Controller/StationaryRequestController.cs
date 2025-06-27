using BSI_Logics.Common;
using BSI_Logics.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Utility = BSI_Logics.Common.Utility;

namespace BSI_Logics.Controller
{
    public class StationaryRequestController
    {
        DatabaseManager db = new DatabaseManager();
        private readonly string Module_Name = "Stationary Request";
        public string GetWorkflowToken()
        {
            string url = "https://us.nintex.io/authentication/v1/token";
            HttpClient client = new HttpClient();
            var requestBody = new
            {
                client_id = "f7bbb84b-b114-4120-9a5f-b0557b6dbee2",
                client_secret = "sNNtUWsKIRJtSsOtTsJPLtSsMNJMLtUsMPtUsI2VsJtWsINMtPsNtW2MtVsRtUUsFRtSTWsFMtTVsPFtRsK2osFtTsP2jsLOKtRsMM2p",
                grant_type = "client_credentials"
            };
            var jsonBody = JsonConvert.SerializeObject(requestBody);
            var HttpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, HttpContent).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseJson);
            string accessToken = responseObject.access_token;
            return accessToken;
        }
        public IEnumerable<dynamic> GetWorkflowTasks()
        {
            string url = "https://us.nintex.io/workflows/v2/tasks";
            string token = GetWorkflowToken();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = client.GetAsync(url).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;
            dynamic responseObject = JsonConvert.DeserializeObject(responseJson);
            var tasks = responseObject.tasks;

            return tasks;
        }
        public AccountModel GetCurrentLoginData(string email)
        {
            using(var conn = new SqlConnection(Utility.GetSQLConnection()))
            {
                conn.Open();
                using(var cmd = new SqlCommand("usp_GetUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@email", email);
                    using(var reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Utility.ConvertDataTableToList<AccountModel>(dt)[0];
                    }
                }
            }
        }
        public List<StationaryItemsModel> GetAllStationary()
        {
            using(var conn = new SqlConnection(Utility.GetSQLConnection()))
            {
                conn.Open();
                string query = "SELECT * FROM dbo.inventory_stationary";
                using(var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    DataTable dt = new DataTable();
                    using(var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        return Utility.ConvertDataTableToList<StationaryItemsModel>(dt);
                    }
                }
            }
        }
        public List<string> GetApproverList()
        {
            List<string> Approvers = new List<string>();
            try
            {
                using(var conn = new SqlConnection(Utility.GetSQLConnection()))
                {
                    conn.Open();
                    using(var cmd = new SqlCommand("usp_GetListApproval", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using(var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Approvers.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                return Approvers;
            }
            catch(Exception ex)
            {
                Utility.InsertErrorLog("Get Approver List", Module_Name, 0, ex.Message);
                return Approvers;
            }
        }
        public StockAndUnitModel GetStockAndUnit(string item_name)
        {
            dt = new DataTable();
            using(var conn = new SqlConnection(Utility.GetSQLConnection()))
            {
                conn.Open();
                string query = "SELECT SUM(stock) stock, uom FROM dbo.inventory_stationary WHERE item_name = @item_name GROUP BY stock, uom";
                using(var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@item_name", item_name);
                    using(var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        return Utility.ConvertDataTableToList<StockAndUnitModel>(dt)[0];
                    }
                }
            }
        }
        public void SaveUpdate(StationaryRequestHeader header, List<StationaryRequestDetail> details)
        {
            try
            {
                int LastInsertedId = 0;
                db.OpenConnection(ref conn, false);
                db.cmd.CommandText = "SaveUpdateHeader";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "applicant", header.applicant);
                db.AddInParameter(db.cmd, "created_by", header.created_by);
                db.AddInParameter(db.cmd, "created_date", header.created_date);
                db.AddInParameter(db.cmd, "current_approver_role", header.current_approver_role);
                db.AddInParameter(db.cmd, "department", header.department);
                db.AddInParameter(db.cmd, "employee_id", header.employee_id);
                db.AddInParameter(db.cmd, "extension", header.extension);
                db.AddInParameter(db.cmd, "folio_no", header.folio_no);
                db.AddInParameter(db.cmd, "modified_by", header.modified_by);
                db.AddInParameter(db.cmd, "modified_date", header.modified_date);
                db.AddInParameter(db.cmd, "role", header.role);

                reader = db.cmd.ExecuteReader();
                while (reader.Read())
                {
                    LastInsertedId = (int)reader["id"];
                }

                db.CloseDataReader(reader);
                db.CloseConnection(ref conn, false);
                for (int i = 0; i < details.Count; i++)
                {
                    db.OpenConnection(ref conn, false);
                    db.cmd.CommandText = "SaveUpdateDetail";
                    db.cmd.CommandType = CommandType.StoredProcedure;
                    db.cmd.Parameters.Clear();
                    db.AddInParameter(db.cmd, "header_id", LastInsertedId);
                    db.AddInParameter(db.cmd, "no", details[i].no);
                    db.AddInParameter(db.cmd, "item_name", details[i].item_name);
                    db.AddInParameter(db.cmd, "uom", details[i].uom);
                    db.AddInParameter(db.cmd, "stock", details[i].stock);
                    db.AddInParameter(db.cmd, "request_qty", details[i].request_qty);
                    db.AddInParameter(db.cmd, "reason", details[i].reason);

                    db.cmd.ExecuteNonQuery();
                    db.CloseConnection(ref conn, false);
                }
                InsertWorkflowHistoryLog(LastInsertedId);

                #region Start workflow
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                NintexWorkflowCloud nwc = new NintexWorkflowCloud();
                nwc.url = "https://npp-elistec.workflowcloud.com/api/v1/workflow/published/6f9f19b2-53ff-4062-80a9-c0e0ddc6241a/instances?token=EzFRP4GtqwENU57H1JEReZq0VvlLsMPQf0LrVFNKiUBqRQ0OAyMGN00aqGCbjySxHFFtY1";
                Task.Run(async () => { await StartWorkflow(nwc, LastInsertedId); }).Wait();
                #endregion
            }
            catch (Exception ex)
            {
                db.CloseConnection(ref conn, false);
                throw new Exception(ex.Message);
            }
        }
        public void InsertWorkflowHistoryLog(int header_id)
        {
            try
            {
                db.OpenConnection(ref conn, false);
                db.cmd.CommandText = "INSERT INTO dbo.workflow_history_log\n" +
                    "(folio_no, pic_name, comment, action_name, action_date)\n" +
                    "SELECT folio_no folio_no, applicant pic_name, 'First Submit' comment, 'First Submit' action_name, GETDATE() action_date\n" +
                    $"FROM dbo.stationary_request_header WHERE id = {header_id}";
                db.cmd.CommandType = CommandType.Text;
                db.cmd.ExecuteNonQuery();

                db.CloseConnection(ref conn, false);
            }
            catch(Exception ex)
            {
                db.OpenConnection(ref conn, false);
                throw ex;
            }
        }
        public static async Task<string> StartWorkflow(NintexWorkflowCloud nwc, int transaction_id)
        {
            try
            {
                nwc.param = new NWCParamModel();
                nwc.param.startData = new StartData();
                nwc.param.startData.se_transactionid = transaction_id;

                string requestBody = new JavaScriptSerializer().Serialize(nwc.param);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(nwc.url);

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, nwc.url);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void InsertApprovalLog(WorkflowHistoryModel model)
        {
            try
            {
                db.OpenConnection(ref conn, false);
                db.cmd.CommandText = "usp_InsertHistoryLog";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "folio_no", model.folio_no);
                db.AddInParameter(db.cmd, "pic_name", model.pic_name);
                db.AddInParameter(db.cmd, "comment", model.comment);
                db.AddInParameter(db.cmd, "action_name", model.action_name);
                db.cmd.ExecuteNonQuery();

                db.CloseConnection(ref conn, false);
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn, false);
                throw ex;
            }
        }
        public StationaryRequestHeader GetRequestHeader(string folio_no)
        {
            try
            {
                string query = $"SELECT id, folio_no, applicant, department, role, employee_id, employee_name, extension, status_id,\n" +
                    $"remarks, created_by, created_date, modified_by, modified_date, approver_target_role_id" +
                    $" FROM stationary_request_header WHERE folio_no = '{folio_no}'";
                db.OpenConnection(ref conn, true);
                db.cmd.CommandText = query;
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                dt.Load(reader);

                db.CloseConnection(ref conn, true);
                db.CloseDataReader(reader);

                StationaryRequestHeader output = new StationaryRequestHeader();

                if(dt.Rows.Count > 0)
                {
                    output = Common.Utility.ConvertDataTableToList<StationaryRequestHeader>(dt)[0];
                    Console.WriteLine(output);
                    //return Common.Utility.ConvertDataTableToList<StationaryRequestHeader>(dt)[0];
                    return output;
                }
                else
                {
                    output = new StationaryRequestHeader();
                    //return new StationaryRequestHeader();
                    return output;
                }
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn, true);
                throw ex;
            }
        }
        public List<StationaryRequestDetail> GetRequestDetails(string folio_no)
        {
            try
            {
                string query = $"DECLARE @header_id INT = (SELECT id FROM stationary_request_header WHERE folio_no = '{folio_no}')\n" +
                    $"SELECT * FROM stationary_request_detail WHERE header_id = @header_id";
                db.OpenConnection(ref conn, true);
                db.cmd.CommandText = query;
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                dt.Load(reader);

                //var output = Common.Utility.ConvertDataTableToList<StationaryRequestDetail>(dt);
                db.CloseConnection(ref conn, true);
                db.CloseDataReader(reader);

                return Common.Utility.ConvertDataTableToList<StationaryRequestDetail>(dt);
                //return output;
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn, true);
                throw ex;
            }
        }
        public StationaryRequestHeader GetHeaderData(string folio_no)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.get_header_data_by_folio_no";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "folio_no", folio_no);

                reader = db.cmd.ExecuteReader();
                dt.Load(reader);

                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);

                if(dt.Rows.Count > 0)
                {
                    return Common.Utility.ConvertDataTableToList<StationaryRequestHeader>(dt)[0];
                }
                else
                {
                    return new StationaryRequestHeader();
                }
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }
        public List<StationaryRequestDetail> GetDetailData(string folio_no)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.get_detail_data_by_folio_no";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "folio_no", folio_no);

                reader = db.cmd.ExecuteReader();
                dt.Load(reader);

                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);

                return Common.Utility.ConvertDataTableToList<StationaryRequestDetail>(dt);
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }
        public int GetCurrentStatusID(string folio_no)
        {
            int status_id = 0;
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT status_id FROM dbo.stationary_request_header WHERE folio_no = '{folio_no}'";
                db.cmd.CommandType = CommandType.Text;
                reader = db.cmd.ExecuteReader();
                while (reader.Read())
                {
                    status_id = reader.GetInt32(0);
                }
                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);
                return status_id;
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }
        public List<CombineModel> GetDataRequestDataByJoin(string folio_no)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "dbo.get_request_data_by_join";
                db.cmd.CommandType = CommandType.StoredProcedure;

                db.cmd.Parameters.Clear();
                db.AddInParameter(db.cmd, "folio_no", folio_no);
                reader = db.cmd.ExecuteReader();

                dt.Load(reader);
                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);

                return Common.Utility.ConvertDataTableToList<CombineModel>(dt);
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }
        public List<WorkflowHistoryModel> GetWorkflowHistories(string folio_no)
        {
            try
            {
                db.OpenConnection(ref conn);
                db.cmd.CommandText = $"SELECT * FROM dbo.workflow_history_log WHERE folio_no = '{folio_no}' ORDER BY action_date ASC";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                dt.Load(reader);

                db.CloseConnection(ref conn);
                db.CloseDataReader(reader);

                return Common.Utility.ConvertDataTableToList<WorkflowHistoryModel>(dt);
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }
    }
}
