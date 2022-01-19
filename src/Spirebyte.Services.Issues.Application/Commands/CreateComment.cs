using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.Commands;

[Contract]
public class CreateComment : ICommand
{
    public CreateComment(string id, string issueId, string projectId, Guid authorId, string body)
    {
        Id = id;
        IssueId = issueId;
        ProjectId = projectId;
        AuthorId = authorId;
        Body = body;
    }

    public string Id { get; }
    public string IssueId { get; }
    public string ProjectId { get; }

    public Guid AuthorId { get; }
    public string Body { get; }
}