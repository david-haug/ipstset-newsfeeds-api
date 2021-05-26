using Dapper;
using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.GetPosts;
using Ipstset.Newsfeeds.Infrastructure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Ipstset.Newsfeeds.Application.Extensions;
using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.Posts.GetPublishedPostsByFeed;
using System.Linq;

namespace Ipstset.Newsfeeds.Infrastructure.SqlData
{
    public class PostReadOnlyRepository : IPostReadOnlyRepository
    {
        private string _connection;
        public PostReadOnlyRepository(string connection)
        {
            _connection = connection;
        }

        public async Task<PostResponse> GetByIdAsync(string id)
        {
            PostResponse postResponse = null;
            var sql = "exec get_json @table,@id";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var document = await sqlConnection.QueryFirstOrDefaultAsync<Document>(sql, new { table = "post", id });
                postResponse = DocumentMapper.ToPostResponse(document);
            }

            return postResponse;
        }

        public async Task<PublishedPostsByFeedResponse> GetPublishedPostsByFeedAsync(GetPublishedPostsByFeedRequest request)
        {
            var published = new PublishedPostsByFeedResponse();
            //get feed first
            var feed = await GetFeedByIdAsync(request.FeedId);
            if (feed == null)
                return null;

            published.FeedId = feed.Id;
            published.FeedName = feed.Name;
            published.IsPublic = feed.IsPublic;
            published.CreatedUserId = feed.CreatedUserId;

            var posts = new List<PostResponse>();
            var sql = "exec get_json_all @table, @startAfter";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var documents = await sqlConnection.QueryAsync<Document>(sql, new { table = "post", startAfter = request.StartAfter });
                foreach (var document in documents)
                {
                    var response = DocumentMapper.ToPostResponse(document);
                    if (response != null)
                        posts.Add(response);
                }
            }

            //filter by feed id and published
            posts = posts.Where(p => p.FeedId == feed.Id && p.IsPublished).ToList();

            //tags
            if(request.Tags != null)
                posts = posts.Where(p => p.Tags.Intersect(request.Tags).Count() == request.Tags.Count()).ToList();

            //sort
            var sorter = new Sorter<PostResponse>();
            posts = sorter.Sort(posts,request.Sort?.ToArray()).ToList();

            published.Posts = new QueryResult<PostResponse> { Items = posts.Take(request.Limit), TotalRecords = posts.Count, Limit = request.Limit, StartAfter = request.StartAfter };
            return published;
        }

        public async Task<QueryResult<PostResponse>> GetPostsAsync(GetPostsRequest request)
        {
            var posts = new List<PostResponse>();
            var sql = "exec get_json_all @table,@startAfter";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var documents = await sqlConnection.QueryAsync<Document>(sql, new { table = "post", startAfter = request.StartAfter });
                foreach (var document in documents)
                {
                    var response = DocumentMapper.ToPostResponse(document);
                    //keep posts user does not have access to out of results
                    if (response != null && request.User.HasAccessToPostResponse(response))
                        posts.Add(response);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.FeedId))
                posts = posts.Where(p => p.FeedId == request.FeedId).ToList();

            if (request.Published.HasValue)
                posts = posts.Where(p => p.IsPublished == request.Published.Value).ToList();

            //tags
            //"or"
            //if (request.Tags != null)
            //    posts = posts.Where(p => p.Tags.Intersect(request.Tags).Any()).ToList();
            //"and"
            if (request.Tags != null)
                posts = posts.Where(p => p.Tags.Intersect(request.Tags).Count() == request.Tags.Count()).ToList();

            //sort
            var sorter = new Sorter<PostResponse>();
            posts = sorter.Sort(posts, request.Sort?.ToArray()).ToList();

            return new QueryResult<PostResponse> { Items = posts.Take(request.Limit), TotalRecords = posts.Count, Limit = request.Limit, StartAfter = request.StartAfter };
        }

        private async Task<FeedResponse> GetFeedByIdAsync(string id)
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

        
    }
}
