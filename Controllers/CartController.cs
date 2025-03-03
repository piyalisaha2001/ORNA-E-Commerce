using e_commerce.Data;
using e_commerce.DTO;
using e_commerce.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        [HttpGet("{username}")]
        //[Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<CartDTO>> GetCart(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.username.ToLower() == username.ToLower());

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.UserId);

            if (cart == null)
            {
                return NotFound("Cart is empty.");
            }

            var cartDto = new CartDTO
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(ci => new CartItemDTO
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.ProductName,
                    ProductImageUrl = ci.Product.ProductImage,
                    ProductPrice = ci.Product.Price,
                    Quantity = ci.Quantity
                }).ToList()
            };

            return Ok(cartDto);
        }
        [HttpPost("AddItem/{username}")]
        //[Authorize(Roles = "User")]
        public async Task<ActionResult<CartDTO>> AddItemToCart(string username, int productId, int quantity)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.username.ToLower() == username.ToLower());

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.UserId);

            if (cart == null)
            {
                cart = new Cart { UserId = user.UserId, CreatedDate = DateTime.UtcNow };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                cartItem = new CartItems
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var cartDto = new CartDTO
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Items = cart.CartItems
                .Where(ci => ci.Product != null)
                .Select(ci => new CartItemDTO
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.ProductName,
                    ProductImageUrl = ci.Product.ProductImage,
                    ProductPrice = ci.Product.Price,
                    Quantity = ci.Quantity
                }).ToList()
            };

            return Ok(cartDto);
        }
       

        // DELETE: api/Cart/RemoveItem
        [HttpDelete("RemoveItem")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<CartDTO>> RemoveItemFromCart(string username, int productId)
        {
            // Fetch the user based on the username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userId = user.UserId;

            // Fetch the user's cart with products included
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product) // Include the Product entity
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            // Find the cart item
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                return NotFound("Product not found in cart.");
            }

            // Remove the item from the cart
            cart.CartItems.Remove(cartItem);
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Prepare the response DTO
            var cartDto = new CartDTO
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(ci => new CartItemDTO
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.ProductName,
                    ProductImageUrl = ci.Product.ProductImage,
                    ProductPrice = ci.Product.Price,
                    Quantity = ci.Quantity
                }).ToList()
            };

            return Ok(cartDto);
        }

        // PUT: api/Cart/UpdateItemQuantity
        [HttpPut("UpdateItemQuantity")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<CartDTO>> UpdateItemQuantity(string username, int productId, int quantity)
        {
            // Fetch the user based on the username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userId = user.UserId;

            // Fetch the user's cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product) // Include the Product details
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            // Find the cart item
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                return NotFound("Product not found in cart.");
            }

            // Update the quantity
            cartItem.Quantity = quantity;
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Prepare the response DTO
            var cartDto = new CartDTO
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(ci => new CartItemDTO
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.ProductName,
                    ProductImageUrl = ci.Product.ProductImage,
                    ProductPrice = ci.Product.Price,
                    Quantity = ci.Quantity,
                }).ToList()
            };

            return Ok(cartDto);
        }

        // POST: api/Cart/Checkout
        [HttpPost("Checkout")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Checkout(string username, PaymentsDTO paymentDetails)
        {
            var user = await _context.Users
                .Where(u => u.username.ToLower() == username.ToLower())
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var cartItems = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .Where(c => c.UserId == user.UserId)
                .SelectMany(c => c.CartItems)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return BadRequest("Cart is empty.");
            }

            var totalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);
            var deliveryCharge = 30; // Assuming a fixed delivery charge
            var finalAmount = totalAmount + deliveryCharge;

            // Assign the calculated amount to the paymentDetails.Amount
            paymentDetails.Amount = finalAmount;

            // Set the PaymentDate automatically
            paymentDetails.PaymentDate = DateTime.UtcNow;

            var orderNo = "ORDER" + new Random().Next(100000, 999999).ToString();
            var orderDate = DateTime.Now;

            var order = new Order
            {
                OrderDate = orderDate,
                UserId = user.UserId,
                TotalAmount = finalAmount,
                Products = cartItems.Select(ci => ci.Product).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var payment = new Payments
            {
                OrderId = order.OrderId, // Set the OrderId from the created order
                PaymentMethod = paymentDetails.PaymentMethod,
                Amount = paymentDetails.Amount, // Use the calculated amount
                PaymentDate = paymentDetails.PaymentDate, // Use the automatically set date
               // ProductId = paymentDetails.ProductId // Added productId
            };

            if (paymentDetails.PaymentMethod == "card")
            {
                if (string.IsNullOrEmpty(paymentDetails.CardNo) || string.IsNullOrEmpty(paymentDetails.ExpiryDate))
                {
                    return BadRequest("Please enter card number and expiry date.");
                }

                payment.CardNo = paymentDetails.CardNo;
                payment.ExpiryDate = paymentDetails.ExpiryDate;
                payment.PaymentStatus = "successful";
            }
            else if (paymentDetails.PaymentMethod == "upi")
            {
                if (string.IsNullOrEmpty(paymentDetails.UPIId))
                {
                    return BadRequest("Please enter UPI ID.");
                }

                payment.UPIId = paymentDetails.UPIId;
                payment.PaymentStatus = "successful";
            }
            else
            {
                return BadRequest("Invalid payment mode.");
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            foreach (var cartItem in cartItems)
            {
                // Decrease the stock of the product
                var product = await _context.Products.FindAsync(cartItem.ProductId);
                if (product != null)
                {
                    product.Stock -= cartItem.Quantity;
                    _context.Entry(product).State = EntityState.Modified;
                }

                _context.CartItems.Remove(cartItem);
            }

            await _context.SaveChangesAsync();

            var userAddress = string.Join(", ", user.Address);

            var cartItemsDto = new CartDTO
            {
                CartId = cartItems.First().CartId,
                UserId = user.UserId,
                Items = cartItems.Select(c => new CartItemDTO
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product.ProductName,
                    ProductImageUrl = c.Product.ProductImage,
                    ProductPrice = c.Product.Price,
                    Quantity = c.Quantity
                }).ToList()
            };

            var paymentResult = new PaymentResultDTO
            {
                CartItems = cartItemsDto,
                IsSuccess = true,
                Status = "Payment Successful",
                ErrorMessage = null,
                GrandTotal = totalAmount,
                DiscountAmount = 0,
                DiscountedAmount = totalAmount,
                DeliveryCharge = deliveryCharge,
                FinalAmount = finalAmount,
                PaymentMode = paymentDetails.PaymentMethod,
                Message = $"Your order will be delivered to {userAddress}"
            };

            return Ok(paymentResult);
        }
    }
}