using System;
using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Exceptions
{
    public class IssueNotFoundException : AppException
    {
        public override string Code { get; } = "project_not_found";
        public Guid IssueId { get; }

        public IssueNotFoundException(Guid issueId) : base($"Issue with ID: '{issueId}' was not found.")
        {
            IssueId = issueId;
        }
    }
}
