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

        public DbSet<Project> Projects => Set<Project>();

        public DbSet<User> Users => Set<User>();

        public DbSet<ProjectUser> Projectusers { get; set; }

        public DbSet<ProjectTask> ProjectTasks { get; set; }

        public DbSet<TaskUser> TaskUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<ProjectTask>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.UserId);

        }

    }
}
