using Ipstset.Newsfeeds.Domain.Feeds;
using Ipstset.Newsfeeds.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Domain.Tests.Posts
{
    public class PostShould
    {
        [Fact]
        public void Create_Given_Valid_Arguments()
        {
            var feed = Feed.Load(Guid.NewGuid(), "testfeed", true, Guid.NewGuid(), DateTimeOffset.Now);
            var title = "Test Title";
            var content = "Test content";
            var userId = Guid.NewGuid();
            var tags = new List<string> { "test" };
            var date = DateTimeOffset.Now;
                       
            var sut = Post.Create(feed,title, content, userId, tags);
            Assert.True(sut.Id != Guid.Empty);
            Assert.Equal(title, sut.Title);
            Assert.Equal(content, sut.Content);
            Assert.Equal(userId, sut.CreatedByUserId);
            Assert.NotEmpty(sut.Tags);
            Assert.Equal(tags[0], sut.Tags[0]);
            Assert.True(sut.DateCreated > date);
        }

        [Fact]
        public void Load_Given_Valid_Arguments()
        {
            var feed = Feed.Load(Guid.NewGuid(), "testfeed", true, Guid.NewGuid(), DateTimeOffset.Now);
            var id = Guid.NewGuid();
            var title = "Test Title";
            var content = "Test content";
            var userId = Guid.NewGuid();
            var tags = new List<string> { "test" };
            var date = DateTimeOffset.Now;

            var sut = Post.Load(id, feed.Id, title, content, userId, date, date, tags);

            Assert.Equal(id, sut.Id);
            Assert.Equal(feed.Id, sut.FeedId);
            Assert.Equal(title, sut.Title);
            Assert.Equal(content, sut.Content);
            Assert.Equal(userId, sut.CreatedByUserId);
            Assert.NotEmpty(sut.Tags);
            Assert.Equal(tags[0], sut.Tags[0]);
            Assert.Equal(date, sut.DateCreated);
            Assert.Equal(date, sut.DatePublished);
        }

        [Fact]
        public void Add_PostCreated_Event()
        {
            var feed = Feed.Load(Guid.NewGuid(), "testfeed", true, Guid.NewGuid(), DateTimeOffset.Now);
            var title = "Test Title";
            var content = "Test content";
            var userId = Guid.NewGuid();
            var tags = new List<string> { "test" };
            var date = DateTimeOffset.Now;

            var sut = Post.Create(feed, title, content, userId, tags);
            var events = sut.DequeueEvents();
            var @event = (PostCreated) events.FirstOrDefault(e => e is PostCreated);
            Assert.Equal(@event.PostId, sut.Id);
        }

        [Fact]
        public void Throw_ArgumentException_Given_No_Title()
        {
            var ex = Assert.Throws<ArgumentException>(() => Post.Create(GetExistingFeed(), "", "content", Guid.NewGuid(), null));
            Assert.Equal("required (Parameter 'title')", ex.Message);
        }

        [Fact]
        public void Throw_ArgumentException_Given_No_Content()
        {
            var ex = Assert.Throws<ArgumentException>(() => Post.Create(GetExistingFeed(), "title", "", Guid.NewGuid(), null));
            Assert.Equal("required (Parameter 'content')", ex.Message);
        }

        [Fact]
        public void Change_Title()
        {
            var title = "A new title";
            var sut = GetExistingPost();
            sut.ChangeTitle(title);
            Assert.Equal(title, sut.Title);
        }

        [Fact]
        public void Add_PostTitleChanged_Event()
        {
            var title = "A new title";
            var sut = GetExistingPost();
            sut.ChangeTitle(title);
            var events = sut.DequeueEvents();
            var @event = (PostTitleChanged)events.FirstOrDefault(e => e is PostTitleChanged);
            Assert.Equal(@event.Title, sut.Title);
        }

        [Fact]
        public void Change_Content()
        {
            var content = "New content...";
            var sut = GetExistingPost();
            sut.ChangeContent(content);
            Assert.Equal(content, sut.Content);
        }

        [Fact]
        public void Add_PostContentChanged_Event()
        {
            var content = "New content...";
            var sut = GetExistingPost();
            sut.ChangeContent(content);
            var events = sut.DequeueEvents();
            var @event = (PostContentChanged)events.FirstOrDefault(e => e is PostContentChanged);
            Assert.Equal(@event.Content, sut.Content);
        }

        [Fact]
        public void Change_Tags()
        {
            var tags = new List<string> { "one", "two", "three" };
            var sut = GetExistingPost();
            sut.ChangeTags(tags);
            Assert.Equal(tags.Count, sut.Tags.Count);
        }

        [Fact]
        public void Add_PostTagsChanged_Event()
        {
            var tags = new List<string> { "one", "two", "three" };
            var sut = GetExistingPost();
            sut.ChangeTags(tags);
            var events = sut.DequeueEvents();
            var @event = (PostTagsChanged)events.FirstOrDefault(e => e is PostTagsChanged);
            Assert.Equal(@event.Tags.Count(), sut.Tags.Count);
        }

        [Fact]
        public void Set_IsPublished_True_When_Published()
        {
            var sut = GetExistingUnpublishedPost();
            sut.Publish();
            Assert.True(sut.IsPublished);
        }

        [Fact]
        public void Publish_When_Only_Unpublished()
        {
            var sut = GetExistingPost();
            var datePublished = sut.DatePublished;
            sut.Publish();
            Assert.Equal(datePublished, sut.DatePublished);
            var events = sut.DequeueEvents();
            Assert.Empty(events);
        }

        [Fact]
        public void Add_PostPublished_Event()
        {
            var sut = GetExistingUnpublishedPost();
            sut.Publish();
            var events = sut.DequeueEvents();
            var @event = (PostPublished)events.FirstOrDefault(e => e is PostPublished);
            Assert.Equal(@event.DatePublished, sut.DatePublished);
        }

        [Fact]
        public void Set_IsPublished_False_When_Unpublished()
        {
            var sut = GetExistingPost();
            sut.Unpublish();
            Assert.False(sut.IsPublished);
        }

        [Fact]
        public void Unpublish_When_Only_Published()
        {
            var sut = GetExistingUnpublishedPost();
            sut.Unpublish();
            var events = sut.DequeueEvents();
            Assert.Empty(events);
        }

        [Fact]
        public void Add_PostUnpublished_Event()
        {
            var sut = GetExistingPost();
            sut.Unpublish();
            var events = sut.DequeueEvents();
            Assert.IsType<PostUnpublished>(events.ToList()[0]);
        }

        [Fact]
        public void Add_PostDeleted_Event()
        {
            var sut = GetExistingPost();
            sut.Delete();
            var events = sut.DequeueEvents();
            var @event = (PostDeleted)events.FirstOrDefault(e => e is PostDeleted);
            Assert.NotNull(@event);
            Assert.Equal(sut.Id, @event.PostId);
        }

        private Feed GetExistingFeed()
        {
            var feed = Feed.Load(Guid.NewGuid(), "testfeed", true, Guid.NewGuid(), DateTimeOffset.Now);
            return feed;
        }

        private Post GetExistingPost()
        {
            var feed = GetExistingFeed();
            var id = Guid.NewGuid();
            var title = "Test Title";
            var content = "Test content";
            var userId = Guid.NewGuid();
            var tags = new List<string> { "test" };
            var date = DateTimeOffset.Now;

            return Post.Load(id, feed.Id, title, content, userId, date, date, tags);
        }

        private Post GetExistingUnpublishedPost()
        {
            var feed = GetExistingFeed();
            var id = Guid.NewGuid();
            var title = "Test Title";
            var content = "Test content";
            var userId = Guid.NewGuid();
            var tags = new List<string> { "test" };
            var date = DateTimeOffset.Now;

            return Post.Load(id, feed.Id, title, content, userId, date, null, tags);
        }
    }
}
