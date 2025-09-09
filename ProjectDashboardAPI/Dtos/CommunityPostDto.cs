namespace ProjectDashboardAPI.Dtos
{
    public class CommunityPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Status { get; set; }
        public int Visibility { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Tags { get; set; }
        public UserDto CreatedBy { get; set; }
        public List<CommunityCommentDto> Comments { get; set; } = new();
    }
}