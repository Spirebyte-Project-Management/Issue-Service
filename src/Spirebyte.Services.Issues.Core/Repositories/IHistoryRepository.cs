using System;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Core.Repositories;

public interface IHistoryRepository
{
    Task<History> GetAsync(Guid historyId);
    Task AddAsync(History history);
}