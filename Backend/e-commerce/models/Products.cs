using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        [ForeignKey("Categories")]
        public int CategoryId { get; set; }

        public Categories Category { get; set; }

        [Required(ErrorMessage = "Product image is required.")]
        [StringLength(255, ErrorMessage = "Product image URL can't be longer than 255 characters.")]
        public string ProductImage { get; set; }
        public ICollection<Feedback> Feedback { get; set; } 

    }
}
