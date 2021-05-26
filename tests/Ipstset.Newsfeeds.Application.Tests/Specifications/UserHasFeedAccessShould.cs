using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Specifications
{
    public class UserHasFeedAccessShould
    {
        [Fact]
        public void Return_True_Given_Owner_When_Private_Feed()
        {
            var userId = Guid.NewGuid().ToString();
            var feedResponse = new FeedResponse { CreatedUserId = userId };
            var sut = new UserHasFeedAccess(new AppUser { UserId = userId });
            var actual = sut.IsSatisifedBy(feedResponse);
            Assert.True(actual);
        }

        [Fact]
        public void Return_True_Given_Admin_When_Private_Feed()
        {
            var feedResponse = new FeedResponse { CreatedUserId = Guid.NewGuid().ToString() };
            var sut = new UserHasFeedAccess(new AppUser { UserId = Guid.NewGuid().ToString(), Roles = new[] {"admin"} });
            var actual = sut.IsSatisifedBy(feedResponse);
            Assert.True(actual);
        }

        [Fact]
        public void Return_True_When_Public_Feed()
        {
            var feedResponse = new FeedResponse { CreatedUserId = Guid.NewGuid().ToString(), IsPublic = true };
            var sut = new UserHasFeedAccess(new AppUser { UserId = Guid.NewGuid().ToString()});
            var actual = sut.IsSatisifedBy(feedResponse);
            Assert.True(actual);
        }

        [Fact]
        public void Return_False_When_Private_Feed_And_User_Not_Admin_Or_Creator()
        {
            var feedResponse = new FeedResponse { CreatedUserId = Guid.NewGuid().ToString() };
            var sut = new UserHasFeedAccess(new AppUser { UserId = Guid.NewGuid().ToString(), Roles = new[] { "user" } });
            var actual = sut.IsSatisifedBy(feedResponse);
            Assert.False(actual);
        }
    }
}
