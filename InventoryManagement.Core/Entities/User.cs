using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // পাসওয়ার্ড হ্যাশ স্টোর করা হবে
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}