using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Feeds.GetFeeds
{
    public class GetFeedsHandler : IRequestHandler<GetFeedsRequest, QueryResult<FeedResponse>>
    {
        private IFeedReadOnlyRepository _repository;
        public GetFeedsHandler(IFeedReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult<FeedResponse>> Handle(GetFeedsRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetFeedsAsync(request);

            //safety check...prevent unauthorized results by throwing error, 
            //all or none scenario because repo should be checking for access to determine correct result set
            foreach (var item in result.Items)
            {
                if (!request.User.HasAccessToFeedResponse(item))
                    throw new NotAuthorizedException();
            }

            return result;
        }
    }
}
