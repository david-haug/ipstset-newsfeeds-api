using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Feeds.GetFeed;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Domain;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Feeds
{
    public class GetFeedHandlerShould
    {
        [Fact]
        public async void Return_FeedResponse_Given_Valid_Request()
        {
            var feed = FeedFactory.GetExistingFeed();
            var repos = new MockFeedRepositories();
            await repos.FeedRepository.SaveAsync(feed);

            var request = new GetFeedRequest
            {
                Id = feed.Id.ToString(),
                User = new AppUser { UserId = feed.CreatedByUserId.ToString() }
            };

            var sut = new GetFeedHandler(repos.FeedReadOnlyRepository);
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.IsType<FeedResponse>(actual);
            Assert.Equal(feed.Id.ToString(), actual.Id);
        }

        [Fact]
        public async void Throw_NotFoundException_Given_Invalid_Id()
        {
            var request = new GetFeedRequest
            {
                Id = Guid.NewGuid().ToString(),
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var repos = new MockFeedRepositories();
            var sut = new GetFeedHandler(repos.FeedReadOnlyRepository);
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_NotAuthorizedException_Given_Invalid_User()
        {
            var feed = FeedFactory.GetExistingFeed();
            var repos = new MockFeedRepositories();
            await repos.FeedRepository.SaveAsync(feed);

            var request = new GetFeedRequest
            {
                Id = feed.Id.ToString(),
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var sut = new GetFeedHandler(repos.FeedReadOnlyRepository);
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
