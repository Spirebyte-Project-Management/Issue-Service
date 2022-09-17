using System;
using System.Linq;
using Spirebyte.Services.Issues.Application.IssueComments.DTO;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers;

internal static class CommentMappers
{
    public static Comment AsEntity(this CommentDocument document)
    {
        return new Comment(document.Id, document.IssueId, document.ProjectId, document.AuthorId, document.Body,
            document.CreatedAt, document.UpdatedAt, document.Reactions);
    }

    public static CommentDocument AsDocument(this Comment entity)
    {
        return new CommentDocument
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
    }

    public static CommentDto AsDto(this CommentDocument document, Guid userId)
    {
        return new CommentDto
        {
            Id = document.Id,
            IssueId = document.IssueId,
            ProjectId = document.ProjectId,
            AuthorId = document.AuthorId,
            Body = document.Body,
            Reactions = document.Reactions ?? Enumerable.Empty<Reaction>(),
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt,
            CanEdit = document.AuthorId == userId,
            CanDelete = document.AuthorId == userId
        };
    }
}