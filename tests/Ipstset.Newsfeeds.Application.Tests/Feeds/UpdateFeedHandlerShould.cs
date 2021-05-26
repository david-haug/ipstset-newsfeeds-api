using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Feeds.UpdateFeed;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Domain;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Feeds
{
    public class UpdateFeedHandlerShould
    {
        [Fact]
        public async void Return_FeedResponse_Given_Valid_Request()
        {
            var feed = FeedFactory.GetExistingFeed();
            var repos = new MockFeedRepositories();
            await repos.FeedRepository.SaveAsync(feed);

            var oldName = feed.Name;
            var oldIsPublic = feed.IsPublic;

            var request = new UpdateFeedRequest 
            { 
                Id = feed.Id.ToString(),
                Name = "a new name",
                IsPublic = !feed.IsPublic,
                User = new AppUser {  UserId = feed.CreatedByUserId.ToString() }
            };

            var sut = new UpdateFeedHandler(repos.FeedRepository, repos.FeedReadOnlyRepository);
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.IsType<FeedResponse>(actual);
            Assert.Equal(feed.Id.ToString(), actual.Id);
            Assert.NotEqual(oldName, actual.Name);
            Assert.NotEqual(oldIsPublic, actual.IsPublic);
        }

        [Fact]
        public async void Throw_NotFoundException_Given_Invalid_Id()
        {
            var request = new UpdateFeedRequest
            {
                Id = Guid.NewGuid().ToString(),
                Name = "a new name",
                IsPublic = true,
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var repos = new MockFeedRepositories();
            var sut = new UpdateFeedHandler(repos.FeedRepository, repos.FeedReadOnlyRepository);
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_NotAuthorizedException_Given_Different_UserId()
        {
            var feed = FeedFactory.GetExistingFeed();
            var repos = new MockFeedRepositories();
            await repos.FeedRepository.SaveAsync(feed);

            var request = new UpdateFeedRequest
            {
                Id = feed.Id.ToString(),
                Name = "a new name",
                IsPublic = !feed.IsPublic,
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var sut = new UpdateFeedHandler(repos.FeedRepository, repos.FeedReadOnlyRepository);
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_Exception_Given_No_Name()
        {
            var feed = FeedFactory.GetExistingFeed();
            var repos = new MockFeedRepositories();
            await repos.FeedRepository.SaveAsync(feed);

            var request = new UpdateFeedRequest
            {
                Id = feed.Id.ToString(),
                Name = "",
                IsPublic = !feed.IsPublic,
                User = new AppUser { UserId = feed.CreatedByUserId.ToString() }
            };

            var sut = new UpdateFeedHandler(repos.FeedRepository, repos.FeedReadOnlyRepository);
            await Assert.ThrowsAnyAsync<Exception>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }



    }
}
