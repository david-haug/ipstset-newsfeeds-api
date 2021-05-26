using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Feeds.GetFeeds
{
    public class GetFeedsRequest: IRequest<QueryResult<FeedResponse>>
    {
        public int Limit { get; set; }
        public string StartAfter { get; set; }
        public AppUser User { get; set; }
        public IEnumerable<SortItem> Sort { get; set; }
    }
}
