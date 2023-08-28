using BSI_Logics.Common;
using BSI_Logics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BSI_Logics.Controller
{
    public class LoginController
    {
        DatabaseManager db = new DatabaseManager();
        SqlConnection conn = new SqlConnection();
        SqlDataReader reader = null;

        public int GetRoleId(string email)
        {
            //var returnedOutput = "";
            int role_id = -1;

            db.OpenConnection(ref conn);
            db.cmd.CommandText = $"SELECT TOP 1 role_id FROM master_users WHERE email = '{email}'";
            db.cmd.CommandType = CommandType.Text;
            reader = db.cmd.ExecuteReader();

            if(reader != null)
            {
                while (reader.Read())
                {
                    role_id = reader.GetInt32(0);
                }
            }

            else
            {
                role_id = -1;
            }

            reader.Close();
            db.CloseConnection(ref conn);
            return role_id;
        }
    }
}
