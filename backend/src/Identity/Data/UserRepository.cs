using Identity.Interfaces;
using Identity.Models;
using Identity.ViewModels;
using IdentityModel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IList<ApplicationUserViewModel>> GetUsersAsync()
        {
            var query = await (
                from u in _context.Users
                join ur in _context.UserRoles on u.Id equals ur.UserId
                from r in _context.Roles.Where(r => r.Id == ur.RoleId)
                select new ApplicationUserViewModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = r.Name
                }).ToListAsync();

            return query;
        }

        public async Task<ApplicationUser> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user;
        }

        public async Task<string> GetUserRoleAsync(ApplicationUser user)
        {
            var role = await _userManager.GetRolesAsync(user);

            return role[0];
        }

        public async Task<IList<IdentityRole>> GetRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user)
        {
            return await _userManager.CreateAsync(user, "Pass1123$");
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, null);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IList<Claim>> GetUserClaimsAsync(ApplicationUser user)
        {
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<IdentityResult> RemoveUserClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims)
        {
            return await _userManager.RemoveClaimsAsync(user, claims);
        }

        public async Task<IdentityResult> AddUserClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>();
            if (!user.FirstName.IsNullOrEmpty() && user.LastName.IsNullOrEmpty())
            {
                claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            }
            else if (user.FirstName.IsNullOrEmpty() && !user.LastName.IsNullOrEmpty())
            {
                claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
                claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
                claims.Add(new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
            }

            return await _userManager.AddClaimsAsync(user, claims);
        }

        public async Task<IdentityResult> SetEmailAsync(ApplicationUser user, string email)
        {
            return await _userManager.SetEmailAsync(user, email);
        }

        public async Task<IdentityResult> SetUserNameAsync(ApplicationUser user, string username)
        {
            return await _userManager.SetUserNameAsync(user, username);
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
