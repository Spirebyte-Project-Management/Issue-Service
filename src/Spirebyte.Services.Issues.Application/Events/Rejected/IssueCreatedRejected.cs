using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events.Rejected
{
    [Contract]
    public class IssueCreatedRejected : IRejectedEvent
    {
        public string ProjectId { get; }
        public string Reason { get; }
        public string Code { get; }

        public IssueCreatedRejected(string projectId, string reason, string code)
        {
            ProjectId = projectId;
            Reason = reason;
            Code = code;
        }
    }
}
