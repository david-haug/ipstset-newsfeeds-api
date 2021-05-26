using Ipstset.Newsfeeds.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ipstset.Newsfeeds.Api.Tests
{
    public class SortItemHelperShould
    {
        [Fact]
        public void CreateSortItems()
        {
            var items = new[] {"test", "-me"};

            var sortItems = SortItemHelper.CreateSortItems(items);
            Assert.NotEmpty(sortItems);
            Assert.Equal(sortItems.ToList()[0].Name, items[0] );
            Assert.False(sortItems.ToList()[0].IsDescending);
            Assert.Equal("-" + sortItems.ToList()[1].Name, items[1]);
            Assert.True(sortItems.ToList()[1].IsDescending);
        }
    }
}
