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
                if (!SaveHeaderResponse.Success)
                {
                    return new CommonResponseModel { Success = false, Message = SaveHeaderResponse.Message };
                }
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

        public StationaryRequestHeader GetDataHeader(int ID)
        {
            try
            {
                using(var conn = new SqlConnection())
                {
                    conn.Open();
                    using(var cmd = new SqlCommand("usp_GetHeaderData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);
                        using(var reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            return dataTable.Rows.Count > 0 ?
                                Utility.ConvertDataTableToList<StationaryRequestHeader>(dataTable)[0] :
                                new StationaryRequestHeader();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Utility.InsertErrorLog("Get Data Header", Module_Name, ID, ex.Message);
                return new StationaryRequestHeader();
            }
        }
        public List<StationaryRequestDetail> GetDataDetails(int Header_ID)
        {
            try
            {
                using(var conn = new SqlConnection(Utility.GetSQLConnection()))
                {
                    conn.Open();
                    using(var cmd = new SqlCommand("[usp_GetDetailData]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@header_id", Header_ID);
                        using(var reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            return Utility.ConvertDataTableToList<StationaryRequestDetail>(dataTable);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Utility.InsertErrorLog("Get Data Detail", Module_Name, Header_ID, ex.Message);
                return new List<StationaryRequestDetail>();
            }
        }        
    }
}
