using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.GetPublishedPostsByFeed
{
    public class GetPublishedPostsByFeedRequest : IRequest<PublishedPostsByFeedResponse>, IQueryOptions
    {
        public string FeedId { get; set; }
        public string[] Tags { get; set; }
        public AppUser User { get; set; }
        public int Limit { get; set; }
        public string StartAfter { get; set; }
        public IEnumerable<SortItem> Sort { get; set; }
    }
}
