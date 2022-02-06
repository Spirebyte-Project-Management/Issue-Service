using System.Collections.Generic;
using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.Issues.DTO;

namespace Spirebyte.Services.Issues.Application.Issues.Queries;

public class GetHistory : IQuery<IEnumerable<HistoryDto>>
{
    public GetHistory(string? issueId)
    {
        IssueId = issueId;
    }

    public string? IssueId { get; set; }
}