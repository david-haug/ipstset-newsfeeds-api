using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Domain.Posts
{
    public class PostTitleChanged: IEvent
    {
        public PostTitleChanged(Post post)
        {
            Id = Guid.NewGuid();
            DateOccurred = DateTimeOffset.Now;

            PostId = post.Id;
            Title = post.Title;
        }

        public Guid Id { get; }
        public DateTimeOffset DateOccurred { get; }

        public Guid PostId { get; }
        public string Title { get; }
    }
}
