using Identity.Interfaces;
using Identity.Models;
using Identity.ViewModels;
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

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
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
                    Role = r
                }).ToListAsync();

            return query;
        }
    }
}
