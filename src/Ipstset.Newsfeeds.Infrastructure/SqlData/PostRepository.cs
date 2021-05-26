using Dapper;
using Ipstset.Newsfeeds.Application.EventHandling;
using Ipstset.Newsfeeds.Domain.Posts;
using Ipstset.Newsfeeds.Infrastructure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Infrastructure.SqlData
{
    public class PostRepository : IPostRepository
    {
        private string _connection;
        private IEventDispatcher _eventDispatcher;
        public PostRepository(string connection, IEventDispatcher eventDispatcher)
        {
            _connection = connection;
            _eventDispatcher = eventDispatcher;
        }

        public async Task DeleteAsync(Post post)
        {
            var sql = "exec delete_json @table,@id";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                await sqlConnection.ExecuteAsync(sql, new { table = "post", id = post.Id.ToString() });
            }

            await _eventDispatcher.DispatchAsync(post.DequeueEvents().ToArray());
        }

        public async Task<IEnumerable<Post>> GetAllByFeedIdAsync(Guid feedId)
        {
            var posts = new List<Post>();
            var sql = "exec get_json_all @table";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var documents = await sqlConnection.QueryAsync<Document>(sql, new { table = "post" });
                foreach (var document in documents)
                {
                    if (document != null)
                    {
                        //parse data
                        var data = JsonConvert.DeserializeObject<PostDocument>(document.Data);
                        if (data != null)
                        {
                            var post = Post.Load(Guid.Parse(data.Id),
                                Guid.Parse(data.FeedId),
                                data.Title,
                                data.Content,
                                Guid.Parse(data.CreatedByUserId),
                                data.DateCreated,
                                data.DatePublished,
                                data.Tags);

                            posts.Add(post);
                        }
                    }
                }
            }

            return posts.Where(p=>p.FeedId == feedId);
        }

        public async Task<Post> GetAsync(Guid id)
        {
            Post post = null;
            var sql = "exec get_json @table,@id";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var document = await sqlConnection.QueryFirstOrDefaultAsync<Document>(sql, new { table = "post", id });

                if (document != null)
                {
                    //parse data
                    var data = JsonConvert.DeserializeObject<PostDocument>(document.Data);
                    if (data != null)
                        post = Post.Load(Guid.Parse(data.Id),
                            Guid.Parse(data.FeedId),
                            data.Title,
                            data.Content,
                            Guid.Parse(data.CreatedByUserId),
                            data.DateCreated,
                            data.DatePublished,
                            data.Tags);
                }
            }

            return post;
        }

        public async Task SaveAsync(Post post)
        {
            var sql = "exec save_json @table,@id,@data";

            var document = new PostDocument
            {
                Id = post.Id.ToString(),
                FeedId = post.FeedId.ToString(),
                Title = post.Title,
                Content = post.Content,
                CreatedByUserId = post.CreatedByUserId.ToString(),
                DateCreated = post.DateCreated,
                DatePublished = post.DatePublished,
                IsPublished = post.IsPublished,
                Tags = post.Tags
            };

            using (var sqlConnection = new SqlConnection(_connection))
            {
                await sqlConnection.ExecuteAsync(sql, new { table = "post", id = post.Id.ToString(), data = JsonHelper.Serialize(document) });
            }

            await _eventDispatcher.DispatchAsync(post.DequeueEvents().ToArray());
        }
    }
}
