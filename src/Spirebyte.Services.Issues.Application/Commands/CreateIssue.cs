﻿using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spirebyte.Services.Issues.Application.Commands
{
    [Contract]
    public class CreateIssue : ICommand
    {
        public string IssueId { get; }
        public IssueType Type { get; private set; }
        public IssueStatus Status { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int StoryPoints { get; private set; }

        public string ProjectId { get; set; }
        public string EpicId { get; set; }
        public IEnumerable<Guid> Assignees { get; private set; }
        public IEnumerable<Guid> LinkedIssues { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public CreateIssue(string issueId, IssueType type, IssueStatus status, string title, string description, int storyPoints, string projectId, string epicId, IEnumerable<Guid> assignees, IEnumerable<Guid> linkedIssues, DateTime createdAt)
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
    }
}
