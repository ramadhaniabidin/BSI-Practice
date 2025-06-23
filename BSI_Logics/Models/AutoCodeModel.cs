using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Models
{
    public class AutoCodeModel
    {
        public int ID { get; set; }
        public string TableName { get; set; }
        public string DetailName { get; set; }
        public string ModuleCode { get; set; }
        public int TransID { get; set; }
        public string ListName { get; set; }
        public string ColumnName { get; set; }
        public int ItemID { get; set; }
        public string Format { get; set; }
        public int LengthOfString { get; set; }
        public int Generated { get; set; }
        public string SysMessage { get; set; }
        public string WorkflowName { get; set; }
    }

    public class GeneratedCode
    {
        public string AutoCode { get; set; }
    }
}
