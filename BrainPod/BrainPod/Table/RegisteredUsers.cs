using System;
using System.Collections.Generic;
using System.Text;

namespace BrainPod.Table
{
    public class RegisteredUsers
    {
        public Guid UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool emailAuth { get; set; }
        public int emailAuthCode { get; set; }

    }
}
