using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Issues.Application.Events.Rejected
{
    [Contract]
    public class IssueCreatedRejected : IRejectedEvent
    {
        public Guid ProjectId { get; }
        public string Reason { get; }
        public string Code { get; }

        public IssueCreatedRejected(Guid projectId, string reason, string code)
        {
            ProjectId = projectId;
            Reason = reason;
            Code = code;
        }
    }
}
