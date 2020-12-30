using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Issues.Application.DTO
{
    public class IssueDto
    {
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


        public IssueDto()
        {
        }

        public IssueDto(Issue issue)
        {
            Id = issue.Id;
            Type = issue.Type;
            Status = issue.Status;
            Title = issue.Title;
            Description = issue.Description;
            StoryPoints = issue.StoryPoints;
            ProjectId = issue.ProjectId;
            EpicId = issue.EpicId;
            SprintId = issue.SprintId;
            Assignees = issue.Assignees;
            LinkedIssues = issue.LinkedIssues;
            CreatedAt = issue.CreatedAt;
        }
    }
}
