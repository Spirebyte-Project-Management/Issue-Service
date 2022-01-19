using Spirebyte.Services.Issues.Core.Exceptions.Base;

namespace Spirebyte.Services.Issues.Core.Exceptions;

public class InvalidIssueIdException : DomainException
{
    public InvalidIssueIdException(string issueId) : base($"Invalid issueId: {issueId}.")
    {
    }

    public override string Code { get; } = "invalid_issue_id";
}