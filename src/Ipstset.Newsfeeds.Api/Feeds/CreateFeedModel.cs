using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Api.Feeds
{
    public class CreateFeedModel
    {
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }
}
