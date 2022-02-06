using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.IssueComments.Events;

[Contract]
public record CommentUpdated(string CommentId) : IEvent;