using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ipstset.Newsfeeds.Application.Users;

namespace Ipstset.Newsfeeds.Api.Auth
{
    public interface IUserStore
    {
        bool ValidateCredentials(string username, string password);
        Task<UserResponse> FindBySubjectIdAsync(string subjectId);
        Task<UserResponse> FindByUsernameAsync(string username);
        UserResponse FindByExternalProvider(string provider, string subjectId);
        UserResponse AutoProvisionUser(string provider, string subjectId, List<Claim> claims);
    }
}
