﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public string Department { get; set; }
    }
}
