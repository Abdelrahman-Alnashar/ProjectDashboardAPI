using ProjectDashboardAPI.Enums;

namespace ProjectDashboardAPI.Dtos
{
    public class ProjectTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int Status { get; set; }
        public string StatusName => ((ProjectTaskStatus)Status).ToString(); //add enum later

        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<TaskUserDto> TaskUsers { get; set; } = new();

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<TaskCommentDto> TaskComments { get; set; } = new();
    }
}
