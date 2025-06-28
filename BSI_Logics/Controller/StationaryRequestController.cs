using BSI_Logics.Common;
using BSI_Logics.Models;
using BSI_Logics.Models.Stationary_Request;
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
            DataTable dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(Utility.GetSQLConnection()))
                {
                    conn.Open();
                    string query = "usp_GetItemStockAndUnit";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@item_name", item_name);
                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                            return Utility.ConvertDataTableToList<StockAndUnitModel>(dt)[0];
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Utility.InsertErrorLog("Get Item Stock and Unit", Module_Name, 0, ex.Message);
                return new StockAndUnitModel { stock = 0, uom = string.Empty };
            }
        }

        public SaveHeaderModel SaveHeader(StationaryRequestHeader header)
        {
            try
            {
                int InsertedID = 0;
                using(var conn = new SqlConnection(Utility.GetSQLConnection()))
                {
                    conn.Open();
                    using(var cmd = new SqlCommand("usp_SaveUpdateHeader", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@applicant", header.applicant);
                        cmd.Parameters.AddWithValue("@created_by", header.created_by);
                        cmd.Parameters.AddWithValue("@current_approver_role", header.current_approver_role);
                        cmd.Parameters.AddWithValue("@department", header.department);
                        cmd.Parameters.AddWithValue("@employee_id", header.employee_id);
                        cmd.Parameters.AddWithValue("@extension", header.extension);
                        cmd.Parameters.AddWithValue("@folio_no", header.folio_no);
                        cmd.Parameters.AddWithValue("@modified_by", header.modified_by);
                        cmd.Parameters.AddWithValue("@role", header.role);
                        var outputParam = new SqlParameter("@out_id", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);
                        cmd.ExecuteNonQuery();
                        InsertedID = (outputParam.Value != DBNull.Value) ? Convert.ToInt32(outputParam.Value) : 0;
                    }
                }
                return new SaveHeaderModel
                {
                    Success = InsertedID > 0,
                    Message = InsertedID > 0 ? "OK" : "Save header failed",
                    InsertedID = InsertedID
                };
                
            }
            catch(Exception ex)
            {
                Utility.InsertErrorLog("Save Header", Module_Name, 0, ex.Message);
                return new SaveHeaderModel { InsertedID = 0, Message = ex.Message, Success = false };
            }
        }

        public CommonResponseModel SaveDetail(int Header_ID, StationaryRequestDetail detail, int no)
        {
            try
            {
                using(var conn = new SqlConnection(Utility.GetSQLConnection()))
                {
                    conn.Open();
                    using(var cmd = new SqlCommand("[usp_SaveUpdateDetail]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@header_id", Header_ID);
                        cmd.Parameters.AddWithValue("@no", no);
                        cmd.Parameters.AddWithValue("@item_name", detail.item_name);
                        cmd.Parameters.AddWithValue("@stock", detail.stock);
                        cmd.Parameters.AddWithValue("@request_qty", detail.request_qty);
                        cmd.Parameters.AddWithValue("@reason", detail.reason);
                        cmd.ExecuteNonQuery();
                        return new CommonResponseModel { Message = "OK", Success = true };
                    }
                }
            }
            catch(Exception ex)
            {
                return new CommonResponseModel { Message = ex.Message, Success = false };
            }
        }

        public CommonResponseModel SaveUpdate(StationaryRequestHeader header, List<StationaryRequestDetail> details)
        {
            try
            {
                var SaveHeaderResponse = SaveHeader(header);
                if (!SaveHeaderResponse.Success) return new CommonResponseModel { Success = false, Message = SaveHeaderResponse.Message };
                foreach(var detail in details)
                {
                    var SaveDetailResponse = SaveDetail(SaveHeaderResponse.InsertedID, detail, detail.no);
                    if (!SaveDetailResponse.Success)
                    {
                        return new CommonResponseModel { 
                            Success = false, 
                            Message = $"Erorr Save Detail at row {detail.no + 1} | {SaveDetailResponse.Message}" 
                        };
                    }
                }
                return new CommonResponseModel { Message = "OK", Success = true };
            }
            catch(Exception ex)
            {
                return new CommonResponseModel { Message = ex.Message, Success = false };
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
        
    }
}
