using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Issues.Application.Events.External
{
    [Message("sprints")]
    public class EndedSprint : IEvent
    {
        public string SprintId { get; }

        public EndedSprint(string sprintId)
        {
            SprintId = sprintId;
        }
    }
}
