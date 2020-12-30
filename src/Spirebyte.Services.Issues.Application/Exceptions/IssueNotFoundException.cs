using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Exceptions
{
    public class IssueNotFoundException : AppException
    {
        public override string Code { get; } = "issue_not_found";
        public string IssueId { get; }

        public IssueNotFoundException(string issueId) : base($"Issue with ID: '{issueId}' was not found.")
        {
            IssueId = issueId;
        }
    }
}
