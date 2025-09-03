namespace ProjectDashboardAPI.Dtos
{
    public class PatchProjectTaskDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int? Status { get; set; }
        // public int? Priority { get; set; }
        public List<int>? TaskUsers { get; set; }
    }
}