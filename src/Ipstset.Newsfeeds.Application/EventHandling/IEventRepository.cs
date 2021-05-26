using Ipstset.Newsfeeds.Application.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.EventHandling
{
    public interface IEventRepository
    {
        Task SaveAsync(EventModel @event);
        Task<QueryResult<EventModel>> GetEventsAsync(GetEventsRequest request);
    }
}
