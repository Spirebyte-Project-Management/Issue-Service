using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events
{
    [Contract]
    public class IssueCreated : IEvent
    {
        public string IssueId { get; }
        public string ProjectId { get; }

        public IssueCreated(string issueId, string projectId)
        {
            IssueId = issueId;
            ProjectId = projectId;
        }
    }
}
