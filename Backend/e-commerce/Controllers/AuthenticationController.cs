using e_commerce.Data;
using e_commerce.DTO;
using e_commerce.models;
using e_commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly AuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;


        public AuthenticationController(AppDbContext context, IConfiguration configuration, AuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _context = context;
            _configuration = configuration;
            _authenticationService = authenticationService;
            _logger = logger;
        }
        
        [HttpPost("SignUp")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDto)
        {
            if (await _context.Users.AnyAsync(u => u.username == registerUserDto.username))
            {
                _logger.LogWarning("Username is already taken.");
                return BadRequest("Username is already taken.");
            }

            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);

            var user = new User
            {
                FullName = registerUserDto.FullName,
                Email = registerUserDto.Email,
                username = registerUserDto.username,
                PasswordHash = passwordHash,
                PhoneNo = registerUserDto.PhoneNo,
                Country = registerUserDto.Country,
                City = registerUserDto.City,
                Address = registerUserDto.Address,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Registered successfully.");
            return Ok("Registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login loginUserDto)
        {
            if (!_authenticationService.ValidateCredentials(loginUserDto.username, loginUserDto.Password))
            {
                _logger.LogError("Invalid username or password.");
                return Unauthorized("Invalid username or password.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.username == loginUserDto.username);
            var token = GenerateJwtToken(user);

            _logger.LogInformation("Token Generated.");
            return Ok(new { token, user.UserId, user.FullName, user.username });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }     
    }
}

