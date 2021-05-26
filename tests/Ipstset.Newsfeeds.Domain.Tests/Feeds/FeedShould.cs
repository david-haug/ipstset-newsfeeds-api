using Ipstset.Newsfeeds.Domain.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Domain.Tests.Feeds
{
    public class FeedShould
    {
        [Fact]
        public void Create_Given_Valid_Arguments()
        {
            //arrange
            var name = "test feed";
            var isPublic = true;
            var userId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            //act
            var sut = Feed.Create(name,isPublic,userId);

            //Assert
            Assert.True(sut.Id != Guid.Empty);
            Assert.Equal(name, sut.Name);
            Assert.Equal(isPublic, sut.IsPublic);
            Assert.Equal(userId, sut.CreatedByUserId);
            Assert.True(sut.DateCreated > date);
        }

        [Fact]
        public void Load_Given_Valid_Arguments()
        {
            //arrange
            var id = Guid.NewGuid();
            var name = "test feed";
            var isPublic = true;
            var userId = Guid.NewGuid();
            var date = DateTimeOffset.Now;

            //act
            var sut = Feed.Load(id,name, isPublic, userId, date);

            //Assert
            Assert.Equal(id, sut.Id);
            Assert.Equal(name, sut.Name);
            Assert.Equal(isPublic, sut.IsPublic);
            Assert.Equal(userId, sut.CreatedByUserId);
            Assert.Equal(date, sut.DateCreated);
        }

        [Fact]
        public void Add_FeedCreated_Event()
        {
            var sut = Feed.Create("test feed", false, Guid.NewGuid());
            var events = sut.DequeueEvents();
            Assert.NotEmpty(events);
            Assert.IsType<FeedCreated>(events.ToArray()[0]);
        }

        [Fact]
        public void Change_Name()
        {
            var name = "A new name";
            var sut = GetExistingFeed();
            sut.ChangeName(name);
            Assert.Equal(name, sut.Name);
        }

        [Fact]
        public void Throw_ArgumentException_Given_No_Name()
        {
            var ex = Assert.Throws<ArgumentException>(() => Feed.Create("", true, Guid.NewGuid()));
            Assert.Equal("required (Parameter 'name')", ex.Message);
        }

        [Fact]
        public void Add_FeedNameChanged_Event()
        {
            var name = "A new name";
            var sut = GetExistingFeed();
            sut.ChangeName(name);
            var events = sut.DequeueEvents();
            var @event = (FeedNameChanged)events.FirstOrDefault(e => e is FeedNameChanged);
            Assert.Equal(@event.Name, sut.Name);
        }

        [Fact]
        public void Change_IsPublic()
        {
            var sut = GetExistingFeed();
            var isPublic = !sut.IsPublic;
            sut.ChangeIsPublic(isPublic);
            Assert.Equal(isPublic, sut.IsPublic);
        }

        [Fact]
        public void Add_FeedIsPublicChanged_Event()
        {
            var sut = GetExistingFeed();
            var isPublic = !sut.IsPublic;
            sut.ChangeIsPublic(isPublic);
            var events = sut.DequeueEvents();
            var @event = (FeedIsPublicChanged)events.FirstOrDefault(e => e is FeedIsPublicChanged);
            Assert.Equal(@event.IsPublic, sut.IsPublic);
        }

        [Fact]
        public void Add_FeedDeleted_Event()
        {
            var sut = GetExistingFeed();
            sut.Delete();
            var events = sut.DequeueEvents();
            var @event = (FeedDeleted)events.FirstOrDefault(e => e is FeedDeleted);
            Assert.NotNull(@event);
            Assert.Equal(sut.Id, @event.FeedId);
        }

        private Feed GetExistingFeed()
        {
            var feed = Feed.Load(Guid.NewGuid(), "testfeed", true, Guid.NewGuid(), DateTimeOffset.Now);
            return feed;
        }
    }
}
