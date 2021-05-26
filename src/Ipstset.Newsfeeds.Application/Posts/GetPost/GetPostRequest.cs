using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.GetPost
{
    public class GetPostRequest: IRequest<PostResponse>
    {
        public string Id { get; set; }
        public AppUser User { get; set; }
    }
}
