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
            
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            Console.WriteLine(userIdClaim);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                User_id = userId,
                Status = dto.Status,
                Prog_lang = dto.Prog_lang,
                Star_count = dto.Star_count,
                IsPublic = dto.IsPublic,
                Tags = dto.Tags,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                // ProjectUsers = new List<ProjectUser>
                // {
                //     new ProjectUser { UserId = userId } // creator
                // }
            };

            project.ProjectUsers.Add(new ProjectUser
            {
                UserId = userId,
            });


            // var projectUser = new ProjectUser
            // {
            //     UserId = dto.User_id,
            //     Project = project

            // };

            _context.Projects.Add(project);
            // _context.ProjectUsers.Add(projectUser);
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
                Tags = project.Tags,
                ProjectUsers = project.ProjectUsers.Select(pu => pu.UserId).ToList()
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
                    Tags = p.Tags,

                    ProjectUsers = p.ProjectUsers
                        .Select(pu => new ProjectUserDto
                        {
                            UserId = pu.UserId,
                            UserName = pu.User.Name,
                        })
                        .ToList(),

                    ProjectTasks = p.ProjectTasks
                        .Select(pt => new ProjectTaskDto
                        {
                            Id = pt.Id,
                            ProjectId = pt.ProjectId,
                            Title = pt.Title,
                            Description = pt.Description,
                            Status = pt.Status,
                            Deadline = pt.Deadline,
                            CreatedAt = pt.CreatedAt,
                            TaskUsers = pt.TaskUsers
                                .Select(tu => new TaskUserDto
                                {
                                    UserId = tu.UserId,
                                    TaskId = tu.TaskId,
                                    Name = tu.User.Name
                                }).ToList(),
                            
                        }).ToList()
                })
                .FirstOrDefault();

            if (project == null)
                return NotFound();

            return Ok(project);
        }
    }
}
