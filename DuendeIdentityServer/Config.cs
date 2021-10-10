// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Collections.Generic;
using Duende.IdentityServer.Models;

namespace DuendeIdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api.Orders"),
                new ApiScope("scope2"),
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
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedScopes = { "scope1" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    RequirePkce = true,

                    RedirectUris = { "https://localhost:5001/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api.Orders", "offline_access" }
                },
            };
    }
}