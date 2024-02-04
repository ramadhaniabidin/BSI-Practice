using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI_Logics.Models
{
    public class WorkflowModel
    {
    }

    public class ResponseWorkflow
    {
        public string id { get; set; }
    }
    public class NintexWorkflowCloud
    {
        public string url { get; set; }
        public NWCParamModel param { get; set; }
    }

    public class NWCParamModel
    {
        public StartData startData { get; set; }
        //public Options options { get; set; }
    }
    public class StartData
    {
        public int se_transactionid { get; set; }

    }
    public class Options
    {
        public string callbackUrl { get; set; }
        public string instanceToken { get; set; }
    }
}
