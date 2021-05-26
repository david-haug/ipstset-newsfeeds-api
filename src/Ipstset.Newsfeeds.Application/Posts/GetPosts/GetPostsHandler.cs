using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Application.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Posts.GetPosts
{
    public class GetPostsHandler : IRequestHandler<GetPostsRequest, QueryResult<PostResponse>>
    {
        private IPostReadOnlyRepository _repository;
        public GetPostsHandler(IPostReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult<PostResponse>> Handle(GetPostsRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetPostsAsync(request);
            foreach (var item in result.Items)
            {
                if (!request.User.HasAccessToPostResponse(item))
                    throw new NotAuthorizedException();
            }

            return result;
        }

    }
}
