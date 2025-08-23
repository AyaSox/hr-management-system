using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRManagementSystem.Data;
using HRManagementSystem.Models;
using HRManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using X.PagedList.Extensions;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HRManagementSystem.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AuditService _auditService;

        public EmployeesController(AppDbContext context, AuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string searchString, int? departmentId, string sortOrder, int? page)
        {
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSort = sortOrder == "date" ? "date_desc" : "date";

            var query = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(e => e.FullName.Contains(searchString) || e.Email.Contains(searchString));

            if (departmentId.HasValue)
                query = query.Where(e => e.DepartmentId == departmentId.Value);

            switch (sortOrder)
            {
                case "name_desc": query = query.OrderByDescending(e => e.FullName); break;
                case "date": query = query.OrderBy(e => e.DateHired); break;
                case "date_desc": query = query.OrderByDescending(e => e.DateHired); break;
                default: query = query.OrderBy(e => e.FullName); break;
            }

            ViewBag.Departments = await _context.Departments.OrderBy(d => d.Name).ToListAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;

            var employees = await query.ToListAsync();
            return View(employees.ToPagedList(pageNumber, pageSize));
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name");
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.ProfilePicture != null)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    Directory.CreateDirectory(folder);
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(employee.ProfilePicture.FileName)}";
                    var path = Path.Combine(folder, fileName);
                    using var stream = System.IO.File.Create(path);
                    await employee.ProfilePicture.CopyToAsync(stream);
                    employee.ProfilePicturePath = $"/images/{fileName}";
                }

                _context.Add(employee);
                await _context.SaveChangesAsync();

                // Log the creation
                await _auditService.LogEmployeeChangeAsync("INSERT", employee);

                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get old values for audit
                    var oldEmployee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeId == id);

                    if (employee.ProfilePicture != null)
                    {
                        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                        Directory.CreateDirectory(folder);
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(employee.ProfilePicture.FileName)}";
                        var path = Path.Combine(folder, fileName);
                        using var stream = System.IO.File.Create(path);
                        await employee.ProfilePicture.CopyToAsync(stream);
                        employee.ProfilePicturePath = $"/images/{fileName}";
                    }

                    _context.Update(employee);
                    await _context.SaveChangesAsync();

                    // Log the update
                    if (oldEmployee != null)
                    {
                        await _auditService.LogEmployeeChangeAsync("UPDATE", employee, oldEmployee);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                // Log before deletion
                await _auditService.LogEmployeeChangeAsync("DELETE", employee);
                
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Export to Excel
        public async Task<IActionResult> ExportExcel()
        {
            var employees = await _context.Employees.Include(e => e.Department).ToListAsync();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Employees");
            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Full Name";
            ws.Cell(1, 3).Value = "Email";
            ws.Cell(1, 4).Value = "Department";
            ws.Cell(1, 5).Value = "Annual Salary";
            ws.Cell(1, 6).Value = "Date Hired";
            ws.Cell(1, 7).Value = "Gender";

            for (int i = 0; i < employees.Count; i++)
            {
                var e = employees[i];
                int r = i + 2;
                ws.Cell(r, 1).Value = e.EmployeeId;
                ws.Cell(r, 2).Value = e.FullName;
                ws.Cell(r, 3).Value = e.Email;
                ws.Cell(r, 4).Value = e.Department?.Name;
                ws.Cell(r, 5).Value = e.Salary;
                ws.Cell(r, 6).Value = e.DateHired.ToShortDateString();
                ws.Cell(r, 7).Value = e.Gender;
            }

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Employees.xlsx");
        }

        // Export to PDF
        public async Task<IActionResult> ExportPdf()
        {
            var employees = await _context.Employees.Include(e => e.Department).ToListAsync();

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.Header().Text("Employee Report").Bold().FontSize(16).AlignCenter();

                    page.Content().Table(t =>
                    {
                        t.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(40);
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.ConstantColumn(80);
                            c.ConstantColumn(80);
                            c.ConstantColumn(60);
                        });

                        void Header(string txt) => t.Cell().Element(v => v.Padding(5).Background(Colors.Grey.Lighten3)).Text(txt).Bold();
                        Header("ID"); Header("Name"); Header("Email"); Header("Department"); Header("Annual Salary"); Header("Date Hired"); Header("Gender");

                        foreach (var e in employees)
                        {
                            void Cell(string txt) => t.Cell().Element(v => v.Padding(5).BorderBottom(1)).Text(txt);
                            Cell(e.EmployeeId.ToString());
                            Cell(e.FullName);
                            Cell(e.Email);
                            Cell(e.Department?.Name ?? "N/A");
                            Cell(e.Salary.ToString("C"));
                            Cell(e.DateHired.ToShortDateString());
                            Cell(e.Gender ?? "N/A");
                        }
                    });
                });
            });

            var bytes = doc.GeneratePdf();
            return File(bytes, "application/pdf", "Employees.pdf");
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}