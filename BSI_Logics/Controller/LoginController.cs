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
using Utility = BSI_Logics.Common.Utility;

namespace BSI_Logics.Controller
{
    public class LoginController
    {
        readonly DatabaseManager db = new DatabaseManager();
        SqlConnection conn = new SqlConnection();
        SqlDataReader reader = null;

        private readonly string JwtKey = "LO6i4DuNxIpmGIpjCPRuPwx1NpA2Deuryh7HOsaw_b0";
        private readonly string JwtIssuer = "https://localhost:44313/";
        private readonly string JwtAudience = "https://localhost:44313/";

        public bool CheckEmailExists(string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email)) return false;
            int itemCount = 0;
            using(var conn = new SqlConnection(Utility.GetSQLConnection()))
            {
                conn.Open();
                using(var cmd = new SqlCommand("usp_Users_CheckEmailExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@email", email);
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itemCount = reader.GetInt32(0);
                        }
                        return itemCount > 0;
                    }
                }
            }
        }

        public void SignUp(string email, string password)
        {
            using(var conn = new SqlConnection(Utility.GetSQLConnection()))
            {
                conn.Open();
                using(var cmd = new SqlCommand("usp_Users_Insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", HashPassword(password));
                    cmd.ExecuteNonQuery();
                }
            }
        }

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

        public string GenerateLoginToken1(string email)
        {
            var LoginToken = "";
            int role_id = -1;
            int account_id = -1;

            db.OpenConnection(ref conn);
            db.cmd.CommandText = $"SELECT TOP 1 id, role_id FROM dbo.master_users WHERE email = '{email}'";
            db.cmd.CommandType = CommandType.Text;
            reader = db.cmd.ExecuteReader();

            if(reader != null)
            {
                while (reader.Read())
                {
                    account_id = reader.GetInt32(0);
                    role_id = reader.GetInt32(1);
                }
            }

            else
            {
                account_id = -1;
                role_id = -1;
            }

            if (account_id != 0)
            {
                var claims = new[]
                {
                    new Claim("Email", email),
                    new Claim("Account Id", account_id.ToString()),
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

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public LoginModel GetUser(string email)
        {
            DataTable dt = new DataTable();
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
                        dt.Load(reader);
                        return Utility.ConvertDataTableToList<LoginModel>(dt)[0];
                    }
                }
            }
        }

        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
