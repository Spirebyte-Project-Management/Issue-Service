using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Issues.Application.Events.External;

[Message("projects")]
public class ProjectCreated : IEvent
{
    public ProjectCreated(string projectId)
    {
        ProjectId = projectId;
    }

    public string ProjectId { get; }
}