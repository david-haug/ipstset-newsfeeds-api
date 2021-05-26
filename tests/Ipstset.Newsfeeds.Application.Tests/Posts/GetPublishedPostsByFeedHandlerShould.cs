using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.GetPosts;
using Ipstset.Newsfeeds.Application.Posts.GetPublishedPostsByFeed;
using Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ipstset.Newsfeeds.Application.Tests.Posts
{
    public class GetPublishedPostsByFeedHandlerShould
    {
        [Fact]
        public async void Return_PublishedPostsByFeedResponse_Given_Valid_Request()
        {
            var request = new GetPublishedPostsByFeedRequest
            {
                FeedId = Guid.NewGuid().ToString(),
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var repository = new PublicPostReadOnlyRepositoryStub();
            var sut = new GetPublishedPostsByFeedHandler(repository);
            var actual = await sut.Handle(request, new System.Threading.CancellationToken());
            Assert.IsType<PublishedPostsByFeedResponse>(actual);
        }

        [Fact]
        public async void Throw_NotFoundException_Given_Invalid_FeedId()
        {
            var request = new GetPublishedPostsByFeedRequest
            {
                FeedId = Guid.NewGuid().ToString(),
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var repository = new NotFoundPostReadOnlyRepositoryStub();
            var sut = new GetPublishedPostsByFeedHandler(repository);
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void Throw_NotAuthorizedException_Given_Different_UserId_For_Private_Feed()
        {
            var request = new GetPublishedPostsByFeedRequest
            {
                FeedId = Guid.NewGuid().ToString(),
                User = new AppUser { UserId = Guid.NewGuid().ToString() }
            };

            var repository = new PrivatePostReadOnlyRepositoryStub();
            var sut = new GetPublishedPostsByFeedHandler(repository);
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }


        private class PublicPostReadOnlyRepositoryStub : IPostReadOnlyRepository
        {
            public Task<PostResponse> GetByIdAsync(string id)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<PostResponse>> GetPostsAsync(GetPostsRequest request)
            {
                throw new NotImplementedException();
            }

            public async Task<PublishedPostsByFeedResponse> GetPublishedPostsByFeedAsync(GetPublishedPostsByFeedRequest request)
            {
                return new PublishedPostsByFeedResponse { IsPublic = true };
            }

            Task<QueryResult<PostResponse>> IPostReadOnlyRepository.GetPostsAsync(GetPostsRequest request)
            {
                throw new NotImplementedException();
            }
        }

        private class PrivatePostReadOnlyRepositoryStub : IPostReadOnlyRepository
        {
            public Task<PostResponse> GetByIdAsync(string id)
            {
                throw new NotImplementedException();
            }

            public Task<QueryResult<PostResponse>> GetPostsAsync(GetPostsRequest request)
            {
                throw new NotImplementedException();
            }

            public async Task<PublishedPostsByFeedResponse> GetPublishedPostsByFeedAsync(GetPublishedPostsByFeedRequest request)
            {
                return new PublishedPostsByFeedResponse();
            }
        }

        private class NotFoundPostReadOnlyRepositoryStub : IPostReadOnlyRepository
        {
            public Task<PostResponse> GetByIdAsync(string id)
            {
                throw new NotImplementedException();
            }

            public Task<QueryResult<PostResponse>> GetPostsAsync(GetPostsRequest request)
            {
                throw new NotImplementedException();
            }

            public async Task<PublishedPostsByFeedResponse> GetPublishedPostsByFeedAsync(GetPublishedPostsByFeedRequest request)
            {
                return null;
            }
        }

    }
}
