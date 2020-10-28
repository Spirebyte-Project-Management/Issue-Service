using System;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<string> GetKeyAsync(Guid projectId);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Project project);
    }
}
