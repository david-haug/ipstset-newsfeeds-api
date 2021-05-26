using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Domain.Feeds
{
    public class FeedIsPublicChanged: IEvent
    {
        public FeedIsPublicChanged(Feed feed)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            FeedId = feed.Id;
            IsPublic = feed.IsPublic;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }

        public Guid FeedId { get; }
        public bool IsPublic { get; }
    }
}
