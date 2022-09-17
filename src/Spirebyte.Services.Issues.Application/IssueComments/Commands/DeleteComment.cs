using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.IssueComments.Commands;

[Message("issues", "delete_comment", "issues.delete_comment")]
public record DeleteComment(string Id) : ICommand;