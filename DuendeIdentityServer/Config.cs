// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Collections.Generic;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

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
                new ApiScope("api"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "orders.client",
                    ClientName = "Orders Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("orders.secret".Sha256()) },

                    AllowedScopes = { "api" },
                },

                // mvc.client client using code flow + pkce
                new Client
                {
                    ClientId = "mvc.client",
                    ClientName = "MvcClient",
                    ClientSecrets = { new Secret("mvc.secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    RequirePkce = true,

                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api", "offline_access" },
                },
                new Client
                {
                    ClientId = "spa",
                    ClientName = "Spa",
                    ClientSecrets = { new Secret("spa.secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    RequirePkce = true,

                    RedirectUris = { "http://localhost:7000/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:7000/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:7000/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api", "offline_access" },
                },
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
    
                    RedirectUris =           { "http://localhost:8000/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:8000/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:8000" },
                    
                    AllowOfflineAccess = true,

                    AllowedScopes = 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api",
                        "offline_access"
                    }
                }
            };
    }
}