using Identity.DTOs;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Tests.UnitTests.Users
{
    public static class MockUserRepo
    {
        public static IReadOnlyList<ApplicationUserDto> TestUsers()
        {
            return new List<ApplicationUserDto>()
            {
                new ApplicationUserDto()
                {
                    Id = "1",
                    Email = "alicesmith@email.com",
                    FirstName = "Alice",
                    LastName = "Smith",
                    Role = "Admin"
                },
                new ApplicationUserDto()
                {
                    Id = "2",
                    Email = "bobsmith@email.com",
                    FirstName = "Bob",
                    LastName = "Smith",
                    Role = "Landlord"
                },
                new ApplicationUserDto()
                {
                    Id = "3",
                    Email = "johndoe@email.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Role = "Tenant"
                }
            };
        }

        public static IList<IdentityRole> GetTestRoles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole() { Id = "1", Name = "Admin" },
                new IdentityRole() { Id = "2", Name = "Landlord" },
                new IdentityRole() { Id = "3", Name = "Tenant" }
            };
        }

        public static ApplicationUser GetTestAppUser()
        {
            var user = TestUsers().First();
            return new ApplicationUser()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        public static string GetUserRole() => "Tenant";
    }
}
