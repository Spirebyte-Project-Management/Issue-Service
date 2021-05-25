using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using System;

namespace Spirebyte.Services.Issues.Application.DTO
{
    public class HistoryDto
    {
        public Guid Id { get; set; }
        public string IssueId { get; set; }
        public Guid UserId { get; set; }
        public HistoryTypes Action { get; set; }
        public DateTime CreatedAt { get; set; }
        public Field[] ChangedFields { get; set; }
    }
}
