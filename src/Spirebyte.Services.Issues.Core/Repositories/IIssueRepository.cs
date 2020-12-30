using Spirebyte.Services.Issues.Core.Entities;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Core.Repositories
{
    public interface IIssueRepository
    {
        Task<Issue> GetAsync(string issueId);
        Task<int> GetIssueCountOfProject(string projectId);
        Task<Issue> GetLatest();
        Task AddAsync(Issue issue);
        Task<bool> ExistsAsync(string issueId);
        Task UpdateAsync(Issue issue);
        Task DeleteAsync(string issueId);
    }
}
