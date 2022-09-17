using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.IssueComments.Events;

[Message("issues", "comment_created")]
public record CommentCreated(string CommentId, string IssueId, string ProjectId) : IEvent;