using System.ComponentModel.DataAnnotations;

namespace ProjectDashboardAPI.Models
{

	public class User
	{
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; } = string.Empty;

		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		[MinLength(8)]
		public string Password { get; set; } = string.Empty;
        
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public string? RefreshToken { get; set; }

		public DateTime? RefreshTokenExpiresAt { get; set; }

        public ICollection<ProjectUser>? ProjectUsers { get; set; }
		public ICollection<TaskUser>? TaskUsers { get; set; }

    }
}
