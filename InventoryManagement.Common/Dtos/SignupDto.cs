using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Common.Dtos
{
    public class SignupDto
    {
        public string Username { get; set; }
        public string Password { get; set; } // ক্লায়েন্ট থেকে পাসওয়ার্ড হিসেবে পাওয়া যাবে
        public string Email { get; set; }
    }
}