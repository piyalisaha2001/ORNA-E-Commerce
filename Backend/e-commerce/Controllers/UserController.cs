using e_commerce.Data;
using e_commerce.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public ActionResult<models.User> GetUser(int id)
        {
            var user = _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new User
                {
                    UserId = u.UserId,
                    username = u.username,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNo = u.PhoneNo,
                    PasswordHash = u.PasswordHash
                })
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult<models.User> CreateUser(models.User u)
        {

            _context.Users.Add(u);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { id = u.UserId }, u);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public IActionResult UpdateUser(int id, models.User u)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.username = u.username;
            user.FullName = u.FullName;
            user.Email = u.Email;
            user.PhoneNo = u.PhoneNo;
            user.PasswordHash = u.PasswordHash;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}


