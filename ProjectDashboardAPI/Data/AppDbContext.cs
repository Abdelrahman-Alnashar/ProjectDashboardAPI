using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using ProjectDashboardAPI.Models;

namespace ProjectDashboardAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

//         rm -rf Migrations
// dotnet ef migrations add InitialCreate
// dotnet ef database update

        public DbSet<Project> Projects => Set<Project>();

        public DbSet<User> Users => Set<User>();

        public DbSet<ProjectUser> ProjectUsers { get; set; }

        public DbSet<ProjectTask> ProjectTasks { get; set; }

        public DbSet<TaskUser> TaskUsers { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        
        public DbSet<CommunityPost> CommunityPosts { get; set; }
        public DbSet<CommunityApplication> CommunityApplications { get; set; }
        public DbSet<CommunityComment> CommunityComments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties()
                            .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
                {
                    property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                        v => v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                }
            }

            // psql -U appuser -d projectdashboard -h localhost

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                .Property(p => p.IsPublic)
                .HasDefaultValue(true);

            modelBuilder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId });

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId);

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<TaskUser>()
                .HasKey(tu => new { tu.TaskId, tu.UserId });

            modelBuilder.Entity<TaskUser>()
                .HasOne(t => t.Task)
                .WithMany(tu => tu.TaskUsers)
                .HasForeignKey(tu => tu.TaskId);

            modelBuilder.Entity<TaskUser>()
                .HasOne(t => t.User)
                .WithMany(tu => tu.TaskUsers)
                .HasForeignKey(tu => tu.UserId);

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.ProjectTask)
                .WithMany(t => t.TaskComments)
                .HasForeignKey(tc => tc.TaskId);

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.User)
                .WithMany(u => u.TaskComments)
                .HasForeignKey(tc => tc.UserId);

            modelBuilder.Entity<CommunityPost>()
                .HasOne(cp => cp.CreatedBy)
                .WithMany(u => u.CommunityPosts)
                .HasForeignKey(cp => cp.CreatedById);

            modelBuilder.Entity<CommunityPost>()
                .HasOne(cp => cp.ProjectTask)
                .WithMany()
                .HasForeignKey(cp => cp.ProjectTaskId);

            modelBuilder.Entity<CommunityApplication>()
                .HasOne(ca => ca.Post)
                .WithMany(p => p.Applications)
                .HasForeignKey(ca => ca.PostId);

            modelBuilder.Entity<CommunityApplication>()
                .HasOne(cp => cp.Applicant)
                .WithMany(u => u.CommunityApplications)
                .HasForeignKey(cp => cp.ApplicantId);

            modelBuilder.Entity<CommunityApplication>()
                .HasOne(ca => ca.ApprovedBy)
                .WithMany()
                .HasForeignKey(ca => ca.ApproverId);

            modelBuilder.Entity<CommunityComment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<CommunityComment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.CommunityComments)
                .HasForeignKey(c => c.AuthorId);

            modelBuilder.Entity<CommunityComment>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentId);

            // modelBuilder.Entity<ProjectTask>()
            //     .HasOne<User>()
            //     .WithMany()
            //     .HasForeignKey(t => t.UserId);

        }

    }
}
