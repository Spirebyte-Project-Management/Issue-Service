using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.IssueComments.Events;

[Contract]
public record CommentDeleted(string CommentId, string IssueId) : IEvent;