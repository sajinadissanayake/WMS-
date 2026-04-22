using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using WorkforceShiftPortal.Data;

namespace WorkforceShiftPorta.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _db;

        public DashboardModel(AppDbContext db)
        {
            _db = db;
        }

        public int TotalUsers { get; set; }
        public int TotalManagers { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }

        public void OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
                Response.Redirect("/Account/Login");

            TotalUsers = _db.Users.Count();
            TotalManagers = _db.Users.Count(u => u.Role == "Manager");
            TotalEmployees = _db.Users.Count(u => u.Role == "Employee");
            TotalDepartments = _db.Departments.Count();
        }
    }
}
