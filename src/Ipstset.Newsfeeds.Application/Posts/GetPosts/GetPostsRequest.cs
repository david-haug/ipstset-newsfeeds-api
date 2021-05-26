using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.GetPosts
{
    public class GetPostsRequest : IRequest<QueryResult<PostResponse>>
    {
        public string FeedId { get; set; }
        public bool? Published { get; set; }
        public string[] Tags { get; set; }
        public int Limit { get; set; }
        public string StartAfter { get; set; }
        public AppUser User { get; set; }
        public IEnumerable<SortItem> Sort { get; set; }
    }
}
