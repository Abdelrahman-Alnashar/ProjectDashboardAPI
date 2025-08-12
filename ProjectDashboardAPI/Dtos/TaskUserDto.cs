namespace ProjectDashboardAPI.Dtos
{
    public class TaskUserDto
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } // Assuming you want to include the user's name
    }
}