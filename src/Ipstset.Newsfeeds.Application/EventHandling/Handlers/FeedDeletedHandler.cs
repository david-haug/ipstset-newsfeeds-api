using Ipstset.Newsfeeds.Application.Posts.DeletePostsByFeed;
using Ipstset.Newsfeeds.Domain.Feeds;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.EventHandling.Handlers
{
    public class FeedDeletedHandler : IEventHandler<FeedDeleted>
    {
        private DeletePostsByFeedHandler _handler;
        private AppUser _appUser;
        public FeedDeletedHandler(DeletePostsByFeedHandler handler, AppUser appUser)
        {
            _handler = handler;
            _appUser = appUser;
        }

        public async Task HandleAsync(FeedDeleted @event)
        {
            await _handler.Handle(new DeletePostsByFeedRequest
            {
                FeedId = @event.FeedId.ToString(),
                User = _appUser
            }, new System.Threading.CancellationToken());
        }
    }
}
