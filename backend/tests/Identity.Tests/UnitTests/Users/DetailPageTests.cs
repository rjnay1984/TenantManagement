using Identity.Interfaces;
using Identity.Models;
using Identity.Pages.Users;
using Identity.ViewModels;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Identity.Tests.UnitTests.Users
{
    public class DetailPageTests
    {
        [Fact]
        public async Task OnGetAsync_PopulatesThePageModel_WithAUserById()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var expectedUser = GetSeededUser();
            var expectedRole = GetSeededRole();
            var expectedUserViewModel = new ApplicationUserViewModel(expectedUser, expectedRole);
            mockRepo.Setup(repo => repo.GetUserAsync(expectedUser.Id)).ReturnsAsync(expectedUser);
            mockRepo.Setup(repo => repo.GetUserRoleAsync(expectedUser)).ReturnsAsync(expectedRole);
            var pageModel = new DetailsModel(mockRepo.Object);

            // Act
            var result = await pageModel.OnGetAsync(expectedUser.Id);

            // Assert
            var actualUser = pageModel.AppUser;
            Assert.Equal(expectedUserViewModel.Id, actualUser.Id);
            Assert.Equal(expectedUserViewModel.Email, actualUser.Email);
            Assert.Equal(expectedUserViewModel.FirstName, actualUser.FirstName);
            Assert.Equal(expectedUserViewModel.LastName, actualUser.LastName);
            Assert.Equal(expectedUserViewModel.Role, actualUser.Role);
        }

        private ApplicationUser GetSeededUser()
        {
            return new ApplicationUser()
            {
                Id = "1",
                FirstName = "User",
                LastName = "One",
                Email = "userone@email.com",
                EmailConfirmed = true,
                UserName = "userone@email.com"
            };
        }

        private string GetSeededRole() => "Tenant";
    }
}
