using Identity.Data;
using Identity.Helpers;
using Identity.Models;
using Identity.ViewModels;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<EditModel> _logger;

        public EditModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<EditModel> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<IdentityRole> AppRoles { get; set; }

        public class InputModel
        {
            public string Id { get; set; }
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [EmailAddress]
            [Required]
            public string Email { get; set; }

            public string Role { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            Input = new InputModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = roles[0],
            };
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            AppRoles = await _roleManager.Roles.ToListAsync();
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            AppRoles = await _roleManager.Roles.ToListAsync(); // TODO: Have to add this here to keep it from blowing up. Not sure why.
            var user = await _userManager.FindByIdAsync(id);

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
                var emailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!emailResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, emailResult.Errors.First().Description);
                    return Page();
                }

                var userNameResult = await _userManager.SetUserNameAsync(user, Input.Email);
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

            var currentClaims = await _userManager.GetClaimsAsync(user);
            await _userManager.RemoveClaimsAsync(user, currentClaims);

            var userResult = await _userManager.UpdateAsync(user);

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
                await ClaimsManager.AddUserClaimsAsync(user, _userManager, _logger);
            }

            var currentUserRole = await _userManager.GetRolesAsync(user);
            if (Input.Role != currentUserRole[0])
            {
                var roleResult = await _userManager.RemoveFromRolesAsync(user, currentUserRole);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, roleResult.Errors.First().Description);
                    return Page();
                }

                roleResult = await _userManager.AddToRoleAsync(user, Input.Role);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, roleResult.Errors.First().Description);
                }
            }

            StatusMessage = $"{user.Email} updated.";

            return RedirectToPage("./Index");
        }
    }
}