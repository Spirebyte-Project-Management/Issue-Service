using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.IssueComments.Events;

[Message("issues", "comment_deleted")]
public record CommentDeleted(string CommentId, string IssueId) : IEvent;