﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Domain.Feeds
{
    public class FeedCreated: IEvent
    {
        public FeedCreated(Feed feed)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            FeedId = feed.Id;
            Name = feed.Name;
            IsPublic = feed.IsPublic;
            CreatedByUserId = feed.CreatedByUserId;
            DateCreated = feed.DateCreated;
        }
        public Guid FeedId { get; }
        public string Name { get; }
        public bool IsPublic { get; }
        public Guid CreatedByUserId { get; }
        public DateTimeOffset DateCreated { get; }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
    }
}
