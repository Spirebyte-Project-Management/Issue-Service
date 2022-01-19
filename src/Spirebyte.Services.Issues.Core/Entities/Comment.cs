using System;
using System.Collections.Generic;
using Spirebyte.Services.Issues.Core.Exceptions;

namespace Spirebyte.Services.Issues.Core.Entities;

public class Comment
{
    public Comment(string id, string issueId, string projectId, Guid authorId, string body, DateTime createdAt,
        IEnumerable<Reaction> reactions)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new InvalidIdException(id);

        if (string.IsNullOrWhiteSpace(projectId)) throw new InvalidProjectIdException(projectId);

        if (string.IsNullOrWhiteSpace(issueId)) throw new InvalidIssueIdException(issueId);

        if (authorId == Guid.Empty) throw new InvalidAuthorIdException(authorId);

        if (string.IsNullOrWhiteSpace(body)) throw new InvalidBodyException(body);

        Id = id;
        IssueId = issueId;
        ProjectId = projectId;
        AuthorId = authorId;
        Body = body;
        CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
        Reactions = reactions;
    }

    public Comment(string id, string issueId, string projectId, Guid authorId, string body, DateTime createdAt,
        DateTime updatedAt, IEnumerable<Reaction> reactions) : this(id, issueId, projectId, authorId, body, createdAt,
        reactions)
    {
        UpdatedAt = updatedAt;
    }

    public string Id { get; }
    public string IssueId { get; }
    public string ProjectId { get; }

    public Guid AuthorId { get; }
    public string Body { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }

    public IEnumerable<Reaction> Reactions { get; }
}

public struct Reaction
{
    public string CharacterCode { get; }
    public Guid CommenterId { get; }

    public Reaction(string characterCode, Guid commenterId)
    {
        CharacterCode = characterCode;
        CommenterId = commenterId;
    }
}