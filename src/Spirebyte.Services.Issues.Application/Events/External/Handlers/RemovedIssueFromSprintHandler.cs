using Convey.CQRS.Events;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Core.Repositories;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Application.Services.Interfaces;

namespace Spirebyte.Services.Issues.Application.Events.External.Handlers
{
    public class RemovedIssueFromSprintHandler : IEventHandler<RemovedIssueFromSprint>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMessageBroker _messageBroker;


        public RemovedIssueFromSprintHandler(IIssueRepository issueRepository, IMessageBroker messageBroker)
        {
            _issueRepository = issueRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(RemovedIssueFromSprint @event)
        {
            if (!await _issueRepository.ExistsAsync(@event.IssueId))
            {
                throw new IssueNotFoundException(@event.IssueId);
            }

            var issue = await _issueRepository.GetAsync(@event.IssueId);
            issue.RemoveFromSprint();
            await _issueRepository.UpdateAsync(issue);

            await _messageBroker.PublishAsync(new IssueUpdated(issue.Id));
        }
    }
}
