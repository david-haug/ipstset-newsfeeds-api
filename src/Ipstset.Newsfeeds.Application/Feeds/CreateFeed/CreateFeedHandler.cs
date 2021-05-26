using Ipstset.Newsfeeds.Domain.Feeds;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Feeds.CreateFeed
{
    public class CreateFeedHandler : IRequestHandler<CreateFeedRequest, FeedResponse>
    {
        private IFeedRepository _repository;
        private IFeedReadOnlyRepository _readOnlyRepository;

        public CreateFeedHandler(IFeedRepository repository, IFeedReadOnlyRepository readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<FeedResponse> Handle(CreateFeedRequest request, CancellationToken cancellationToken)
        {
            var feed = Feed.Create(request.Name, request.IsPublic, Guid.Parse(request.UserId));
            await _repository.SaveAsync(feed);

            return await _readOnlyRepository.GetByIdAsync(feed.Id.ToString());
        }
    }
}
