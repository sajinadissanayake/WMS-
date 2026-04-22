using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkforceShiftPortal.Data;

namespace WorkforceShiftPortal.Pages.Admin
{
    public class ManageUsersModel : PageModel
    {
        private readonly AppDbContext _db;

        public ManageUsersModel(AppDbContext db)
        {
            _db = db;
        }

        public List<UserViewModel> Users { get; set; } = new();

        public class UserViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public string Role { get; set; } = "";
            public string DepartmentName { get; set; } = "";
        }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
                return RedirectToPage("/Account/Login");

            Users = _db.Users
                .Include(u => u.Department)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    DepartmentName = u.Department != null ? u.Department.Name : "Unassigned"
                })
                .ToList();

            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
                return RedirectToPage("/Account/Login");

            var user = _db.Users.Find(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
            }

            return RedirectToPage(); // reload page after deletion
        }
    }
}
