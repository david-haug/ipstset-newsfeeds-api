using Ipstset.Newsfeeds.Domain.Feeds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Tests.Common.Fakes.Domain
{
    public class FeedFactory
    {
        public static Feed GetExistingFeed()
        {
            var feed = Feed.Load(Guid.NewGuid(), "testfeed", true, Guid.NewGuid(), DateTimeOffset.Now);
            return feed;
        }

    }
}
