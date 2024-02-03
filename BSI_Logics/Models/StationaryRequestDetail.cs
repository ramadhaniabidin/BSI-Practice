using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Models
{
    public class StationaryRequestDetail
    {
        //public int id { get; set; }
        public int header_id { get; set; }
        public int no { get; set; }
        public string item_name { get; set; }
        public string uom { get; set; }
        public int stock { get; set; }
        public int request_qty { get; set; }
        public string reason { get; set; }
    }
}
