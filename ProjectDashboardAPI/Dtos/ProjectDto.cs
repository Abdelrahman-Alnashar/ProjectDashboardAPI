using ProjectDashboardAPI.Enums;

namespace ProjectDashboardAPI.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int User_id { get; set; }
        public int Status { get; set; }
        public string StatusName => ((ProjectStatus)Status).ToString(); //add enum later
        public string Prog_lang { get; set; } = string.Empty;
        public int Star_count { get; set; }
        public bool IsPublic { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<ProjectUserDto> ProjectUsers { get; set; } = new List<ProjectUserDto>();
        public List<ProjectTaskDto> ProjectTasks { get; set; } = new List<ProjectTaskDto>();

    }
}
