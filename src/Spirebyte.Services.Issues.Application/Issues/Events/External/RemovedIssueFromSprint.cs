using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Issues.Application.Issues.Events.External;

[Message("sprints")]
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