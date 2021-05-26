using Ipstset.Newsfeeds.Api.Attributes;
using Ipstset.Newsfeeds.Application.Posts.PublishPost;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Api.Posts.Publish
{
    [Route("posts/{id}/publish")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    public class PostsPublishController : BaseController
    {
        private readonly IMediator _mediator;
        public PostsPublishController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Publish post
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PostResponse</returns>
        [HttpPatch(Name = Constants.Routes.Posts.PublishPost)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Patch([FromRoute]string id)
        {
            var result = await _mediator.Send(new PublishPostRequest { Id = id, User = AppUser });
            return Ok(result);
        }
    }
}
