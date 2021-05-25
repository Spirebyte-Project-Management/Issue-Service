using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;
using System.Collections.Generic;

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
