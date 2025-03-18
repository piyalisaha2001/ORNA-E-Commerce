using e_commerce.Data;
using e_commerce.DTO;
using e_commerce.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Products)
                .ThenInclude(p => p.Category)
                .ToListAsync();

            var orderDtos = orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                UserId = o.UserId,
                TotalAmount = o.TotalAmount,
                Products = o.Products.Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    ProductImage = p.ProductImage,
                    CategoryName = p.Category.CategoryName
                }).ToList()
            }).ToList();

            return Ok(orderDtos);
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Products)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Products = order.Products.Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    ProductImage = p.ProductImage,
                    CategoryName = p.Category.CategoryName
                }).ToList()
            };

            return Ok(orderDto);
        }

        // POST: api/Order
        //[HttpPost]
        //[Authorize(Roles = "User")]
        //public async Task<ActionResult<OrderDto>> PostOrder(OrderDto orderDto)
        //{
        //    var order = new Order
        //    {
        //        OrderDate = orderDto.OrderDate,
        //        UserId = orderDto.UserId,
        //        TotalAmount = orderDto.TotalAmount,
        //        OrderId = orderDto.OrderId,
        //    };

        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();

        //    orderDto.OrderId = order.OrderId;

        //    return CreatedAtAction(nameof(GetOrder), new { id = orderDto.OrderId }, orderDto);
        //} 
    }
}
