using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Domain.Feeds;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Feeds.UpdateFeed
{
    public class UpdateFeedHandler : IRequestHandler<UpdateFeedRequest, FeedResponse>
    {
        private IFeedRepository _repository;
        private IFeedReadOnlyRepository _readOnlyRepository;

        public UpdateFeedHandler(IFeedRepository repository, IFeedReadOnlyRepository readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<FeedResponse> Handle(UpdateFeedRequest request, CancellationToken cancellationToken)
        {
            var feed = await _repository.GetAsync(Guid.Parse(request.Id));
            if (feed == null)
                throw new NotFoundException($"Feed not found for id: {request.Id}");

            if (feed.CreatedByUserId.ToString() != request.User.UserId)
                throw new NotAuthorizedException();

            if (feed.Name != request.Name)
                feed.ChangeName(request.Name);

            if (feed.IsPublic != request.IsPublic)
                feed.ChangeIsPublic(request.IsPublic);

            await _repository.SaveAsync(feed);
            return await _readOnlyRepository.GetByIdAsync(feed.Id.ToString());
        }
    }
}
