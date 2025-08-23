using HRManagementSystem.Models;
using HRManagementSystem.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace HRManagementSystem.Services
{
    public class AuditService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string tableName, string action, int? recordId, object? oldValues = null, object? newValues = null, string? changes = null)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";
            var userName = user?.Identity?.Name ?? "System";

            var auditLog = new AuditLog
            {
                TableName = tableName,
                Action = action,
                RecordId = recordId,
                OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
                NewValues = newValues != null ? JsonSerializer.Serialize(newValues) : null,
                UserId = userId,
                UserName = userName,
                Changes = changes,
                Timestamp = DateTime.Now
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task LogEmployeeChangeAsync(string action, Employee employee, Employee? oldEmployee = null)
        {
            string? changes = null;
            
            if (oldEmployee != null && action == "UPDATE")
            {
                var changesList = new List<string>();
                
                if (oldEmployee.FullName != employee.FullName)
                    changesList.Add($"Name: '{oldEmployee.FullName}' ? '{employee.FullName}'");
                    
                if (oldEmployee.Email != employee.Email)
                    changesList.Add($"Email: '{oldEmployee.Email}' ? '{employee.Email}'");
                    
                if (oldEmployee.Salary != employee.Salary)
                    changesList.Add($"Salary: {oldEmployee.Salary:C} ? {employee.Salary:C}");
                    
                if (oldEmployee.DepartmentId != employee.DepartmentId)
                    changesList.Add($"Department changed");

                changes = string.Join("; ", changesList);
            }

            await LogAsync("Employees", action, employee.EmployeeId, oldEmployee, employee, changes);
        }
    }
}