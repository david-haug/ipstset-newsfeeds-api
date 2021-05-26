using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Ipstset.Newsfeeds.Api.Auth
{
    public class ProfileService : IProfileService
    {
        private IUserStore _userStore;

        public ProfileService(IUserStore userStore)
        {
            _userStore = userStore;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context.RequestedClaimTypes.Any())
            {
                var user = await _userStore.FindBySubjectIdAsync(context.Subject.GetSubjectId());
                if (user != null)
                {
                    context.AddRequestedClaims(ClaimsService.Create(user));
                }
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = _userStore.FindBySubjectIdAsync(context.Subject.GetSubjectId());
            context.IsActive = true;//(user != null) && user.Active;
            return Task.FromResult(0);
        }
    }
}
