namespace ProjectDashboardAPI.Models
{
    public class TaskUser
    {
        public Guid TaskId { get; set; }
        public ProjectTask Task { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
