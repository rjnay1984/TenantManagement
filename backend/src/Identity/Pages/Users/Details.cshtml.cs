using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public UserViewModel AppUser { get; set; }

        public class UserViewModel
        {
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Email Address")]
            public string Email { get; set; }

            public string Role { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userResult = new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = roles[0]
            };

            AppUser = userResult;

            return Page();
        }
    }
}
