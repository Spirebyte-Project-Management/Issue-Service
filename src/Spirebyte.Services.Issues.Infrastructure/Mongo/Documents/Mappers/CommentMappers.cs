using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Core.Entities;
using System.Linq;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers
{
    internal static class CommentMappers
    {
        public static Comment AsEntity(this CommentDocument document)
            => new Comment(document.Id, document.IssueId, document.ProjectId, document.AuthorId, document.Body, document.CreatedAt, document.UpdatedAt, document.Reactions);

        public static CommentDocument AsDocument(this Comment entity)
            => new CommentDocument
            {
                Id = entity.Id,
                IssueId = entity.IssueId,
                ProjectId = entity.ProjectId,
                AuthorId = entity.AuthorId,
                Body = entity.Body,
                Reactions = entity.Reactions ?? Enumerable.Empty<Reaction>(),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

        public static CommentDto AsDto(this CommentDocument document, IIdentityContext identityContext)
            => new CommentDto
            {
                Id = document.Id,
                IssueId = document.IssueId,
                ProjectId = document.ProjectId,
                AuthorId = document.AuthorId,
                Body = document.Body,
                Reactions = document.Reactions ?? Enumerable.Empty<Reaction>(),
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt,
                CanEdit = document.AuthorId == identityContext.Id,
                CanDelete = document.AuthorId == identityContext.Id
            };
    }
}
