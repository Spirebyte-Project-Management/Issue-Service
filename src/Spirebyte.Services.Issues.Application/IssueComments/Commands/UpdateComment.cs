using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.IssueComments.Commands;

[Message("issues", "update_comment", "issues.update_comment")]
public record UpdateComment(string Id, string Body) : ICommand;