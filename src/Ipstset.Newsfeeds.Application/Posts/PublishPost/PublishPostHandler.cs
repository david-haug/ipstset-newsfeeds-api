using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Domain.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Posts.PublishPost
{
    public class PublishPostHandler : IRequestHandler<PublishPostRequest, PostResponse>
    {
        private IPostRepository _repository;
        private IPostReadOnlyRepository _readOnlyRepository;

        public PublishPostHandler(IPostRepository repository, IPostReadOnlyRepository readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<PostResponse> Handle(PublishPostRequest request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetAsync(Guid.Parse(request.Id));

            if (post == null)
                throw new NotFoundException($"Post not found for id: {request.Id}");

            if (post.CreatedByUserId.ToString() != request.User.UserId)
                throw new NotAuthorizedException();

            post.Publish();
            await _repository.SaveAsync(post);
            return await _readOnlyRepository.GetByIdAsync(post.Id.ToString());
        }
    }
}
