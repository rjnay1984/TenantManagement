using Identity.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Helpers
{
    public static class ClaimsManager
    {
        public static async Task AddUserClaimsAsync(ApplicationUser user, UserManager<ApplicationUser> userManager, ILogger logger)
        {
            IdentityResult result;
            if (!user.FirstName.IsNullOrEmpty() && user.LastName.IsNullOrEmpty())
            {
                result = await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.GivenName, user.FirstName));
            }
            else if (user.FirstName.IsNullOrEmpty() && !user.LastName.IsNullOrEmpty())
            {
                result = await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.FamilyName, user.LastName));
            }
            else
            {
                result = await userManager.AddClaimsAsync(user, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, user.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, user.LastName),
                });
            }

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            logger.LogInformation("Claims added to user.");
        }
    }
}
