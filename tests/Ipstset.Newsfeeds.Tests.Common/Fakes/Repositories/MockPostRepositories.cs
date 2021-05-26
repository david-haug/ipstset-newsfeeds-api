using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.GetPosts;
using Ipstset.Newsfeeds.Application.Posts.GetPublishedPostsByFeed;
using Ipstset.Newsfeeds.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories
{
    //Mock class for both domain and read-only repository so they can share the same collection
    public class MockPostRepositories
    {
       
        public MockPostRepositories()
        {
            PostRepository = new Repository(_posts);
            PostReadOnlyRepository = new ReadOnlyRepository(_posts);
        }

        public IPostRepository PostRepository { get; set; }
        public IPostReadOnlyRepository PostReadOnlyRepository { get; set; }
        private List<Post> _posts = new List<Post>();

        private class Repository : IPostRepository
        {
            private List<Post> _posts;
            public Repository(List<Post> posts)
            {
                _posts = posts;
            }

            public Task<Post> GetAsync(Guid id)
            {
                var post = _posts.FirstOrDefault(x => x.Id == id);
                return Task.FromResult(post);
            }

            public Task SaveAsync(Post post)
            {
                _posts.Add(post);
                return Task.CompletedTask;
            }

            public Task DeleteAsync(Post post)
            {
                _posts.Remove(post);
                return Task.CompletedTask;
            }

            public Task<IEnumerable<Post>> GetAllByFeedIdAsync(Guid feedId)
            {
                throw new NotImplementedException();
            }
        }

        private class ReadOnlyRepository : IPostReadOnlyRepository
        {
            private List<Post> _posts;
            public ReadOnlyRepository(List<Post> posts)
            {
                _posts = posts;
            }

            public async Task<PostResponse> GetByIdAsync(string id)
            {
                var post = _posts.FirstOrDefault(o => o.Id.ToString() == id);
                if (post == null)
                    return null;

                var dto = new PostResponse
                {
                    Id = post.Id.ToString(),
                    FeedId = post.FeedId.ToString(),
                    Title = post.Title,
                    Content = post.Content,
                    CreatedByUserId = post.CreatedByUserId.ToString(),
                    DateCreated = post.DateCreated,
                    DatePublished = post.DatePublished,
                    Tags = post.Tags
                };

                return dto;
            }

            public Task<IEnumerable<PostResponse>> GetPostsAsync(GetPostsRequest request)
            {
                throw new NotImplementedException();
            }

            public Task<PublishedPostsByFeedResponse> GetPublishedPostsByFeedAsync(GetPublishedPostsByFeedRequest request)
            {
                throw new NotImplementedException();
            }

            Task<QueryResult<PostResponse>> IPostReadOnlyRepository.GetPostsAsync(GetPostsRequest request)
            {
                throw new NotImplementedException();
            }
        }
        
    }
}
