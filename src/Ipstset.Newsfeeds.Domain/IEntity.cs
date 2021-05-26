using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Domain
{
    public interface IEntity
    {
        void AddEvent(IEvent @event);
        IReadOnlyCollection<IEvent> DequeueEvents();
    }
}
