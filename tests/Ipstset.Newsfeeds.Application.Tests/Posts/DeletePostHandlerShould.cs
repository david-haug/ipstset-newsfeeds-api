using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Posts.DeletePost;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Domain;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Posts
{
    public class DeletePostHandlerShould
    {
        [Fact]
        public async void Delete_Post()
        {
            var repos = new MockPostRepositories();
            var post = PostFactory.GetExistingPost();
            await repos.PostRepository.SaveAsync(post);

            var request = new DeletePostRequest
            {
                Id = post.Id.ToString(),
                User = new AppUser { UserId = post.CreatedByUserId.ToString() }
            };

            var sut = new DeletePostHandler(repos.PostRepository);
            await sut.Handle(request, new System.Threading.CancellationToken());

            var actual = await repos.PostReadOnlyRepository.GetByIdAsync(request.Id);
            Assert.Null(actual);
        }

        [Fact]
        public async void Throw_NotFoundException_Given_Invalid_Id()
        {
            var repos = new MockPostRepositories();
            var request = new DeletePostRequest
            {
                Id = Guid.NewGuid().ToString()
            };

            var sut = new DeletePostHandler(repos.PostRepository);
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_NotAuthorizedException_Given_Different_UserId()
        {
            var repos = new MockPostRepositories();
            var post = PostFactory.GetExistingPost();
            await repos.PostRepository.SaveAsync(post);

            var request = new DeletePostRequest
            {
                Id = post.Id.ToString(),
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var sut = new DeletePostHandler(repos.PostRepository);
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
