using HRManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HRManagementSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var perDept = await _context.Employees
                .Include(e => e.Department)
                .GroupBy(e => e.Department!.Name)
                .Select(g => new { Dept = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.DeptLabels = perDept.Select(x => x.Dept).ToList();
            ViewBag.DeptCounts = perDept.Select(x => x.Count).ToList();

            var gender = await _context.Employees
                .GroupBy(e => e.Gender)
                .Select(g => new { Gender = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.Genders = gender.Select(x => x.Gender ?? "Unknown").ToList();
            ViewBag.GenderCounts = gender.Select(x => x.Count).ToList();

            ViewBag.TotalEmployees = await _context.Employees.CountAsync();
            ViewBag.TotalDepartments = await _context.Departments.CountAsync();
            ViewBag.AvgSalary = await _context.Employees.AnyAsync()
                ? await _context.Employees.AverageAsync(e => e.Salary)
                : 0;

            return View();
        }
    }
}