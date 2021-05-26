using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Feeds.CreateFeed
{
    public class CreateFeedRequest: IRequest<FeedResponse>
    {
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public string UserId { get; set; }
    }
}
