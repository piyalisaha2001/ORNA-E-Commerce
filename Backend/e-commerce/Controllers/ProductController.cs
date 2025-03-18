using e_commerce.Data;
using e_commerce.DTO;
using e_commerce.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Controllers
{
   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }


        // GET: api/Product
        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }


        // GET: api/Product
        [HttpGet("Filter")]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts([FromQuery] string? categoryName, [FromQuery] decimal? price)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p => p.Category.CategoryName == categoryName);
            }

            if (price.HasValue)
            {
                query = query.Where(p => p.Price <= price.Value); /// == ha <= kia
            }

            var products = await query.ToListAsync();

            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        // POST: api/Product
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Products>> PostProduct(ProductDTO productDto)
        {
            var product = new Products
            {
                ProductId = productDto.ProductId,
                ProductName = productDto.ProductName,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId,
                ProductImage = productDto.ProductImage
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

       

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDTO)
        {
            var product = await _context.Products
                .Where(p => p.ProductId == id)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            // Check if a product with the same name or image URL already exists (excluding the current product)
            var existingProductByName = await _context.Products
                .Where(p => p.ProductName == productDTO.ProductName && p.ProductId != product.ProductId)
                .FirstOrDefaultAsync();

            var existingProductByImageUrl = await _context.Products
                .Where(p => p.ProductImage == productDTO.ProductImage && p.ProductId != product.ProductId)
                .FirstOrDefaultAsync();

            if (existingProductByName != null)
            {
                return BadRequest($"Product name {productDTO.ProductName} already exists.");
            }

            if (existingProductByImageUrl != null)
            {
                return BadRequest("Image URL already exists. Same URL not allowed.");
            }

            // Additional validations
            if (productDTO.Price <= 0)
            {
                return BadRequest("Product price must be greater than zero.");
            }

            if (string.IsNullOrEmpty(productDTO.ProductImage))
            {
                return BadRequest("Image URL is required.");
            }

            if (productDTO.ProductName.Length > 100)
            {
                return BadRequest("Product name must be 100 characters or less.");
            }

            if (productDTO.Description.Length > 500)
            {
                return BadRequest("Product description must be 500 characters or less.");
            }

            if (!Uri.IsWellFormedUriString(productDTO.ProductImage, UriKind.Absolute))
            {
                return BadRequest("Invalid image URL format.");
            }

            product.ProductName = productDTO.ProductName;
            product.Description = productDTO.Description;
            product.Price = productDTO.Price;
            product.ProductImage = productDTO.ProductImage;
            //product.CategoryId = category.CategoryId; // Use the category ID from the found category

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("ProductsByCategory/{category}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<Products>>> ProductsByCategoryName(string category)
        {
            var product = await _context.Products.Where(p => p.Category.CategoryName == category).ToListAsync();

            if(product == null)
            {
                return Ok(new { message = "No product exist" });
            }
            return Ok(product);

        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        //public async Task<bool> UpdateProduct(int productId, Products product)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
