using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ApplicationUser AppUser { get; set; }

        public string UserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            AppUser = await _userManager.FindByIdAsync(id);
            if (AppUser == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(AppUser);
            UserRole = roles[0];


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser = await _userManager.FindByIdAsync(id);
            if (AppUser == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(AppUser);
            UserRole = roles[0];

            var result = await _userManager.DeleteAsync(AppUser);
            if (!result.Succeeded)
            {
                StatusMessage = $"Error: { result.Errors.First().Description }";
                return Page();
            }

            StatusMessage = "User deleted.";

            return RedirectToPage("./Index");
        }
    }
}
