using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.Issues.Events.External;

[Message("sprints", "ended_sprint", "issues.ended_sprint")]
public class EndedSprint : IEvent
{
    public EndedSprint(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; }
}