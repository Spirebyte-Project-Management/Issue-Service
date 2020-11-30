using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;
using System.Collections.Generic;

namespace Spirebyte.Services.Issues.Application.Queries
{
    public class GetIssues : IQuery<IEnumerable<IssueDto>>
    {
        public string ProjectKey { get; set; }

        public GetIssues(string projectKey)
        {
            ProjectKey = projectKey;
        }
    }
}
