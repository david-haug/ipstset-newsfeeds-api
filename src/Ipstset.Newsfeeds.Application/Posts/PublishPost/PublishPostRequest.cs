using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.PublishPost
{
    public class PublishPostRequest: IRequest<PostResponse>
    {
        public string Id { get; set; }
        public AppUser User { get; set; }
    }
}
