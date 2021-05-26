using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Infrastructure.SqlData;
using Xunit;
namespace Ipstset.Newsfeeds.Infrastructure.Tests.SqlData
{
    public class SorterShould
    {
        [Fact]
        public void Sort()
        {
            var items = _posts;
            var sut = new Sorter<PostResponse>();

            var sortItems = new List<SortItem> { new SortItem { Name = "DatePublished", IsDescending = false } };
            var sorted = sut.Sort(items, sortItems.ToArray()).ToList();
            Assert.NotEmpty(sorted);
            Assert.Equal(items.Count, sorted.Count);
            Assert.True(sorted[0].Id == "3");
        }

        [Fact]
        public void Sort_Multiple()
        {
            var items = _posts;
            var sut = new Sorter<PostResponse>();

            var sortItems = new List<SortItem> {
                new SortItem { Name = "Title", IsDescending = false },
                new SortItem { Name = "DateCreated", IsDescending = true }
            };

            var sorted = sut.Sort(items, sortItems.ToArray()).ToList();
            Assert.NotEmpty(sorted);
            Assert.Equal(items.Count, sorted.Count);

            Assert.Equal("2",sorted[0].Id);
            Assert.Equal("1", sorted[1].Id);
            Assert.Equal("5", sorted[2].Id);
            Assert.Equal("4", sorted[3].Id);

            Assert.Equal("8", sorted[7].Id);
        }

        [Fact]
        public void Sort_Multiple_2()
        {
            var items = _posts;
            var sut = new Sorter<PostResponse>();

            var sortItems = new List<SortItem> {
                new SortItem { Name = "datepublished", IsDescending = true },
                new SortItem { Name = "dateCreated", IsDescending = false },
                new SortItem { Name = "Title", IsDescending = false },
            };

            var sorted = sut.Sort(items, sortItems.ToArray()).ToList();
            Assert.NotEmpty(sorted);
            Assert.Equal(items.Count, sorted.Count);

            Assert.Equal("8", sorted[0].Id);
            Assert.Equal("7", sorted[1].Id);
            Assert.Equal("6", sorted[2].Id);
            Assert.Equal("5", sorted[3].Id);

            Assert.Equal("1", sorted[4].Id);
            Assert.Equal("2", sorted[5].Id);
            Assert.Equal("4", sorted[6].Id);
            Assert.Equal("3", sorted[7].Id);

        }

        List<PostResponse> _posts = new List<PostResponse>
            {
                new PostResponse {
                    Id = "1",
                    DatePublished = new DateTimeOffset(new DateTime(2020,1,8)),
                    DateCreated = new DateTimeOffset(new DateTime(2020,1,1)),
                    Title = "Abc Title"
                },
                new PostResponse {
                    Id = "2",
                    DatePublished = new DateTimeOffset(new DateTime(2020,1,8)),
                    DateCreated = new DateTimeOffset(new DateTime(2020,1,2)),
                    Title = "Abc Title"
                },
                new PostResponse {
                    Id = "3",
                    DatePublished = new DateTimeOffset(new DateTime(2020,1,3)),
                    DateCreated = new DateTimeOffset(new DateTime(2020,1,3)),
                    Title = "BacTitle"
                },
                new PostResponse {
                    Id = "4",
                    DatePublished = new DateTimeOffset(new DateTime(2020,1,8)),
                    DateCreated = new DateTimeOffset(new DateTime(2020,1,4)),
                    Title = "Bac Title"
                },
                new PostResponse {
                    Id = "5",
                    DatePublished = new DateTimeOffset(new DateTime(2020,1,10)),
                    DateCreated = new DateTimeOffset(new DateTime(2020,1,5)),
                    Title = "Bac Title"
                },
                new PostResponse {
                    Id = "6",
                    DatePublished = new DateTimeOffset(new DateTime(2020,1,11)),
                    DateCreated = new DateTimeOffset(new DateTime(2020,1,6)),
                    Title = "C Title"
                },
                new PostResponse {
                    Id = "7",
                    DatePublished = new DateTimeOffset(new DateTime(2020,1,12)),
                    DateCreated = new DateTimeOffset(new DateTime(2020,1,7)),
                    Title = "D Title"
                },
                new PostResponse {
                    Id = "8",
                    DatePublished = new DateTimeOffset(new DateTime(2020,1,13)),
                    DateCreated = new DateTimeOffset(new DateTime(2020,1,7)),
                    Title = "E Title"
                }
            };

    }
}
