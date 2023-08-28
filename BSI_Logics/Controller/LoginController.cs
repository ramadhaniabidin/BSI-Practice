using BSI_Logics.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Controller
{
    public class LoginController
    {
        DatabaseManager db = new DatabaseManager();
        SqlConnection conn = new SqlConnection();
        
        public string GetLoginToken()
        {
            var returnedOutput = "";
            return returnedOutput;
        }
    }
}
