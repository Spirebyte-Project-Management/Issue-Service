using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.Issues.Events.External;

[Message("sprints", "removed_issue_from_sprint", "issues.removed_issue_from_sprint")]
public class RemovedIssueFromSprint : IEvent
{
    public RemovedIssueFromSprint(string sprintId, string issueId)
    {
        SprintId = sprintId;
        IssueId = issueId;
    }

    public string SprintId { get; }
    public string IssueId { get; }
}