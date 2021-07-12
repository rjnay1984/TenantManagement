using Core.Entities;
using Identity.Interfaces;
using Identity.Pages.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Identity.Tests.UnitTests.Users
{
    public class DeletePageTests
    {
        [Fact]
        public async Task OnGetAsync_ReturnsNotFoundWhenUserIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var pageModel = new DeleteModel(mockRepo.Object);

            // Act
            var result = await pageModel.OnGetAsync("45");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnGetAsync_ReturnsPageResultWhenValid()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            var role = MockUserRepo.GetUserRole();
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.GetUserRoleAsync(It.IsAny<ApplicationUser>()).Result).Returns(role);
            var pageModel = new DeleteModel(mockRepo.Object);

            // Act
            var result = await pageModel.OnGetAsync(user.Id);

            // Assert
            Assert.True(pageModel.ModelState.IsValid);
            Assert.Equal(user, pageModel.AppUser);
            Assert.Equal(role, pageModel.UserRole);
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsNotFoundWhenAppUserIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            var role = MockUserRepo.GetUserRole();
            var pageModel = new DeleteModel(mockRepo.Object);

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsErrorStatusMessageWhenDeleteUserFails()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            var role = MockUserRepo.GetUserRole();
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.GetUserRoleAsync(It.IsAny<ApplicationUser>()).Result).Returns(role);
            mockRepo.Setup(r => r.DeleteUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Failed(new IdentityError[]
            {
                new IdentityError() { Code = string.Empty, Description = "This is an error." }
            }));
            var pageModel = new DeleteModel(mockRepo.Object);

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal("Error: This is an error.", pageModel.StatusMessage);
        }

        [Fact]
        public async Task OnPostAsync_RedirectsOnSuccessFullDelete()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var user = MockUserRepo.GetTestAppUser();
            var role = MockUserRepo.GetUserRole();
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<string>()).Result).Returns(user);
            mockRepo.Setup(r => r.GetUserRoleAsync(It.IsAny<ApplicationUser>()).Result).Returns(role);
            mockRepo.Setup(r => r.DeleteUserAsync(It.IsAny<ApplicationUser>()).Result).Returns(IdentityResult.Success);
            var pageModel = new DeleteModel(mockRepo.Object);

            // Act
            var result = await pageModel.OnPostAsync(user.Id);

            // Assert
            Assert.Equal("User deleted.", pageModel.StatusMessage);
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
