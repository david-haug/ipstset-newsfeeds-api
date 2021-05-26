using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Feeds.GetFeeds;
using Ipstset.Newsfeeds.Domain.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Tests.Common.Fakes.Repositories
{
    //Mock class for both domain and read-only repository so they can share the same collection
    public class MockFeedRepositories
    {
        public MockFeedRepositories()
        {
            _feeds = new List<Feed>();
            FeedRepository = new Repository(_feeds);
            FeedReadOnlyRepository = new ReadOnlyRepository(_feeds);
        }
        public IFeedRepository FeedRepository { get; set; }
        public IFeedReadOnlyRepository FeedReadOnlyRepository { get; set; }
        private List<Feed> _feeds;

        private class Repository : IFeedRepository
        {
            private List<Feed> _feeds;
            public Repository(List<Feed> feeds)
            {
                _feeds = feeds;
            }
            public Task<Feed> GetAsync(Guid id)
            {
                var feed = _feeds.FirstOrDefault(x => x.Id == id);
                return Task.FromResult(feed);
            }

            public Task SaveAsync(Feed feed)
            {
                _feeds.Add(feed);
                return Task.CompletedTask;
            }

            public Task DeleteAsync(Feed feed)
            {
                _feeds.Remove(feed);
                return Task.CompletedTask;
            }
        }

        private class ReadOnlyRepository : IFeedReadOnlyRepository
        {
            private List<Feed> _feeds;
            public ReadOnlyRepository(List<Feed> feeds)
            {
                _feeds = feeds;
            }

            public async Task<FeedResponse> GetByIdAsync(string id)
            {
                var feed = _feeds.FirstOrDefault(o => o.Id.ToString() == id);
                if (feed == null)
                    return null;

                var dto = new FeedResponse
                {
                    Id = feed.Id.ToString(),
                    Name = feed.Name,
                    IsPublic = feed.IsPublic,
                    CreatedUserId = feed.CreatedByUserId.ToString(),
                    DateCreated = feed.DateCreated
                };

                return dto;
            }

            public async Task<QueryResult<FeedResponse>> GetFeedsAsync(GetFeedsRequest request)
            {
                var results = new List<FeedResponse>();
                foreach (var feed in _feeds)
                {
                    var dto = new FeedResponse
                    {
                        Id = feed.Id.ToString(),
                        Name = feed.Name,
                        IsPublic = feed.IsPublic,
                        CreatedUserId = feed.CreatedByUserId.ToString(),
                        DateCreated = feed.DateCreated
                    };

                    results.Add(dto);
                }

                return new QueryResult<FeedResponse> { Items = results, TotalRecords = results.Count, Limit = request.Limit, StartAfter = request.StartAfter };

            }
        }
    }
}
