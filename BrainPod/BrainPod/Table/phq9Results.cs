using System;
using System.Collections.Generic;
using System.Text;

namespace BrainPod.Table
{
    class phq9Results
    {
        public Guid UserID { get; set; }
        public int overallResult { get; set; }
        public string submissionDate { get; set; }
        public string submissionDateWithoutTime { get; set; }
        public string year { get; set; }
    }
}
