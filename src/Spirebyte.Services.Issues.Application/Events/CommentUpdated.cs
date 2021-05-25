using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.Events
{
    [Contract]
    public class CommentUpdated : IEvent
    {
        public string CommentId { get; }

        public CommentUpdated(string commentId)
        {
            CommentId = commentId;
        }
    }
}
