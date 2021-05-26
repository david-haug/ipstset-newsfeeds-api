using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Domain.Posts
{
    public interface IPostRepository
    {
        Task<Post> GetAsync(Guid id);
        Task<IEnumerable<Post>> GetAllByFeedIdAsync(Guid feedId);
        Task SaveAsync(Post post);
        Task DeleteAsync(Post post);
    }
}
