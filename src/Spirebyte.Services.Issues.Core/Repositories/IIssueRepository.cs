using Spirebyte.Services.Issues.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Core.Repositories
{
    public interface IIssueRepository
    {
        Task<Issue> GetAsync(Guid issueId);
        Task<Issue> GetAsync(string issueKey);
        Task<int> GetIssueCountOfProject(Guid projectId);
        Task AddAsync(Issue issue);
        Task<bool> ExistsAsync(Guid issueId);
        Task UpdateAsync(Issue issue);
        Task DeleteAsync(Guid issueId);
    }
}
