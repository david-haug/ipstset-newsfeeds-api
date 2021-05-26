using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.UpdatePost
{
    public class UpdatePostRequest: IRequest<PostResponse>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public AppUser User { get; set; }
    }
}
