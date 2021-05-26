using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.DeletePost
{
    public class DeletePostRequest : IRequest
    {
        public string Id { get; set; }
        public AppUser User { get; set; }
    }

}
