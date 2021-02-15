using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;

namespace Spirebyte.Services.Issues.Application.Queries
{
    public class GetComments : IQuery<IEnumerable<CommentDto>>
    {
        public string? ProjectId { get; set; }
        public string? IssueId { get; set; }

        public GetComments(string? projectId = null, string? issueId = null)
        {
            ProjectId = projectId;
            IssueId = issueId;
        }
    }
}
