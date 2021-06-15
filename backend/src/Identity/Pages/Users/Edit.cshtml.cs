using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Identity.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public EditModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<IdentityRole> AppRoles { get; set; }

        public class InputModel
        {
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [EmailAddress]
            [Required]
            public string Email { get; set; }

            public string Role { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await (
                from u in _context.Users.Where(u => u.Id == id)
                join au in _context.UserRoles on u.Id equals au.UserId
                join r in _context.Roles on au.RoleId equals r.Id
                select new InputModel()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = r.Name,
                }
            ).FirstOrDefaultAsync();
            AppRoles = await _roleManager.Roles.ToListAsync();
            Input = user;
            return Page();
        }
    }
}