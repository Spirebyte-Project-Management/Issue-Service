using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;

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
