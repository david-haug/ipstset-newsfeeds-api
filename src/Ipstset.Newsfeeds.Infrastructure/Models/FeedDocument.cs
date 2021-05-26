using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Infrastructure.Models
{
    public class FeedDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public string CreatedUserId { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
