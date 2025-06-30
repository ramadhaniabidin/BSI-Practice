using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Models
{
    public class ListDataModel
    {
        public int ID { get; set; }
        public string Transaction_Number { get; set; }
        public string Requestor { get; set; }
        public string Status { get; set; }
        public string Current_Approver_Role { get; set; }
        public string Current_Approver { get; set; }
        public string Current_Approver_Email { get; set; }
        public string CSS_Class { get; set; }
        public int Total_Stage { get; set; }
        public int Current_Stage { get; set; }
    }
}
