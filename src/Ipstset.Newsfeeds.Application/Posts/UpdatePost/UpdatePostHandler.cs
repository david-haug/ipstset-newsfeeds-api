using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Domain.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Posts.UpdatePost
{
    public class UpdatePostHandler : IRequestHandler<UpdatePostRequest, PostResponse>
    {
        private IPostRepository _repository;
        private IPostReadOnlyRepository _readOnlyRepository;

        public UpdatePostHandler(IPostRepository repository, IPostReadOnlyRepository readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<PostResponse> Handle(UpdatePostRequest request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetAsync(Guid.Parse(request.Id));

            if (post == null)
                throw new NotFoundException($"Post not found for id: {request.Id}");

            if (post.CreatedByUserId.ToString() != request.User.UserId)
                throw new NotAuthorizedException();

            if (post.Title != request.Title)
                post.ChangeTitle(request.Title);

            if (post.Content != request.Content)
                post.ChangeContent(request.Content);

            if(!request.Tags.ToArray().SequenceEqual(post.Tags.ToArray()))
                post.ChangeTags(request.Tags);

            await _repository.SaveAsync(post);
            return await _readOnlyRepository.GetByIdAsync(post.Id.ToString());
        }
    }
}
