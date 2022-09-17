using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Issues.Commands;

[Message("issues", "update_issue", "issues.update_issue")]
public record UpdateIssue(string Id, IssueType Type, IssueStatus Status, string Title, string Description,
    int StoryPoints, string EpicId, IEnumerable<Guid> Assignees, IEnumerable<Guid> LinkedIssues) : ICommand;