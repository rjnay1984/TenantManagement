using Identity.Interfaces;
using Identity.Pages.Users;
using Identity.ViewModels;
using Moq;
using System.Collections.Generic;
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
            var expectedUsers = GetSeededUsers();
            mockUserRepo.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(expectedUsers);
            var pageModel = new IndexModel(mockUserRepo.Object);

            // Act
            await pageModel.OnGetAsync();

            //Assert
            var actualUsers = pageModel.AppUsers;
            Assert.Equal(expectedUsers, actualUsers);
        }

        private static IList<ApplicationUserViewModel> GetSeededUsers()
        {
            return new List<ApplicationUserViewModel>()
            {
                new ApplicationUserViewModel() { FirstName = "User", LastName = "One", Email = "userone@email.com", Id = "1", Role = "Tenant" },
                new ApplicationUserViewModel() { FirstName = "User", LastName = "Two", Email = "usertwo@email.com", Id = "2", Role = "Landlord" }
            };
        }
    }
}
