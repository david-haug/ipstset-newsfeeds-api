using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.UpdatePost;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Domain;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Posts
{
    public class UpdatePostHandlerShould
    {
        [Fact]
        public async void Return_PostResponse_Given_Valid_Request()
        {
            var post = PostFactory.GetExistingPost();
            var repos = new MockPostRepositories();
            await repos.PostRepository.SaveAsync(post);

            var request = new UpdatePostRequest
            {
                Id = post.Id.ToString(),
                Title = $"new title {DateTime.Now.ToString()}",
                Content = $"new content {DateTime.Now.ToString()}",
                Tags = new List<string> { "1","2","3" },
                User = new AppUser { UserId = post.CreatedByUserId.ToString() }
            };

            var sut = new UpdatePostHandler(repos.PostRepository, repos.PostReadOnlyRepository);
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.IsType<PostResponse>(actual);
            Assert.Equal(post.Id.ToString(), actual.Id);
            Assert.Equal(request.Title, actual.Title);
            Assert.Equal(request.Content, actual.Content);
            Assert.Equal(request.Tags.ToList().Count(), actual.Tags.Count());
        }

        [Fact]
        public async void Throw_NotFoundException_Given_Invalid_Id()
        {
            var repos = new MockPostRepositories();
            var request = new UpdatePostRequest
            {
                Id = Guid.NewGuid().ToString(),
                Title = $"new title {DateTime.Now.ToString()}",
                Content = $"new content {DateTime.Now.ToString()}",
                Tags = new List<string> { "1", "2", "3" },
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var sut = new UpdatePostHandler(repos.PostRepository, repos.PostReadOnlyRepository);
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_NotAuthorizedException_Given_Different_UserId()
        {
            var post = PostFactory.GetExistingPost();
            var repos = new MockPostRepositories();
            await repos.PostRepository.SaveAsync(post);

            var request = new UpdatePostRequest
            {
                Id = post.Id.ToString(),
                Title = $"new title {DateTime.Now.ToString()}",
                Content = $"new content {DateTime.Now.ToString()}",
                Tags = new List<string> { "1", "2", "3" },
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var sut = new UpdatePostHandler(repos.PostRepository, repos.PostReadOnlyRepository);
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_Exception_Given_No_Content()
        {
            var post = PostFactory.GetExistingPost();
            var repos = new MockPostRepositories();
            await repos.PostRepository.SaveAsync(post);

            var request = new UpdatePostRequest
            {
                Id = post.Id.ToString(),
                Title = $"new title {DateTime.Now.ToString()}",
                Content = "",
                Tags = new List<string> { "1", "2", "3" },
                User = new AppUser { UserId = post.CreatedByUserId.ToString() }
            };

            var sut = new UpdatePostHandler(repos.PostRepository, repos.PostReadOnlyRepository);
            await Assert.ThrowsAnyAsync<Exception>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Update_Tags()
        {
            var post = PostFactory.GetExistingPost();
            var repos = new MockPostRepositories();
            await repos.PostRepository.SaveAsync(post);

            var oldTags = post.Tags.ToArray();
            var request = new UpdatePostRequest
            {
                Id = post.Id.ToString(),
                Title = $"new title {DateTime.Now.ToString()}",
                Content = $"new content {DateTime.Now.ToString()}",
                Tags = new List<string> { "1" },
                User = new AppUser { UserId = post.CreatedByUserId.ToString() }
            };

            var sut = new UpdatePostHandler(repos.PostRepository, repos.PostReadOnlyRepository);
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.IsType<PostResponse>(actual);
            Assert.Equal(post.Id.ToString(), actual.Id);
            Assert.Equal(request.Title, actual.Title);
            Assert.Equal(request.Content, actual.Content);
            Assert.Equal(request.Tags.ToList().Count(), actual.Tags.Count());
            Assert.NotEqual(oldTags.FirstOrDefault(), actual.Tags.FirstOrDefault());
        }

    }
}
