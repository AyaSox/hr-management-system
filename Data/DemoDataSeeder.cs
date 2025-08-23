using Microsoft.EntityFrameworkCore;
using HRManagementSystem.Data;
using HRManagementSystem.Models;

namespace HRManagementSystem.Data
{
    public static class DemoDataSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            try
            {
                // Only seed if no data exists
                if (await context.Employees.AnyAsync())
                    return;

                // Create departments
                var departments = new[]
                {
                    new Department { Name = "Human Resources" },
                    new Department { Name = "Information Technology" },
                    new Department { Name = "Finance & Accounting" },
                    new Department { Name = "Sales & Marketing" },
                    new Department { Name = "Operations" }
                };

                context.Departments.AddRange(departments);
                await context.SaveChangesAsync();

                // Create sample employees
                var employees = new[]
                {
                    new Employee
                    {
                        FullName = "Sarah Johnson",
                        Email = "sarah.johnson@company.co.za",
                        DateHired = DateTime.Now.AddYears(-3),
                        Salary = 650000,
                        Gender = "Female",
                        DepartmentId = departments[0].DepartmentId
                    },
                    new Employee
                    {
                        FullName = "Thabo Mthembu",
                        Email = "thabo.mthembu@company.co.za",
                        DateHired = DateTime.Now.AddYears(-2),
                        Salary = 750000,
                        Gender = "Male",
                        DepartmentId = departments[1].DepartmentId
                    },
                    new Employee
                    {
                        FullName = "Jessica Smith",
                        Email = "jessica.smith@company.co.za",
                        DateHired = DateTime.Now.AddMonths(-8),
                        Salary = 580000,
                        Gender = "Female",
                        DepartmentId = departments[2].DepartmentId
                    },
                    new Employee
                    {
                        FullName = "Mpho Molefe",
                        Email = "mpho.molefe@company.co.za",
                        DateHired = DateTime.Now.AddYears(-1),
                        Salary = 620000,
                        Gender = "Male",
                        DepartmentId = departments[3].DepartmentId
                    },
                    new Employee
                    {
                        FullName = "Lisa Van Der Merwe",
                        Email = "lisa.vandermerwe@company.co.za",
                        DateHired = DateTime.Now.AddMonths(-4),
                        Salary = 540000,
                        Gender = "Female",
                        DepartmentId = departments[4].DepartmentId
                    }
                };

                context.Employees.AddRange(employees);
                await context.SaveChangesAsync();

                Console.WriteLine("? Demo data seeded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Demo data seeding error: {ex.Message}");
            }
        }
    }
}