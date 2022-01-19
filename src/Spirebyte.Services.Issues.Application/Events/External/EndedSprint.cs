using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Issues.Application.Events.External;

[Message("sprints")]
public class EndedSprint : IEvent
{
    public EndedSprint(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; }
}