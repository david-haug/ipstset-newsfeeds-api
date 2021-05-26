using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Feeds.GetFeeds;
using Ipstset.Newsfeeds.Domain.Feeds;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
namespace Ipstset.Newsfeeds.Application.Tests.Feeds
{
    public class GetFeedsHandlerShould
    {
        [Fact]
        public async void Return_QueryResult_Given_Valid_Request()
        {
            var repos = new MockFeedRepositories();
            var feeds = GetExistingFeeds();
            foreach(var feed in feeds)
                await repos.FeedRepository.SaveAsync(feed);

            var request = new GetFeedsRequest
            {
                User = new AppUser { Roles = new[] { "admin" } }
            };

            var sut = new GetFeedsHandler(repos.FeedReadOnlyRepository);
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.IsType<QueryResult<FeedResponse>>(actual);
            Assert.Equal(feeds.ToList().Count, actual.Items.Count());
        }

        [Fact]
        public async void Throw_NotAuthorized_Given_User_With_No_Feed_Access()
        {
            var repos = new MockFeedRepositories();
            var feeds = GetExistingFeeds();
            foreach (var feed in feeds)
                await repos.FeedRepository.SaveAsync(feed);

            var request = new GetFeedsRequest
            {
                User = new AppUser()
            };

            var sut = new GetFeedsHandler(repos.FeedReadOnlyRepository);
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        //[Fact]
        //public async void Not_Return_Private_Feeds_Given_User_Not_Owner_Or_Admin()
        //{
        //    var repos = new MockFeedRepositories();
        //    var feeds = GetExistingFeeds();
        //    foreach (var feed in feeds)
        //        await repos.FeedRepository.SaveAsync(feed);

        //    var request = new GetFeedsRequest
        //    {
        //        User = new AppUser { UserId = Guid.NewGuid().ToString() }
        //    };

        //    var sut = new GetFeedsHandler(repos.FeedReadOnlyRepository);
        //    var actual = await sut.Handle(request, new System.Threading.CancellationToken());
        //    Assert.IsType<QueryResult<FeedResponse>>(actual);
        //    Assert.True(actual.Items.Count(i=>!i.IsPublic) == 0);
        //}

        //[Fact]
        //public async void Return_Private_Feeds_Given_Admin()
        //{
        //    var repos = new MockFeedRepositories();
        //    var feeds = GetExistingFeeds();
        //    foreach (var feed in feeds)
        //        await repos.FeedRepository.SaveAsync(feed);

        //    var request = new GetFeedsRequest
        //    {
        //        User = new AppUser { Roles = new[] { "admin" } }
        //    };

        //    var sut = new GetFeedsHandler(repos.FeedReadOnlyRepository);
        //    var actual = await sut.Handle(request, new System.Threading.CancellationToken());
        //    Assert.IsType<QueryResult<FeedResponse>>(actual);
        //    Assert.Equal(feeds.ToList().Count, actual.Items.Count());
        //}

        //[Fact]
        //public async void Return_Private_Feeds_Given_Owner()
        //{
        //    var repos = new MockFeedRepositories();
        //    var feeds = GetExistingFeeds().Where(f=>!f.IsPublic);
        //    foreach (var feed in feeds)
        //        await repos.FeedRepository.SaveAsync(feed);

        //    var userFeed = feeds.FirstOrDefault(f => f.Name == "Private 1");
        //    var request = new GetFeedsRequest
        //    {
        //        User = new AppUser { UserId = userFeed.CreatedByUserId.ToString() }
        //    };

        //    var sut = new GetFeedsHandler(repos.FeedReadOnlyRepository);
        //    var actual = await sut.Handle(request, new System.Threading.CancellationToken());
        //    Assert.IsType<QueryResult<FeedResponse>>(actual);
        //    Assert.Single(actual.Items);
        //    Assert.Equal(userFeed.Id.ToString(), actual.Items.FirstOrDefault().Id);
        //}

        private IEnumerable<Feed> GetExistingFeeds()
        {
            var feeds = new List<Feed>();
            feeds.Add(Feed.Create("Public 1", true, Guid.NewGuid()));
            feeds.Add(Feed.Create("Public 2", true, Guid.NewGuid()));
            feeds.Add(Feed.Create("Private 1", false, Guid.NewGuid()));
            feeds.Add(Feed.Create("Private 2", false, Guid.NewGuid()));
            return feeds;
        }

    }
}
