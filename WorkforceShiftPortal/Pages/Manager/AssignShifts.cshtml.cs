using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkforceShiftPortal.Data;
using WorkforceShiftPortal.Models;

namespace WorkforceShiftPorta.Pages.Manager
{
    public class AssignShiftsModel : PageModel
    {
        private readonly AppDbContext _db;
        public AssignShiftsModel(AppDbContext db) { _db = db; }

        public List<User> Employees { get; set; } = new();

        [BindProperty] public int SelectedEmployeeId { get; set; }
        [BindProperty] public int SelectedMonth { get; set; } = DateTime.Now.Month;
        [BindProperty] public int SelectedYear { get; set; } = DateTime.Now.Year;
        [BindProperty] public Dictionary<int, string> DailyShifts { get; set; } = new();

        public int DaysInMonth => DateTime.DaysInMonth(SelectedYear, SelectedMonth);
        public string Message { get; set; } = "";

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");
            var managerId = HttpContext.Session.GetInt32("UserId");
            if (role != "Manager" || managerId == null) return RedirectToPage("/Account/Login");

            LoadEmployees(managerId.Value);
            return Page();
        }

        public IActionResult OnPost()
        {
            var role = HttpContext.Session.GetString("UserRole");
            var managerId = HttpContext.Session.GetInt32("UserId");
            if (role != "Manager" || managerId == null) return RedirectToPage("/Account/Login");

            LoadEmployees(managerId.Value);

            var employee = _db.Users.FirstOrDefault(u => u.Id == SelectedEmployeeId);
            if (employee == null)
            {
                Message = "Select a valid employee";
                return Page();
            }

            var department = _db.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId);
            if (department == null)
            {
                Message = "Employee has no department assigned";
                return Page();
            }

            foreach (var kvp in DailyShifts)
            {
                if (string.IsNullOrEmpty(kvp.Value)) continue;

                var date = new DateTime(SelectedYear, SelectedMonth, kvp.Key);

                // Check for existing shift for that day
                if (_db.Shifts.Any(s => s.EmployeeId == SelectedEmployeeId && s.Date == date)) continue;

                _db.Shifts.Add(new Shift
                {
                    EmployeeId = SelectedEmployeeId,
                    DepartmentId = department.Id,
                    ShiftType = kvp.Value,
                    Date = date
                });
            }

            _db.SaveChanges();
            Message = "Daily shifts assigned successfully!";
            return Page();
        }

        private void LoadEmployees(int managerId)
        {
            var department = _db.Departments.FirstOrDefault(d => d.ManagerId == managerId);
            if (department != null)
                Employees = _db.Users.Where(u => u.DepartmentId == department.Id && u.Role == "Employee").ToList();
        }
    }
}
