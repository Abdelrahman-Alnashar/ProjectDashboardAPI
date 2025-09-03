using System.Text.Json.Serialization;

namespace ProjectDashboardAPI.Models
{
    public class TaskUser
    {
        public int TaskId { get; set; }
        [JsonIgnore]
        public ProjectTask Task { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
