using System;
using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Commands;

[Contract]
public class CreateIssue : ICommand
{
    public CreateIssue(string issueId, IssueType type, IssueStatus status, string title, string description,
        int storyPoints, string projectId, string epicId, IEnumerable<Guid> assignees, IEnumerable<Guid> linkedIssues,
        DateTime createdAt)
    {
        IssueId = issueId;
        Type = type;
        Status = status;
        Title = title;
        Description = description;
        StoryPoints = storyPoints;
        ProjectId = projectId;
        EpicId = epicId;
        Assignees = assignees ?? Enumerable.Empty<Guid>();
        LinkedIssues = linkedIssues ?? Enumerable.Empty<Guid>();
        CreatedAt = createdAt;
    }

    public string IssueId { get; }
    public IssueType Type { get; }
    public IssueStatus Status { get; }
    public string Title { get; }
    public string Description { get; }
    public int StoryPoints { get; }

    public string ProjectId { get; set; }
    public string EpicId { get; set; }
    public IEnumerable<Guid> Assignees { get; }
    public IEnumerable<Guid> LinkedIssues { get; }

    public DateTime CreatedAt { get; }
}