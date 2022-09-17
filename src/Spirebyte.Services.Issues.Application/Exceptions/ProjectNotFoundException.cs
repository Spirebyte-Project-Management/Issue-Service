using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Application.Exceptions;

public class ProjectNotFoundException : AppException
{
    public ProjectNotFoundException(string projectId) : base($"Project with ID: '{projectId}' was not found.")
    {
        ProjectId = projectId;
    }
    public string ProjectId { get; }
}