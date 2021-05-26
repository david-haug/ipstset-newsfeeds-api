using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Users
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public string[] Roles { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
