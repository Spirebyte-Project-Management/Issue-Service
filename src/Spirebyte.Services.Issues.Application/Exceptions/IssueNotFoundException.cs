using System;
using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Exceptions
{
    public class IssueNotFoundException : AppException
    {
        public override string Code { get; } = "issue_not_found";
        public Guid IssueId { get; }
        public string IssueKey { get; }

        public IssueNotFoundException(Guid issueId) : base($"Issue with ID: '{issueId}' was not found.")
        {
            IssueId = issueId;
        }
        public IssueNotFoundException(string issueKey) : base($"Issue with Key: '{issueKey}' was not found.")
        {
            IssueKey = issueKey;
        }
    }
}
