using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Issues.Commands;

[Contract]
public record UpdateIssue(string Id, IssueType Type, IssueStatus Status, string Title, string Description,
    int StoryPoints, string EpicId, IEnumerable<Guid> Assignees, IEnumerable<Guid> LinkedIssues) : ICommand;