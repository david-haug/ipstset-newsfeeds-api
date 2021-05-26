using Ipstset.Newsfeeds.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Tests.Common.Fakes.Domain
{
    public class PostFactory
    {
        public static Post GetExistingPost(DateTimeOffset? publishDate = null)
        {
            var post = Post.Load(Guid.NewGuid(), Guid.NewGuid(), "testpost", "test content", Guid.NewGuid(), DateTimeOffset.Now, publishDate, new List<string> { "one" });
            return post;
        }

    }

}
