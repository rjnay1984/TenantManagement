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
            #region snippet1
            var mockUserRepo = new Mock<IUserRepository>();
            var expectedUsers = GetSeededUsers();
            mockUserRepo.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(expectedUsers);
            var pageModel = new IndexModel(mockUserRepo.Object);
            #endregion

            #region snippet2
            // Act
            await pageModel.OnGetAsync();
            #endregion

            //Assert
            #region snippet3
            var actualUsers = pageModel.AppUsers;
            Assert.Equal(expectedUsers, actualUsers);
            #endregion
        }

        private static IList<ApplicationUserViewModel> GetSeededUsers()
        {
            return new List<ApplicationUserViewModel>()
            {
                new ApplicationUserViewModel() { FirstName = "User", LastName = "One", Email = "userone@email.com", Id = "1", Role = new Microsoft.AspNetCore.Identity.IdentityRole() { Name = "Tenant" } },
                new ApplicationUserViewModel() { FirstName = "User", LastName = "Two", Email = "usertwo@email.com", Id = "2", Role = new Microsoft.AspNetCore.Identity.IdentityRole() { Name = "Landlord" } }
            };
        }
    }
}
