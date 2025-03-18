using System.Collections.Generic;
using System.Linq;

namespace e_commerce.DTO
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal GrandTotal => Items.Sum(item => item.TotalPrice);
    }
}



