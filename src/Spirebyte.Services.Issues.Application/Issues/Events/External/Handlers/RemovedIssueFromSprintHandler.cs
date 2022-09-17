using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Application.Issues.Exceptions;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Issues.Events.External.Handlers;

public class RemovedIssueFromSprintHandler : IEventHandler<RemovedIssueFromSprint>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IMessageBroker _messageBroker;


    public RemovedIssueFromSprintHandler(IIssueRepository issueRepository, IMessageBroker messageBroker)
    {
        _issueRepository = issueRepository;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(RemovedIssueFromSprint @event, CancellationToken cancellationToken = default)
    {
        if (!await _issueRepository.ExistsAsync(@event.IssueId)) throw new IssueNotFoundException(@event.IssueId);

        var issue = await _issueRepository.GetAsync(@event.IssueId);
        issue.RemoveFromSprint();
        await _issueRepository.UpdateAsync(issue);
    }
}