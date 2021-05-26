using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.GetPosts;
using Ipstset.Newsfeeds.Infrastructure.SqlData;
using Ipstset.Newsfeeds.Tests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Infrastructure.Tests.SqlData
{
    public class PostReadOnlyRepositoryShould
    {
        private string _exisingPostId = "1e2f6bc9-ca0c-48f7-a342-476efbda025b";
        private string _exisingFeedId = "4787eb74-875b-4a72-9142-b33f3dcc4015";

        [Fact]
        public async void DB_Return_PostResponse_Given_Valid_Id()
        {
            var id = _exisingPostId;
            var sut = new PostReadOnlyRepository(Config.Connections.Newsfeeds);
            var actual = await sut.GetByIdAsync(id);
            Assert.NotNull(actual);
            Assert.IsType<PostResponse>(actual);
        }

        [Fact]
        public async void DB_Return_Null_Given_Invalid_Id()
        {
            var id = Guid.NewGuid().ToString();
            var sut = new PostReadOnlyRepository(Config.Connections.Newsfeeds);
            var actual = await sut.GetByIdAsync(id);
            Assert.Null(actual);
        }

        //[Fact]
        //public async void DB_Return_PublishedPostsByFeed_Given_Valid_Id()
        //{
        //    var id = _exisingFeedId;
        //    var sut = new PostReadOnlyRepository(Config.Connections.Newsfeeds);
        //    var actual = await sut.GetPublishedPostsByFeedAsync(id);
        //    Assert.NotNull(actual);
        //    Assert.IsType<PublishedPostsByFeedResponse>(actual);
        //}

        [Fact]
        public async void DB_Return_Posts_Given_Valid_Request()
        {
            var request = new GetPostsRequest { User = new AppUser { Roles = new[] { "admin" } } };
            var sut = new PostReadOnlyRepository(Config.Connections.Newsfeeds);
            var actual = await sut.GetPostsAsync(request);
            Assert.NotNull(actual);
            Assert.IsType<QueryResult<PostResponse>>(actual);
        }
    }
}
