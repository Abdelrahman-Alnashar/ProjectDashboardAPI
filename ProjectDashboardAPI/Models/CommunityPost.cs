using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace ProjectDashboardAPI.Models
{
    public class CommunityPost
    {
        public int Id { get; set; }
        public int? ProjectTaskId { get; set; }
        public ProjectTask? ProjectTask { get; set; }
        public int CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        [Required, MaxLength(150)]
        public string Title { get; set; } = String.Empty;
        [Required, MaxLength(2000)]
        public string Body { get; set; }
        public int Visiblity { get; set; }
        public int Status { get; set; } = 1;
        [NotMapped]
        public List<string> Tags { get; set; } = new List<string>();

        public string TagsJson
        {
            get => JsonSerializer.Serialize(Tags);
            set => Tags = string.IsNullOrEmpty(value)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(value);
        }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<CommunityApplication> Applications { get; set; } = new List<CommunityApplication>();
        public ICollection<CommunityComment> Comments { get; set; } = new List<CommunityComment>();
    }
}