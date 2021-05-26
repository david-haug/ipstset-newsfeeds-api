using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.CreatePost;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Domain;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Posts
{
    public class CreatePostHandlerShould
    {
        [Fact]
        public async void Return_PostResponse_Given_Valid_Request()
        {
            var repos = new MockPostRepositories();

            var feed = FeedFactory.GetExistingFeed();
            var feedRepos = new MockFeedRepositories();
            await feedRepos.FeedRepository.SaveAsync(feed);

            var request = new CreatePostRequest
            {
                FeedId = feed.Id.ToString(),
                Title = "test title",
                Content = "test content",
                UserId = Guid.NewGuid().ToString(),
                Tags = new List<string> { "test" }
            };

            var sut = new CreatePostHandler(repos.PostRepository, repos.PostReadOnlyRepository, feedRepos.FeedRepository);
            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.IsType<PostResponse>(actual);
        }

        [Fact]
        public async void Throw_NotFoundException_Given_Invalid_FeedId()
        {
            var repos = new MockPostRepositories();
            var feedRepos = new MockFeedRepositories();
            var request = new CreatePostRequest
            {
                FeedId = Guid.NewGuid().ToString(),
                Title = "test title",
                Content = "test content",
                UserId = Guid.NewGuid().ToString(),
                Tags = new List<string> { "test" }
            };

            var sut = new CreatePostHandler(repos.PostRepository, repos.PostReadOnlyRepository, feedRepos.FeedRepository);
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
