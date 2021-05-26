using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.UnpublishPost
{
    public class UnpublishPostRequest : IRequest<PostResponse>
    {
        public string Id { get; set; }
        public AppUser User { get; set; }
    }
}
