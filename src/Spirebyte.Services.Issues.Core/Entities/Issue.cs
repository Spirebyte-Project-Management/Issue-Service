﻿using System;
using System.Collections.Generic;
using System.Linq;
using Spirebyte.Services.Issues.Core.Attributes;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Exceptions;

namespace Spirebyte.Services.Issues.Core.Entities;

public class Issue
{
    public static readonly Issue Empty = new();

    private Issue()
    {
    }

    public Issue(string id, IssueType type, IssueStatus status, string title, string description, int storyPoints,
        string projectId, string epicId, string sprintId, IEnumerable<Guid> assignees, IEnumerable<Guid> linkedIssues,
        DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new InvalidIdException(id);

        if (string.IsNullOrWhiteSpace(projectId)) throw new InvalidProjectIdException(projectId);

        if (string.IsNullOrWhiteSpace(title)) throw new InvalidTitleException(title);

        Id = id;
        Type = type;
        Status = status;
        Title = title;
        Description = description;
        StoryPoints = storyPoints;
        ProjectId = projectId;
        EpicId = epicId;
        SprintId = sprintId;
        Assignees = assignees ??= Enumerable.Empty<Guid>();
        LinkedIssues = linkedIssues ??= Enumerable.Empty<Guid>();
        CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
    }

    public string Id { get; set; }

    [HistoryFieldType(FieldTypes.Type)] public IssueType Type { get; set;}

    [HistoryFieldType(FieldTypes.Status)] public IssueStatus Status { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public int StoryPoints { get; set; }

    [IgnoreHistory] public string ProjectId { get; set; }

    [HistoryFieldType(FieldTypes.Epic)] public string EpicId { get; set; }

    [HistoryFieldType(FieldTypes.Sprint)] public string SprintId { get; private set; }

    [HistoryFieldType(FieldTypes.Assignees)]
    public IEnumerable<Guid> Assignees { get; set; }

    [IgnoreHistory] public IEnumerable<Guid> LinkedIssues { get; set; }

    [IgnoreHistory] public DateTime CreatedAt { get; set; }


    public void AddToSprint(string sprintId)
    {
        SprintId = sprintId;
    }

    public void RemoveFromSprint()
    {
        SprintId = null;
    }
}