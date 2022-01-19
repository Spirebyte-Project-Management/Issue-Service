using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events;

[Contract]
public class IssueDeleted : IEvent
{
    public IssueDeleted(string issueId, string projectId)
    {
        IssueId = issueId;
        ProjectId = projectId;
    }

    public string IssueId { get; }
    public string ProjectId { get; set; }
}