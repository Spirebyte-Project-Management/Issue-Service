using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.Issues.Events.External;

[Message("sprints", "added_issue_to_sprint", "issues.added_issue_to_sprint")]
public class AddedIssueToSprint : IEvent
{
    public AddedIssueToSprint(string sprintId, string issueId)
    {
        SprintId = sprintId;
        IssueId = issueId;
    }

    public string SprintId { get; }
    public string IssueId { get; }
}