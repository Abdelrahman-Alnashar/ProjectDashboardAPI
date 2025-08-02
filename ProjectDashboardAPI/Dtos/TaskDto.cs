namespace ProjectDashboardAPI.Dtos
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public int Status { get; set; }
        public string StatusName => ((TaskStatus)Status).ToString(); //add enum later

        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<int> TaskUsers { get; set; }


    }
}
