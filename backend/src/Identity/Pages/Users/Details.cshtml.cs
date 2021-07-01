using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Identity.Interfaces;
using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public DetailsModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ApplicationUserViewModel AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var role = await _userRepository.GetUserRoleAsync(user);

            var userResult = new ApplicationUserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = role
            };

            AppUser = userResult;

            return Page();
        }
    }
}
