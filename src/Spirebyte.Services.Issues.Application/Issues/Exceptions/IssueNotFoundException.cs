using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Issues.Exceptions;

public class IssueNotFoundException : AppException
{
    public IssueNotFoundException(string issueId) : base($"Issue with ID: '{issueId}' was not found.")
    {
        IssueId = issueId;
    }

    public override string Code { get; } = "issue_not_found";
    public string IssueId { get; }
}