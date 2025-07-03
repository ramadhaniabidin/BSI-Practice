using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Models
{
    public class WorkflowHistoryModel
    {
        public int ID { get; set; }
        public string Transaction_No { get; set; }
        public string PIC_Name { get; set; }
        public string Role { get; set; }
        public string Comment { get; set; }
        public string Action_Name { get; set; }
        public DateTime Action_Date { get; set; }
    }
}
