using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Issues.Application.Issues.DTO;

namespace Spirebyte.Services.Issues.Application.Issues.Queries;

public class GetIssue : IQuery<IssueDto?>
{
    public GetIssue(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}