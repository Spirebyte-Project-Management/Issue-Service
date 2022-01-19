using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Issues.Application.Events.External;

[Message("sprints")]
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