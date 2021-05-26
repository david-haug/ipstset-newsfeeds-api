using Ipstset.Newsfeeds.Application.Feeds.GetFeeds;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Feeds
{
    public interface IFeedReadOnlyRepository
    {
        Task<FeedResponse> GetByIdAsync(string id);
        Task<QueryResult<FeedResponse>> GetFeedsAsync(GetFeedsRequest request);
    }
}
