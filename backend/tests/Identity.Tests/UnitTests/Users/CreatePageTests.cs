using Identity.Interfaces;
using Identity.Models;
using Identity.Pages.Users;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Identity.Tests.UnitTests.Users
{
    public class CreatePageTests
    {
        [Fact]
        public async Task OnGetAsync_PopulatesThePageModel_WithAListOfRoles()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var expectedRoles = MockUserRepo.GetTestRoles();
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(expectedRoles);
            var pageModel = new CreateModel(mockRepo.Object);

            // Act
            await pageModel.OnGetAsync();

            // Assert
            var actualRoles = pageModel.AppRoles;
            Assert.Equal(expectedRoles, actualRoles);
        }

        [Fact]
        public async Task OnPostAsync_ShowsStatusMessageAndRedirectsWhenValid()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles);
            mockRepo.Setup(r => r.CreateUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);

            var pageModel = new CreateModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserViewModel()
            {
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@email.com",
                Role = "Landlord"
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.True(pageModel.ModelState.IsValid);
            Assert.Equal($"{pageModel.Input.Email} created.", pageModel.StatusMessage);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsAPageResultWhenCreateUserAsyncFails()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var errors = new IdentityError[]
            {
                new IdentityError() { Code = "InvalidUserName", Description = "Username '' is invalid, can only contain letters or digits." }
            };
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles);
            mockRepo.Setup(r => r.CreateUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Failed(errors));
            var pageModel = new CreateModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserViewModel()
            {
                Role = "Tenant"
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(1, pageModel.ModelState.ErrorCount);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsAPageResultWhenAddUserToRoleAsyncFails()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var errors = new IdentityError[]
            {
                new IdentityError() { Code = "RoleError", Description = "This is a role error." }
            };
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles);
            mockRepo.Setup(r => r.CreateUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Failed(errors));
            var pageModel = new CreateModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserViewModel()
            {
                Email = "testuser@email.com",
                Role = "Tenant"
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(1, pageModel.ModelState.ErrorCount);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsAPageResultWhenAddClaimsAsyncFails()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var errors = new IdentityError[]
            {
                new IdentityError() { Code = "ClaimError", Description = "This is a claim error." },
                new IdentityError() { Code = "ClaimError", Description = "This is another claim error." },
            };
            mockRepo.Setup(r => r.GetRolesAsync().Result).Returns(MockUserRepo.GetTestRoles);
            mockRepo.Setup(r => r.CreateUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()).Result).Returns(IdentityResult.Success);
            mockRepo.Setup(r => r.AddUserClaimsAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Failed(errors));
            var pageModel = new CreateModel(mockRepo.Object);
            pageModel.Input = new ApplicationUserViewModel()
            {
                FirstName = "Test",
                Email = "testuser@email.com",
                Role = "Tenant"
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
            Assert.Equal(2, pageModel.ModelState.ErrorCount);
        }
    }
}
