using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Domain.Feeds
{
    public class FeedNameChanged: IEvent
    {
        public FeedNameChanged(Feed feed)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            FeedId = feed.Id;
            Name = feed.Name;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }

        public Guid FeedId { get; }
        public string Name { get; }
    }
}
