using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2Indvidual.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Manager" или "Employee"
    }
}
