using Microsoft.AspNetCore.Mvc;
using AuctionSystem.Data;
using AuctionSystem.Models;
using AuctionSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AuctionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwt;

        public AuthController(AppDbContext db, JwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User request)
        {
            if (await _db.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest("User already exists");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = HashPassword(request.PasswordHash)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok("User registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || user.PasswordHash != HashPassword(request.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _jwt.GenerateToken(user);
            return Ok(new { token });
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
