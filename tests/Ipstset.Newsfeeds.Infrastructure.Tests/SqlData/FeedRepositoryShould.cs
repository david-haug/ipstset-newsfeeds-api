using Ipstset.Newsfeeds.Domain.Feeds;
using Ipstset.Newsfeeds.Infrastructure.SqlData;
using Ipstset.Newsfeeds.Tests.Common;
using Ipstset.Newsfeeds.Tests.Common.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Infrastructure.Tests.SqlData
{
    public class FeedRepositoryShould
    {
        private string _exisingFeedId = "05657085-1a35-4e13-805d-bb2bb7173d84";
        [Fact]
        public async void DB_Return_Feed_Given_Valid_Id()
        {
            var id = _exisingFeedId;
            var sut = new FeedRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            var feed = await sut.GetAsync(Guid.Parse(id));
            Assert.NotNull(feed);
        }

        [Fact]
        public async void DB_Create_New_Feed()
        {
            var feed = Feed.Create($"Test feed {DateTime.Now}", true, Guid.NewGuid());
            var sut = new FeedRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            await sut.SaveAsync(feed);

            var actual = await sut.GetAsync(feed.Id);
            Assert.NotNull(actual);
            Assert.Equal(feed.Id, actual.Id);
            Assert.Equal(feed.Name, actual.Name);
            Assert.Equal(feed.IsPublic, actual.IsPublic);
            Assert.Equal(feed.CreatedByUserId, actual.CreatedByUserId);
            Assert.Equal(feed.DateCreated, actual.DateCreated);
        }

        [Fact]
        public async void DB_Update_Existing_Feed()
        {
            //get exisiting
            var id = _exisingFeedId;
            var sut = new FeedRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            var feed = await sut.GetAsync(Guid.Parse(id));

            var oldName = feed.Name;
            feed.ChangeName($"Test feed changed name {DateTime.Now}");
            await sut.SaveAsync(feed);

            var actual = await sut.GetAsync(feed.Id);
            Assert.NotNull(actual);
            Assert.Equal(feed.Id, actual.Id);
            Assert.Equal(feed.Name, actual.Name);
            Assert.NotEqual(oldName, actual.Name);
            Assert.Equal(feed.IsPublic, actual.IsPublic);
            Assert.Equal(feed.CreatedByUserId, actual.CreatedByUserId);
            Assert.Equal(feed.DateCreated, actual.DateCreated);
        }
        
        [Fact]
        public async void DB_Dequeue_Events_When_Saved()
        {
            var feed = Feed.Create($"Test feed {DateTime.Now}", true, Guid.NewGuid());
            var sut = new FeedRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            await sut.SaveAsync(feed);

            var events = feed.DequeueEvents();
            Assert.Empty(events.ToArray());
        }

        [Fact]
        public async void DB_Delete_Feed()
        {
            //create feed
            var feed = Feed.Create($"Test feed {DateTime.Now}", true, Guid.NewGuid());
            var sut = new FeedRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            await sut.SaveAsync(feed);

            //delete what was created
            await sut.DeleteAsync(feed);

            var saved = await sut.GetAsync(feed.Id);
            Assert.Null(saved);
        }

    }
}
