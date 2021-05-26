using Ipstset.Newsfeeds.Api.Attributes;
using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.EventHandling;
using Ipstset.Newsfeeds.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.Newsfeeds.Api.Helpers;
namespace Ipstset.Newsfeeds.Api.Events
{
    [Route("events")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    public class EventsController : BaseController
    {
        private readonly IMediator _mediator;
        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all events matching supplied criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<QueryResult<EventModel>> Get([FromQuery] GetEventsModel request)
        {
            return await _mediator.Send(new GetEventsRequest
            {
                Name = request.Name,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Limit = request.Limit ?? Constants.MaxRequestLimit,
                StartAfter = request.StartAfter,
                User = AppUser,
                Sort = request.Sort.ToSortItems("DateOccurred")
            });
        }
    }
}
