using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Models
{
    public class HomeModel
    {
        public int id { get; set; }
        public string folio_no { get; set; }
        public int status_id { get; set; }
        public string status_name { get; set; }
        public string current_approver { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public string approver_name { get; set; }
    }
}
