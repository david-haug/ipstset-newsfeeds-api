using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts
{
    public class PublishedPostsByFeedResponse
    {
        public string FeedId { get; set; }
        public string FeedName { get; set; }
        public bool IsPublic { get; set; }
        public string CreatedUserId { get; set; }
        public QueryResult<PostResponse> Posts { get; set; }
    }
}
