using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events
{
    [Contract]
    public class IssueUpdated : IEvent
    {
        public string IssueId { get; }

        public IssueUpdated(string issueId)
        {
            IssueId = issueId;
        }
    }
}
