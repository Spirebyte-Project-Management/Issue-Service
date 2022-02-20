using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Issues.Commands;

[Contract]
public record CreateIssue(IssueType Type, IssueStatus Status, string Title, string Description,
    int StoryPoints, string ProjectId, string EpicId, IEnumerable<Guid> Assignees, IEnumerable<Guid> LinkedIssues,
    DateTime CreatedAt) : ICommand
{
    public Guid ReferenceId = Guid.NewGuid();
}