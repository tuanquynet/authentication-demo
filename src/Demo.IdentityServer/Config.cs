using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using static IdentityServer4.IdentityServerConstants;

namespace Demo.IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            var list = new List<ApiResource>
            {
                new ApiResource("course-api", "Course API")
            };
            return list;

        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "w2",
                    ClientName = "MVC Web Application - Authorization Code",
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    EnableLocalLogin = false,
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:10001/signin-oidc" },

                    PostLogoutRedirectUris  = { "https://localhost:10001/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        // allow access to course-api
                        "course-api",
                        StandardScopes.OfflineAccess
                    },
                     AllowOfflineAccess = true,
                     AlwaysIncludeUserClaimsInIdToken = true,
                     IdentityProviderRestrictions = new string[]{"aad"},
                },
                new Client
                {
                    ClientId = "u1",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "course-api",IdentityServerConstants.StandardScopes.OpenId, StandardScopes.OfflineAccess },
                    Claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(JwtClaimTypes.Role, "Admin")
                    },
                    ClientClaimsPrefix = "",
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "o1",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "order-api" }
                },
                new Client
                {
                    ClientId = "u2",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {
                        "course-api",
                        StandardScopes.OfflineAccess
                    },
                     // Offline access
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 300,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 3600
                },
                new Client
                {
                    ClientId = "w1",
                    ClientName = "MVC Web Application - Implicit",
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:10000/signin-oidc" },

                    PostLogoutRedirectUris  = { "https://localhost:10000/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        // allow access to course-api
                        "course-api",
                        StandardScopes.OfflineAccess
                    },
                    // allow transfer access token via browser
                    AllowAccessTokensViaBrowser = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true
                },
                 // hybrid
                 new Client
                {
                    ClientId = "w3",
                    ClientName = "MVC Web Application - Hybrid flow",
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:10002/signin-oidc" },

                    PostLogoutRedirectUris  = { "https://localhost:10002/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        // allow access to course-api
                        "course-api",
                        StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },
                  // code with refresh token
                 new Client
                {
                    ClientId = "swagger_client",
                    ClientName = "Swagger documentation",
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:2500/swagger/o2c.html",
                      "http://localhost:2500/swagger/o2c.html"},

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:10000/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        // allow access to user-api
                        "course-api",
                        StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
