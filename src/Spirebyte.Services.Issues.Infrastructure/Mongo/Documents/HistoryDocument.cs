using System;
using Spirebyte.Framework.Shared.Types;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Infrastructure.Mongo.Documents;

public sealed class HistoryDocument : IIdentifiable<Guid>
{
    public string IssueId { get; set; }
    public Guid UserId { get; set; }
    public HistoryTypes Action { get; set; }
    public DateTime CreatedAt { get; set; }
    public Field[] ChangedFields { get; set; }
    public Guid Id { get; set; }
}