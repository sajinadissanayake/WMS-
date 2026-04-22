using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkforceShiftPortal.Data;
using WorkforceShiftPortal.Models;
using System.Linq;

namespace WorkforceShiftPortal.Pages.Admin
{
    public class ManageUserFormModel : PageModel
    {
        private readonly AppDbContext _db;

        public ManageUserFormModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public User AppUser { get; set; } = new();

        public string ActionType { get; set; } = "Create";

        public List<SelectListItem> DepartmentList { get; set; } = new();

        public void OnGet(string action, int? id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
                Response.Redirect("/Account/Login");

            ActionType = action ?? "Create";

            // Load departments for dropdown
            DepartmentList = _db.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();

            // Edit existing user
            if (action == "Edit" && id.HasValue)
            {
                var user = _db.Users.Find(id.Value);
                if (user != null)
                {
                    AppUser = user;
                }
            }
        }

        public IActionResult OnPost()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
                return RedirectToPage("/Account/Login");

            if (!ModelState.IsValid)
            {
                // reload department list if validation fails
                DepartmentList = _db.Departments
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    }).ToList();
                return Page();
            }

            if (AppUser.Id == 0)
            {
                _db.Users.Add(AppUser);
            }
            else
            {
                var existingUser = _db.Users.Find(AppUser.Id);
                if (existingUser != null)
                {
                    existingUser.Name = AppUser.Name;
                    existingUser.Email = AppUser.Email;
                    existingUser.Password = AppUser.Password;
                    existingUser.Role = AppUser.Role;
                    existingUser.DepartmentId = AppUser.DepartmentId;
                }
            }

            _db.SaveChanges();
            return RedirectToPage("ManageUsers");
        }
    }
}
