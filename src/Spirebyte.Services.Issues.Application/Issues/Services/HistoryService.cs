using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Spirebyte.Framework.Contexts;
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Attributes;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Exceptions;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Issues.Services;

internal sealed class HistoryService : IHistoryService
{
    private readonly IContextAccessor _contextAccessor;
    private readonly IHistoryRepository _historyRepository;

    public HistoryService(IHistoryRepository historyRepository, IContextAccessor contextAccessor)
    {
        _historyRepository = historyRepository;
        _contextAccessor = contextAccessor;
    }

    public async Task SaveHistory(Issue? oldIssue, Issue newIssue, HistoryTypes action)
    {
        oldIssue ??= Issue.Empty;

        if (newIssue == null) throw new ArgumentNullException(nameof(newIssue));

        if (oldIssue != Issue.Empty && oldIssue.Id != newIssue.Id) throw new InvalidIssueIdException(newIssue.Id);

        var oldType = oldIssue.GetType();
        var newType = newIssue.GetType();

        var oldProperties = oldType.GetProperties();
        var newProperties = newType.GetProperties();

        var changedFields = new List<Field>();

        foreach (var oldProperty in oldProperties)
        {
            var matchingProperty = newProperties.FirstOrDefault(x =>
                !Attribute.IsDefined(x, typeof(IgnoreHistoryAttribute))
                && x.Name == oldProperty.Name && x.PropertyType == oldProperty.PropertyType);

            if (matchingProperty == null) continue;

            var fieldTypeAttribute =
                oldProperty.GetCustomAttribute(typeof(HistoryFieldTypeAttribute)) as HistoryFieldTypeAttribute;
            var fieldType = fieldTypeAttribute?.FieldType ?? FieldTypes.String;

            string? oldValue;
            string? newValue;

            switch (fieldType)
            {
                case FieldTypes.Assignees:
                case FieldTypes.LinkedIssues:
                    var oldPropertyArray =
                        ((IEnumerable<Guid>)oldProperty.GetValue(oldIssue) ?? Array.Empty<Guid>()).ToArray();
                    var newPropertyArray =
                        ((IEnumerable<Guid>)matchingProperty.GetValue(newIssue) ?? Array.Empty<Guid>()).ToArray();
                    oldValue = string.Join(',', oldPropertyArray);
                    newValue = string.Join(',', newPropertyArray);
                    break;
                default:
                    oldValue = oldProperty.GetValue(oldIssue)?.ToString() ?? string.Empty;
                    newValue = matchingProperty.GetValue(newIssue)?.ToString() ?? string.Empty;
                    break;
            }

            if (oldValue == newValue) continue;

            var field = new Field(matchingProperty.Name, oldValue, newValue, fieldType);
            changedFields.Add(field);
        }

        var dateChanged = DateTime.Now;
        var userId = _contextAccessor.Context.GetUserId();

        if (changedFields.Count == 0) return;

        var history = new History(Guid.NewGuid(), newIssue.Id, userId, action, dateChanged, changedFields.ToArray());
        await _historyRepository.AddAsync(history);
    }
}