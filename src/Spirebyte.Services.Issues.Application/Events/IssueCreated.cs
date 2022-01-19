using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events;

[Contract]
public class IssueCreated : IEvent
{
    public IssueCreated(string issueId, string projectId, int storyPoints)
    {
        IssueId = issueId;
        ProjectId = projectId;
        StoryPoints = storyPoints;
    }

    public string IssueId { get; }
    public string ProjectId { get; }
    public int StoryPoints { get; }
}