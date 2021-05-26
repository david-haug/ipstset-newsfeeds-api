using Ipstset.Newsfeeds.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.EventHandling
{
    public class EventModel
    {
        public EventModel()
        {

        }

        //public EventModel(IEvent @event)
        //{
        //    Id = @event.Id.ToString();
        //    DateOccurred = @event.DateOccurred;
        //    Name = @event.GetType().Name;
        //    Event = @event;
        //}

        public EventModel(IEvent @event, AppUser appUser)
        {
            Id = @event.Id.ToString();
            DateOccurred = @event.DateOccurred;
            Name = @event.GetType().Name;
            Event = @event;
            User = appUser;
        }

        public string Id { get; set; }
        public DateTimeOffset DateOccurred { get; set; }
        public string Name { get; set; }
        public object Event { get; set; }
        public AppUser User { get; set; }
    }
}
