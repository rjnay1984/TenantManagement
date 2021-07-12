using Core.Entities;
using Identity.Interfaces;
using Identity.ViewModels;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public CreateModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ApplicationUserViewModel Input { get; set; }

        public IList<IdentityRole> AppRoles { get; set; }

        public async Task OnGetAsync()
        {
            AppRoles = await _userRepository.GetRolesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AppRoles = await _userRepository.GetRolesAsync();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                UserName = Input.Email,
                Email = Input.Email,
                EmailConfirmed = true
            };

            // Create the user
            IdentityResult result;
            result = await _userRepository.CreateUserAsync(user); // TODO: Add password generation
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            // Add the user to role
            result = await _userRepository.AddToRoleAsync(user, Input.Role);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            // Add claims to the user for the frontend
            if (!user.FirstName.IsNullOrEmpty() || !user.LastName.IsNullOrEmpty())
            {
                result = await _userRepository.AddUserClaimsAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return Page();
                }
            }

            StatusMessage = $"{Input.Email} created.";

            return RedirectToPage("./Index");
        }
    }
}
