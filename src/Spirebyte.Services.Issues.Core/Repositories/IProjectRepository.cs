using Spirebyte.Services.Issues.Core.Entities;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Core.Repositories
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);
        Task<bool> ExistsAsync(string projectId);
    }
}
