using Ipstset.Newsfeeds.Api.Attributes;
using Ipstset.Newsfeeds.Api.Helpers;
using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Feeds.CreateFeed;
using Ipstset.Newsfeeds.Application.Feeds.DeleteFeed;
using Ipstset.Newsfeeds.Application.Feeds.GetFeed;
using Ipstset.Newsfeeds.Application.Feeds.GetFeeds;
using Ipstset.Newsfeeds.Application.Feeds.UpdateFeed;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Api.Feeds
{
    [Route("feeds")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    public class FeedsController: BaseController
    {
        private readonly IMediator _mediator;
        public FeedsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all feeds
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<FeedResponse>>> Get([FromQuery]GetFeedsModel request) => await _mediator.Send(new GetFeedsRequest
        {
            Limit = request.Limit ?? Constants.MaxRequestLimit,
            StartAfter = request.StartAfter,
            User = AppUser,
            Sort = request.Sort.ToSortItems("DateCreated")
        });

        /// <summary>
        /// Get feed by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = Constants.Routes.Feeds.GetFeed)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<FeedResponse>> Get([FromRoute]string id) => await _mediator.Send(new GetFeedRequest { Id = id, User = AppUser });

        /// <summary>
        /// Create new feed
        /// </summary>
        /// <param name="request">CreateFeedModel</param>
        /// <returns></returns>
        [HttpPost(Name = Constants.Routes.Feeds.CreateFeed)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<FeedResponse>> Post([FromBody]CreateFeedModel request)
        {
            var result = await _mediator.Send(new CreateFeedRequest
            {
                Name = request.Name,
                IsPublic = request.IsPublic,
                UserId = AppUser.UserId
            });

            return CreatedAtRoute(Constants.Routes.Feeds.GetFeed, new { result.Id }, result);
        }

        /// <summary>
        /// Update feed
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request">UpdateFeedModel</param>
        /// <returns></returns>
        [HttpPut("{id}", Name = Constants.Routes.Feeds.UpdateFeed)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FeedResponse>> Put([FromRoute]string id, [FromBody]UpdateFeedModel request)
        {
            var result = await _mediator.Send(new UpdateFeedRequest
            {
                Id = id,
                Name = request.Name,
                IsPublic = request.IsPublic,
                User = AppUser
            });

            return Ok(result);
        }

        /// <summary>
        /// Delete feed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = Constants.Routes.Feeds.DeleteFeed)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute]string id)
        {
            await _mediator.Send(new DeleteFeedRequest { Id = id, User = AppUser });
            return NoContent();
        }

    }
}
