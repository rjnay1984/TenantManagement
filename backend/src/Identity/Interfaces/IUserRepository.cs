using Identity.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Interfaces
{
    public interface IUserRepository
    {
        Task<IList<ApplicationUserViewModel>> GetUsersAsync();
    }
}
