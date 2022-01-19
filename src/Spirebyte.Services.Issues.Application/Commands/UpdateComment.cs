using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.Commands;

[Contract]
public class UpdateComment : ICommand
{
    public UpdateComment(string id, string body)
    {
        Id = id;
        Body = body;
    }

    public string Id { get; }
    public string Body { get; }
}