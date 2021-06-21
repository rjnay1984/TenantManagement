using Identity.Interfaces;
using Identity.Pages.Users;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Identity.Tests.UnitTests.Users
{
    public class IndexPageTests
    {
        [Fact]
        public async Task OnGetAsync_PopulatesThePageModel_WithAListOfUsers()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var expectedUsers = MockUserRepo.TestUsers();
            mockUserRepo.Setup(repo => repo.GetUsersAsync().Result).Returns(expectedUsers);
            var pageModel = new IndexModel(mockUserRepo.Object);

            // Act
            await pageModel.OnGetAsync();

            //Assert
            var actualUsers = pageModel.AppUsers;
            Assert.Equal(expectedUsers, actualUsers);
        }
    }
}
