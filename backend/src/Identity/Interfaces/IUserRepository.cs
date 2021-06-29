using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Interfaces
{
    public interface IUserRepository
    {
        Task<IList<ApplicationUserViewModel>> GetUsersAsync();
        Task<ApplicationUser> GetUserAsync(string id);
        Task<string> GetUserRoleAsync(ApplicationUser user);
        Task<IList<IdentityRole>> GetRolesAsync();
        Task<IdentityResult> CreateUserAsync(ApplicationUser user);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> AddUserClaimsAsync(ApplicationUser user);
    }
}
