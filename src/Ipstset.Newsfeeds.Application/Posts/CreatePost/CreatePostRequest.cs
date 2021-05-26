using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.CreatePost
{
    public class CreatePostRequest: IRequest<PostResponse>
    {
        public string FeedId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
