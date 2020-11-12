using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Application.DTO
{
    public class IssueDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public IssueType Type { get; set; }
        public IssueStatus Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StoryPoints { get; set; }

        public Guid ProjectId { get; set; }
        public IEnumerable<Guid> Assignees { get; set; }
        public IEnumerable<Guid> LinkedIssues { get; set; }

        public DateTime CreatedAt { get; set; }


        public IssueDto()
        {
        }

        public IssueDto(Issue issue)
        {
            Id = issue.Id;
            Key = issue.Key;
            Type = issue.Type;
            Status = issue.Status;
            Title = issue.Title;
            Description = issue.Description;
            StoryPoints = issue.StoryPoints;
            Assignees = issue.Assignees;
            LinkedIssues = issue.LinkedIssues;
            CreatedAt = issue.CreatedAt;
        }
    }
}
