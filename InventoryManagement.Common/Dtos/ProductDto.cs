using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Common.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        // [Required] // এটি সরানো যেতে পারে যদি CategoryName প্রয়োজন না হয়
        public string CategoryName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal? Discount { get; set; }
        public int? CategoryId { get; set; }
    }
}