using Microsoft.AspNetCore.Mvc;
using ProjectDashboardAPI.Models;
using ProjectDashboardAPI.Data;
using ProjectDashboardAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace ProjectDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommunityController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<CommunityPostDto>>> GetPosts(
            [FromQuery] int? status,
            [FromQuery] string? tag,
            [FromQuery] string? q)
        {
            var query = _context.CommunityPosts
                .Include(p => p.CreatedBy)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(p => p.Status == status);

            if (!string.IsNullOrEmpty(tag))
                query = query.Where(p => p.Tags.Contains(tag));

            if (!string.IsNullOrEmpty(q))
                query = query.Where(p => p.Title.Contains(q) || p.Body.Contains(q));

            var posts = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new CommunityPostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Body = p.Body,
                    Status = p.Status,
                    Visibility = p.Visiblity,
                    CreatedAt = p.CreatedAt,
                    Tags = p.Tags,
                    CreatedBy = new UserDto
                    {
                        Id = p.CreatedBy.Id,
                        Name = p.CreatedBy.Name
                    }
                })
                .ToListAsync();

            return Ok(posts);
        }

        [HttpGet("posts/{id}")]
        public async Task<ActionResult<CommunityPostDto>> GetPost(int id)
        {
            var post = await _context.CommunityPosts
                .Include(p => p.CreatedBy)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

            var dto = new CommunityPostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Status = post.Status,
                Visibility = post.Visiblity,
                CreatedAt = post.CreatedAt,
                Tags = post.Tags,
                CreatedBy = new UserDto
                {
                    Id = post.CreatedBy.Id,
                    Name = post.CreatedBy.Name
                },
                Comments = post.Comments.Select(c => new CommunityCommentDto
                {
                    Id = c.Id,
                    Body = c.Comment,
                    CreatedAt = c.CreatedAt,
                    CreatedBy = new UserDto
                    {
                        Id = c.Author.Id,
                        Name = c.Author.Name
                    }
                }).ToList()
            };


            return Ok(dto);
        }


        [HttpPost("posts")]
        public async Task<ActionResult<CommunityPostDto>> CreatePost(CreateCommunityPostDto input)
        {
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            var post = new CommunityPost
            {
                Title = input.Title,
                Body = input.Body,
                Visiblity = input.Visibility,
                Status = 1,
                CreatedById = userId,
                ProjectTaskId = input.ProjectTaskId,
                Tags = input.Tags
            };

            _context.CommunityPosts.Add(post);
            await _context.SaveChangesAsync();

            var dto = new CommunityPostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Status = post.Status,
                Visibility = post.Visiblity,
                CreatedAt = post.CreatedAt,
                Tags = post.Tags,
                CreatedBy = new UserDto
                {
                    Id = post.CreatedById,
                    Name = (await _context.Users.FindAsync(post.CreatedById))?.Name
                }
            };

            return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, dto);
        }

        [HttpPatch("posts/{id}/status")]
        public async Task<IActionResult> UpdatePostStatus(int id, [FromBody] int status)
        {
            var post = await _context.CommunityPosts.FindAsync(id);
            if (post == null) return NotFound();

            post.Status = status;
            post.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("posts/{id}/applications")]
        public async Task<ActionResult<CommunityApplication>> ApplyToPost(
            int id, [FromBody] CommunityApplication application)
        {
            if (!await _context.CommunityPosts.AnyAsync(p => p.Id == id))
                return NotFound();

            application.PostId = id;
            application.Status = 1;
            application.CreatedAt = DateTime.UtcNow;

            _context.CommunityApplications.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
        }

        // GET: api/community/posts/5/applications
        [HttpGet("posts/{postId}/applications")]
        public async Task<ActionResult<IEnumerable<CommunityApplicationDto>>> GetApplications(int postId)
        {
            var apps = await _context.CommunityApplications
                .Include(a => a.Applicant)
                .Where(a => a.PostId == postId)
                .Select(a => new CommunityApplicationDto
                {
                    Id = a.Id,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt,
                    CreatedBy = new UserDto
                    {
                        Id = a.Applicant.Id,
                        Name = a.Applicant.Name
                    }
                })
                .ToListAsync();

            return Ok(apps);
        }

        // GET: api/community/applications/3
        [HttpGet("applications/{id}")]
        public async Task<ActionResult<CommunityApplication>> GetApplication(int id)
        {
            var app = await _context.CommunityApplications
                .Include(a => a.Applicant)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (app == null)
                return NotFound();

            return app;
        }

        // PATCH: api/community/applications/3
        [HttpPatch("applications/{id}")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] int status)
        {
            var app = await _context.CommunityApplications.FindAsync(id);
            if (app == null) return NotFound();

            app.Status = status;
            app.DecidedAt = DateTime.UtcNow;
            // app.DecidedById = currentUserId (todo: auth integration)

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/community/posts/5/comments
        [HttpPost("posts/{postId}/comments")]
        public async Task<ActionResult<CommunityComment>> AddComment(
            int postId,
            [FromBody] CreatePostCommentDto dto)
        {
            var postExists = await _context.CommunityPosts.AnyAsync(p => p.Id == postId);
            if (!postExists)
                return NotFound();

            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            var comment = new CommunityComment
            {
                PostId = postId,
                AuthorId = userId,
                Comment = dto.Comment,
                ParentId = dto.ParentId,
                CreatedAt = DateTime.UtcNow
            };

            _context.CommunityComments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetComments),
                new { postId = postId },
                comment
            );
        }

        // GET: api/community/posts/5/comments
        [HttpGet("posts/{postId}/comments")]
        public async Task<ActionResult<IEnumerable<CommunityCommentDto>>> GetComments(int postId)
        {
            var comments = await _context.CommunityComments
                .Include(c => c.Author)
                .Where(c => c.PostId == postId)
                .Select(c => new CommunityCommentDto
                {
                    Id = c.Id,
                    Body = c.Comment,
                    CreatedAt = c.CreatedAt,
                    CreatedBy = new UserDto
                    {
                        Id = c.Author.Id,
                        Name = c.Author.Name
                    },
                    // Replies will be filled later
                })
                .ToListAsync();

            var commentDict = comments.ToDictionary(c => c.Id);

            List<CommunityCommentDto> topLevelComments = new();

            foreach (var comment in comments)
            {
                var entity = await _context.CommunityComments.FindAsync(comment.Id);

                if (entity.ParentId.HasValue && commentDict.ContainsKey(entity.ParentId.Value))
                {
                    commentDict[entity.ParentId.Value].Replies.Add(comment);
                }
                else
                {
                    topLevelComments.Add(comment);
                }
            }

            return Ok(topLevelComments);
        }


    }
}