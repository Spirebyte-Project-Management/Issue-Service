using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;

namespace Spirebyte.Services.Issues.Application.Queries
{
    public class GetEpics : IQuery<IEnumerable<IssueDto>>
    {
        public string ProjectKey { get; set; }

        public GetEpics(string projectKey)
        {
            ProjectKey = projectKey;
        }
    }
}
