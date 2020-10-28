using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events
{
    [Contract]
    public class IssueCreated : IEvent
    {
        public Guid ProjectId { get; }

        public IssueCreated(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}
