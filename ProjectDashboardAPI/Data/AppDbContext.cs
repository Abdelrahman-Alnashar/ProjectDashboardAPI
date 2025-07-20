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

    }
}
