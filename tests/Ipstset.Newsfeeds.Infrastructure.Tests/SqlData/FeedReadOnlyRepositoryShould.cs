using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Infrastructure.SqlData;
using Ipstset.Newsfeeds.Tests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Infrastructure.Tests.SqlData
{
    public class FeedReadOnlyRepositoryShould
    {
        private string _exisingFeedId = "05657085-1a35-4e13-805d-bb2bb7173d84";
        
        [Fact]
        public async void DB_Return_FeedResponse_Given_Valid_Id()
        {
            var id = _exisingFeedId;
            var sut = new FeedReadOnlyRepository(Config.Connections.Newsfeeds);
            var actual = await sut.GetByIdAsync(id);
            Assert.NotNull(actual);
            Assert.IsType<FeedResponse>(actual);
        }

        [Fact]
        public async void DB_Return_Null_Given_Invalid_Id()
        {
            var id = Guid.NewGuid().ToString();
            var sut = new FeedReadOnlyRepository(Config.Connections.Newsfeeds);
            var actual = await sut.GetByIdAsync(id);
            Assert.Null(actual);
        }

        [Fact]
        public async void DB_Return_QueryResult_Given_Valid_Request()
        {

        }


    }
}
