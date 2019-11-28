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
                    ClientId = "implicit-flow",
                    ClientName = "Implicit flow client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    // where to redirect to after login
                    RedirectUris = {
                        "https://localhost:10000/callback",
                        "https://localhost:10000/silent_renew.html",
                        "http://localhost:10000/callback",
                        "http://localhost:10000/silent_renew.html"
                     },
                    PostLogoutRedirectUris = {
                     "http://localhost:10000",
                     "https://localhost:10000"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        // allow access to course-api
                        "course-api"
                    },
                    // allow transfer access token via browser
                    AllowAccessTokensViaBrowser = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },
                new Client
                {
                    ClientId = "authorization-code-flow",
                    ClientName = "Authorization Code",
                    AllowedGrantTypes = GrantTypes.Code,
                    EnableLocalLogin = true,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:10000/signin-oidc" },
                    PostLogoutRedirectUris  = { "https://localhost:10000/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        // allow access to course-api
                        "course-api"
                    },
                     AlwaysIncludeUserClaimsInIdToken = true,
                    //  IdentityProviderRestrictions = new string[]{"aad"},
                },
                new Client
                {
                    ClientId = "client-credential",
                    Description = "Client credential flow",
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        // allow access to course-api
                        "course-api"
                    },
                    Claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(JwtClaimTypes.Role, "Admin")
                    },
                    ClientClaimsPrefix = ""
                },
                new Client
                {
                    ClientId = "resource-owner-password",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                new Client
                {
                    ClientId = "pkce",
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {
                        "https://localhost:10000/callback",
                        "https://localhost:10000/silent_renew.html",
                        "http://localhost:10000/callback",
                        "http://localhost:10000/silent_renew.html"
                     },
                    PostLogoutRedirectUris = {
                     "http://localhost:10000",
                     "https://localhost:10000"
                    },
                    // scopes that client has access to
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "course-api"
                    },
                    RequirePkce = true
                },
                new Client
                {
                    ClientId = "device-flow",
                    ClientSecrets =
                    {
                        new Secret("123456".Sha256())
                    },
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.DeviceFlow,
                    // scopes that client has access to
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "course-api"
                    }
                },
                 // hybrid
                 new Client
                {
                    ClientId = "hybrid",
                    ClientName = "Hybrid flow",
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
