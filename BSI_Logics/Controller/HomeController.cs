using BSI_Logics.Common;
using BSI_Logics.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
