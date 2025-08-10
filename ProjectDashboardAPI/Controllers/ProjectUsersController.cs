using Microsoft.AspNetCore.Mvc;
using ProjectDashboardAPI.Models;
using ProjectDashboardAPI.Data;
using ProjectDashboardAPI.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ProjectDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectUsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectUsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/projectusers
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var projectUsers = _context.ProjectUsers
                .Select(pu => new
                {
                    pu.UserId,
                    pu.ProjectId,
                    UserName = pu.User.Name,
                    ProjectName = pu.Project.Name
                })
                .ToList();

            return Ok(projectUsers);
        }

        // POST: api/projectusers
        [HttpPost]
        public IActionResult Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var projectUser = new ProjectUser
            {
                UserId = dto.TaskUsers.FirstOrDefault(),
                ProjectId = dto.ProjectId
            };

            _context.ProjectUsers.Add(projectUser);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAll), new { id = projectUser.UserId }, projectUser);
        }
    }
}