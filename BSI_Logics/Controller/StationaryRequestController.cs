using BSI_Logics.Common;
using BSI_Logics.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Controller
{
    public class StationaryRequestController
    {
        private readonly string JwtKey = "LO6i4DuNxIpmGIpjCPRuPwx1NpA2Deuryh7HOsaw_b0";
        private readonly string JwtIssuer = "https://localhost:44313/";
        private readonly string JwtAudience = "https://localhost:44313/";

        DatabaseManager db = new DatabaseManager();
        SqlConnection conn = new SqlConnection();
        SqlDataReader reader = null;
        DataTable dt = new DataTable();

        public AccountModel GetCurrentLoginData(string loginToken)
        {
            try
            {
                int account_id = -1;
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = JwtIssuer,
                    ValidAudience = JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey)),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                ClaimsPrincipal claimsPrincipal = tokenHandler.
                    ValidateToken(loginToken, validationParameters, out SecurityToken validatedToken);
                var claims = claimsPrincipal.Claims;
                if(claims != null)
                {
                    account_id = Convert.ToInt32(claims.FirstOrDefault(c => c.Type == "Account Id")?.Value);
                }
                else
                {
                    account_id = -1;
                }

                Debug.WriteLine($"Account Id = {account_id}");

                dt = new DataTable();
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "SELECT u.*, r.name role FROM dbo.master_users u " +
                    "LEFT JOIN dbo.master_roles r ON u.role_id = r.id " +
                    $"WHERE u.id = '{account_id}'";
                db.cmd.CommandType = CommandType.Text;
                reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                if(dt.Rows.Count > 0)
                {
                    return Common.Utility.ConvertDataTableToList < AccountModel > (dt)[0];
                }
                else
                {
                    return new AccountModel();
                }
            }

            catch (Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }

        }
        public List<StationaryItemsModel> GetAllStationary()
        {
            try
            {
                dt = new DataTable();
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "SELECT * FROM dbo.inventory_stationary";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                if (dt.Rows.Count > 0)
                {
                    return Common.Utility.ConvertDataTableToList<StationaryItemsModel>(dt);
                }
                else
                {
                    return new List<StationaryItemsModel>();
                }
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }
        public List<string> GetApproverList()
        {
            try
            {
                List<string> output = new List<string>();

                db.OpenConnection(ref conn);
                db.cmd.CommandText = "SELECT B.name FROM dbo.master_approver A LEFT JOIN dbo.master_roles B ON A.order_no = B.id";
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                while (reader.Read())
                {
                    output.Add(reader.GetString(0));
                }

                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                return output;

            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }
        public StockAndUnitModel GetStockAndUnit(string item_name)
        {
            try
            {
                dt = new DataTable();
                db.OpenConnection(ref conn);
                db.cmd.CommandText = "SELECT SUM(stock) stock, uom FROM dbo.inventory_stationary" +
                    $" WHERE item_name = '{item_name}'" +
                    " GROUP BY uom";
                db.cmd.CommandType = CommandType.Text;
                reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseDataReader(reader);
                db.CloseConnection(ref conn);

                if (dt.Rows.Count > 0)
                {
                    return Common.Utility.ConvertDataTableToList<StockAndUnitModel>(dt)[0];
                }
                else
                {
                    return new StockAndUnitModel();
                }
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
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
            }
            catch(Exception ex)
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
                    "SELECT folio_no folio_no, applicant pic_name, '' comment, 'First Submit' action_name, GETDATE() action_date\n" +
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
        public void InsertApprovalLog(WorkflowHistoryModel model)
        {
            try
            {
                db.OpenConnection(ref conn, false);
                db.cmd.CommandText = $"INSERT INTO dbo.workflow_history_log\n" +
                    $"(folio_no, pic_name, comment, action_name, action_date)\n" +
                    $"VALUES\n" +
                    $"('{model.folio_no}', '{model.pic_name}', '{model.comment}', '{model.action_name}', '{model.action_date}')";
                db.cmd.CommandType = CommandType.Text;
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
