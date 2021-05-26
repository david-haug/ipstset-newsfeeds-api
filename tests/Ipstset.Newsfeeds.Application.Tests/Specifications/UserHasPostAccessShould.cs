using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Specifications
{
    public class UserHasPostAccessShould
    {
        [Fact]
        public void Return_True_Given_Owner()
        {
            var userId = Guid.NewGuid().ToString();
            var postResponse = new PostResponse { CreatedByUserId = userId };
            var sut = new UserHasPostAccess(new AppUser { UserId = userId });
            var actual = sut.IsSatisifedBy(postResponse);
            Assert.True(actual);
        }

        [Fact]
        public void Return_True_Given_Admin()
        {
            var postResponse = new PostResponse { CreatedByUserId = Guid.NewGuid().ToString() };
            var sut = new UserHasPostAccess(new AppUser { UserId = Guid.NewGuid().ToString(), Roles = new[] { "admin" } });
            var actual = sut.IsSatisifedBy(postResponse);
            Assert.True(actual);
        }

        [Fact]
        public void Return_False_Given_User_Not_Admin_Or_Creator()
        {
            var postResponse = new PostResponse { CreatedByUserId = Guid.NewGuid().ToString() };
            var sut = new UserHasPostAccess(new AppUser { UserId = Guid.NewGuid().ToString(), Roles = new[] { "user" } });
            var actual = sut.IsSatisifedBy(postResponse);
            Assert.False(actual);
        }
    }
}
