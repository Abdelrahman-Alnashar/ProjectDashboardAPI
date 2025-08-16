namespace ProjectDashboardAPI.Dtos
{
    public class UpdateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Prog_lang { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}