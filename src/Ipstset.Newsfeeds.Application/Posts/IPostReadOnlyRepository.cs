using Ipstset.Newsfeeds.Application.Posts.GetPosts;
using Ipstset.Newsfeeds.Application.Posts.GetPublishedPostsByFeed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Posts
{
    public interface IPostReadOnlyRepository
    {
        Task<PostResponse> GetByIdAsync(string id);
        Task<PublishedPostsByFeedResponse> GetPublishedPostsByFeedAsync(GetPublishedPostsByFeedRequest request);
        Task<QueryResult<PostResponse>> GetPostsAsync(GetPostsRequest request);
    }
}
