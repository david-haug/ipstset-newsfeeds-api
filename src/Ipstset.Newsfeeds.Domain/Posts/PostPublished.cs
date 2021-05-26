using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Domain.Posts
{
    public class PostPublished : IEvent
    {
        public PostPublished(Post post)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            PostId = post.Id;
            DatePublished = post.DatePublished.Value;

        }

        public Guid PostId { get; }
        public DateTimeOffset DatePublished { get; }
        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }
    }
}
