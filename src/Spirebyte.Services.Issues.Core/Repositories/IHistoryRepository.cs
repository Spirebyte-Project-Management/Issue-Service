using Spirebyte.Services.Issues.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Core.Repositories
{
    public interface IHistoryRepository
    {
        Task<History> GetAsync(Guid historyId);
        Task AddAsync(History history);
    }
}
