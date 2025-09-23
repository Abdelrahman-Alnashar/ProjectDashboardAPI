namespace ProjectDashboardAPI.Dtos
{
    public class CommunityCommentDto
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto CreatedBy { get; set; }

        public List<CommunityCommentDto> Replies { get; set; } = new();
    }
}