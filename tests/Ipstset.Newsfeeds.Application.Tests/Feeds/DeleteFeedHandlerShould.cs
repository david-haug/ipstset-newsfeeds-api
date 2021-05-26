using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Feeds.DeleteFeed;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Domain;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Feeds
{
    public class DeleteFeedHandlerShould
    {
        [Fact]
        public async void Delete_Feed()
        {
            var repos = new MockFeedRepositories();
            var feed = FeedFactory.GetExistingFeed();
            await repos.FeedRepository.SaveAsync(feed);

            var request = new DeleteFeedRequest
            {
                Id = feed.Id.ToString(),
                User = new AppUser {  UserId = feed.CreatedByUserId.ToString() }
            };

            var sut = new DeleteFeedHandler(repos.FeedRepository);
            await sut.Handle(request, new System.Threading.CancellationToken());

            var actual = await repos.FeedReadOnlyRepository.GetByIdAsync(request.Id);
            Assert.Null(actual);
        }

        [Fact]
        public async void Throw_NotFoundException_Given_Invalid_Id()
        {
            var repos = new MockFeedRepositories();
            var request = new DeleteFeedRequest
            {
                Id = Guid.NewGuid().ToString()
            };

            var sut = new DeleteFeedHandler(repos.FeedRepository);
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_NotAuthorizedException_Given_Different_UserId()
        {
            var repos = new MockFeedRepositories();
            var feed = FeedFactory.GetExistingFeed();
            await repos.FeedRepository.SaveAsync(feed);

            var request = new DeleteFeedRequest
            {
                Id = feed.Id.ToString(),
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var sut = new DeleteFeedHandler(repos.FeedRepository);
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
