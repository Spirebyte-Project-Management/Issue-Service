using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Core.Enums;
using System.Collections.Generic;

namespace Spirebyte.Services.Issues.Application.Queries
{
    public class GetIssues : IQuery<IEnumerable<IssueDto>>
    {
        public string? ProjectId { get; set; }
        public IssueType? Type { get; set; }
        public bool? HasSprint { get; set; }
        public string[]? IssueIds { get; set; }


        public GetIssues(string? projectId = null, IssueType? issueType = null, bool? hasSprint = null, string[]? issueIds = null)
        {
            ProjectId = projectId;
            Type = issueType;
            HasSprint = hasSprint;
            IssueIds = issueIds;
        }
    }
}
