using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Extensions
{
    public static class AppUserExtensions
    {
        public static bool HasAccessToFeedResponse(this AppUser user, FeedResponse feedResponse)
        {
            var specification = new UserHasFeedAccess(user);
            return specification.IsSatisifedBy(feedResponse);
        }

        public static bool HasAccessToPostResponse(this AppUser user, PostResponse response)
        {
            var specification = new UserHasPostAccess(user);
            return specification.IsSatisifedBy(response);
        }
    }
}
