using Microsoft.AspNetCore.Mvc;
using ProjectDashboardAPI.Models;
using ProjectDashboardAPI.Data;

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
        [HttpGet]
        public IActionResult GetAll()
        {
            var projects = _context.Projects.ToList();
            return Ok(projects);
        }

        // POST: api/projects
        [HttpPost]
        public IActionResult Create([FromBody] Project project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Projects.Add(project);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAll), new { id = project.Id }, project);
        }
    }
}
