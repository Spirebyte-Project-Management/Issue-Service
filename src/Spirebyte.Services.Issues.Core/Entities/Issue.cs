using System;
using System.Collections.Generic;
using System.Linq;
using Spirebyte.Services.Issues.Core.Entities.Base;
using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Exceptions;

namespace Spirebyte.Services.Issues.Core.Entities
{
    public class Issue : AggregateRoot
    {
        public string Key { get; private set; }
        public IssueType Type { get; private set; }
        public IssueStatus Status { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int StoryPoints { get; private set; }

        public Guid ProjectId { get; private set; }
        public IEnumerable<Guid> Assignees { get; private set; }
        public IEnumerable<Guid> LinkedIssues { get; private set; }

        public DateTime CreatedAt { get; private set; }


        public Issue(Guid id, string key, IssueType type, IssueStatus status, string title, string description, int storyPoints, Guid projectId, IEnumerable<Guid> assignees, IEnumerable<Guid> linkedIssues, DateTime createdAt)
        {
            if (projectId == Guid.Empty)
            {
                throw new InvalidProjectIdException(projectId);
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidKeyException(key);
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidTitleException(title);
            }

            Id = id;
            Key = key;
            Type = type;
            Status = status;
            Title = title;
            Description = description;
            StoryPoints = storyPoints;
            ProjectId = projectId;
            Assignees = assignees ??= Enumerable.Empty<Guid>(); ;
            LinkedIssues = linkedIssues ??= Enumerable.Empty<Guid>(); ;
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
        }
    }
}
