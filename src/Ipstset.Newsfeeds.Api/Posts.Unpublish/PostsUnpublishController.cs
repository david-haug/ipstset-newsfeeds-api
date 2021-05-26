using Ipstset.Newsfeeds.Api.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ipstset.Newsfeeds.Application.Posts.UnpublishPost;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Api.Posts.Unpublish
{
    [Route("posts/{id}/unpublish")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    public class PostsUnpublishController : BaseController
    {
        private readonly IMediator _mediator;
        public PostsUnpublishController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Unpublish post
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PostResponse</returns>
        [HttpPatch(Name = Constants.Routes.Posts.UnpublishPost)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Patch([FromRoute]string id)
        {
            var result = await _mediator.Send(new UnpublishPostRequest { Id = id, User = AppUser });
            return Ok(result);
        }
    }
}
