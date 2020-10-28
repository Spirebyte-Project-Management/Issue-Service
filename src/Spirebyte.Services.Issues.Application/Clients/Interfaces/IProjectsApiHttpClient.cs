using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Application.Clients.Interfaces
{
    public interface IProjectsApiHttpClient
    {
        Task<bool> IsProjectUserAsync(Guid projectId, Guid userId);
    }
}
