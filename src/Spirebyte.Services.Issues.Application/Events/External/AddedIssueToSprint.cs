using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Issues.Application.Events.External
{
    [Message("sprints")]
    public class AddedIssueToSprint : IEvent
    {
        public string SprintId { get; }
        public string IssueId { get; }

        public AddedIssueToSprint(string sprintId, string issueId)
        {
            SprintId = sprintId;
            IssueId = issueId;
        }
    }
}
