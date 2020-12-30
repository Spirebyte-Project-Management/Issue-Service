using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Core.Entities;
using System;
using System.Linq;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers
{
    internal static class IssueMappers
    {
        public static Issue AsEntity(this IssueDocument document)
            => new Issue(document.Id, document.Type, document.Status, document.Title, document.Description, document.StoryPoints, document.ProjectId, document.EpicId, document.SprintId, document.Assignees, document.LinkedIssues, document.CreatedAt);

        public static IssueDocument AsDocument(this Issue entity)
            => new IssueDocument
            {
                Id = entity.Id,
                Type = entity.Type,
                Status = entity.Status,
                Title = entity.Title,
                Description = entity.Description,
                StoryPoints = entity.StoryPoints,
                ProjectId = entity.ProjectId,
                EpicId = entity.EpicId,
                SprintId = entity.SprintId,
                Assignees = entity.Assignees ?? Enumerable.Empty<Guid>(),
                LinkedIssues = entity.LinkedIssues ?? Enumerable.Empty<Guid>(),
                CreatedAt = entity.CreatedAt
            };

        public static IssueDto AsDto(this IssueDocument document)
            => new IssueDto
            {
                Id = document.Id,
                Type = document.Type,
                Status = document.Status,
                Title = document.Title,
                Description = document.Description,
                StoryPoints = document.StoryPoints,
                ProjectId = document.ProjectId,
                EpicId = document.EpicId,
                SprintId = document.SprintId,
                Assignees = document.Assignees ?? Enumerable.Empty<Guid>(),
                LinkedIssues = document.LinkedIssues ?? Enumerable.Empty<Guid>(),
                CreatedAt = document.CreatedAt
            };
    }
}
