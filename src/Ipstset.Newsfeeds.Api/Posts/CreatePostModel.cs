using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Api.Posts
{
    public class CreatePostModel
    {
        public string FeedId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
