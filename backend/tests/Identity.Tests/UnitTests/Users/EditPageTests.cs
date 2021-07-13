using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Identity.Pages.Users;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Identity.Tests.UnitTests.Users
{
    public class EditPageTests
    {
        [Fact]
        public async Task OnGetAsync_ReturnsNotFoundWhenUserIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            var pageModel = new EditModel(mockRepo.Object);

            // Act
            var result = await pageModel.OnGetAsync("45");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);

            var resultValue = result as NotFoundObjectResult;
            Assert.Equal("Unable to load user with ID '45'.", resultValue.Value);
        }

        [Fact]
        public async Task OnGetAsync_ReturnsAUser()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(MockUserRepo.GetTestAppUser());
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            var pageModel = new EditModel(mockRepo.Object);

            // Act
            var result = await pageModel.OnGetAsync(MockUserRepo.GetTestAppUser().Id);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsNotFoundWhenUserIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            var pageModel = new EditModel(mockRepo.Object);

            // Act
            var result = await pageModel.OnPostAsync("45");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsErrorsWhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            var pageModel = new EditModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = "newemail@email.com",
                Role = "Admin"
            };
            pageModel.ModelState.AddModelError("Error", "This is an error.");

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(1, pageModel.ModelState.ErrorCount);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsErrorsWhenEmailResultIsFailure()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Failed(new IdentityError[]
            {
                new IdentityError() { Code = "Error", Description = "This is an error." }
            }));
            var pageModel = new EditModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = "newemail@email.com",
                Role = "Admin"
            };

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(1, pageModel.ModelState.ErrorCount);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsErrorsWhenUserNamelResultIsFailure()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Failed(new IdentityError[]
            {
                new IdentityError() { Code = "UserNameError", Description = "This is an error." }
            }));
            var pageModel = new EditModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = "newemail@email.com",
                Role = "Admin"
            };

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(1, pageModel.ModelState.ErrorCount);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsErrorsWhenRemoveClaimsResultIsFailure()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            var claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.GivenName, user.FirstName)
            };
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.GetUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(claims);
            mockRepo.Setup(r => r.RemoveUserClaimsAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<Claim>>()).Result).Returns(IdentityResult.Failed(new IdentityError[]
            {
               new IdentityError() { Code = "RemoveClaimError", Description = "This is an error." }
            }));
            var pageModel = new EditModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = "newemail@email.com",
                Role = "Admin"
            };

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(1, pageModel.ModelState.ErrorCount);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsErrorsWhenUpdateUserResultIsFailure()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            var claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.GivenName, user.FirstName)
            };
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.GetUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(claims);
            mockRepo.Setup(r => r.RemoveUserClaimsAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<Claim>>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.UpdateUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Failed(new IdentityError[]
            {
                new IdentityError() { Code = string.Empty, Description = "This is an error." }
            }));
            var pageModel = new EditModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = "newemail@email.com",
                Role = "Admin"
            };

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(1, pageModel.ModelState.ErrorCount);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsErrorsWhenAddUserClaimsResultIsFailure()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            var claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.GivenName, user.FirstName)
            };
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.GetUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(claims);
            mockRepo.Setup(r => r.RemoveUserClaimsAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<Claim>>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.UpdateUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Failed(new IdentityError[]
            {
                new IdentityError() { Code = string.Empty, Description = "This is an error." }
            }));
            var pageModel = new EditModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = "newemail@email.com",
                Role = "Admin"
            };

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(1, pageModel.ModelState.ErrorCount);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsErrorsWhenRemoveRoleResultIsFailure()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            var claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.GivenName, user.FirstName)
            };
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.GetUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(claims);
            mockRepo.Setup(r => r.RemoveUserClaimsAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<Claim>>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.UpdateUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.GetUserRoleAsync(It.IsAny<ApplicationUser>()).Result).Returns("Tenant");
            mockRepo.Setup(r => r.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Failed(new IdentityError[]
            {
                new IdentityError() { Code = string.Empty, Description = "This is an error." }
            }));
            var pageModel = new EditModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = "newemail@email.com",
                Role = "Admin"
            };

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(1, pageModel.ModelState.ErrorCount);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsErrorsWhenAddRoleResultIsFailure()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            var claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.GivenName, user.FirstName)
            };
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.GetUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(claims);
            mockRepo.Setup(r => r.RemoveUserClaimsAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<Claim>>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.UpdateUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.GetUserRoleAsync(It.IsAny<ApplicationUser>()).Result).Returns("Tenant");
            mockRepo.Setup(r => r.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Failed(new IdentityError[]
            {
                new IdentityError() { Code = string.Empty, Description = "This is an error." }
            }));
            var pageModel = new EditModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = "newemail@email.com",
                Role = "Admin"
            };

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(1, pageModel.ModelState.ErrorCount);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsARedirectResultOnSuccess()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            var claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.GivenName, user.FirstName)
            };
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles());
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.GetUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(claims);
            mockRepo.Setup(r => r.RemoveUserClaimsAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<Claim>>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.UpdateUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.GetUserRoleAsync(It.IsAny<ApplicationUser>()).Result).Returns("Tenant");
            mockRepo.Setup(r => r.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            var pageModel = new EditModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = "newemail@email.com",
                Role = "Admin"
            };

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.True(pageModel.ModelState.IsValid);
            Assert.Equal(0, pageModel.ModelState.ErrorCount);
        }
    }
}
