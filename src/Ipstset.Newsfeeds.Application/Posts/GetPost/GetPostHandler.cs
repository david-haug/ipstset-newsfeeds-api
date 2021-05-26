using Ipstset.Newsfeeds.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Posts.GetPost
{
    public class GetPostHandler : IRequestHandler<GetPostRequest, PostResponse>
    {
        private IPostReadOnlyRepository _repository;
        public GetPostHandler(IPostReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<PostResponse> Handle(GetPostRequest request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetByIdAsync(request.Id);
            if (post == null)
                throw new NotFoundException($"Post not found for id: {request.Id}");

            if (!request.User.HasRole(Constants.UserRoles.Admin) && post.CreatedByUserId != request.User.UserId)
                throw new NotAuthorizedException();

            return post;
        }
    }
}
