using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Application.DTO
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string IssueId { get; set; }
        public string ProjectId { get; set; }

        public Guid AuthorId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<Reaction> Reactions { get; set; }

        public CommentDto()
        {
            
        }

        public CommentDto(string id, string issueId, string projectId, Guid authorId, string body, DateTime createdAt, IEnumerable<Reaction> reactions)
        {
            Id = id;
            IssueId = issueId;
            ProjectId = projectId;
            AuthorId = authorId;
            Body = body;
            CreatedAt = createdAt;
            Reactions = reactions;
        }
    }
}
