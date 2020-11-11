using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Issues.Application.Events.External
{

    [Message("projects")]
    public class ProjectCreated : IEvent
    {
        public Guid ProjectId { get; }
        public string Key { get; }

        public ProjectCreated(Guid projectId, string key)
        {
            ProjectId = projectId;
            Key = key;
        }
    }
}
