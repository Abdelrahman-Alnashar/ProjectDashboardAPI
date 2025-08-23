
namespace ProjectDashboardAPI.Dtos
{
    public class CreateTaskCommentDto
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}