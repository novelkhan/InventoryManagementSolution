using System;
using System.Collections.Generic;

namespace InventoryManagement.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } // যোগ করা হয়েছে
        public DateTime CreatedAt { get; set; }
        public List<Product> Products { get; set; }
    }
}