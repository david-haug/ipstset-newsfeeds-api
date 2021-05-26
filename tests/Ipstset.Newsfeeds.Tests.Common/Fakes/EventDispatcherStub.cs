using Ipstset.Newsfeeds.Application.EventHandling;
using Ipstset.Newsfeeds.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Tests.Common.Fakes
{
    public class EventDispatcherStub : IEventDispatcher
    {
        public async Task DispatchAsync<T>(params T[] events) where T : IEvent
        {
            //do nothing
        }
    }
}
