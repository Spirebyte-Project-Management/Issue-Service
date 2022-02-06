using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.IssueComments.Commands;

[Contract]
public record CreateComment(string Id, string IssueId, string ProjectId, Guid AuthorId, string Body) : ICommand
{
    public Guid ReferenceId = Guid.NewGuid();
}