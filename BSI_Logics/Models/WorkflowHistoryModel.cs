using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Models
{
    public class WorkflowHistoryModel
    {
        public int id { get; set; }
        public string folio_no { get; set; }
        public string pic_name { get; set; }
        public string comment { get; set; }
        public string action_name { get; set; }
        public DateTime action_date { get; set; }
    }
}
