using Identity.Interfaces;
using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
    }
}
