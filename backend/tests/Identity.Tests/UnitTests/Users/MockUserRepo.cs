using Identity.Models;
using Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Tests.UnitTests.Users
{
    public static class MockUserRepo
    {
        public static IList<ApplicationUserViewModel> TestUsers()
        {
            return new List<ApplicationUserViewModel>()
            {
                new ApplicationUserViewModel()
                {
                    Id = "1",
                    Email = "alicesmith@email.com",
                    FirstName = "Alice",
                    LastName = "Smith",
                    Role = "Admin"
                },
                new ApplicationUserViewModel()
                {
                    Id = "2",
                    Email = "bobsmith@email.com",
                    FirstName = "Bob",
                    LastName = "Smith",
                    Role = "Landlord"
                },
                new ApplicationUserViewModel()
                {
                    Id = "3",
                    Email = "johndoe@email.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Role = "Tenant"
                }
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
