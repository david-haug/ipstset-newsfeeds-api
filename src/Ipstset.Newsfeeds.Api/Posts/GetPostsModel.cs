using Ipstset.Newsfeeds.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Api.Posts
{
    public class GetPostsModel:QueryModel
    {
        public string FeedId { get; set; }
        public string[] Tags { get; set; }
        public bool? Published { get; set; }
    }
}
