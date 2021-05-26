using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Domain.Feeds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Specifications
{
    public class UserHasFeedAccess : ISpecification<FeedResponse>, ISpecification<Feed>
    {
        private AppUser _user;
        public UserHasFeedAccess(AppUser user)
        {
            _user = user;
        }

        public bool IsSatisifedBy(Feed entity)
        {
            return IsSatisfiedBy(entity.CreatedByUserId.ToString(), entity.IsPublic);
        }

        public bool IsSatisifedBy(FeedResponse entity)
        {
            return IsSatisfiedBy(entity.CreatedUserId, entity.IsPublic);
        }

        private bool IsSatisfiedBy(string createdUserId, bool isPublic)
        {
            return _user.HasRole(Constants.UserRoles.Admin) ||isPublic || _user.UserId == createdUserId;
        }
    }
}
