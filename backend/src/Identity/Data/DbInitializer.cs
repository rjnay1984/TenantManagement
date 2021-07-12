using Core.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace Identity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminRole = roleManager.FindByNameAsync("Admin").Result;
            if (adminRole == null)
            {
                adminRole = new IdentityRole("Admin");

                var result = roleManager.CreateAsync(adminRole);
                result.Wait();
                if (!result.IsCompletedSuccessfully)
                {
                    throw new Exception(result.Exception.Message);
                }
            }

            var landlordRole = roleManager.FindByNameAsync("Landlord").Result;
            if (landlordRole == null)
            {
                landlordRole = new IdentityRole("Landlord");

                var result = roleManager.CreateAsync(landlordRole);
                result.Wait();
                if (!result.IsCompletedSuccessfully)
                {
                    throw new Exception(result.Exception.Message);
                }
            }

            var tenantRole = roleManager.FindByNameAsync("Tenant").Result;
            if (tenantRole == null)
            {
                tenantRole = new IdentityRole("Tenant");

                var result = roleManager.CreateAsync(tenantRole);
                result.Wait();
                if (!result.IsCompletedSuccessfully)
                {
                    throw new Exception(result.Exception.Message);
                }
            }

            var alice = userManager.FindByNameAsync("alice").Result;
            if (alice == null)
            {
                alice = new ApplicationUser
                {
                    UserName = "AliceSmith@email.com",
                    Email = "AliceSmith@email.com",
                    FirstName = "Alice",
                    LastName = "Smith",
                    EmailConfirmed = true,
                };

                var result = userManager.CreateAsync(alice, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddToRolesAsync(alice, new[] { "Admin" }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, $"{alice.FirstName} {alice.LastName}"),
                            new Claim(JwtClaimTypes.GivenName, alice.FirstName),
                            new Claim(JwtClaimTypes.FamilyName, alice.LastName),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }

            var bob = userManager.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    FirstName = "Bob",
                    LastName = "Smith",
                    UserName = "BobSmith@email.com",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddToRoleAsync(bob, "Landlord").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, $"{bob.FirstName} {bob.LastName}"),
                            new Claim(JwtClaimTypes.GivenName, bob.FirstName),
                            new Claim(JwtClaimTypes.FamilyName, bob.LastName),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }

            var aaron = userManager.FindByNameAsync("aaron").Result;
            if (aaron == null)
            {
                aaron = new ApplicationUser
                {
                    FirstName = "Aaron",
                    LastName = "Rodgers",
                    UserName = "aaron@email.com",
                    Email = "aaron@email.com",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(aaron, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddToRoleAsync(aaron, "Tenant").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddClaimsAsync(aaron, new Claim[]{
                            new Claim(JwtClaimTypes.Name, $"{aaron.FirstName} {aaron.LastName}"),
                            new Claim(JwtClaimTypes.GivenName, aaron.FirstName),
                            new Claim(JwtClaimTypes.FamilyName, aaron.LastName),
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

            }
        }
    }
}
