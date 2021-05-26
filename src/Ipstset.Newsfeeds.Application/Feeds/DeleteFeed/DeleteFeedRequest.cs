using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Feeds.DeleteFeed
{
    public class DeleteFeedRequest:IRequest
    {
        public string Id { get; set; }
        public AppUser User { get; set; }
    }
}
