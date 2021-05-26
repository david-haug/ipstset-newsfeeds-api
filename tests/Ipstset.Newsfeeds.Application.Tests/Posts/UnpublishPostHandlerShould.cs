using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.UnpublishPost;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Domain;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Posts
{
    public class UnpublishPostHandlerShould
    {
        [Fact]
        public async void Return_FeedResponse_Given_Valid_Request()
        {
            var repos = new MockPostRepositories();
            var post = PostFactory.GetExistingPost(DateTimeOffset.Now);
            await repos.PostRepository.SaveAsync(post);

            var request = new UnpublishPostRequest
            {
                Id = post.Id.ToString(),
                User = new AppUser { UserId = post.CreatedByUserId.ToString() }
            };

            var sut = new UnpublishPostHandler(repos.PostRepository, repos.PostReadOnlyRepository);
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.IsType<PostResponse>(actual);
            Assert.Null(actual.DatePublished);
            Assert.False(actual.IsPublished);
        }

        [Fact]
        public async void Throw_NotFoundException_Given_Invalid_Id()
        {
            var repos = new MockPostRepositories();

            var request = new UnpublishPostRequest
            {
                Id = Guid.NewGuid().ToString(),
                User = new AppUser()
            };

            var sut = new UnpublishPostHandler(repos.PostRepository, repos.PostReadOnlyRepository);
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_NotAuthorizedException_Given_Different_UserId()
        {
            var post = PostFactory.GetExistingPost();
            var repos = new MockPostRepositories();
            await repos.PostRepository.SaveAsync(post);

            var request = new UnpublishPostRequest
            {
                Id = post.Id.ToString(),
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var sut = new UnpublishPostHandler(repos.PostRepository, repos.PostReadOnlyRepository);
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
