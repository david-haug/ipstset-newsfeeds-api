﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Domain.Posts
{
    public class PostDeleted:IEvent
    {
        public PostDeleted(Post post)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;
            PostId = post.Id;
            FeedId = post.FeedId;
            Title = post.Title;
            Content = post.Content;
            CreatedByUserId = post.CreatedByUserId;
            DateCreated = post.DateCreated;
            Tags = post.Tags;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
        public Guid PostId { get; }
        public Guid FeedId { get; }
        public string Title { get; }
        public string Content { get; }
        public Guid CreatedByUserId { get; }
        public DateTimeOffset DateCreated { get; }
        public IEnumerable<string> Tags { get; }
    }
}
