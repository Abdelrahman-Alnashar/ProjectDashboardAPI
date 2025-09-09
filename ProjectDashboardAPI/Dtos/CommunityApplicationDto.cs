namespace ProjectDashboardAPI.Dtos
{
    public class CommunityApplicationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto CreatedBy { get; set; }
    }
}