using System.ComponentModel.DataAnnotations;

namespace ProjectDashboardAPI.Dtos
{
    public class CreateCommunityPostDto
    {
        public int? ProjectTaskId { get; set; }
        public int CreatedById { get; set; }
        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;
        [Required, MaxLength(2000)]
        public string Body { get; set; } = string.Empty;
        public int Visibility { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}