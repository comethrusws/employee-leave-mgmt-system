using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Admin User
            // Password: Admin@123
            
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                FullName = "Admin",
                Email = "admin@company.com",
                PasswordHash = "e86f78a8a3caf0b60d8e74e5942aa6d86dc150cd3c03338aef25b7d2d7e3acc7", // Hash for "Admin@123"
                Role = "Admin",
                CreatedAt = DateTime.Now
            });
        }
    }
}
