using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;

namespace Ipstset.Newsfeeds.Api.Auth
{
    public class AuthConfig
    {
        public static IEnumerable<Client> Clients = new List<Client>
        {
            new Client
            {
                ClientId = Constants.ClientCredentialsClientIdForSystemUser,

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets = { new Secret("dev_secret".Sha256())},

                // scopes that client has access to
                AllowedScopes = { "newsfeeds_api" }
            },
            //PASSWORD
            new Client
            {
                ClientId = "internal_dev",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = false,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "newsfeeds_api",
                    "newsfeeds_user"
                }
            },
            //new Client
            //{
            //    ClientId = "newsfeeds_app",
            //    AllowedGrantTypes = GrantTypes.Implicit,
            //    RequireClientSecret = false,
            //    AllowedScopes = new List<string>
            //    {
            //        IdentityServerConstants.StandardScopes.OpenId,
            //        IdentityServerConstants.StandardScopes.Profile,
            //        IdentityServerConstants.StandardScopes.Email,
            //        "newsfeeds_api",
            //        "newsfeeds_user"
            //    },
            //    AllowAccessTokensViaBrowser = true,
            //    RedirectUris = {
            //        "https://newsfeeds-app.netlify.com/callback.html",
            //        "https://newsfeeds-app.netlify.com/popup.html",
            //        "https://newsfeeds-app.netlify.com/silent.html"
            //    },
            //    PostLogoutRedirectUris = { "https://newsfeeds-app.netlify.com/index.html" },
            //    AllowedCorsOrigins = { "https://newsfeeds-app.netlify.com" },
            //    RequireConsent = false,
            //    AccessTokenLifetime = 3600,
            //    IdentityTokenLifetime = 300
            //},
            ////TEST JS APP
            //new Client
            //{
            //    ClientId = "test_js_app",
            //    AllowedGrantTypes = GrantTypes.Implicit,
            //    RequireClientSecret = false,
            //    AllowedScopes = new List<string>
            //    {
            //        IdentityServerConstants.StandardScopes.OpenId,
            //        IdentityServerConstants.StandardScopes.Profile,
            //        IdentityServerConstants.StandardScopes.Email,
            //        "newsfeeds_api",
            //        "newsfeeds_user"
            //    },
            //    AllowAccessTokensViaBrowser = true,
            //    RedirectUris = {
            //        "http://localhost:5010/callback.html",
            //        "http://localhost:5010/popup.html",
            //        "http://localhost:5010/silent.html"
            //    },
            //    PostLogoutRedirectUris = { "http://localhost:5010/index.html" },
            //    AllowedCorsOrigins = { "http://localhost:5010" },
            //    RequireConsent = false,
            //    AccessTokenLifetime = 3600,
            //    IdentityTokenLifetime = 300
            //},
            new Client
            {
                ClientId = "js_test_app",
                ClientName = "JavaScript Client",
                //ClientUri = "http://identityserver.io",
                //LogoUri = "https://pbs.twimg.com/profile_images/1612989113/Ki-hanja_400x400.png",

                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                AccessTokenType = AccessTokenType.Jwt,

                RedirectUris =
                {
                    "https://localhost:5011/index.html",
                    "https://localhost:5011/callback.html",
                    "https://localhost:5011/silent.html",
                    "https://localhost:5011/popup.html"
                },

                PostLogoutRedirectUris = { "https://localhost:5011/index.html" },
                AllowedCorsOrigins = { "https://localhost:5011" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "newsfeeds_api","newsfeeds_user"
                }
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources = new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),

            //custom user info
            new IdentityResource(
                name:        "newsfeeds_user",
                displayName: "Newsfeeds User Info",
                claimTypes:  new[] { "roles" })
        };

        public static IEnumerable<ApiResource> Apis = new List<ApiResource>
        {
            new ApiResource("newsfeeds_api", "Newsfeeds API")
            {
                //claims we want passed back in the jwt
                //UserClaims = new [] { "email", "roles" } 
                UserClaims = new [] { "roles" }
            }
        };
    }
}
