using BSI_Logics.Common;
using BSI_Logics.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BSI_Logics.Controller
{
    public class HomeController
    {
        public List<ListDataModel> GetRequestList()
        {
            try
            {
                DataTable dt = new DataTable();
                using(var conn = new SqlConnection(Utility.GetSQLConnection()))
                {
                    conn.Open();
                    using(var cmd = new SqlCommand("usp_TransactionData_GetList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using(var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                            return Utility.ConvertDataTableToList<ListDataModel>(dt);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Utility.InsertErrorLog("Get Transaction Data", "Stationary Request", 0, ex.Message);
                throw;
            }
        }
        public List<WorkflowHistoryModel> HistoryLogs(int Transaction_ID)
        {
            try
            {
                DataTable dt = new DataTable();
                using(var _conn = new SqlConnection(Utility.GetSQLConnection()))
                {
                    _conn.Open();
                    using(var command = new SqlCommand("usp_GetHistoryLog", _conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Transaction_ID", Transaction_ID);
                        using(var reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                            return Utility.ConvertDataTableToList<WorkflowHistoryModel>(dt);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Utility.InsertErrorLog("Get History Log List", "Stationary Request", Transaction_ID, ex.Message);
                throw;
            }
        }
    }
}
