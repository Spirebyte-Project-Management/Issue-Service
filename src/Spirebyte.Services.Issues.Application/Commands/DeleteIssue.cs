using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.Commands
{
    [Contract]
    public class DeleteIssue : ICommand
    {
        public string Key { get; set; }

        public DeleteIssue(string key)
        {
            Key = key;
        }
    }
}
