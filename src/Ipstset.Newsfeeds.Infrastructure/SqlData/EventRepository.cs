using Dapper;
using Ipstset.Newsfeeds.Application;
using Ipstset.Newsfeeds.Application.EventHandling;
using Ipstset.Newsfeeds.Application.Events;
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
    public class EventRepository : IEventRepository
    {
        private string _connection;
        public EventRepository(string connection)
        {
            _connection = connection;
        }

        public async Task<QueryResult<EventModel>> GetEventsAsync(GetEventsRequest request)
        {
            var events = new List<EventModel>();
            var sql = "exec get_json_all @table,@startAfter";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                var documents = await sqlConnection.QueryAsync<Document>(sql, new { table = "event", startAfter = request.StartAfter });
                foreach(var document in documents)
                {
                    var @event = JsonConvert.DeserializeObject<EventModel>(document.Data);
                    events.Add(@event);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
                events = events.Where(r => r.Name == request.Name).ToList();
            if (request.StartDate.HasValue)
                events = events.Where(r => r.DateOccurred >= request.StartDate.Value).ToList();
            if (request.EndDate.HasValue)
                events = events.Where(r => r.DateOccurred <= request.EndDate.Value).ToList();

            var sorter = new Sorter<EventModel>();
            events = sorter.Sort(events, request.Sort?.ToArray()).ToList();

            return new QueryResult<EventModel> { Items = events.Take(request.Limit), TotalRecords = events.Count, Limit = request.Limit, StartAfter = request.StartAfter };
        }

        public async Task SaveAsync(EventModel @event)
        {
            var sql = "exec save_json @table,@id,@data";
            using (var sqlConnection = new SqlConnection(_connection))
            {
                await sqlConnection.ExecuteAsync(sql, new { table = "event", id = @event.Id, data = JsonHelper.Serialize(@event) });
            }

        }
    }
}
