using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.Commands
{
    [Contract]
    public class DeleteComment : ICommand
    {
        public string Id { get; private set; }

        public DeleteComment(string id)
        {
            Id = id;
        }
    }
}
