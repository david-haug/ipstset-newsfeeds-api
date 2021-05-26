using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts
{
    public class PostResponse
    {
        public string Id { get; set; }
        public string FeedId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DatePublished { get; set; }
        public bool IsPublished => DatePublished.HasValue;
        public IEnumerable<string> Tags { get; set; }
    }
}
