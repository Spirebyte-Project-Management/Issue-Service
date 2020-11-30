using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Issues.Application.Queries
{
    public class GetIssuesByIds : IQuery<IEnumerable<IssueDto>>
    {
        public Guid[] IssueIds { get; set; }

        public GetIssuesByIds(Guid[] issueIds)
        {
            IssueIds = issueIds;
        }
    }
}
