using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Domain.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Posts.DeletePost
{
    public class DeletePostHandler : IRequestHandler<DeletePostRequest>
    {
        private IPostRepository _repository;

        public DeletePostHandler(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeletePostRequest request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetAsync(Guid.Parse(request.Id));
            if (post == null)
                throw new NotFoundException($"Post not found for id: {request.Id}");

            if (post.CreatedByUserId.ToString() != request.User.UserId)
                throw new NotAuthorizedException();

            post.Delete();
            await _repository.DeleteAsync(post);

            return await Unit.Task;
        }
    }
}
