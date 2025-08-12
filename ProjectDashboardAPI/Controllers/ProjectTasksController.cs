using Microsoft.AspNetCore.Mvc;
using ProjectDashboardAPI.Models;
using ProjectDashboardAPI.Data;
using ProjectDashboardAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace ProjectDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectTasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectTasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/projecttasks
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var tasks = _context.ProjectTasks
                .Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Deadline = t.Deadline,
                    ProjectId = t.ProjectId,
                    TaskUsers = t.TaskUsers
                        .Select(tu => new TaskUserDto
                        {
                            UserId = tu.UserId,
                            Name = tu.User.Name // Assuming you want to include the user's name
                        }).ToList()
                })
                .ToList();

            return Ok(tasks);
        }

        // POST: api/projecttasks
        [HttpPost]
        public IActionResult Create([FromBody] CreateProjectTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            Console.WriteLine(userIdClaim);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            var task = new ProjectTask
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = 1,
                Deadline = dto.Deadline,
                ProjectId = dto.ProjectId,
                UserId = userId,
                TaskUsers = dto.TaskUsers.Select(uid => new TaskUser { UserId = uid }).ToList()

            };

            // Now link TaskUsers via navigation
            // task.TaskUsers = dto.TaskUsers.Select(userId => new TaskUser
            // {
            //     UserId = userId,
            //     Task = task
            // }).ToList();

            _context.ProjectTasks.Add(task);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAll), new { id = task.Id }, task);
        }

        [HttpGet("{id}")]
        public IActionResult GetTask(int id)
        {
            var task = _context.ProjectTasks
                .Where(t => t.Id == id)
                .Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Deadline = t.Deadline,
                    TaskUsers = t.TaskUsers
                        .Select(tu => new TaskUserDto
                        {
                            UserId = tu.UserId,
                            Name = tu.User.Name
                        }).ToList()
                })
                .FirstOrDefault();

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateProjectTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = _context.ProjectTasks.Find(id);
            if (task == null)
                return NotFound();

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Deadline = dto.Deadline;
            task.ProjectId = dto.ProjectId;
            task.TaskUsers = dto.TaskUsers.Select(userId => new TaskUser { UserId = userId }).ToList();

            _context.SaveChanges();

            return NoContent();

        }
    }
}