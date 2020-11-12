using System;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Core.Repositories
{
    public interface IIssueRepository
    {
        Task<Issue> GetAsync(Guid issueId);
        Task<Issue> GetAsync(string issueKey);
        Task<int> GetIssueCountOfProject(Guid projectId);
        Task AddAsync(Issue issue);
        Task UpdateAsync(Issue issue);
    }
}
