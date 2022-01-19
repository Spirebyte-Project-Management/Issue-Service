using System.Collections.Generic;
using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;

namespace Spirebyte.Services.Issues.Application.Queries;

public class GetHistory : IQuery<IEnumerable<HistoryDto>>
{
    public GetHistory(string? issueId)
    {
        IssueId = issueId;
    }

    public string? IssueId { get; set; }
}