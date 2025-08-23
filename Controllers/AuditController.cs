using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRManagementSystem.Data;
using HRManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using X.PagedList.Extensions;

namespace HRManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditController : Controller
    {
        private readonly AppDbContext _context;

        public AuditController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchUser, string searchTable, int? page)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchUser))
                query = query.Where(a => a.UserName.Contains(searchUser));

            if (!string.IsNullOrWhiteSpace(searchTable))
                query = query.Where(a => a.TableName.Contains(searchTable));

            query = query.OrderByDescending(a => a.Timestamp);

            int pageNumber = page ?? 1;
            int pageSize = 20;

            ViewBag.SearchUser = searchUser;
            ViewBag.SearchTable = searchTable;

            var auditLogs = await query.ToListAsync();
            return View(auditLogs.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> Details(int id)
        {
            var auditLog = await _context.AuditLogs.FindAsync(id);
            if (auditLog == null)
            {
                return NotFound();
            }

            return View(auditLog);
        }
    }
}