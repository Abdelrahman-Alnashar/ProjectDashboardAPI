namespace ProjectDashboardAPI.Models
{

    public class TaskComment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public ProjectTask ProjectTask { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // public ProjectTask ProjectTask { get; set; }
        // public User User { get; set; }

    }
}