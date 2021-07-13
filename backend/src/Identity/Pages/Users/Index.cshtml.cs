using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public IndexModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IReadOnlyList<ApplicationUserDto> AppUsers { get; private set; }

        public async Task OnGetAsync()
        {
            AppUsers = await _userRepository.GetUsersAsync();
        }
    }
}
