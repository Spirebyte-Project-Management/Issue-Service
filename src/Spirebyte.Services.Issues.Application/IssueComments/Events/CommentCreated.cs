using Convey.CQRS.Events;

namespace Spirebyte.Services.Issues.Application.IssueComments.Events;

[Contract]
public record CommentCreated(string CommentId, string IssueId, string ProjectId) : IEvent;