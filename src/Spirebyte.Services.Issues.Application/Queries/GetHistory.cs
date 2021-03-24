using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;

namespace Spirebyte.Services.Issues.Application.Queries
{
    public class GetHistory : IQuery<IEnumerable<HistoryDto>>
    {
        public string? IssueId { get; set; }

        public GetHistory(string? issueId)
        {
            IssueId = issueId;
        }
    }
}
