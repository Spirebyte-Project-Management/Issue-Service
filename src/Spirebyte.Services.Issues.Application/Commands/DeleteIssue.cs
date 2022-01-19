using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.Commands;

[Contract]
public class DeleteIssue : ICommand
{
    public DeleteIssue(string issueId)
    {
        IssueId = issueId;
    }

    public string IssueId { get; set; }
}