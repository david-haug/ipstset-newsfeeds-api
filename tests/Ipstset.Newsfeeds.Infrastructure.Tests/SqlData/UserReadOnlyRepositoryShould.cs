using Ipstset.Newsfeeds.Application.Users;
using Ipstset.Newsfeeds.Infrastructure.SqlData;
using Ipstset.Newsfeeds.Tests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Infrastructure.Tests.SqlData
{
    public class UserReadOnlyRepositoryShould
    {
        private string _exisingUserId = "7c685c1a-e6be-408e-bbea-7fa35f1c074b";

        [Fact]
        public async void DB_Return_UserResponse_Given_Valid_Id()
        {
            var id = _exisingUserId;
            var sut = new UserReadOnlyRepository(Config.Connections.Newsfeeds);
            var actual = await sut.GetByIdAsync(id);
            Assert.NotNull(actual);
            Assert.IsType<UserResponse>(actual);
        }

        [Fact]
        public async void DB_Return_UserResponse_Given_Valid_UserName()
        {
            var userName = "system";
            var sut = new UserReadOnlyRepository(Config.Connections.Newsfeeds);
            var actual = await sut.GetByUserNameAsync(userName);
            Assert.NotNull(actual);
            Assert.IsType<UserResponse>(actual);
            Assert.Equal(userName, actual.UserName);
        }

        [Fact]
        public async void DB_Return_Null_Given_Invalid_Id()
        {
            var id = Guid.NewGuid().ToString();
            var sut = new UserReadOnlyRepository(Config.Connections.Newsfeeds);
            var actual = await sut.GetByIdAsync(id);
            Assert.Null(actual);
        }
    }
}
