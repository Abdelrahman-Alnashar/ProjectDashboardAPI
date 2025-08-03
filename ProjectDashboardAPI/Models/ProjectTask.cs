using System.ComponentModel.DataAnnotations;

namespace ProjectDashboardAPI.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required, MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        public int Status { get; set; }
        public string StatusName => ((Enums.TaskStatus)Status).ToString();
        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();
    }
}
