using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkforceShiftPortal.Data;

namespace WorkforceShiftPorta.Pages.Manager
{
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _db;

        public DashboardModel(AppDbContext db)
        {
            _db = db;
        }

        public int TotalEmployees { get; set; }
        public int TotalShifts { get; set; }

        // List of assigned shifts for display
        public List<ShiftViewModel> AssignedShifts { get; set; } = new();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");
            var managerId = HttpContext.Session.GetInt32("UserId");

            if (role != "Manager" || managerId == null)
                return RedirectToPage("/Account/Login");

            // Get manager's department
            var department = _db.Departments.FirstOrDefault(d => d.ManagerId == managerId);
            if (department != null)
            {
                TotalEmployees = _db.Users.Count(u => u.DepartmentId == department.Id);
                TotalShifts = _db.Shifts.Count(s => s.DepartmentId == department.Id);

                // Load assigned shifts with employee names
                AssignedShifts = _db.Shifts
                    .Where(s => s.DepartmentId == department.Id)
                    .Include(s => s.Employee)
                    .OrderBy(s => s.Date)
                    .Select(s => new ShiftViewModel
                    {
                        EmployeeName = s.Employee != null ? s.Employee.Name : "Unknown",
                        ShiftType = s.ShiftType,
                        Date = s.Date
                    })
                    .ToList();
            }

            return Page();
        }
    }

    public class ShiftViewModel
    {
        public string EmployeeName { get; set; } = "";
        public string ShiftType { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
