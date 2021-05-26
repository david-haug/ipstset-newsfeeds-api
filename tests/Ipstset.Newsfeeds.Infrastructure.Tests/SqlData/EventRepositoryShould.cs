using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.EventHandling;
using Ipstset.Newsfeeds.Application.Events;
using Ipstset.Newsfeeds.Infrastructure.SqlData;
using Ipstset.Newsfeeds.Tests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Infrastructure.Tests.SqlData
{
    public class EventRepositoryShould
    {
        [Fact]
        public async void DB_Save_Event()
        {
            var @event = new EventModel 
            { 
                Id = Guid.NewGuid().ToString(),
                Name = "TestEvent",
                Event = new object(),
                DateOccurred = DateTime.Now
            };

            var sut = new EventRepository(Config.Connections.Newsfeeds);
            await sut.SaveAsync(@event);
        }

        [Fact]
        public async void DB_Get_Events()
        {
            var request = new GetEventsRequest
            {
                User = new AppUser { Roles = new[] { "admin"} },
                Limit = 100
            };

            var sut = new EventRepository(Config.Connections.Newsfeeds);
            var actual = await sut.GetEventsAsync(request);
            Assert.NotEmpty(actual.Items);
        }
    }
}
