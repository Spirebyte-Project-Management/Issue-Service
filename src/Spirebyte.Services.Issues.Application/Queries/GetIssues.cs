using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;

namespace Spirebyte.Services.Issues.Application.Queries
{
    public class GetIssues : IQuery<IEnumerable<IssueDto>>
    {
        public Guid ProjectId { get; set; }
    }
}
