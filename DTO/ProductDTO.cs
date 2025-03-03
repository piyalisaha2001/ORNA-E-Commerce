using System.ComponentModel.DataAnnotations;

namespace e_commerce.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string ProductImage { get; set; }

        // Additional field to include category name
        public string CategoryName { get; set; }
    }
}