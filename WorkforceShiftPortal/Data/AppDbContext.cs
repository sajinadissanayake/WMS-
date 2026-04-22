using Microsoft.EntityFrameworkCore;
using WorkforceShiftPortal.Models;

namespace WorkforceShiftPortal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Shift> Shifts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-many relationship: Department -> Employees
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull); // optional, avoid cascade delete

            base.OnModelCreating(modelBuilder);
        }
    }
}
