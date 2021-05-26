using Ipstset.Newsfeeds.Api.Attributes;
using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.GetPublishedPostsByFeed;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.Newsfeeds.Api.Helpers;
using Microsoft.AspNetCore.Cors;

namespace Ipstset.Newsfeeds.Api.Feeds.Posts
{
    [Route("feeds/{id}/posts")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    public class FeedsPostsController: BaseController
    {
        private readonly IMediator _mediator;
        public FeedsPostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all published posts for feed
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PublishedPostsByFeedResponse>> Get([FromRoute]string id,[FromQuery]GetPostsByFeedModel request)
        {
            return await _mediator.Send(new GetPublishedPostsByFeedRequest {
                FeedId = id,
                Limit = request.Limit ?? Constants.MaxRequestLimit,
                StartAfter = request.StartAfter,
                Sort = request.Sort.ToSortItems("DatePublished",true),
                Tags = request.Tags,
                User = AppUser }); ;
        }
    }
}
