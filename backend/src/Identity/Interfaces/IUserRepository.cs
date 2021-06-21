using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Interfaces
{
    public interface IUserRepository
    {
        Task<IList<ApplicationUserViewModel>> GetUsersAsync();
        Task<ApplicationUser> GetUserAsync(string id);
        Task<string> GetUserRoleAsync(ApplicationUser user);
    }
}
