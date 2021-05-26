using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Domain.Feeds;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Feeds.DeleteFeed
{
    public class DeleteFeedHandler : IRequestHandler<DeleteFeedRequest>
    {
        private IFeedRepository _repository;

        public DeleteFeedHandler(IFeedRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteFeedRequest request, CancellationToken cancellationToken)
        {
            var feed = await _repository.GetAsync(Guid.Parse(request.Id));
            if (feed == null)
                throw new NotFoundException($"Feed not found for id: {request.Id}");

            if (feed.CreatedByUserId.ToString() != request.User.UserId)
                throw new NotAuthorizedException();

            feed.Delete();
            await _repository.DeleteAsync(feed);

            return await Unit.Task;
        }
    }
}
