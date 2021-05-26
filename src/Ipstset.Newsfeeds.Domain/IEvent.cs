using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Domain
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTimeOffset DateOccurred { get; }

        //MOVE THIS TO APPLICATION...different type of event...AppEvent: IEvent
        /// <summary>
        /// The ID assigned by the event dispatcher at time of dispatch
        /// </summary>
        /// <remarks>
        /// Intended for identification of a group of events dispatched at the same time
        /// </remarks>
        //Guid? DispatcherProcessId { get; set; }
    }
}
