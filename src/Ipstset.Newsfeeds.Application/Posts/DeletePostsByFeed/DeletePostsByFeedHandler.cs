using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Domain.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Posts.DeletePostsByFeed
{
    public class DeletePostsByFeedHandler : IRequestHandler<DeletePostsByFeedRequest>
    {
        private IPostRepository _repository;

        public DeletePostsByFeedHandler(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeletePostsByFeedRequest request, CancellationToken cancellationToken)
        {
            var posts = await _repository.GetAllByFeedIdAsync(Guid.Parse(request.FeedId));
            foreach(var post in posts)
            {
                if (post.CreatedByUserId.ToString() != request.User.UserId)
                    throw new NotAuthorizedException();

                post.Delete();
                await _repository.DeleteAsync(post);
            }

            return await Unit.Task;
        }
    }
}
