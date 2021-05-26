using Ipstset.Newsfeeds.Application.Exceptions;
using Ipstset.Newsfeeds.Domain.Feeds;
using Ipstset.Newsfeeds.Domain.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Posts.CreatePost
{
    public class CreatePostHandler : IRequestHandler<CreatePostRequest, PostResponse>
    {
        private IPostRepository _repository;
        private IPostReadOnlyRepository _readOnlyRepository;
        private IFeedRepository _feedRepository;

        public CreatePostHandler(IPostRepository repository, IPostReadOnlyRepository readOnlyRepository, IFeedRepository feedRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _feedRepository = feedRepository;
        }

        public async Task<PostResponse> Handle(CreatePostRequest request, CancellationToken cancellationToken)
        {
            var feed = await _feedRepository.GetAsync(Guid.Parse(request.FeedId));
            if (feed == null)
                throw new NotFoundException($"Feed not found for id: {request.FeedId}");

            var post = Post.Create(feed, request.Title, request.Content, Guid.Parse(request.UserId), request.Tags);
            await _repository.SaveAsync(post);

            return await _readOnlyRepository.GetByIdAsync(post.Id.ToString());
        }
    }
}
