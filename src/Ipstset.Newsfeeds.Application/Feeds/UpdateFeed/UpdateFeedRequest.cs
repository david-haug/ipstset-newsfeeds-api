using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Feeds.UpdateFeed
{
    public class UpdateFeedRequest: IRequest<FeedResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public AppUser User { get; set; }
    }
}
