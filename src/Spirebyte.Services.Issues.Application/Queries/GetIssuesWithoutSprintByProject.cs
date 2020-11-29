using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;

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
