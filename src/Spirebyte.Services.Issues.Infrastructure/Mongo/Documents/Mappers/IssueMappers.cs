using System;
using System.Linq;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers
{
    internal static class IssueMappers
    {
        public static Issue AsEntity(this IssueDocument document)
            => new Issue(document.Id, document.Key, document.Type, document.Status, document.Title, document.Description, document.StoryPoints, document.ProjectId, document.Assignees, document.LinkedIssues, document.CreatedAt);

        public static IssueDocument AsDocument(this Issue entity)
            => new IssueDocument
            {
                Id = entity.Id,
                Key = entity.Key,
                Type = entity.Type,
                Status = entity.Status,
                Title = entity.Title,
                Description = entity.Description,
                StoryPoints = entity.StoryPoints,
                ProjectId = entity.ProjectId,
                Assignees = entity.Assignees ?? Enumerable.Empty<Guid>(),
                LinkedIssues = entity.LinkedIssues ?? Enumerable.Empty<Guid>(),
                CreatedAt = entity.CreatedAt
            };

        public static IssueDto AsDto(this IssueDocument document)
            => new IssueDto
            {
                Id = document.Id,
                Key = document.Key,
                Type = document.Type,
                Status = document.Status,
                Title = document.Title,
                Description = document.Description,
                StoryPoints = document.StoryPoints,
                ProjectId = document.ProjectId,
                Assignees = document.Assignees ?? Enumerable.Empty<Guid>(),
                LinkedIssues = document.LinkedIssues ?? Enumerable.Empty<Guid>(),
                CreatedAt = document.CreatedAt
            };
    }
}
