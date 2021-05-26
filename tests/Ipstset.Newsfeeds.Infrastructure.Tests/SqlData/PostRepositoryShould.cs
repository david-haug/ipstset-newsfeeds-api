using Ipstset.Newsfeeds.Domain.Feeds;
using Ipstset.Newsfeeds.Domain.Posts;
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
    public class PostRepositoryShould
    {
        private string _exisingPostId = "1e2f6bc9-ca0c-48f7-a342-476efbda025b";
        private string _exisingFeedId = "4787eb74-875b-4a72-9142-b33f3dcc4015";
        [Fact]
        public async void DB_Return_Post_Given_Valid_Id()
        {
            var id = _exisingPostId;
            var sut = new PostRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            var actual = await sut.GetAsync(Guid.Parse(id));
            Assert.NotNull(actual);
            Assert.IsType<Post>(actual);
        }

        [Fact]
        public async void DB_Create_New_Post()
        {
            var feedId = _exisingFeedId;
            var feedRepo = new FeedRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            var feed = await feedRepo.GetAsync(Guid.Parse(feedId));

            var post = Post.Create(feed, $"Title {DateTime.Now}", $"Content  {DateTime.Now}", feed.CreatedByUserId, new List<string> { "tag1" });
            var sut = new PostRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            await sut.SaveAsync(post);

            var actual = await sut.GetAsync(post.Id);
            Assert.NotNull(actual);
            Assert.Equal(post.Id, post.Id);
            Assert.Equal(post.FeedId, post.FeedId);
            Assert.Equal(post.Title, actual.Title);
            Assert.Equal(post.Content, actual.Content);
            Assert.Equal(post.CreatedByUserId, actual.CreatedByUserId);
            Assert.Equal(post.DateCreated, actual.DateCreated);
            Assert.Equal(post.DatePublished, actual.DatePublished);
            Assert.Equal(post.Tags, actual.Tags);
        }

        [Fact]
        public async void DB_Update_Existing_Post()
        {
            //get exisiting
            var id = _exisingPostId;
            var sut = new PostRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            var post = await sut.GetAsync(Guid.Parse(id));

            var oldTitle = post.Title;
            post.ChangeTitle($"Test post changed title {DateTime.Now}");
            await sut.SaveAsync(post);

            var actual = await sut.GetAsync(post.Id);
            Assert.NotNull(actual);
            Assert.Equal(post.Id, actual.Id);
            Assert.Equal(post.FeedId, actual.FeedId);
            Assert.NotEqual(oldTitle, actual.Title);
            Assert.Equal(post.Content, actual.Content);
            Assert.Equal(post.CreatedByUserId, actual.CreatedByUserId);
            Assert.Equal(post.DateCreated, actual.DateCreated);
            Assert.Equal(post.DatePublished, actual.DatePublished);
            Assert.Equal(post.Tags, actual.Tags);
        }

        [Fact]
        public async void DB_Dequeue_Events_When_Saved()
        {
            var feedId = _exisingFeedId;
            var feedRepo = new FeedRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            var feed = await feedRepo.GetAsync(Guid.Parse(feedId));

            var post = Post.Create(feed, $"Title {DateTime.Now}", $"Content  {DateTime.Now}", feed.CreatedByUserId, new List<string> { "tag1" });
            var sut = new PostRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            await sut.SaveAsync(post);

            var events = post.DequeueEvents();
            Assert.Empty(events.ToArray());
        }

        [Fact]
        public async void DB_Delete_Post()
        {
            var feedId = _exisingFeedId;
            var feedRepo = new FeedRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            var feed = await feedRepo.GetAsync(Guid.Parse(feedId));

            var post = Post.Create(feed, $"Title {DateTime.Now}", $"Content  {DateTime.Now}", feed.CreatedByUserId, new List<string> { "tag1" });
            var sut = new PostRepository(Config.Connections.Newsfeeds, new EventDispatcherStub());
            await sut.SaveAsync(post);

            //delete what was created
            await sut.DeleteAsync(post);

            var saved = await sut.GetAsync(post.Id);
            Assert.Null(saved);
        }


    }
}
