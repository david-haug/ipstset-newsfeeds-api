using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Feeds.CreateFeed;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Feeds
{
    public class CreateFeedHandlerShould
    {
        [Fact]
        public void Return_FeedResponse_Given_Valid_Request()
        {
            var request = new CreateFeedRequest
            {
                Name = "test",
                IsPublic = true,
                UserId = Guid.NewGuid().ToString()
            };

            var repos = new MockFeedRepositories();
            var sut = new CreateFeedHandler(repos.FeedRepository, repos.FeedReadOnlyRepository);
            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.IsType<FeedResponse>(actual);
        }
    }
}
