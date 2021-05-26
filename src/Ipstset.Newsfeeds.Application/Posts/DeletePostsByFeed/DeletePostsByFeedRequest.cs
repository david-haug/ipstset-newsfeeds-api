using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.DeletePostsByFeed
{
    public class DeletePostsByFeedRequest: IRequest
    {
        public string FeedId { get; set; }
        public AppUser User { get; set; }
    }
}
