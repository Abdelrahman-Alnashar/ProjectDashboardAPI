using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ProjectDashboardAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int User_id { get; set; }
        public int Status { get; set; }
        public string Prog_lang { get; set; } = string.Empty;
        public int Star_count { get; set; }
        public bool IsPublic { get; set; } = true; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public List<string> Tags { get; set; } = new List<string>();

        public string TagsJson
        {
            get => JsonSerializer.Serialize(Tags);
            set => Tags = string.IsNullOrEmpty(value) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(value);
        }

        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    }
}
