using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Exceptions
{
    public class ProjectNotFoundException : AppException
    {
        public override string Code { get; } = "project_not_found";
        public string ProjectId { get; }

        public ProjectNotFoundException(string projectId) : base($"Project with ID: '{projectId}' was not found.")
        {
            ProjectId = projectId;
        }
    }
}
