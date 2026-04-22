using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkforceShiftPortal.Data;
using WorkforceShiftPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkforceShiftPorta.Pages.Employee
{
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _db;
        public DashboardModel(AppDbContext db) { _db = db; }

        public string EmployeeName { get; set; } = "";
        public string EmployeeEmail { get; set; } = "";
        public string DepartmentName { get; set; } = "";

        public List<Shift> Shifts { get; set; } = new();

        public int CurrentMonth { get; set; } = DateTime.Now.Month;
        public int CurrentYear { get; set; } = DateTime.Now.Year;
        public string CurrentMonthName => new DateTime(CurrentYear, CurrentMonth, 1).ToString("MMMM");

        public void OnGet()
        {
            var employeeId = HttpContext.Session.GetInt32("UserId");
            if (employeeId == null) Response.Redirect("/Account/Login");

            var employee = _db.Users.Include(u => u.Department).FirstOrDefault(u => u.Id == employeeId);
            if (employee != null)
            {
                EmployeeName = employee.Name;
                EmployeeEmail = employee.Email;
                DepartmentName = employee.Department?.Name ?? "";
            }

            // Fetch daily shifts for current month
            Shifts = _db.Shifts
                .Where(s => s.EmployeeId == employeeId && s.Date.Month == CurrentMonth && s.Date.Year == CurrentYear)
                .OrderBy(s => s.Date)
                .ToList();
        }
    }
}
