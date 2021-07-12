using Core.Entities;
using Identity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public DeleteModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ApplicationUser AppUser { get; set; }

        public string UserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            AppUser = await _userRepository.GetUserAsync(id);
            if (AppUser == null)
            {
                return NotFound();
            }

            var role = await _userRepository.GetUserRoleAsync(AppUser);
            UserRole = role;


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser = await _userRepository.GetUserAsync(id);
            if (AppUser == null)
            {
                return NotFound();
            }

            var role = await _userRepository.GetUserRoleAsync(AppUser);
            UserRole = role;

            var result = await _userRepository.DeleteUserAsync(AppUser);
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
