using System;
using System.Collections.Generic;
using System.Text;

namespace BrainPod.Table
{
    class newEvent
    {
        public Guid userID { get; set; }
        public Guid EventID { get; set; }
        public string eventTitle { get; set; }
        public string reason { get; set; }
        public string eventDate { get; set; }
        public int anxietyLevel { get; set; }
        public int worryLevel { get; set; }
    }
}
