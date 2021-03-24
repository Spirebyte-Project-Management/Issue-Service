using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents.Mappers
{
    internal static class HistoryMappers
    {
        public static History AsEntity(this HistoryDocument document)
            => new History(document.Id, document.IssueId, document.UserId, document.Action, document.CreatedAt, document.ChangedFields);

        public static HistoryDocument AsDocument(this History entity)
            => new HistoryDocument
            {
                Id = entity.Id,
                IssueId = entity.IssueId,
                UserId = entity.UserId,
                Action = entity.Action,
                CreatedAt = entity.CreatedAt,
                ChangedFields = entity.ChangedFields
            };

        public static HistoryDto AsDto(this HistoryDocument document)
            => new HistoryDto
            {
                Id = document.Id,
                IssueId = document.IssueId,
                UserId = document.UserId,
                Action = document.Action,
                CreatedAt = document.CreatedAt,
                ChangedFields = document.ChangedFields
            };
    }
}
