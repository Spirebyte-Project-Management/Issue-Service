using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.IssueComments.Commands;

[Contract]
public record DeleteComment(string Id) : ICommand;