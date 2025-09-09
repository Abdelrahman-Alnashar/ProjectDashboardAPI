using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace ProjectDashboardAPI.Models
{
    public class CommunityComment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public CommunityPost Post { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        [Required, MaxLength(1000)]
        public string Comment { get; set; }
        public int? ParentId { get; set; }
        public CommunityComment Parent { get; set; }
        public ICollection<CommunityComment> Replies { get; set; } = new List<CommunityComment>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}