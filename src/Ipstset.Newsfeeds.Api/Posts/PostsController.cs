using Ipstset.Newsfeeds.Api;
using Ipstset.Newsfeeds.Api.Attributes;
using Ipstset.Newsfeeds.Api.Helpers;
using Ipstset.Newsfeeds.Api.Posts;
using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.CreatePost;
using Ipstset.Newsfeeds.Application.Posts.DeletePost;
using Ipstset.Newsfeeds.Application.Posts.GetPost;
using Ipstset.Newsfeeds.Application.Posts.GetPosts;
using Ipstset.Newsfeeds.Application.Posts.UpdatePost;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Constants = Ipstset.Newsfeeds.Api.Constants;

namespace Ipstset.Newsposts.Api.Posts
{
    [Route("posts")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    public class PostsController : BaseController
    {
        private readonly IMediator _mediator;
        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all posts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<PostResponse>>> Get([FromQuery]GetPostsModel request)
        {
            return await _mediator.Send(new GetPostsRequest
            {
                FeedId = request.FeedId,
                Published = request.Published,
                Tags = request.Tags,
                Limit = request.Limit ?? Constants.MaxRequestLimit,
                StartAfter = request.StartAfter,
                User = AppUser,
                Sort = request.Sort.ToSortItems("DateCreated")
            });
        }

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = Constants.Routes.Posts.GetPost)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<PostResponse>> Get([FromRoute]string id)
        {
            return await _mediator.Send(new GetPostRequest { Id = id, User = AppUser });
        }

        /// <summary>
        /// Create new post
        /// </summary>
        /// <param name="request">CreatePostModel</param>
        /// <returns></returns>
        [HttpPost(Name = Constants.Routes.Posts.CreatePost)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<PostResponse>> Post([FromBody]CreatePostModel request)
        {
            var result = await _mediator.Send(new CreatePostRequest
            {
                FeedId = request.FeedId,
                Title = request.Title,
                Content = request.Content,
                Tags = request.Tags,
                UserId = AppUser.UserId
            });

            return CreatedAtRoute(Constants.Routes.Posts.GetPost, new { result.Id }, result);
        }

        /// <summary>
        /// Update post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request">UpdatePostModel</param>
        /// <returns></returns>
        [HttpPut("{id}", Name = Constants.Routes.Posts.UpdatePost)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PostResponse>> Put([FromRoute]string id, [FromBody]UpdatePostModel request)
        {
            var result = await _mediator.Send(new UpdatePostRequest
            {
                Id = id,
                Title = request.Title,
                Content = request.Content,
                Tags = request.Tags,
                User = AppUser
            });

            return Ok(result);
        }

        /// <summary>
        /// Delete post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = Constants.Routes.Posts.DeletePost)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute]string id)
        {
            await _mediator.Send(new DeletePostRequest { Id = id, User = AppUser });
            return NoContent();
        }

    }
}
