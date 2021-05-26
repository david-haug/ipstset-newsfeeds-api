using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Domain.Feeds;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Posts.GetPublishedPostsByFeed
{
    public class GetPublishedPostsByFeedHandler : IRequestHandler<GetPublishedPostsByFeedRequest, PublishedPostsByFeedResponse>
    {
        private IPostReadOnlyRepository _repository;

        public GetPublishedPostsByFeedHandler(IPostReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<PublishedPostsByFeedResponse> Handle(GetPublishedPostsByFeedRequest request, CancellationToken cancellationToken)
        {
            var response = await _repository.GetPublishedPostsByFeedAsync(request);
            if (response == null)
                throw new NotFoundException($"Feed not found for id: {request.FeedId}");

            if (!response.IsPublic && response.CreatedUserId != request.User?.UserId)
                throw new NotAuthorizedException();

            return response;
        }
    }
}
