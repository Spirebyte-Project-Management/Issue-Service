using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Issues.Application.Issues.Events.External;

[Message("sprints")]
public class SprintDeleted : IEvent
{
    public SprintDeleted(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; }
}