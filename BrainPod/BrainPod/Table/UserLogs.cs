using System;
using System.Collections.Generic;
using System.Text;

namespace BrainPod.Table
{
    public class UserLogs
    {
        public Guid UserID { get; set; }
        public string logData { get; set; }
        public string sliderValue { get; set; }
        public string mood { get; set; }
        public string logDate { get; set; }
        public string logTime { get; set; }
        public string activities { get; set; }

    }
}
