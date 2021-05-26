﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Domain.Posts
{
    public class PostContentChanged: IEvent
    {
        public PostContentChanged(Post post)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            PostId = post.Id;
            Content = post.Content;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }

        public Guid PostId { get; }
        public string Content { get; }
    }
}
