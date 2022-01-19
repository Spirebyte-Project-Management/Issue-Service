using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.Commands;

[Contract]
public class DeleteComment : ICommand
{
    public DeleteComment(string id)
    {
        Id = id;
    }

    public string Id { get; }
}