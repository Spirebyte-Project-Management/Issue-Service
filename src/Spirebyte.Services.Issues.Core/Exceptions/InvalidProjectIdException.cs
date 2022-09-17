using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Core.Exceptions;

public class InvalidProjectIdException : DomainException
{
    public InvalidProjectIdException(string projectId) : base($"Invalid projectId: {projectId}.")
    {
    }

    public string Code { get; } = "invalid_project_id";
}