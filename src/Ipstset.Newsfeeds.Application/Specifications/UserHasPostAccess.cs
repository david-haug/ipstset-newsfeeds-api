using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Specifications
{
    public class UserHasPostAccess : ISpecification<PostResponse>, ISpecification<Post>
    {
        private AppUser _user;
        public UserHasPostAccess(AppUser user)
        {
            _user = user;
        }

        //Questionable decision here not having feeds and posts directly related...cutting corner by not accounting for feed IsPublic?
        public bool IsSatisifedBy(Post entity)
        {
            return IsSatisfiedBy(entity.CreatedByUserId.ToString());
        }

        public bool IsSatisifedBy(PostResponse entity)
        {
            return IsSatisfiedBy(entity.CreatedByUserId);
        }

        private bool IsSatisfiedBy(string createdUserId)
        {
            return _user.HasRole(Constants.UserRoles.Admin) || _user.UserId == createdUserId;
        }
    }
}
