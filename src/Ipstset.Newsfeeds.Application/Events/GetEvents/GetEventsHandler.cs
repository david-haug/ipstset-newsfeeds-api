using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.EventHandling;
using Ipstset.Newsfeeds.Application.Events;
using Ipstset.Newsfeeds.Application.Exceptions;
using MediatR;

namespace Ipstset.OttaJot.Application.Events.GetEvents
{
    public class GetEventsHandler: IRequestHandler<GetEventsRequest,QueryResult<EventModel>>
    {
        private IEventRepository _repository;
        public GetEventsHandler(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult<EventModel>> Handle(GetEventsRequest request, CancellationToken cancellationToken)
        {
            if (!request.User.HasRole(Constants.UserRoles.Admin))
                throw new NotAuthorizedException();

            return await _repository.GetEventsAsync(request);
        }
    }
}
