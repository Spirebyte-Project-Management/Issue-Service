using Spirebyte.Services.Issues.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Issues.Core.Entities
{
    public class Comment
    {
        public string Id { get; private set; }
        public string IssueId { get; private set; }
        public string ProjectId { get; private set; }

        public Guid AuthorId { get; private set; }
        public string Body { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public IEnumerable<Reaction> Reactions { get; private set; }

        public Comment(string id, string issueId, string projectId, Guid authorId, string body, DateTime createdAt, IEnumerable<Reaction> reactions)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidIdException(id);
            }

            if (string.IsNullOrWhiteSpace(projectId))
            {
                throw new InvalidProjectIdException(projectId);
            }

            if (string.IsNullOrWhiteSpace(issueId))
            {
                throw new InvalidIssueIdException(issueId);
            }

            if (authorId == Guid.Empty)
            {
                throw new InvalidAuthorIdException(authorId);
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new InvalidBodyException(body);
            }

            Id = id;
            IssueId = issueId;
            ProjectId = projectId;
            AuthorId = authorId;
            Body = body;
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
            Reactions = reactions;
        }

        public Comment(string id, string issueId, string projectId, Guid authorId, string body, DateTime createdAt,
            DateTime updatedAt, IEnumerable<Reaction> reactions) : this(id, issueId, projectId, authorId, body, createdAt, reactions)
        {
            UpdatedAt = updatedAt;
        }
    }

    public struct Reaction
    {
        public string CharacterCode { get; private set; }
        public Guid CommenterId { get; private set; }

        public Reaction(string characterCode, Guid commenterId)
        {
            CharacterCode = characterCode;
            CommenterId = commenterId;
        }
    }
}
