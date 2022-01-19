using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Application.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Events.External.Handlers;

public class AddedIssueToSprintHandler : IEventHandler<AddedIssueToSprint>
{
    private readonly IIssueRepository _issueRepository;
    private readonly ILogger<AddedIssueToSprintHandler> _logger;
    private readonly IMessageBroker _messageBroker;


    public AddedIssueToSprintHandler(IIssueRepository issueRepository, IMessageBroker messageBroker,
        ILogger<AddedIssueToSprintHandler> logger)
    {
        _issueRepository = issueRepository;
        _messageBroker = messageBroker;
        _logger = logger;
    }

    public async Task HandleAsync(AddedIssueToSprint @event)
    {
        _logger.LogError("IssueID: {0} - SprintID: {1}", @event.IssueId, @event.SprintId);
        if (!await _issueRepository.ExistsAsync(@event.IssueId)) throw new IssueNotFoundException(@event.IssueId);

        var issue = await _issueRepository.GetAsync(@event.IssueId);
        issue.AddToSprint(@event.SprintId);
        await _issueRepository.UpdateAsync(issue);

        await _messageBroker.PublishAsync(new IssueUpdated(issue.Id, issue.StoryPoints, issue.Status));
    }
}