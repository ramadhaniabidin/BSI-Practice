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

        public int GetLoginToken(AccountModel model)
        {
            //var returnedOutput = "";
            int role_id = 0;

            db.OpenConnection(ref conn);
            db.cmd.CommandText = $"SELECT TOP 1 role_id FROM master_users WHERE email = {model.email}";
            db.cmd.CommandType = CommandType.Text;
            reader = db.cmd.ExecuteReader();

            while (reader.Read())
            {
                role_id = reader.GetInt32(0);
            }
            reader.Close();
            db.CloseConnection(ref conn);
            return role_id;
        }
    }
}
