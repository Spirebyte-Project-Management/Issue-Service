using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.IssueComments.Commands;

[Contract]
public record UpdateComment(string Id, string Body) : ICommand;