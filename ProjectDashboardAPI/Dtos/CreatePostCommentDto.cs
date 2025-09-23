using System.ComponentModel.DataAnnotations;

namespace ProjectDashboardAPI.Dtos
{
    public class CreatePostCommentDto
    {
        [Required, MaxLength(1000)]
        public string Comment { set; get; }
        public int? ParentId { get; set; }
    }
}