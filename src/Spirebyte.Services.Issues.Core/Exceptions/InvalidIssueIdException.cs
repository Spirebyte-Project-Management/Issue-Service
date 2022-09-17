using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Core.Exceptions;

public class InvalidIssueIdException : DomainException
{
    public InvalidIssueIdException(string issueId) : base($"Invalid issueId: {issueId}.")
    {
    }

    public string Code { get; } = "invalid_issue_id";
}