using System;
using System.Collections.Generic;
using System.Text;

namespace BrainPod.Table
{
    class cbtEntry
    {
        public Guid uID { get; set; }
        public Guid cbtEntryID { get; set; }
        public string thoughts { get; set; }
        public string evidence { get; set; }
        public string factsfeeling { get; set; }
        public string scenarioEvaluation { get; set; }
    }
}
