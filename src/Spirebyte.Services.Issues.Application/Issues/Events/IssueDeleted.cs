using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Issues.Events;

[Contract]
public class IssueDeleted : IEvent
{
    public IssueDeleted(string id, IssueType type, IssueStatus status, string title, string description,
        int storyPoints, string projectId, string epicId, string sprintId, IEnumerable<Guid> assignees,
        IEnumerable<Guid> linkedIssues, DateTime createdAt)
    {
        Id = id;
        Type = type;
        Status = status;
        Title = title;
        Description = description;
        StoryPoints = storyPoints;
        ProjectId = projectId;
        EpicId = epicId;
        SprintId = sprintId;
        Assignees = assignees;
        LinkedIssues = linkedIssues;
        CreatedAt = createdAt;
    }

    public IssueDeleted(Issue entity)
    {
        Id = entity.Id;
        Type = entity.Type;
        Status = entity.Status;
        Title = entity.Title;
        Description = entity.Description;
        StoryPoints = entity.StoryPoints;
        ProjectId = entity.ProjectId;
        EpicId = entity.EpicId;
        SprintId = entity.SprintId;
        Assignees = entity.Assignees;
        LinkedIssues = entity.LinkedIssues;
        CreatedAt = entity.CreatedAt;
    }

    public string Id { get; set; }
    public IssueType Type { get; set; }
    public IssueStatus Status { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StoryPoints { get; set; }

    public string ProjectId { get; set; }
    public string EpicId { get; set; }
    public string SprintId { get; set; }
    public IEnumerable<Guid> Assignees { get; set; }
    public IEnumerable<Guid> LinkedIssues { get; set; }

    public DateTime CreatedAt { get; set; }
}