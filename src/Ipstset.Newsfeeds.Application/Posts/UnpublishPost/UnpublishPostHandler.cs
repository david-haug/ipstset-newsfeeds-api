using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Domain.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Posts.UnpublishPost
{
    public class UnpublishPostHandler : IRequestHandler<UnpublishPostRequest, PostResponse>
    {
        private IPostRepository _repository;
        private IPostReadOnlyRepository _readOnlyRepository;

        public UnpublishPostHandler(IPostRepository repository, IPostReadOnlyRepository readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<PostResponse> Handle(UnpublishPostRequest request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetAsync(Guid.Parse(request.Id));

            if (post == null)
                throw new NotFoundException($"Post not found for id: {request.Id}");

            if (post.CreatedByUserId.ToString() != request.User.UserId)
                throw new NotAuthorizedException();

            post.Unpublish();
            await _repository.SaveAsync(post);
            return await _readOnlyRepository.GetByIdAsync(post.Id.ToString());
        }
    }
}
