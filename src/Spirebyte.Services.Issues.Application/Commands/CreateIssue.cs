using System;
using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.Commands
{
    [Contract]
    public class CreateIssue : ICommand
    {
        public Guid IssueId { get; }
        public string Key { get; private set; }
        public IssueType Type { get; private set; }
        public IssueStatus Status { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int StoryPoints { get; private set; }

        public Guid ProjectId { get; set; }
        public IEnumerable<Guid> Assignees { get; private set; }
        public IEnumerable<Guid> LinkedIssues { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public CreateIssue(Guid issueId, string key, IssueType type, IssueStatus status, string title, string description, int storyPoints, Guid projectId, IEnumerable<Guid> assignees, IEnumerable<Guid> linkedIssues, DateTime createdAt)
        {
            IssueId = issueId;
            Key = key;
            Type = type;
            Status = status;
            Title = title;
            Description = description;
            StoryPoints = storyPoints;
            ProjectId = projectId;
            Assignees = assignees ?? Enumerable.Empty<Guid>();
            LinkedIssues = linkedIssues ?? Enumerable.Empty<Guid>();
            CreatedAt = createdAt;
        }
    }
}
