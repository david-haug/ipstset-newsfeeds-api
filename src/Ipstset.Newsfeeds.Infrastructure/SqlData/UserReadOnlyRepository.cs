using Dapper;
using Ipstset.Newsfeeds.Application.Users;
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
    public class UserReadOnlyRepository : IUserReadOnlyRepository
    {
        private string _connection;
        public UserReadOnlyRepository(string connection)
        {
            _connection = connection;
        }

        public async Task<UserResponse> GetByIdAsync(string userId)
        {
            UserResponse response = null;
            var sql = "exec get_json @table,@id";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var document = await sqlConnection.QueryFirstOrDefaultAsync<Document>(sql, new { table = "user", id = userId });

                if (document != null)
                {
                    //parse data
                    var data = JsonConvert.DeserializeObject<UserDocument>(document.Data);
                    if (data != null)
                        response = new UserResponse
                        {
                            Id = data.Id,
                            FirstName = data.FirstName,
                            LastName = data.LastName,
                            UserName = data.UserName,
                            Email = data.Email,
                            DateCreated = data.DateCreated,
                            Roles = data.Roles,
                            Password = data.Password,
                            Salt = data.Salt
                        };
                }
            }

            return response;
        }

        public async Task<UserResponse> GetByUserNameAsync(string userName)
        {
            var users = new List<UserResponse>();
            var sql = "exec get_json_all @table";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var documents = await sqlConnection.QueryAsync<Document>(sql, new { table = "user" });
                foreach(var document in documents)
                {
                    var data = JsonConvert.DeserializeObject<UserDocument>(document.Data);
                    if (data != null)
                        users.Add(new UserResponse
                        {
                            Id = data.Id,
                            FirstName = data.FirstName,
                            LastName = data.LastName,
                            UserName = data.UserName,
                            Email = data.Email,
                            DateCreated = data.DateCreated,
                            Roles = data.Roles,
                            Password = data.Password,
                            Salt = data.Salt
                        });
                }
            }

            return users.FirstOrDefault(u=>u.UserName == userName);
        }
    }
}
