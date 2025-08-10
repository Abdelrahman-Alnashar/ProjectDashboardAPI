using Microsoft.AspNetCore.Mvc;
using ProjectDashboardAPI.Models;
using ProjectDashboardAPI.Data;
using ProjectDashboardAPI.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ProjectDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/projects
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var projects = _context.Projects
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    User_id = p.User_id,
                    Status = p.Status,
                    Prog_lang = p.Prog_lang,
                    Star_count = p.Star_count,
                    IsPublic = p.IsPublic,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Tags = p.Tags
                })
                .ToList();

            return Ok(projects);
        }

        // POST: api/projects
        [HttpPost]
        public IActionResult Create([FromBody] CreateProjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                User_id = dto.User_id,
                Status = dto.Status,
                Prog_lang = dto.Prog_lang,
                Star_count = dto.Star_count,
                IsPublic = dto.IsPublic,
                Tags = dto.Tags,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            _context.SaveChanges();

            var projectUser = new ProjectUser
            {
                UserId = dto.User_id,
                ProjectId = project.Id
            };

            _context.ProjectUsers.Add(projectUser);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAll), new { id = project.Id }, new
            {
                project.Id,
                project.Name,
                project.Description,
                project.User_id,
                project.Status,
                project.Prog_lang,
                project.Star_count,
                project.IsPublic,
                project.CreatedAt,
                project.UpdatedAt,
                Tags = project.Tags
            });
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetProject(int id)
        {
            var project = _context.Projects
                .Where(p => p.Id == id)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    User_id = p.User_id,
                    Status = p.Status,
                    Prog_lang = p.Prog_lang,
                    Star_count = p.Star_count,
                    IsPublic = p.IsPublic,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Tags = p.Tags
                })
                .FirstOrDefault();

            if (project == null)
                return NotFound();

            return Ok(project);
        }
    }
}
