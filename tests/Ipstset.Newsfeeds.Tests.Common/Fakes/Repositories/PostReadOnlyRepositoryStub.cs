using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.GetPosts;
using Ipstset.Newsfeeds.Application.Posts.GetPublishedPostsByFeed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories
{
    public class PostReadOnlyRepositoryStub : IPostReadOnlyRepository
    {
        public async Task<PostResponse> GetByIdAsync(string id)
        {
            return new PostResponse();
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
}
