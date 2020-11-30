using Spirebyte.Services.Issues.Application.Exceptions.Base;
using System;

namespace Spirebyte.Services.Issues.Application.Exceptions
{
    public class ProjectNotFoundException : AppException
    {
        public override string Code { get; } = "project_not_found";
        public Guid ProjectId { get; }

        public ProjectNotFoundException(Guid projectId) : base($"Project with ID: '{projectId}' was not found.")
        {
            ProjectId = projectId;
        }
    }
}
