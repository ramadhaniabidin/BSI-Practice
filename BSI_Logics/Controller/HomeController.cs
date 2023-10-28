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
        DatabaseManager db = new DatabaseManager();
        SqlConnection conn = new SqlConnection();
        SqlDataReader reader = null;
        DataTable dt = new DataTable();

        public List<HomeModel> GetRequestList(int current_login_role_id)
        {
            try
            {
                string query = "";
                query = "SELECT header.id, header.folio_no, header.status_id, s.name status_name, header.created_by, \n" +
                    "header.created_date, header.current_approver_role, isnull(r.name, '') current_approver, isnull(u.name, '') approver_name\n" +
                    "FROM stationary_request_header header\n" +
                    "JOIN master_status s ON header.status_id = s.id\n" +
                    "LEFT JOIN master_roles r ON header.current_approver_role = r.id" +
                    "LEFT JOIN master_users u ON r.id = u.role_id\n" +
                    "WHERE header.status_id != 6";

                string condition = "";
                if(current_login_role_id == 0)
                {
                    condition = "";
                }
                else
                {
                    condition = $" AND current_approver_role = {current_login_role_id}";
                }

                query += condition;

                db.OpenConnection(ref conn, false);
                db.cmd.CommandText = query;
                db.cmd.CommandType = CommandType.Text;

                reader = db.cmd.ExecuteReader();
                dt.Load(reader);
                db.CloseConnection(ref conn, false);
                db.CloseDataReader(reader);

                return Utility.ConvertDataTableToList<HomeModel>(dt);
            }
            catch(Exception ex)
            {
                db.CloseConnection(ref conn);
                throw ex;
            }
        }
    }
}
