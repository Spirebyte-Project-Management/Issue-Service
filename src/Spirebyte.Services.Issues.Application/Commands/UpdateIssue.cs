using Convey.CQRS.Commands;
using Spirebyte.Services.Issues.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spirebyte.Services.Issues.Application.Commands
{
    [Contract]
    public class UpdateIssue : ICommand
    {
        public Guid IssueId { get; }
        public string Key { get; private set; }
        public IssueType Type { get; private set; }
        public IssueStatus Status { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int StoryPoints { get; private set; }

        public IEnumerable<Guid> Assignees { get; private set; }
        public IEnumerable<Guid> LinkedIssues { get; private set; }

        public UpdateIssue(Guid issueId, string key, IssueType type, IssueStatus status, string title, string description, int storyPoints, IEnumerable<Guid> assignees, IEnumerable<Guid> linkedIssues)
        {
            IssueId = issueId;
            Key = key;
            Type = type;
            Status = status;
            Title = title;
            Description = description;
            StoryPoints = storyPoints;
            Assignees = assignees ?? Enumerable.Empty<Guid>();
            LinkedIssues = linkedIssues ?? Enumerable.Empty<Guid>();
        }
    }
}
