using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ProjectDashboardAPI.Data;
using ProjectDashboardAPI.Models;
using ProjectDashboardAPI.Services;

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
        public IActionResult Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hasher = new PasswordHasher<User>();
            string hashedPassword = hasher.HashPassword(user, user.Password);
            user.Password = hashedPassword;

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
        }

    }
}
