using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.Commands
{
    [Contract]
    public class DeleteIssue : ICommand
    {
        public string IssueId { get; set; }

        public DeleteIssue(string issueId)
        {
            IssueId = issueId;
        }
    }
}
