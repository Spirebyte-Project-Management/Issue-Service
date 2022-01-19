using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Application.Clients.Interfaces;

public interface IProjectsApiHttpClient
{
    Task<bool> HasPermission(string permissionKey, Guid userId, string projectId);
}