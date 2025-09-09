using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace ProjectDashboardAPI.Models
{
    public class CommunityApplication
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public CommunityPost Post { get; set; }
        public int ApplicantId { get; set; }
        public User Applicant { get; set; }
        [MaxLength(1000)]
        public string Message { get; set; }
        public int Status { get; set; } = 1;
        public int ApproverId { get; set; }
        public User ApprovedBy { get; set; }
        public DateTime? DecidedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}