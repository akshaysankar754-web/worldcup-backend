using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Team>()
                .HasIndex(t => t.TeamName)
                .IsUnique();

            modelBuilder.Entity<Poll>()
                .HasIndex(p => p.UserId)
                .IsUnique(); // One User = One Vote Only

            // Seed Settings
            modelBuilder.Entity<Setting>().HasData(
                new Setting { Id = 1, ResultVisible = false, PollOpen = false }
            );

            // Seed Admin User (Password is 'admin')
            // Using a precomputed bcrypt hash for 'admin' to avoid migration issues.
            // Hash: $2a$11$wKz01234567890123456789012345678901234567890123456789
            // Wait, an invalid hash will cause login to fail. I will use a simple mechanism to seed it in Program.cs instead.
        }
    }
}
