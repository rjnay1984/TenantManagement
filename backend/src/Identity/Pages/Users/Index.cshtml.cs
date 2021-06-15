using Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Message { get; set; }

        public IList<AppUser> AppUsers { get; set; }

        public class AppUser
        {
            public string Id { get; set; }
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            public string Email { get; set; }
            public string Role { get; set; }
        }

        public async Task OnGetAsync()
        {
            var query = await (from a in _context.Users
                               join ur in _context.UserRoles on a.Id equals ur.UserId
                               join r in _context.Roles on ur.RoleId equals r.Id
                               select new AppUser()
                               {
                                   Id = a.Id,
                                   Email = a.Email,
                                   FirstName = a.FirstName,
                                   LastName = a.LastName,
                                   Role = r.Name
                               }).ToListAsync();

            AppUsers = query;
        }
    }
}
