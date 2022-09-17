using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.IssueComments.Events;

[Message("issues", "comment_updated")]
public record CommentUpdated(string CommentId) : IEvent;