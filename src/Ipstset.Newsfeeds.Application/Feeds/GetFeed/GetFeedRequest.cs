using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Feeds.GetFeed
{
    public class GetFeedRequest: IRequest<FeedResponse>
    {
        public string Id { get; set; }
        public AppUser User { get; set; }
    }
}
