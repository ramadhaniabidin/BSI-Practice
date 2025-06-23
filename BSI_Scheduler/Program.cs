using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSI_Logics.Common;
using BSI_Logics.Models;

namespace BSI_Scheduler
{
    public class Program
    {
        public static List<AutoCodeModel> GetAutoCodeList()
        {
            using(var conn = new SqlConnection(Utility.GetSQLConnection()))
            {
                conn.Open();
                using(var cmd = new SqlCommand("usp_AutoCodeBatch_GetList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    using(var reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Utility.ConvertDataTableToList<AutoCodeModel>(dt);
                    }
                }
            }
        }

        public static string GetGenereatedCode(AutoCodeModel item)
        {
            using(var conn = new SqlConnection(Utility.GetSQLConnection()))
            {
                conn.Open();
                using(var cmd = new SqlCommand("usp_Utility_AutoCounter", conn))        // SP nya belum dibuat!!!
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@FieldName", item.ColumnName);
                    cmd.Parameters.AddWithValue("@TableName", item.TableName);
                    cmd.Parameters.AddWithValue("@FieldCriteria", item.ColumnName);
                    cmd.Parameters.AddWithValue("@ValueCriteria", item.Format);
                    cmd.Parameters.AddWithValue("@LengthOfString", item.LengthOfString);
                    using(var reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return Utility.ConvertDataTableToList<GeneratedCode>(dt)[0].AutoCode;
                    }
                }
            }
        }

        public static void UpdateAutoCodeBatch(AutoCodeModel item)
        {
            using(var conn = new SqlConnection(Utility.GetSQLConnection()))
            {
                conn.Open();
                using(var cmd = new SqlCommand("usp_AutoCodeBatch_UpdateFlag", conn))       // SP nya belum dibuat
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", item.ID);
                    cmd.Parameters.AddWithValue("@Generated", 1);
                    cmd.Parameters.AddWithValue("@SysMessage", "Generated");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void GenerateCode()
        {
            var list = GetAutoCodeList();
            foreach(var item in list)
            {
                var isError = false;
                try
                {
                    string generatedCode = GetGenereatedCode(item);         // SP nya belum dibuat
                    UpdateAutoCodeBatch(item);                              // SP nya belum dibuat
                    // Update folio_no on header table
                }
                catch(Exception ex)
                {
                    isError = true;
                }
            }
        }

        public static void Main(string[] args)
        {
        }
    }
}
