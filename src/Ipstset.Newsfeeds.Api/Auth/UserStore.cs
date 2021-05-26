using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ipstset.Newsfeeds.Application.Security;
using Ipstset.Newsfeeds.Application.Users;

//using Ipstset.Security;

namespace Ipstset.Newsfeeds.Api.Auth
{
    public class UserStore : IUserStore
    {
        private IUserReadOnlyRepository _userRepository;

        public UserStore(IUserReadOnlyRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ValidateCredentials(string username, string password)
        {
            //var user = _users.FirstOrDefault(u => string.Equals(u.UserName, username, StringComparison.CurrentCultureIgnoreCase));
            var user = _userRepository.GetByUserNameAsync(username).Result;
            if (user == null) return false;

            var pwdHash = SaltedHash.Create(password, user.Salt, Constants.HashKey).Hash;
            return pwdHash == user.Password;
        }

        public async Task<UserResponse> FindBySubjectIdAsync(string subjectId)
        {
            //var user = _users.FirstOrDefault(u => u.Id.ToString() == subjectId);
            var user = await _userRepository.GetByIdAsync(subjectId);
            return user;
        }

        public async Task<UserResponse> FindByUsernameAsync(string username)
        {
            //var user = _users.FirstOrDefault(u => string.Equals(u.UserName, username, StringComparison.CurrentCultureIgnoreCase));
            var user = await _userRepository.GetByUserNameAsync(username);
            return user;
        }

        public UserResponse FindByExternalProvider(string provider, string subjectId)
        {
            throw new NotImplementedException();
        }

        public UserResponse AutoProvisionUser(string provider, string subjectId, List<Claim> claims)
        {
            throw new NotImplementedException();
        }
    }
}
