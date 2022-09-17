using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Issues.Application.Issues.Exceptions;

public class IssueNotFoundException : AppException
{
    public IssueNotFoundException(string issueId) : base($"Issue with ID: '{issueId}' was not found.")
    {
        IssueId = issueId;
    }
    public string IssueId { get; }
}