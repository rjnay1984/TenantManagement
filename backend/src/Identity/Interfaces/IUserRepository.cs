﻿using Identity.Models;
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
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
        Task<IList<Claim>> GetUserClaimsAsync(ApplicationUser user);
        Task<IdentityResult> RemoveUserClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims);
        Task<IdentityResult> AddUserClaimsAsync(ApplicationUser user);
        Task<IdentityResult> SetEmailAsync(ApplicationUser user, string email);
        Task<IdentityResult> SetUserNameAsync(ApplicationUser user, string username);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
    }
}
