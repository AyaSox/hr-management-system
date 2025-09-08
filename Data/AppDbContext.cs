using Microsoft.EntityFrameworkCore;
using HRManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; } = default!;
        public DbSet<Department> Departments { get; set; } = default!;
        public DbSet<AuditLog> AuditLogs { get; set; } = default!;
        public DbSet<StatusChangeRequest> StatusChangeRequests { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global query filter for soft-deleted employees
            modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);

            // Configure self-referencing relationship for Line Manager
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.LineManager)
                .WithMany(e => e.DirectReports)
                .HasForeignKey(e => e.LineManagerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // Configure Department relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique indexes
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeNumber)
                .IsUnique();
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();
            modelBuilder.Entity<Department>()
                .HasIndex(d => d.Name)
                .IsUnique();

            // Audit default timestamp (note: for SQLite, this will be set from app code)
            modelBuilder.Entity<AuditLog>()
                .Property(a => a.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // StatusChangeRequest relationships
            modelBuilder.Entity<StatusChangeRequest>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for better performance
            modelBuilder.Entity<StatusChangeRequest>()
                .HasIndex(s => s.Status);

            modelBuilder.Entity<StatusChangeRequest>()
                .HasIndex(s => s.RequestedDate);
        }
    }
}