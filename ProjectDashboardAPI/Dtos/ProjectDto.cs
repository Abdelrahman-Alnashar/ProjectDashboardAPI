namespace ProjectDashboardAPI.Dtos
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int User_id { get; set; }
        public int Status { get; set; }
        public string Prog_lang { get; set; } = string.Empty;
        public int Star_count { get; set; }
        public bool IsPublic { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

    }
}
