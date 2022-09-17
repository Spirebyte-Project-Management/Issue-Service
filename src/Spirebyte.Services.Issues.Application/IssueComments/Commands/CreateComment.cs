using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Issues.Application.IssueComments.Commands;

[Message("issues", "create_comment", "issues.create_comment")]
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
    public Guid ReferenceId = Guid.NewGuid();

    public string Id { get; set; }
    public string IssueId { get; set; }
    public string ProjectId { get; set; }
    public Guid AuthorId { get; set; }
    public string Body { get; set; }
}