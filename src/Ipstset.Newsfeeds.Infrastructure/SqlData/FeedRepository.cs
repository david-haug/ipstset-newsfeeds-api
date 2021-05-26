using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Ipstset.Newsfeeds.Application.EventHandling;
using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Domain.Feeds;
using Ipstset.Newsfeeds.Infrastructure.Models;
using Newtonsoft.Json;

namespace Ipstset.Newsfeeds.Infrastructure.SqlData
{
    public class FeedRepository: IFeedRepository
    {
        private string _connection;
        private IEventDispatcher _eventDispatcher;
        public FeedRepository(string connection, IEventDispatcher eventDispatcher)
        {
            _connection = connection;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<Feed> GetAsync(Guid id)
        {
            Feed feed = null;
            var sql = "exec get_json @table,@id";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var document = await sqlConnection.QueryFirstOrDefaultAsync<Document>(sql, new { table = "feed", id });

                if(document != null)
                {
                    //parse data
                    var data = JsonConvert.DeserializeObject<FeedDocument>(document.Data);
                    if (data != null)
                        feed = Feed.Load(Guid.Parse(data.Id),
                            data.Name,
                            data.IsPublic,
                            Guid.Parse(data.CreatedUserId),
                            data.DateCreated);
                }
            }

            return feed;
        }

        public async Task SaveAsync(Feed feed)
        {
            var sql = "exec save_json @table,@id,@data";

            var document = new FeedDocument 
            {
                Id = feed.Id.ToString(),
                Name = feed.Name,
                IsPublic = feed.IsPublic,
                CreatedUserId = feed.CreatedByUserId.ToString(),
                DateCreated = feed.DateCreated
            };

            using (var sqlConnection = new SqlConnection(_connection))
            {
                await sqlConnection.ExecuteAsync(sql, new { table = "feed", id = feed.Id.ToString(), data = JsonHelper.Serialize(document) });
            }

            await _eventDispatcher.DispatchAsync(feed.DequeueEvents().ToArray());
        }

        public async Task DeleteAsync(Feed feed)
        {
            var sql = "exec delete_json @table,@id";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                await sqlConnection.ExecuteAsync(sql, new { table = "feed", id = feed.Id.ToString() });
            }

            await _eventDispatcher.DispatchAsync(feed.DequeueEvents().ToArray());
        }
    }
}
