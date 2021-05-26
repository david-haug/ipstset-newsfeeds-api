using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Ipstset.Newsfeeds.Domain.Feeds;
using Ipstset.Newsfeeds.Domain.Users;

namespace Ipstset.Newsfeeds.Domain.Posts
{
    public class Post: Entity
    {
        public Guid Id { get; private set; }
        public Guid FeedId { get; private set; }
        public string Title { get; private set;  }
        public string Content { get; private set; }
        public Guid CreatedByUserId { get; private set; }
        public DateTimeOffset DateCreated { get; private set; }
        public DateTimeOffset? DatePublished { get; private set; }
        public bool IsPublished => DatePublished.HasValue;

        private readonly List<string> _tags = new List<string>();
        public ReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        public static Post Create(Feed feed, string title, string content, Guid createdByUserId, IEnumerable<string> tags)
        {
            var post = Load(Guid.NewGuid(), feed.Id, title, content, createdByUserId, DateTimeOffset.Now,null, tags);
            post.AddEvent(new PostCreated(post));
            return post;
        }

        public static Post Load(Guid id, Guid feedId, string title, string content, Guid createdByUserId, DateTimeOffset dateCreated, DateTimeOffset? datePublished, IEnumerable<string> tags)
        {
            ValidateTitle(title);
            ValidateContent(content);

            var post = new Post
            {
                Id = id,
                FeedId = feedId,
                Title = title,
                Content = content,
                CreatedByUserId = createdByUserId,
                DateCreated = dateCreated,
                DatePublished = datePublished
            };

            if (tags != null)
                post._tags.AddRange(tags);

            return post;
        }

        private static void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("required", "title");
        }

        private static void ValidateContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("required", "content");
        }

        public void ChangeTitle(string title)
        {
            ValidateTitle(title);
            Title = title;
            AddEvent(new PostTitleChanged(this));
        }

        public void ChangeContent(string content)
        {
            ValidateContent(content);
            Content = content;
            AddEvent(new PostContentChanged(this));
        }

        public void ChangeTags(IEnumerable<string> tags)
        {
            _tags.Clear();
            _tags.AddRange(tags);
            AddEvent(new PostTagsChanged(this));
        }

        public void Publish()
        {
            if (IsPublished)
                return;

            DatePublished = DateTimeOffset.Now;
            AddEvent(new PostPublished(this));
        }

        public void Unpublish()
        {
            if (!IsPublished)
                return;

            DatePublished = null;
            AddEvent(new PostUnpublished(this));
        }

        public void Delete()
        {
            AddEvent(new PostDeleted(this));
        }


    }
}
