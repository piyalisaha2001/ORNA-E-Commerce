using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.models
{
    public class Payments
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public string? CardNo { get; set; } // Made nullable
        public string? ExpiryDate { get; set; } // Made nullable
        public string? UPIId { get; set; } // Made nullable
        public string PaymentStatus { get; set; }

        public int ProductId { get; set; } // Added productId

        public Order Order { get; set; }
    }
}
