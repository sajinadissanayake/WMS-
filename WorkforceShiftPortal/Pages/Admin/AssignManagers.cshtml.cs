using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkforceShiftPortal.Data;
using WorkforceShiftPortal.Models;

namespace WorkforceShiftPorta.Pages.Admin
{
    public class AssignManagersModel : PageModel
    {
        private readonly AppDbContext _db;

        public AssignManagersModel(AppDbContext db)
        {
            _db = db;
        }

        public List<Department> Departments { get; set; } = new();
        public List<User> Managers { get; set; } = new();

        [BindProperty]
        public int SelectedDepartmentId { get; set; }

        [BindProperty]
        public int SelectedManagerId { get; set; }

        public string Message { get; set; } = "";

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
                return RedirectToPage("/Account/Login");

            LoadData();
            return Page();
        }

        public IActionResult OnPost()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
                return RedirectToPage("/Account/Login");

            LoadData();

            var department = _db.Departments.Find(SelectedDepartmentId);
            var manager = _db.Users.FirstOrDefault(u => u.Id == SelectedManagerId && u.Role == "Manager");

            if (department == null || manager == null)
            {
                Message = "Invalid department or manager selection.";
                return Page();
            }

            // Assign manager to department
            department.ManagerId = manager.Id;
            _db.SaveChanges();

            Message = $"Manager {manager.Name} assigned to department {department.Name} successfully.";

            return Page();
        }

        private void LoadData()
        {
            Departments = _db.Departments.ToList();
            Managers = _db.Users.Where(u => u.Role == "Manager").ToList();
        }
    }
}
