using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Extensions;
using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Feeds.GetFeeds;
using Ipstset.Newsfeeds.Infrastructure.Models;
using Newtonsoft.Json;

namespace Ipstset.Newsfeeds.Infrastructure.SqlData
{
    public class FeedReadOnlyRepository : IFeedReadOnlyRepository
    {
        private string _connection;
        public FeedReadOnlyRepository(string connection)
        {
            _connection = connection;
        }

        public async Task<FeedResponse> GetByIdAsync(string id)
        {
            FeedResponse feedResponse = null;
            var sql = "exec get_json @table,@id";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var document = await sqlConnection.QueryFirstOrDefaultAsync<Document>(sql, new { table = "feed", id });
                feedResponse = DocumentMapper.ToFeedResponse(document);
            }

            return feedResponse;
        }

        public async Task<QueryResult<FeedResponse>> GetFeedsAsync(GetFeedsRequest request)
        {
            //query process:
            //1. get all json from starting point
            //2. filter unauthorized records
            //3. apply remaining request filters
            //4. take # of records based on limit
            var feeds = new List<FeedResponse>();
            var sql = "exec get_json_all @table,@startAfter";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var documents = await sqlConnection.QueryAsync<Document>(sql, new { table = "feed", startAfter = request.StartAfter });
                foreach (var document in documents)
                {
                    var response = DocumentMapper.ToFeedResponse(document);
                    //keep feeds user does not have access to out of results
                    if (response != null && request.User.HasAccessToFeedResponse(response))
                        feeds.Add(response);
                }
            }

            //sort
            var sorter = new Sorter<FeedResponse>();
            feeds = sorter.Sort(feeds, request.Sort?.ToArray()).ToList();

            return new QueryResult<FeedResponse> { Items = feeds.Take(request.Limit), TotalRecords = feeds.Count, Limit = request.Limit, StartAfter = request.StartAfter };
        }
    }
}
