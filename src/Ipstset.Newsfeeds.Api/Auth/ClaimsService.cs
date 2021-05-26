using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Users;

namespace Ipstset.Newsfeeds.Api.Auth
{
    public class ClaimsService
    {
        public static IEnumerable<Claim> Create(UserResponse user)
        {
            //map claims here
            return new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtClaimTypes.GivenName, user.FirstName ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.LastName ?? ""),
                new Claim(JwtClaimTypes.Email, user.Email ?? ""),
                new Claim("roles",string.Join(",", user.Roles))
            };
        }

        public static AppUser CreateAppUser(IEnumerable<Claim> claims)
        {
            var claimList = claims.ToList();

            var clientId = claimList.FirstOrDefault(c => c.Type == "client_id")?.Value;
            if (clientId == Constants.ClientCredentialsClientIdForSystemUser)
            {
                return new AppUser
                {
                    UserId = Constants.SystemUserId,
                    Roles = new [] {"admin","user"}
                };
            }

            return new AppUser
            {
                UserId = claimList.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value,
                Roles = claimList.FirstOrDefault(c => c.Type == "roles")?.Value.Split(",")
            };
        }
    }
}
