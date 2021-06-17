using Identity.Data;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IList<ApplicationUserViewModel> AppUsers { get; set; }

        public async Task OnGetAsync()
        {
            var query = await (
                from u in _context.Users
                join ur in _context.UserRoles on u.Id equals ur.UserId
                from r in _context.Roles.Where(r => r.Id == ur.RoleId)
                select new ApplicationUserViewModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = r
                }).ToListAsync();

            AppUsers = query;
        }
    }
}
