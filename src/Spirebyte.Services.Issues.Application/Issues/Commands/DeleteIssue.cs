using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.Issues.Commands;

[Message("issues", "delete_issue", "issues.delete_issue")]
public record DeleteIssue(string Id) : ICommand;