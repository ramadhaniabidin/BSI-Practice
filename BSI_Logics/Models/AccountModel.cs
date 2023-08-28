using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Models
{
    public class AccountModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int role_id { get; set; }
        public string email { get; set; }
        public string department { get; set; }
        public string role { get; set; }
    }
}
