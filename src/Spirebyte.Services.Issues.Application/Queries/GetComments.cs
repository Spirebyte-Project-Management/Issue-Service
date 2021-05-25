using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;
using System.Collections.Generic;

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
