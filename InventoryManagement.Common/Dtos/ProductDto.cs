namespace InventoryManagement.Common.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal? Discount { get; set; }
        public int? CategoryId { get; set; } // যোগ করা হয়েছে
    }
}