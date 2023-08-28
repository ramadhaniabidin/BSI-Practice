using BSI_Logics.Common;
using BSI_Logics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace BSI_Logics.Controller
{
    public class LoginController
    {
        DatabaseManager db = new DatabaseManager();
        SqlConnection conn = new SqlConnection();
        SqlDataReader reader = null;

        private readonly string JwtKey = "LO6i4DuNxIpmGIpjCPRuPwx1NpA2Deuryh7HOsaw_b0";
        private readonly string JwtIssuer = "https://localhost:44313/";
        private readonly string JwtAudience = "https://localhost:44313/";
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

        public string GenerateLoginToken(string email)
        {
            var LoginToken = "";
            int role_id = GetRoleId(email);
            if(role_id >= 0)
            {
                var claims = new[]
                {
                    new Claim("Email", email),
                    new Claim("Role Id", role_id.ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
                var login = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(JwtIssuer, 
                    JwtAudience, claims, expires: DateTime.UtcNow.AddHours(3), signingCredentials: login);
                var JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                LoginToken = JwtToken;
            }
            else
            {
                LoginToken = "";
            }
            return LoginToken;
        }
    }
}
