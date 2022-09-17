using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.Issues.Events.External;

[Message("sprints", "sprint_deleted", "issues.sprint_deleted")]
public class SprintDeleted : IEvent
{
    public SprintDeleted(string sprintId)
    {
        SprintId = sprintId;
    }

    public string SprintId { get; }
}