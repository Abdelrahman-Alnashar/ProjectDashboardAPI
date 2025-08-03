using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ProjectDashboardAPI.Data;
using ProjectDashboardAPI.Models;
using ProjectDashboardAPI.Services;
using ProjectDashboardAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace ProjectDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        // POST: api/users
        [HttpPost]
        public IActionResult Create([FromBody] UserRegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
            };

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAll), new { id = user.Id }, new
            {
                user.Id,
                user.Name,
                user.Email
            });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult getUser()
        {
            //var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            Console.WriteLine(userIdClaim);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);
            Console.WriteLine(userId);
            var user = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.Id, u.Name, u.Email })
                .FirstOrDefault();

            if (user == null)
                return NotFound();

            return Ok(user);

        }

    }
}
