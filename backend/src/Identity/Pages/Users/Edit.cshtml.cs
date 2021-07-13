using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public EditModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ApplicationUserDto Input { get; set; }

        public IList<IdentityRole> AppRoles { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            var role = await _userRepository.GetUserRoleAsync(user);

            Input = new ApplicationUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = role,
            };
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userRepository.GetUserAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{id}'.");
            }

            AppRoles = await _userRepository.GetRolesAsync();
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            AppRoles = await _userRepository.GetRolesAsync(); // TODO: Have to add this here to keep it from blowing up. Not sure why.
            var user = await _userRepository.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.Email != user.Email)
            {
                var emailResult = await _userRepository.SetEmailAsync(user, Input.Email);
                if (!emailResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, emailResult.Errors.First().Description);
                    return Page();
                }

                var userNameResult = await _userRepository.SetUserNameAsync(user, Input.Email);
                if (!userNameResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, userNameResult.Errors.First().Description);
                    return Page();
                }
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            var currentClaims = await _userRepository.GetUserClaimsAsync(user);
            if (currentClaims.Any())
            {
                var claimsResult = await _userRepository.RemoveUserClaimsAsync(user, currentClaims);
                if (!claimsResult.Succeeded)
                {
                    foreach(var error in claimsResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                        return Page();
                    }
                }
            }

            var userResult = await _userRepository.UpdateUserAsync(user);
            if (!userResult.Succeeded)
            {
                foreach (var error in userResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            if (!user.FirstName.IsNullOrEmpty() || !user.LastName.IsNullOrEmpty())
            {
                var claimsResult = await _userRepository.AddUserClaimsAsync(user);
                if (!claimsResult.Succeeded)
                {
                    foreach (var error in claimsResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                        return Page();
                    }
                }
            }

            var currentUserRole = await _userRepository.GetUserRoleAsync(user);
            if (Input.Role != currentUserRole)
            {
                var roleResult = await _userRepository.RemoveFromRoleAsync(user, currentUserRole);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, roleResult.Errors.First().Description);
                    return Page();
                }

                roleResult = await _userRepository.AddToRoleAsync(user, Input.Role);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, roleResult.Errors.First().Description);
                    return Page();
                }
            }

            StatusMessage = $"{user.Email} updated.";

            return RedirectToPage("./Index");
        }
    }
}