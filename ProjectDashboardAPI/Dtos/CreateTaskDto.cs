using System.ComponentModel.DataAnnotations;

namespace ProjectDashboardAPI.Dtos
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } = 1;

        public DateTime? Deadline { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        public List<int> TaskUsers { get; set; } = new();

    }
}
