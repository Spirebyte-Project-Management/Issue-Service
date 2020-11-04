using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;

namespace Spirebyte.Services.Issues.Application.Queries
{
    public class GetIssue : IQuery<IssueDto>
    {
        public string IssueKey { get; set; }
    }
}
