using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Attributes;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Exceptions;
using Spirebyte.Services.Issues.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Application.Services
{
    internal sealed class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IAppContext _appContext;

        public HistoryService(IHistoryRepository historyRepository, IAppContext appContext)
        {
            _historyRepository = historyRepository;
            _appContext = appContext;
        }

        public async Task SaveHistory(Issue oldIssue, Issue newIssue, HistoryTypes action)
        {
            if (oldIssue == null)
            {
                oldIssue = Issue.Empty;
            }

            if (newIssue == null)
            {
                throw new ArgumentNullException(nameof(newIssue));
            }

            if (oldIssue != Issue.Empty && oldIssue.Id != newIssue.Id)
            {
                throw new InvalidIssueIdException(newIssue.Id);
            }

            var oldType = oldIssue.GetType();
            var newType = newIssue.GetType();

            var oldProperties = oldType.GetProperties();
            var newProperties = newType.GetProperties();

            List<Field> changedFields = new List<Field>();

            foreach (var oldProperty in oldProperties)
            {
                var matchingProperty = newProperties.FirstOrDefault(x =>
                    !Attribute.IsDefined(x, typeof(IgnoreHistoryAttribute))
                    && x.Name == oldProperty.Name && x.PropertyType == oldProperty.PropertyType);

                if (matchingProperty == null) continue;

                var fieldTypeAttribute = oldProperty.GetCustomAttribute(typeof(HistoryFieldTypeAttribute)) as HistoryFieldTypeAttribute;
                var fieldType = fieldTypeAttribute?.FieldType ?? FieldTypes.String;

                string oldValue;
                string newValue;

                switch (fieldType)
                {
                    case FieldTypes.Assignees:
                    case FieldTypes.LinkedIssues:
                        var oldPropertyArray = ((IEnumerable<Guid>) oldProperty.GetValue(oldIssue)).ToArray();
                        var newPropertyArray = ((IEnumerable<Guid>) oldProperty.GetValue(oldIssue)).ToArray();
                        oldValue = string.Join(',', oldPropertyArray);
                        newValue = string.Join(',', newPropertyArray);
                        break;
                    default:
                        oldValue = oldProperty.GetValue(oldIssue)?.ToString();
                        newValue = matchingProperty.GetValue(newIssue)?.ToString();
                        break;

                }

                if (oldValue == newValue) continue;

                var field = new Field(matchingProperty.Name, oldValue, newValue, fieldType);
                changedFields.Add(field);
            }

            var dateChanged = DateTime.Now;
            var userId = _appContext.Identity.Id;

            if (changedFields.Count == 0) return;

            var history = new History(Guid.NewGuid(), newIssue.Id, userId, action, dateChanged, changedFields.ToArray());
            await _historyRepository.AddAsync(history);
        }
    }
}
