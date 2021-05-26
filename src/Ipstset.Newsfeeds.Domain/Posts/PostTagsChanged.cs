using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Domain.Posts
{
    public class PostTagsChanged: IEvent
    {
        public PostTagsChanged(Post post)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            PostId = post.Id;
            Tags = post.Tags;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }

        public Guid PostId { get; }
        public IEnumerable<string> Tags { get; }
    }
}
