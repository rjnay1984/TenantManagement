﻿using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("role", displayName: "See the roles for the user", userClaims: new[] { JwtClaimTypes.Role })
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedCorsOrigins = { "http://localhost:4200" },

                    RedirectUris = { "http://localhost:4200/signin-oidc", "http://localhost:4200" },
                    FrontChannelLogoutUri = "http://localhost:4200/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:4200/signout-callback-oidc", "http://localhost:4200" },

                    RequireClientSecret = false,

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email", "role" }
                },
            };
    }
}