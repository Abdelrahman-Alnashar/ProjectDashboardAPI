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
                            Name = tu.User.Name
                        }).ToList()
                })
                .ToList();

            return Ok(tasks);
        }

        // POST: api/projecttasks
        [HttpPost("{id}/addTask")]
        public IActionResult AddTaskToProject(int id, [FromBody] CreateProjectTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            var task = new ProjectTask
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = 1,
                Deadline = dto.Deadline,
                ProjectId = id,
                UserId = userId,
                TaskUsers = dto.TaskUsers.Select(uid => new TaskUser { UserId = uid }).ToList()
            };

            _context.ProjectTasks.Add(task);
            _context.SaveChanges();

            var createdTaskDto = _context.ProjectTasks
                .Where(pt => pt.Id == task.Id)
                .Select(pt => new ProjectTaskDto
                {
                    Id = pt.Id,
                    ProjectId = id,
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
                        }).ToList()
                })
                .FirstOrDefault();

            return CreatedAtAction(nameof(GetAll), new { id = createdTaskDto.Id }, createdTaskDto);
        }

        [HttpGet("{projectId}/{taskId}")]
        public IActionResult GetTask(int projectId, int taskId)
        {
            var project = _context.Projects.Find(projectId);
            if (project == null)
                return NotFound();

            var task = _context.ProjectTasks
                .Where(t => t.Id == taskId && t.ProjectId == projectId)
                .Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Deadline = t.Deadline,
                    ProjectName = project.Name,
                    TaskUsers = t.TaskUsers
                        .Select(tu => new TaskUserDto
                        {
                            UserId = tu.UserId,
                            Name = tu.User.Name
                        }).ToList(),
                    TaskComments = t.TaskComments
                        .Select(tc => new TaskCommentDto
                        {
                            Id = tc.Id,
                            TaskId = tc.TaskId,
                            UserId = tc.UserId,
                            UserName = tc.User.Name,
                            Content = tc.Content,
                            CreatedAt = tc.CreatedAt
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = _context.ProjectTasks.Find(id);
            if (task == null)
                return NotFound();

            _context.ProjectTasks.Remove(task);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost("{taskId}/comments")]
        public IActionResult AddComment(int taskId, [FromBody] CreateTaskCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = _context.ProjectTasks.Find(taskId);
            if (task == null)
                return NotFound();

            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            var comment = new TaskComment
            {
                Content = dto.Content,
                UserId = userId,
                ProjectTask = task,
                CreatedAt = DateTime.UtcNow
            };

            _context.TaskComments.Add(comment);
            _context.SaveChanges();

            var user = _context.Users.FirstOrDefault(user => user.Id == userId);

            return CreatedAtAction(nameof(GetTask),
                new { projectId = task.ProjectId, taskId = task.Id },
                new TaskCommentDto
                {
                    Id = comment.Id,
                    TaskId = task.Id,
                    UserId = comment.UserId,
                    UserName = user.Name,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt
                });
        }

        [HttpPut("{taskId}/updateComment/{commentId}")]
        public IActionResult UpdateComment(int taskId, int commentId, [FromBody] CreateTaskCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = _context.TaskComments
                .FirstOrDefault(c => c.Id == commentId && c.TaskId == taskId);

            if (comment == null)
                return NotFound("Comment not found for this task");

            comment.Content = dto.Content;
            comment.CreatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok(new
            {
                comment.Content,
            });

        }

        [HttpDelete("{taskId}/deleteComment/{commentId}")]
        public IActionResult DeleteComment(int taskId, int commentId)
        {
            var comment = _context.TaskComments
                .FirstOrDefault(c => c.Id == commentId && c.TaskId == taskId);

            if (comment == null)
                return NotFound("Comment not found to be deleted");

            _context.TaskComments.Remove(comment);
            _context.SaveChanges();

            return NoContent();
        }
    }
}