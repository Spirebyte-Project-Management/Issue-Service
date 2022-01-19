using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;

namespace Spirebyte.Services.Issues.Application.Queries;

public class GetIssue : IQuery<IssueDto>
{
    public GetIssue(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}