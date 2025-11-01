using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public string? SecurityQuestion { get; set; }
        public string? SecurityAnswer { get; set; }


    }

    public class Admin : User
    {
        // Additional properties or methods for Admin can be added here
    }
}
