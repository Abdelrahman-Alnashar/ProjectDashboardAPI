using System.ComponentModel.DataAnnotations;

namespace ProjectDashboardAPI.Dtos
{
    public class CreateProjectTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        [Required]
        public int ProjectId { get; set; }

        public List<int> TaskUsers { get; set; } = new();

    }
}
