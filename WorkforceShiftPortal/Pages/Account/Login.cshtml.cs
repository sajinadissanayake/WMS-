using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkforceShiftPortal.Data;
using WorkforceShiftPortal.Models;

namespace WorkforceShiftPortal.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _db;

        public LoginModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password";
                return Page();
            }

            // Store user role in session (or cookie)
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetInt32("UserId", user.Id);

            // Redirect based on role
            return user.Role switch
            {
                "Admin" => RedirectToPage("/Admin/Dashboard"),
                "Manager" => RedirectToPage("/Manager/Dashboard"),
                "Employee" => RedirectToPage("/Employee/Dashboard"),
                _ => Page(),
            };
        }
    }
}
