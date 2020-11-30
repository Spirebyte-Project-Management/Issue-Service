using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;
using System.Collections.Generic;

namespace Spirebyte.Services.Issues.Application.Queries
{
    public class GetIssuesWithoutSprintByProject : IQuery<IEnumerable<IssueDto>>
    {
        public string ProjectKey { get; set; }

        public GetIssuesWithoutSprintByProject(string projectKey)
        {
            ProjectKey = projectKey;
        }
    }
}
