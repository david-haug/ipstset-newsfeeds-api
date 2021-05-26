using Ipstset.Newsfeeds.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Api.Feeds.Posts
{
    public class GetPostsByFeedModel:QueryModel
    {
        public string[] Tags { get; set; }
    }
}
