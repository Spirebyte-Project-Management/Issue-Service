using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers
{
    internal static class CommentMappers
    {
        public static Comment AsEntity(this CommentDocument document)
            => new Comment(document.Id, document.IssueId, document.ProjectId, document.AuthorId, document.Body, document.CreatedAt, document.Reactions);

        public static CommentDocument AsDocument(this Comment entity)
            => new CommentDocument
            {
                Id = entity.Id,
                IssueId = entity.IssueId,
                ProjectId = entity.ProjectId,
                AuthorId = entity.AuthorId,
                Body = entity.Body,
                Reactions = entity.Reactions ?? Enumerable.Empty<Reaction>(),
                CreatedAt = entity.CreatedAt
            };

        public static CommentDto AsDto(this CommentDocument document)
            => new CommentDto
            {
                Id = document.Id,
                IssueId = document.IssueId,
                ProjectId = document.ProjectId,
                AuthorId = document.AuthorId,
                Body = document.Body,
                Reactions = document.Reactions ?? Enumerable.Empty<Reaction>(),
                CreatedAt = document.CreatedAt
            };
    }
}
