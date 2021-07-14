using Core.Data;
using Core.Entities;
using FluentAssertions;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Core.Tests.UnitTests
{
    public class UserRepoTests
    {
        [Fact]
        public async Task GetUsersAsync_ReturnsAListOfUsers()
        {
            using (var db = new IdentityContext(Utilities.TestIdentityContextOptions()))
            {
                await db.AddRangeAsync(TestUsers());
                await db.AddRangeAsync(GetTestRoles());
                await db.AddRangeAsync(GetUserRoles());
                await db.SaveChangesAsync();

                var repo = new UserRepository(db, MockHelpers.MockUserManager<ApplicationUser>().Object, MockHelpers.MockRoleManager<IdentityRole>().Object);
                var result = await repo.GetUsersAsync();

                Assert.Equal(3, result.Count);
            }
        }

        [Fact]
        public async Task GetUserById_ReturnsAUser()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()).Result).Returns(TestUsers().First());
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.GetUserAsync("1");

            Assert.Equal(TestUsers().First().Email, result.Email);
        }

        [Fact]
        public async Task GetUserById_ReturnsNull()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.GetUserAsync("4");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserRoleAsync_ReturnsARole()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()).Result).Returns(new List<string>() { GetTestRoles().First().Name });
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.GetUserRoleAsync(TestUsers().First());

            Assert.Equal(GetTestRoles().First().Name, result);
        }

        [Fact]
        public async Task GetRolesAsync_ReturnsAListOfRoles()
        {
            using (var db = new IdentityContext(Utilities.TestIdentityContextOptions()))
            {
                await db.AddRangeAsync(GetTestRoles());
                await db.SaveChangesAsync();

                var repo = new UserRepository(db, MockHelpers.MockUserManager<ApplicationUser>().Object, MockHelpers.MockRoleManager<IdentityRole>().Object);
                var result = await repo.GetRolesAsync();

                Assert.Equal(3, result.Count);
            }
        }

        [Fact]
        public async Task AddUserAsync_ReturnsSuccess()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.CreateUserAsync(TestUsers().First());

            result.Should().Be(IdentityResult.Success);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsFailed()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Failed(new IdentityError[]
            {
                new IdentityError() { Code = "TestError", Description = "This is an error." }
            }
            ));
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.CreateUserAsync(TestUsers().First());

            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async Task AddToRoleAsync_ReturnsSuccess()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.AddToRoleAsync(TestUsers().First(), GetTestRoles().First().Name);

            result.Succeeded.Should().BeTrue();
            result.Errors.Count().Should().Be(0);
        }

        [Fact]
        public async Task RemoveFromRoleAsync_ReturnsSuccess()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.RemoveFromRoleAsync(TestUsers().First(), GetTestRoles().First().Name);

            result.Succeeded.Should().BeTrue();
            result.Errors.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetUserClaimsAsync_ReturnsAListOfClaims()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(TestClaims());
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.GetUserClaimsAsync(TestUsers().First());

            result.Should().BeEquivalentTo(TestClaims());
            result.Count().Should().Be(TestClaims().Count);
        }

        [Fact]
        public async Task RemoveUserClaimsAsync_ReturnsSuccess()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.RemoveClaimsAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<Claim>>()).Result).Returns(IdentityResult.Success);
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.RemoveUserClaimsAsync(TestUsers().First(), TestClaims());

            result.Succeeded.Should().BeTrue();
            result.Errors.Count().Should().BeLessOrEqualTo(0);
        }

        [Fact]
        public async Task AddUserClaimsAsync_ReturnsSuccess()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.AddClaimsAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<Claim>>()).Result).Returns(IdentityResult.Success);
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.AddUserClaimsAsync(TestUsers().First());

            result.Succeeded.Should().BeTrue();
            result.Errors.Count().Should().BeLessOrEqualTo(0);
        }

        [Fact]
        public async Task SetEmailAsync_ReturnsSuccess()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.SetEmailAsync(TestUsers().First(), "newemail@email.com");

            result.Succeeded.Should().BeTrue();
            result.Errors.Count().Should().BeLessOrEqualTo(0);
        }

        [Fact]
        public async Task SetUsernameAsync_ReturnsSuccess()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.SetUserNameAsync(TestUsers().First(), "newusername");

            result.Succeeded.Should().BeTrue();
            result.Errors.Count().Should().BeLessOrEqualTo(0);
        }

        [Fact]
        public async Task UpdateUserAsync_ReturnsSuccess()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.UpdateUserAsync(TestUsers().First());

            result.Succeeded.Should().BeTrue();
            result.Errors.Count().Should().BeLessOrEqualTo(0);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsSuccess()
        {
            var context = new Mock<IdentityContext>(Utilities.TestIdentityContextOptions());
            var userManager = MockHelpers.MockUserManager<ApplicationUser>();
            var roleManager = MockHelpers.MockRoleManager<IdentityRole>();
            userManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            var repo = new UserRepository(context.Object, userManager.Object, roleManager.Object);

            var result = await repo.DeleteUserAsync(TestUsers().First());

            result.Succeeded.Should().BeTrue();
            result.Errors.Count().Should().BeLessOrEqualTo(0);
        }

        public static List<ApplicationUser> TestUsers()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = "1",
                    Email = "alicesmith@email.com",
                    FirstName = "Alice",
                    LastName = "Smith",
                },
                new ApplicationUser()
                {
                    Id = "2",
                    Email = "bobsmith@email.com",
                    FirstName = "Bob",
                    LastName = "Smith",
                },
                new ApplicationUser()
                {
                    Id = "3",
                    Email = "johndoe@email.com",
                    FirstName = "John",
                    LastName = "Doe",
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

        public static IList<IdentityUserRole<string>> GetUserRoles()
        {
            return new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>() { RoleId = "1", UserId = "1" },
                new IdentityUserRole<string>() { RoleId = "2", UserId = "2" },
                new IdentityUserRole<string>() { RoleId = "3", UserId = "3" },
            };
        }

        public static IList<Claim> TestClaims()
            => new List<Claim>
            {
                new Claim(JwtClaimTypes.Email, "email@email.com"),
                new Claim(JwtClaimTypes.GivenName, "Test"),
                new Claim(JwtClaimTypes.FamilyName, "User"),
            };
    }
}
