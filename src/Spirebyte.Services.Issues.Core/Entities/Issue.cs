using Spirebyte.Services.Issues.Core.Enums;
using Spirebyte.Services.Issues.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spirebyte.Services.Issues.Core.Entities
{
    public class Issue
    {
        public string Id { get; private set; }
        public IssueType Type { get; private set; }
        public IssueStatus Status { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int StoryPoints { get; private set; }

        public string ProjectId { get; private set; }
        public string EpicId { get; private set; }
        public string SprintId { get; private set; }
        public IEnumerable<Guid> Assignees { get; private set; }
        public IEnumerable<Guid> LinkedIssues { get; private set; }

        public DateTime CreatedAt { get; private set; }


        public Issue(string id, IssueType type, IssueStatus status, string title, string description, int storyPoints, string projectId, string epicId, string sprintId, IEnumerable<Guid> assignees, IEnumerable<Guid> linkedIssues, DateTime createdAt)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidIdException(id);
            }

            if (string.IsNullOrWhiteSpace(projectId))
            {
                throw new InvalidProjectIdException(projectId);
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidTitleException(title);
            }

            Id = id;
            Type = type;
            Status = status;
            Title = title;
            Description = description;
            StoryPoints = storyPoints;
            ProjectId = projectId;
            EpicId = epicId;
            SprintId = sprintId;
            Assignees = assignees ??= Enumerable.Empty<Guid>(); ;
            LinkedIssues = linkedIssues ??= Enumerable.Empty<Guid>(); ;
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.Now : createdAt;
        }

        public void AddToSprint(string sprintId)
        {
            SprintId = sprintId;
        }

        public void RemoveFromSprint()
        {
            SprintId = null;
        }
    }
}
