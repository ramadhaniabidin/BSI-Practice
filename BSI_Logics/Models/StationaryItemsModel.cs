using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Models
{
    public class StationaryItemsModel
    {
        public int id { get; set; }
        public string id_item { get; set; }
        public string item_name { get; set; }
        public string uom { get; set; }
        public int stock { get; set; }
        public string reference_no { get; set; }
        public DateTime reference_date { get; set; }
    }
}
