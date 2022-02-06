using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.Issues.Commands;

[Contract]
public record DeleteIssue(string Id) : ICommand;