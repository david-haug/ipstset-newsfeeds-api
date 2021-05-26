using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Text;
using Ipstset.Newsfeeds.Domain.Posts;
using Ipstset.Newsfeeds.Domain.Users;

namespace Ipstset.Newsfeeds.Domain.Feeds
{
    public class Feed: Entity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsPublic { get; private set; }
        public Guid CreatedByUserId { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        public static Feed Create(string name, bool isPublic, Guid createdByUserId)
        {
            var feed = Load(Guid.NewGuid(), name, isPublic, createdByUserId, DateTimeOffset.Now);
            feed.AddEvent(new FeedCreated(feed));
            return feed;
        }

        public static Feed Load(Guid id, string name, bool isPublic, Guid createdByUserId, DateTimeOffset dateCreated)
        {
            ValidateName(name);

            var feed = new Feed
            {
                Id = id,
                Name = name,
                IsPublic = isPublic,
                CreatedByUserId = createdByUserId,
                DateCreated = dateCreated
            };

            return feed;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("required", "name");
        }

        public void ChangeName(string name)
        {
            ValidateName(name);
            Name = name;
            AddEvent(new FeedNameChanged(this));
        }

        public void ChangeIsPublic(bool isPublic)
        {
            IsPublic = isPublic;
            AddEvent(new FeedIsPublicChanged(this));
        }

        public void Delete()
        {
            AddEvent(new FeedDeleted(this));
        }
        

    }
}
