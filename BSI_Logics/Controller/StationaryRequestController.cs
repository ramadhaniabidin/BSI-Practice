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
    }
}
