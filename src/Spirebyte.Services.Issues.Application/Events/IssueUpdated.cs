using Convey.CQRS.Events;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Events;

[Contract]
public class IssueUpdated : IEvent
{
    public IssueUpdated(string issueId, int storyPoints, IssueStatus status)
    {
        IssueId = issueId;
        StoryPoints = storyPoints;
        Status = status;
    }

    public string IssueId { get; }
    public int StoryPoints { get; }

    public IssueStatus Status { get; }
}