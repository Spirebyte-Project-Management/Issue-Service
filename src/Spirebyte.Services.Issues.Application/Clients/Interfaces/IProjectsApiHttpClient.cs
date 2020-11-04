using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Application.Clients.Interfaces
{
    public interface IProjectsApiHttpClient
    {
        Task<bool> IsProjectUserAsync(string key, Guid userId);
    }
}
