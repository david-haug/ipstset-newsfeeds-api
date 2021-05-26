using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Domain.Feeds
{
    public interface IFeedRepository
    {
        Task<Feed> GetAsync(Guid id);
        Task SaveAsync(Feed feed);
        Task DeleteAsync(Feed feed);
    }
}
