using Convey.CQRS.Queries;
using Spirebyte.Services.Issues.Application.DTO;

namespace Spirebyte.Services.Issues.Application.Queries;

public class GetComment : IQuery<CommentDto>
{
    public GetComment(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}