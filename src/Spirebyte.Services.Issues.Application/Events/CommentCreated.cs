using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events;

[Contract]
public class CommentCreated : IEvent
{
    public CommentCreated(string commentId, string issueId, string projectId)
    {
        CommentId = commentId;
        IssueId = issueId;
        ProjectId = projectId;
    }

    public string CommentId { get; }
    public string IssueId { get; }
    public string ProjectId { get; }
}