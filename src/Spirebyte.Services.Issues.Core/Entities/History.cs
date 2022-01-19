using System;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Exceptions;

namespace Spirebyte.Services.Issues.Core.Entities;

public class History
{
    public History(Guid id, string issueId, Guid userId, HistoryTypes action, DateTime createdAt, Field[] changedFields)
    {
        if (id == Guid.Empty) throw new InvalidIdException(issueId);

        if (string.IsNullOrEmpty(issueId)) throw new InvalidIssueIdException(issueId);

        if (userId == Guid.Empty) throw new InvalidUserIdException(userId);

        Id = id;
        IssueId = issueId;
        UserId = userId;
        Action = action;
        CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
        ChangedFields = changedFields;
    }

    public Guid Id { get; set; }
    public string IssueId { get; set; }
    public Guid UserId { get; set; }
    public HistoryTypes Action { get; set; }
    public DateTime CreatedAt { get; set; }
    public Field[] ChangedFields { get; set; }
}

public class Field
{
    public Field(string fieldName, string valueBefore, string valueAfter, FieldTypes fieldType)
    {
        FieldName = fieldName;
        ValueBefore = valueBefore;
        ValueAfter = valueAfter;
        FieldType = fieldType;
    }

    public string FieldName { get; set; }
    public string ValueBefore { get; set; }
    public string ValueAfter { get; set; }
    public FieldTypes FieldType { get; set; }
}