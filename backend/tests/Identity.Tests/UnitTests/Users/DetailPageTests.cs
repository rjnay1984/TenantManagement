using Core.DTOs;
using Core.Interfaces;
using Identity.Pages.Users;
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
            var expectedUser = MockUserRepo.GetTestAppUser();
            var expectedRole = MockUserRepo.GetUserRole();
            var expectedUserViewModel = new ApplicationUserDto(expectedUser, expectedRole);
            mockRepo.Setup(repo => repo.GetUserAsync(expectedUser.Id).Result).Returns(expectedUser);
            mockRepo.Setup(repo => repo.GetUserRoleAsync(expectedUser).Result).Returns(expectedRole);
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
    }
}
