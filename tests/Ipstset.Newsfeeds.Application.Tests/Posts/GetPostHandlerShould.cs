using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.GetPost;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Domain;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Posts
{
    public class GetPostHandlerShould
    {
        [Fact]
        public async void Return_PostResponse_Given_Valid_Request()
        {
            var post = PostFactory.GetExistingPost();
            var repos = new MockPostRepositories();
            await repos.PostRepository.SaveAsync(post);

            var request = new GetPostRequest
            {
                Id = post.Id.ToString(),
                User = new AppUser { UserId = post.CreatedByUserId.ToString() }
            };

            var sut = new GetPostHandler(repos.PostReadOnlyRepository);
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.IsType<PostResponse>(actual);
            Assert.Equal(post.Id.ToString(), actual.Id);
        }

        [Fact]
        public async void Throw_NotFoundException_Given_Invalid_Id()
        {
            var request = new GetPostRequest
            {
                Id = Guid.NewGuid().ToString(),
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var repos = new MockPostRepositories();
            var sut = new GetPostHandler(repos.PostReadOnlyRepository);
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_NotAuthorizedException_Given_Invalid_User()
        {
            var post = PostFactory.GetExistingPost();
            var repos = new MockPostRepositories();
            await repos.PostRepository.SaveAsync(post);

            var request = new GetPostRequest
            {
                Id = post.Id.ToString(),
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var sut = new GetPostHandler(repos.PostReadOnlyRepository);
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
