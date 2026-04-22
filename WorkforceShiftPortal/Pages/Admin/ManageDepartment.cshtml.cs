using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using WorkforceShiftPortal.Data;
using WorkforceShiftPortal.Models;

namespace WorkforceShiftPorta.Pages.Admin
{
    public class ManageDepartmentsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ManageDepartmentsModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department NewDepartment { get; set; } = new Department();

        public List<Department> Departments { get; set; } = new List<Department>();

        public void OnGet()
        {
            Departments = _context.Departments.ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Departments = _context.Departments.ToList();
                return Page();
            }

            _context.Departments.Add(NewDepartment);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var dept = _context.Departments.FirstOrDefault(d => d.Id == id);
            if (dept != null)
            {
                _context.Departments.Remove(dept);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}
