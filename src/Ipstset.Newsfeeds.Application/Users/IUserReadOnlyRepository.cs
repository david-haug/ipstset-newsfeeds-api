using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Application.Users
{
    public interface IUserReadOnlyRepository
    {
        Task<UserResponse> GetByUserNameAsync(string userName);
        Task<UserResponse> GetByIdAsync(string id);
    }
}
