using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace HRManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DateHired { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Annual Salary")]
        public decimal Salary { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; }  // e.g., "Male", "Female", "Other"

        // FK
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Profile picture
        public string? ProfilePicturePath { get; set; }

        [NotMapped]
        public IFormFile? ProfilePicture { get; set; }
    }
}