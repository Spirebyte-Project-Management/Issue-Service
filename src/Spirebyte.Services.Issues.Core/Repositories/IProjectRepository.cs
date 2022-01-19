using System.Threading.Tasks;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Core.Repositories;

public interface IProjectRepository
{
    Task AddAsync(Project project);
    Task<bool> ExistsAsync(string projectId);
}