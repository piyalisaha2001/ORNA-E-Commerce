using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using e_commerce.DTO;

namespace e_commerce.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; } // Additional field to include user information
        public decimal TotalAmount { get; set; }

        public ICollection<ProductDTO> Products { get; set; } // Assuming ProductDto is already defined

    }
}


