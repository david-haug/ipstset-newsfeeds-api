using System;
using System.Linq;

namespace Ipstset.Newsfeeds.Application
{
    public class AppUser
    {
        public string UserId { get; set; }
        public string[] Roles { get; set; }

        public bool HasRole(string role)
        {
            if (Roles == null || !Roles.Any())
                return false;

            return Roles.Select(r => r.ToLower()).Contains(role?.ToLower());
        }

    }
}
