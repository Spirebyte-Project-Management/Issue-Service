using Spirebyte.Services.Issues.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Core.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(User user);
    }
}
