using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events;

[Contract]
public class CommentUpdated : IEvent
{
    public CommentUpdated(string commentId)
    {
        CommentId = commentId;
    }

    public string CommentId { get; }
}