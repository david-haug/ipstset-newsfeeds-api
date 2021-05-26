using Ipstset.Newsfeeds.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Feeds.GetFeed
{
    public class GetFeedHandler : IRequestHandler<GetFeedRequest, FeedResponse>
    {
        private IFeedReadOnlyRepository _repository;
        public GetFeedHandler(IFeedReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<FeedResponse> Handle(GetFeedRequest request, CancellationToken cancellationToken)
        {
            var feed = await _repository.GetByIdAsync(request.Id);
            if(feed == null)
                throw new NotFoundException($"Feed not found for id: {request.Id}");

            if (!request.User.HasRole(Constants.UserRoles.Admin) && feed.CreatedUserId != request.User.UserId)
                throw new NotAuthorizedException();

            return feed;
        }
    }
}
