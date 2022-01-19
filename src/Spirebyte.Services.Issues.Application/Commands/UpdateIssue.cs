using System;
using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Commands;

[Contract]
public class UpdateIssue : ICommand
{
    public UpdateIssue(string issueId, IssueType type, IssueStatus status, string title, string description,
        int storyPoints, string epicId, IEnumerable<Guid> assignees, IEnumerable<Guid> linkedIssues)
    {
        IssueId = issueId;
        Type = type;
        Status = status;
        Title = title;
        Description = description;
        StoryPoints = storyPoints;
        EpicId = epicId;
        Assignees = assignees ?? Enumerable.Empty<Guid>();
        LinkedIssues = linkedIssues ?? Enumerable.Empty<Guid>();
    }

    public string IssueId { get; }
    public IssueType Type { get; }
    public IssueStatus Status { get; }
    public string Title { get; }
    public string Description { get; }
    public int StoryPoints { get; }
    public string EpicId { get; }

    public IEnumerable<Guid> Assignees { get; }
    public IEnumerable<Guid> LinkedIssues { get; }
}