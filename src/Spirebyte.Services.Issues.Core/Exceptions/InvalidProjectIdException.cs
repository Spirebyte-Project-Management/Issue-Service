using Spirebyte.Services.Issues.Core.Exceptions.Base;

namespace Spirebyte.Services.Issues.Core.Exceptions
{
    public class InvalidProjectIdException : DomainException
    {
        public override string Code { get; } = "invalid_project_id";

        public InvalidProjectIdException(string projectId) : base($"Invalid projectId: {projectId}.")
        {
        }
    }
}