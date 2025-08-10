using Microsoft.AspNetCore.Mvc;
using ProjectDashboardAPI.Models;
using ProjectDashboardAPI.Data;
using ProjectDashboardAPI.Dtos;
using Microsoft.AspNetCore.Authorization;

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
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Deadline = t.Deadline,
                    ProjectId = t.ProjectId,
                    TaskUsers = t.TaskUsers.Select(u => u.UserId).ToList()
                })
                .ToList();

            return Ok(tasks);
        }

        // POST: api/projecttasks
        [HttpPost]
        public IActionResult Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = new ProjectTask
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Deadline = dto.Deadline,
                ProjectId = dto.ProjectId,
                TaskUsers = dto.TaskUsers.Select(userId => new TaskUser { UserId = userId }).ToList()
            };

            _context.ProjectTasks.Add(task);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAll), new { id = task.Id }, task);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var task = _context.ProjectTasks
                .Where(t => t.Id == id)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Deadline = t.Deadline,
                    ProjectId = t.ProjectId,
                    TaskUsers = t.TaskUsers.Select(u => u.UserId).ToList()
                })
                .FirstOrDefault();

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = _context.ProjectTasks.Find(id);
            if (task == null)
                return NotFound();

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.Deadline = dto.Deadline;
            task.ProjectId = dto.ProjectId;
            task.TaskUsers = dto.TaskUsers.Select(userId => new TaskUser { UserId = userId }).ToList();

            _context.SaveChanges();

            return NoContent();

        }
    }
}